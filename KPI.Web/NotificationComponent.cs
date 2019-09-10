using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using KPI.Model.EF;
using KPI.Model;
using KPI.Model.helpers;

namespace KPI.Web
{
    public class NotificationComponent
    {
        //Here we will add a function for register notification (will add sql dependency)
        public void RegisterNotification()
        {
            string conStr = ConfigurationManager.ConnectionStrings["KPIDbContext"].ConnectionString.ToSafetyString();
            //string sqlCommand = @"SELECT [ContactID],[ContactName],[ContactNo] from [dbo].[Contacts] where [AddedOn] > @AddedOn";
            string sql = @"SELECT [ID]
                          ,[UserID]
                          ,[KPIName]
                          ,[Period]
                          ,[Seen]
                          ,[Link]
                          ,[CreateTime]
                          ,[Tag]
                      FROM [dbo].[Notifications]";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                NotificationHub.SendNotifications();
            }
        }

        public List<Notification> GetContacts(DateTime afterDate)
        {
            using (KPIDbContext dc = new KPIDbContext())
            {
                return dc.Notifications.Where(a => a.CreateTime > afterDate).OrderByDescending(a => a.CreateTime).ToList();
            }
        }
    }
}