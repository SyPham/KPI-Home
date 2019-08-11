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
    public class AdminUserController : BaseController
    {
        // GET: Account
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.SetLabel("User");
            return View();
        }
        public JsonResult Add(Model.EF.User entity)
        {
            return Json(new UserAdminDAO().Add(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAll()
        {
            return Json(new UserAdminDAO().GetAll(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadData(string search, int page, int pageSize)
        {
            return Json(new UserAdminDAO().LoadData(search, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            return Json(new UserAdminDAO().Delete(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Model.EF.User entity)
        {
            return Json(new UserAdminDAO().Update(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            return Json(new UserAdminDAO().GetbyID(ID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LockUser(int ID)
        {
            return Json(new UserAdminDAO().LockUser(ID), JsonRequestBehavior.AllowGet);
        }
        //update kpiLevel
        public JsonResult UpdateKPILevel(Model.EF.KPILevel entity)
        {
            return Json(new KPILevelDAO().Update(entity), JsonRequestBehavior.AllowGet);
        }
        //get all kpilevel
        public JsonResult GetAllKPILevel()
        {
            return Json(new KPILevelDAO().GetAll(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListAllPermissions(int userid)
        {
            return Json(new UserAdminDAO().GetListAllPermissions(userid), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllMenus()
        {
            return Json(new UserAdminDAO().GetAllMenus(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Checkpermisson(int userid)
        {
            return Json(new UserAdminDAO().Checkpermisson(userid), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangePassword(string username,string password)
        {
            return Json(new UserAdminDAO().ChangePassword(username, password), JsonRequestBehavior.AllowGet);
        }
    }
}