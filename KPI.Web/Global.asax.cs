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
        string con = ConfigurationManager.ConnectionStrings["KPIDbContext"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //here in Application Start we will start Sql Dependency
            //SqlDependency.Start(con);
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{

        //    NotificationHub.SendNotifications();
        //}
        protected void Application_End()
        {
            //here we will stop Sql Dependency
            //SqlDependency.Stop(con);
        }
    }

}

