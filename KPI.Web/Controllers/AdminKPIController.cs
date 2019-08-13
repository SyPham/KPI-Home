using KPI.Model.DAO;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear =true)]
    public class AdminKPIController : BaseController
    {
        // GET: AdminKPI
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("KPI");
            return View();
        }
        public ActionResult ListKPI()
        {
            return View();
        }
        public JsonResult Add(Model.EF.KPI entity)
        {
            return Json(new KPIAdminDAO().Add(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListCategory(Model.EF.Category entity)
        {
            return Json(new KPIAdminDAO().ListCategory(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddKPILevel(Model.EF.KPILevel entity)
        {
            return Json(new KPIAdminDAO().AddKPILevel(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAll()
        {
            return Json(new KPIAdminDAO().GetAll(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            return Json(new KPIAdminDAO().Delete(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Model.EF.KPI entity)
        {
            return Json(new KPIAdminDAO().Update(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            return Json(new KPIAdminDAO().GetbyID(ID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadData(int? catID,string name, int page, int pageSize)
        {
            return Json(new KPIAdminDAO().LoadData(catID,name,page,pageSize), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Autocomplete(string name)
        {
            return Json(new KPIAdminDAO().Autocomplete(name), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllUnit()
        {
            return Json(new KPIAdminDAO().GetAllUnit(), JsonRequestBehavior.AllowGet);
        }
    }
}