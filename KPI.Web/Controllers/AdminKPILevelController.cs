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
    public class AdminKPILevelController : BaseController
    {
        // GET: AdminKPILevel
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("KPI Level");
            return View();
        }
        public JsonResult GetListTree()
        {
            return Json(new LevelDAO().GetListTree(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDataKPILevel(int level, int category, int page, int pageSize)
        {
            return Json(new KPILevelDAO().LoadData(level, category, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCategoryCode(Model.EF.Category entity)
        {
            return Json(new KPILevelDAO().GetAllCategory(), JsonRequestBehavior.AllowGet);
        }
        //update kpiLevel
        public JsonResult UpdateKPILevel(Model.EF.KPILevel entity)
        {
            return Json(new KPILevelDAO().Update(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            return Json(new KPILevelDAO().GetbyID(ID), JsonRequestBehavior.AllowGet);
        }
    }
}