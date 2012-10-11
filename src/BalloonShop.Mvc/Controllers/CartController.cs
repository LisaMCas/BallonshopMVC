﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using BalloonShop.Model;
using BalloonShop.Mvc.Models;
using BalloonShop.Mvc.Helpers;
using System.Security.Principal;
using BalloonShop.Messages;
using Rhino.ServiceBus;
using Rhino.Queues.Utils;

namespace BalloonShop.Mvc.Controllers
{
    public class CartController : Controller
    {
		private readonly IIdentity _identity;
		private readonly ISession _session;
        private readonly IOnewayBus _bus;

        public CartController(IIdentity identity, ISession session, IOnewayBus bus)
        {
			_identity = identity;
            _session = session;
            _bus = bus;
        }

        [HttpPost]
        public ActionResult Add(string customerCartId, int balloonId)
        {
            var balloon = _session.Load<Balloon>(balloonId);

            var item = _session.Get<ShoppingCart>(new ShoppingCart { Balloon = balloon, CartId = customerCartId });

            if (item == null) {
                item = new ShoppingCart() { Balloon = balloon, CartId = customerCartId, DateAdded = DateTime.Now };
                _session.Save(item);
            }

            item.Quantity += 1;

            return RedirectToAction("Show", "Balloon", new { id = balloonId });
        }

        [HttpPost]
        public ActionResult Remove(string customerCartId, int remove) {
            _session.Delete(_session.Load<ShoppingCart>(new ShoppingCart() { CartId = customerCartId, Balloon = _session.Load<Balloon>(remove) }));

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public ActionResult Update(string customerCartId, IDictionary<int, int> items) {
            var cart = _session.QueryOver<ShoppingCart>().Where(x => x.CartId == customerCartId).List();

            foreach (var item in items)
            {
                cart.Single(x => x.Balloon.Id == item.Key).Quantity = item.Value;
            }

            return RedirectToAction("Index", "Cart");
        }

        public ActionResult Summary(string customerCartId) {
            var model = _session.QueryOver<ShoppingCart>().Where(x => x.CartId == customerCartId).List();

            ViewBag.Total = model.Sum(x => x.Balloon.Price * x.Quantity);

            return View(model);
        }

        public ActionResult Index(string customerCartId) {
            var model = _session.QueryOver<ShoppingCart>().Where(x => x.CartId == customerCartId).List();

            ViewBag.Total = model.Sum(x => x.Balloon.Price * x.Quantity);

            return View(model);
        }

		[Authorize]
		public ActionResult Checkout(string customerCartId) {
			var cart = _session.QueryOver<ShoppingCart>().Where(x => x.CartId == customerCartId).List();
			var account = _session.Get<Account>(_identity.Identity());

			ViewBag.Cart = cart;
			ViewBag.Total = cart.Sum(x => x.Balloon.Price * x.Quantity);
			ViewBag.ShippingRegions = _session.QueryOver<ShippingRegion>().List().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == account.Details.ShippingRegion }).ToList();
			ViewBag.ShippingTypes = _session.QueryOver<Shipping>().List();

			return View(new CheckoutViewModel() { AccountDetails = new AccountDetailsViewModel(account.Details) });
		}

		[Authorize]
		[HttpPost]
		public ActionResult Checkout(string customerCartId, CheckoutViewModel model)
		{
			var cart = _session.QueryOver<ShoppingCart>().Where(x => x.CartId == customerCartId).List();
			var account = _session.Get<Account>(_identity.Identity());

			ViewBag.Cart = cart;
			ViewBag.Total = cart.Sum(x => x.Balloon.Price * x.Quantity);
			ViewBag.ShippingRegions = _session.QueryOver<ShippingRegion>().List().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == account.Details.ShippingRegion }).ToList();
			ViewBag.ShippingTypes = _session.QueryOver<Shipping>().List();

			if (!ModelState.IsValid)
				return View();

			int taxId;
			switch (model.AccountDetails.ShippingRegion)
			{
				case 2:
					taxId = 1;
					break;
				default:
					taxId = 2;
					break;
			}

			var order = new Order() { 
				CustomerId = account.Id,
				CustomerEmail = account.Email,
				ShippingId = model.ShippingType,
				TaxId = taxId,
                SagaCorrelationId = GuidCombGenerator.Generate()
			};

			foreach (var item in cart) {
				order.AddOrderDetail(new OrderDetail() { 
					ProductId = item.Balloon.Id, 
					ProductName = item.Balloon.Name, 
					Quantity = item.Quantity, 
					UnitCost = item.Balloon.Price 
				});
				_session.Delete(item);
			}

			_session.Save(order);

            _bus.Send(new InitialNotificationMessage() { OrderId = order.Id, CorrelationId = order.SagaCorrelationId });

			return RedirectToAction("Placed", "Order");
		}
    }
}
