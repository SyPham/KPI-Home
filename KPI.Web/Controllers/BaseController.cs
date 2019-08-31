using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KPI.Web.Controllers
{
    public class BaseController : Controller
    {
      
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userprofile = Session["UserProfile"] as UserProfileVM;
            //var username = Session["UserName"].ToSafetyString();
            if (userprofile == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action ="Index"}));
            }
            base.OnActionExecuting(filterContext);
        }

    }
} 