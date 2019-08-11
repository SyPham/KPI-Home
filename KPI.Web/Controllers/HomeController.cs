using KPI.Model.DAO;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KPI.Model.helpers;
using KPI.Model.ViewModel;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class HomeController : BaseController
    {
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {
          
            BreadCrumb.Add("/Home/Index", "Home");
            BreadCrumb.SetLabel("Dashboard");
            ViewBag.TotalUser = new UserAdminDAO().Total().ToInt();
            ViewBag.TotalKPI = new KPIAdminDAO().Total().ToInt();
            ViewBag.TotalLevel = new LevelDAO().Total().ToInt();
            ViewBag.TotalKPILevel = new KPILevelDAO().Total().ToInt();
            ViewBag.TotalCategory = new AdminCategoryDAO().Total().ToInt();
            return View();
        }
        public ActionResult UserDashBoard()
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            if (userprofile != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}