using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    [BreadCrumb(Clear = true)]
    public class ChartPeriodController : BaseController
    {
        // GET: Month
        [BreadCrumb(Clear = true)]
        public ActionResult Index(string kpilevelcode, string period, int? year, int? start, int? end)
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.Add("/KPI/Index", "KPI");
            if (period == "W")
            {
                BreadCrumb.SetLabel("Chart / Weekly");
            }
            else if (period == "M")
            {
                BreadCrumb.SetLabel("Chart / Monthly");
            }
            else if (period == "Q")
            {
                BreadCrumb.SetLabel("Chart / Quarterly");
            }
            else if (period == "Y")
            {
                BreadCrumb.SetLabel("Chart / Yearly");
            }


            var model = new DataChartDAO().ListDatas(kpilevelcode, period, year, start, end);
            ViewBag.Datasets = model.datasets;
            ViewBag.Labels = model.labels;
            ViewBag.Targets = model.targets;
            ViewBag.Standards = model.standards;
            ViewBag.Label = model.label;
            ViewBag.KPIName = model.kpiname;
            ViewBag.Period = model.period;

            ViewBag.KPILevelCode = model.kpilevelcode;
            ViewBag.StatusFavorite = model.statusfavorite == true ? "true" : "false";
            ViewBag.Standard = model.Standard;
            ViewBag.Unit = model.Unit;
            ViewBag.Dataremarks = model.Dataremarks;

            if (model.period == "W") { ViewBag.PeriodText = "Weekly"; };
            if (model.period == "M") { ViewBag.PeriodText = "Monthly"; };
            if (model.period == "Q") { ViewBag.PeriodText = "Quarterly"; };
            if (model.period == "Y") { ViewBag.PeriodText = "Yearly"; };
            return View();
        }
        public JsonResult GetAllComments(int dataid)
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            var userid = 0;
            if (userprofile != null)
            {
                userid = userprofile.User.ID;
            }
            var data = new CommentRepository().GetAllComments(userid, dataid);
            var total = data.Count();
            return Json(new
            {
                data = data,
                total = total
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddComment(AddCommentViewModel entity)
        {
            var data = new KPILevelDAO().AddComment(entity);
            if (data.Status)
            {
                if (data.ListEmails.Count > 0)
                {

                    foreach (var item in data.ListEmails)
                    {
                        string content = item[0] + "mentioned you in KPI System Apps. Content: " + item[4] + ". " + item[3] + " Link: " + item[2];
                        var html = string.Empty;
                        var sessionUser = Session["UserProfile"] as UserProfileVM;
                        string from = ConfigurationManager.AppSettings["FromEmailAddress"].ToSafetyString();
                        string password = ConfigurationManager.AppSettings["FromEmailPassword"].ToSafetyString();
                        string to = item[1].ToSafetyString();
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

                                smtp.Host = clientHost;
                                smtp.UseDefaultCredentials = true;
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
                }
            }
            return Json(new { status = false, isSendmail = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDataComment(int dataid, int userid)
        {
            return Json(new KPILevelDAO().ListComments(dataid, userid), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddCommentHistory(int userid, int dataid)
        {
            return Json(new KPILevelDAO().AddCommentHistory(userid, dataid), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Remark(int dataid)
        {
            return Json(new DataChartDAO().Remark(dataid), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddFavourite(Model.EF.Favourite entity)
        {
            return Json(new FavouriteDAO().Add(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDataProvide(string obj, int page, int pageSize)
        {
            return Json(new KPILevelDAO().LoadDataProvide(obj, page, pageSize), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateRemark(int dataid, string remark)
        {
            return Json(new DataChartDAO().UpdateRemark(dataid, remark), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(ActionPlan item)
        {
            return Json(new ActionPlanDAO().Update(item), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Add(ActionPlanViewModel2 obj)
        {
            var item = new ActionPlan();
            item.Title = obj.Title;
            item.Description = obj.Description;
            item.KPILevelCodeAndPeriod = obj.KPILevelCodeAndPeriod;
            item.Tag = obj.Tag;
            item.UserID = obj.UserID;
            item.DataID = obj.DataID;
            item.CommentID = obj.CommentID;
            item.Link = obj.Link;
            item.SubmitDate = obj.SubmitDate.ToDateTime();
            item.Deadline = obj.Deadline.ToDateTime();

            var data = new ActionPlanDAO().Add(item, obj.Subject);
            if (data.Status)
            {
                if (data.ListEmails.Count > 0)
                {

                    foreach (var item2 in data.ListEmails)
                    {
                        string content = item2[0] + "mentioned you in KPI System Apps. Content: " + item2[4] + ". " + item2[3] + " Link: " + item2[2];
                        var html = string.Empty;
                        var sessionUser = Session["UserProfile"] as UserProfileVM;
                        string from = ConfigurationManager.AppSettings["FromEmailAddress"].ToSafetyString();
                        string password = ConfigurationManager.AppSettings["FromEmailPassword"].ToSafetyString();
                        string to = item2[1].ToSafetyString();
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
                }
            }
            return Json(new { status = false, isSendmail = false }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Delete(int id)
        {
            return Json(new ActionPlanDAO().Delete(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAll(int DataID, int CommentID, int UserID)
        {
            return Json(new ActionPlanDAO().GetAll(DataID, CommentID, UserID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetByID(int id)
        {
            return Json(new ActionPlanDAO().GetByID(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Approval(int id, int approveby)
        {
            return Json(new ActionPlanDAO().Approval(id, approveby), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Done(int id)
        {
            return Json(new ActionPlanDAO().Done(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddNotification(Notification notification)
        {
            var status = new NotificationDAO().Add(notification);
            NotificationHub.SendNotifications();
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateActionPlan(UpdateActionPlanVM actionPlan)
        {
            return Json(new ActionPlanDAO().UpdateActionPlan(actionPlan), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(string name,string value, string pk)
        {
            return Json(new ActionPlanDAO().Update(name,value,pk), JsonRequestBehavior.AllowGet);
        }

    }
}