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
    public class CompareController : BaseController
    {
        // GET: Compare
        [BreadCrumb(Clear = true)]
        public ActionResult Index(string obj)
        {
            BreadCrumb.Add(Url.Action("Index", "Home"), "Home");
            BreadCrumb.Add("/KPI/Index", "KPI");
            BreadCrumb.SetLabel("Compare");
            var compare = new DataChartDAO().Compare(obj);
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
            return View();
        }
    }
}