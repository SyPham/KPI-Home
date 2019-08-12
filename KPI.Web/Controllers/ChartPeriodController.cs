using KPI.Model.DAO;
using KPI.Model.helpers;
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
            ViewBag.Label = model.label;
            ViewBag.KPIName = model.kpiname;
            ViewBag.Period = model.period;
            ViewBag.KPILevelCode = model.kpilevelcode;
            ViewBag.StatusFavorite = model.statusfavorite == true ? "true" : "false";
            ViewBag.Url = model.url;
            ViewBag.Dataremarks = model.Dataremarks;
            return View();
        }

        public JsonResult AddComment(Model.EF.Comment entity)
        {
            var value = entity.KPILevelCode;
            entity.KPILevelCode = value.Substring(0, value.Length - 1);
            entity.Period = value.Substring(value.Length - 1, 1).ToUpper();
            return Json(new KPILevelDAO().AddComment(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDataComment(string kpilevelcode)
        {
            return Json(new KPILevelDAO().ListComments(kpilevelcode), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Remark(int dataid)
        {
            return Json(new DataChartDAO().Remark(dataid), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddFavourite(Model.EF.Favourite entity)
        {
            return Json(new FavouriteDAO().Add(entity), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDataProvide(string obj)
        {
            return Json(new KPILevelDAO().LoadDataProvide(obj), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateRemark(int dataid, string remark)
        {
            return Json(new DataChartDAO().UpdateRemark(dataid, remark), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Compare()
        {
            return View();
        }
    }
}