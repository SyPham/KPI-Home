using KPI.Model.DAO;
using KPI.Model.ViewModel;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    public class NotificationController : BaseController
    {
        // GET: AddUserToLevel
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult UpdateRange(string listID)
        {
            return Json(new NotificationDAO().UpdateRange(listID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(int ID)
        {
            return Json(new NotificationDAO().Update(ID), JsonRequestBehavior.AllowGet);
        }
    }
}