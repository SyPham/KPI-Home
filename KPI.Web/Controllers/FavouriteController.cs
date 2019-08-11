using KPI.Model.DAO;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class FavouriteController : BaseController
    {
        // GET: Favourite
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("Favourite");
            return View();
        }
        [HttpGet]
        public JsonResult LoadData(int userid, int page, int pageSize)
        {
            return Json(new FavouriteDAO().LoadData(userid, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new FavouriteDAO().Delete(id), JsonRequestBehavior.AllowGet);
        }

    }
}