using KPI.Model.DAO;
using KPI.Model.EF;
using MvcBreadCrumbs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class AdminLevelController : BaseController
    {
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("Level");
            return View();
        }
        public JsonResult GetListTree()
        {
            return Json(new LevelDAO().GetListTree(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddOrUpdate(Level entity)
        {
            return Json(new LevelDAO().AddOrUpdate(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetByID(int id)
        {
            //string Code = JsonConvert.SerializeObject(code);
            return Json(new LevelDAO().GetByID(id), JsonRequestBehavior.AllowGet);
        }
    }
}