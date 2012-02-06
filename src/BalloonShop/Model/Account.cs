﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using FluentNHibernate.Mapping;

namespace BalloonShop.Model
{
	public class Account
	{
		public Account()
		{

		}

		public Account(string email, string password)
		{
			Email = email;
			Password = Hash(password);
		}

		public virtual int Id { get; protected set; }
		public virtual string Email { get; protected set; }
		public virtual string Password { get; protected set; }
		public virtual AccountDetails Details { get; set; }

		private string Hash(string password) {
			return Encoding.ASCII.GetString(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(Email + "StaticSalt" + password)));
		}

		public virtual bool Authenticate(string password) {
			return Password == Hash(password);
		}
	}

	public class AccountMap : ClassMap<Account>
	{
		public AccountMap()
		{
			Id(x => x.Id).GeneratedBy.Identity();
			Map(x => x.Email);
			Map(x => x.Password);
			Component(x => x.Details, component => {
				component.Map(x => x.Address1);
				component.Map(x => x.Address2);
				component.Map(x => x.City);
				component.Map(x => x.Country);
				component.Map(x => x.PostalCode);
				component.Map(x => x.Region);
				component.Map(x => x.ShippingRegion);
				component.Map(x => x.DaytimePhone);
				component.Map(x => x.EveningPhone);
				component.Map(x => x.MobilePhone);
			});
		}
	}
}
