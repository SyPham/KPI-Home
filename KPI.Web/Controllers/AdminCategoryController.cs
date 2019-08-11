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
    public class AdminCategoryController : BaseController
    {
        // GET: Category
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("Categories");
            return View();
        }
        public JsonResult Add(Model.EF.Category entity)
        {
            return Json(new AdminCategoryDAO().Add(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Model.EF.Category entity)
        {
            return Json(new AdminCategoryDAO().Update(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListCategory(Model.EF.Category entity)
        {
            return Json(new AdminCategoryDAO().ListCategory(), JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetAll()
        {
            return Json(new AdminCategoryDAO().GetAll(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            return Json(new AdminCategoryDAO().Delete(id), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetbyID(int ID)
        {
            return Json(new AdminCategoryDAO().GetbyID(ID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadData( string name, int page, int pageSize)
        {
            return Json(new AdminCategoryDAO().LoadData( name, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Autocomplete(string name)
        {
            return Json(new AdminCategoryDAO().Autocomplete(name), JsonRequestBehavior.AllowGet);
        }
    }
}