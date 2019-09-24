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
using System.Configuration;
using System.Net.Mail;
using KPI.Model.EF;

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


           var model= new ActionPlanDAO().CheckDeadline();
            foreach (var item in model)
            {
                string content = "Please note that the action plan we going to deadline on "+item.Deadline;
                var html = string.Empty;
             
                string from = ConfigurationManager.AppSettings["FromEmailAddress"].ToSafetyString();
                string password = ConfigurationManager.AppSettings["FromEmailPassword"].ToSafetyString();
                string to = item.Email.ToSafetyString();
                string clientHost = ConfigurationManager.AppSettings["ClientHost"].ToSafetyString();
                string subject = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToSafetyString();
                MailMessage mail = new MailMessage();
                mail.To.Add(to.ToString());
                mail.From = new MailAddress(from, "KPI.App");
                mail.Subject = subject;
                mail.Body = content;
                mail.IsBodyHtml = false;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.Priority = MailPriority.High;

                try
                {
                    using (var smtp = new SmtpClient())
                    {
                        smtp.UseDefaultCredentials = true;
                        smtp.Host = clientHost;
                        smtp.Send(mail);
                    }
                    return Json(new { status = true, isSendmail = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    var a = new ErrorMessage();
                    a.Name = ex.Message;
                    new ErrorMessageDAO().Add(a);
                    return Json(new { status = true, isSendmail = false }, JsonRequestBehavior.AllowGet);

                }
            }
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

        public JsonResult GetNotifications(int? userID)
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            if(userID == null)
                return Json("", JsonRequestBehavior.AllowGet);
            var listNotifications = new NotificationDAO().ListNotifications(userID.Value);
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