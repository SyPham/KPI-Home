using KPI.Model.DAO;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System.Threading.Tasks;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class HomeController : BaseController
    {
        [BreadCrumb(Clear = true)]
        public ActionResult Index()
        {

            BreadCrumb.Add("/", "Home");
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
        public async Task<ActionResult> header()
        {
            var collection = await new NotificationDAO().NotifyCollection();
            ViewBag.NotifierEntity = new NotificationDAO().GetNotifierEntity();
            return PartialView(collection);
        }
      
        public JsonResult GetNotifications()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            var userprofile = Session["UserProfile"] as UserProfileVM;
            if (userprofile != null)
            {
                var listNotifications = new NotificationDAO().Notification(userprofile.User.ID, notificationRegisterTime);
                //update session here for get only new added contacts (notification)
                Session["LastUpdate"] = DateTime.Now;
                return Json(listNotifications, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNotificationContacts()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetContacts(notificationRegisterTime);
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}