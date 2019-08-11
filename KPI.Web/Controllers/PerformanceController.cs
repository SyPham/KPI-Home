using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.Controllers
{
    public class PerformanceController : BaseController
    {
        // GET: Performance
        public ActionResult Index()
        {
            return View();
        }
    }
}