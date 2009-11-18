using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BalloonShop.Data;
using BalloonShop.Infrastructure;
using BalloonShop.Model;

namespace BalloonShop.Mvc.Controllers
{
    public class CategoryController : Controller
    {
        public ActionResult Navigation(int departmentId, int? selectedCategoryId)
        {
            ViewData["CategoryId"] = selectedCategoryId ?? 0;
            return PartialView(CatalogAccess.GetCategoriesInDepartment(departmentId));
        }

        public ActionResult Show(int id, int? page)
        {
            int howManyPages, p = page ?? 1;
            
            var category = CatalogAccess.GetCategoryDetails(id);

            var balloons = CatalogAccess.GetProductsInCategory(id, p, out howManyPages);
            category.Balloons = new PagedList<Balloon>(p, BalloonShopConfiguration.ProductsPerPage, howManyPages, balloons);
            return View(category);
        }
    }
}