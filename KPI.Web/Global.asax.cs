using KPI.Model;
using KPI.Model.DAO;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KPI.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected String SqlConnectionString { get; set; }
        protected void Application_Start()
        {
            using (var context = new KPIDbContext())
                SqlConnectionString = context.Database.Connection.ConnectionString;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            if (!String.IsNullOrEmpty(SqlConnectionString))
                SqlDependency.Start(SqlConnectionString);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            NotificationDAO NC = new NotificationDAO();
            var currentTime = DateTime.Now;
            HttpContext.Current.Session["LastUpdated"] = currentTime;
            var userprofile = Session["UserProfile"] as UserProfileVM;
            if (userprofile == null)
            {
               var model= NC.Notification(userprofile.User.ID,currentTime);
            }
           
        }
        protected void Application_End()
        {
            if (!String.IsNullOrEmpty(SqlConnectionString))
                SqlDependency.Start(SqlConnectionString);
        }
    }

}

