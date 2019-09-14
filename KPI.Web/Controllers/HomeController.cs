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
            var userprofile = Session["UserProfile"] as UserProfileVM; 
            if(userprofile==null)
                return Json("", JsonRequestBehavior.AllowGet);
            var listNotifications = new NotificationsRepository().GetAllNotifications(userprofile.User.Username);
            var total = 0;
            var listID = new List<int>();
            foreach (var item in listNotifications)
            {
                if (item.Seen == false)
                {
                    total++;
                    listID.Add(item.ID);
                }
                   
            }
            return Json( new {arrayID = listID.ToArray(), total = total, data = listNotifications }, JsonRequestBehavior.AllowGet);
        }
       public ActionResult ListHistoryNotification()
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            if (userprofile == null)
                return Json("", JsonRequestBehavior.AllowGet);
            IEnumerable<NotificationViewModel> model = new NotificationDAO().GetHistoryNotification(userprofile.User.ID);
            return View(model);
        }
    }
}