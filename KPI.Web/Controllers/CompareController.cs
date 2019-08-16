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
    public class CompareController : BaseController
    {
        // GET: Compare
        [BreadCrumb(Clear = true)]
        public ActionResult Index(string obj)
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.Add("/KPI/Index", "KPI");
            BreadCrumb.SetLabel("Compare");
            if (obj == null)
                return View();
            var value = obj.Split(';')[1].Split(',');
            var standard = value[0].ToInt();
            var unit = value[1].ToString();
            var comp = obj.Split(';')[0].ToString();
            var compare = new DataChartDAO().Compare(comp);
            if (compare.list1 == null)
            {
                compare.list1 = new Model.ViewModel.ChartVM();
            }
            if (compare.list2 == null)
            {
                compare.list2 = new Model.ViewModel.ChartVM();
            }
            if (compare.list3 == null)
            {
                compare.list3 = new Model.ViewModel.ChartVM();
            }
            if (compare.list4 == null)
            {
                compare.list4 = new Model.ViewModel.ChartVM();
            }
            ViewBag.List1 = compare.list1;
            ViewBag.List2 = compare.list2;
            ViewBag.List3 = compare.list3;
            ViewBag.List4 = compare.list4;
            if (compare.Period == "W") ViewBag.PeriodText = "Weekly";
            if (compare.Period == "M") ViewBag.PeriodText = "Monthly";
            if (compare.Period == "Q") ViewBag.PeriodText = "Quarterly";
            if (compare.Period == "Y") ViewBag.PeriodText = "Yearly";
            ViewBag.Standard = standard;
            ViewBag.Unit = unit;
            return View();
        }
    }
}