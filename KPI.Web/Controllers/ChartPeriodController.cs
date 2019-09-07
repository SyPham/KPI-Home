using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using MvcBreadCrumbs;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
        }

        public JsonResult AddComment(Model.EF.Comment entity)
        {
            return Json(new KPILevelDAO().AddComment(entity), JsonRequestBehavior.AllowGet);
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
        public ActionResult Compare()
        {
            return View();
        }
        public JsonResult UserSendMail(int userid)
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadToDo()
        {
            return Json("", JsonRequestBehavior.AllowGet);
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
            item.SubmitDate = obj.SubmitDate.ToDateTime();
            item.Deadline = obj.Deadline.ToDateTime();
            return Json(new ActionPlanDAO().Add(item), JsonRequestBehavior.AllowGet);
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
            return Json(new NotificationDAO().Add(notification), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult Notification(int userid)
        //{
        //    return Json(new NotificationDAO().Notification(userid), JsonRequestBehavior.AllowGet);
        //}

    }
}