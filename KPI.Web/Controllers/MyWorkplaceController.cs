using KPI.Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    public class MyWorkplaceController : Controller
    {
        // GET: MyWorkplace
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadDataUser(int teamid, string code, int page, int pageSize)
        {
            return Json(new UserAdminDAO().LoadDataUser(teamid, code, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyWorkplace(int levelid, int page, int pageSize)
        {
            return Json(new UploadDAO().MyWorkplace(levelid,page,pageSize), JsonRequestBehavior.AllowGet);
        }
    }
}