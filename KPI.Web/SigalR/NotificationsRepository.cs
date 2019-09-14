
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KPI.Web
{
    public class NotificationsRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["KPIDbContext"].ConnectionString;

        public IEnumerable<NotificationViewModel> GetAllNotifications(string Tag)
        {
            var messages = new List<NotificationViewModel>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sql = @"SELECT
                           dbo.Notifications.ID
						  ,dbo.Users.Username
                          ,UserID
                          ,KPIName
                          ,Period
                          ,Seen
                          ,Link
                          ,CreateTime
                          ,Tag
                FROM dbo.Notifications  
                INNER JOIN dbo.Users on dbo.Notifications.UserID = dbo.Users.ID
                WHERE Tag like @Tag";
                using (var command = new SqlCommand(sql, connection))
                {
                    var value = "%" + Tag + "%";
                    command.Parameters.AddWithValue("@Tag", value);
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        messages.Add(item: new NotificationViewModel { ID = reader["ID"].ToInt() , UserID=reader["UserID"].ToInt(),Username=reader["Username"].ToSafetyString(), KPIName = reader["KPIName"].ToSafetyString(), Period =  reader["Period"].ToSafetyString(), Seen = reader["Seen"].ToBool(), Link = reader["Link"].ToSafetyString(), CreateTime = Convert.ToDateTime(reader["CreateTime"]), Tag = reader["Tag"].ToSafetyString() });
                    }
                }
              
            }
            
            return messages;
        }
       
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                NotificationHub.SendNotifications();
            }
        }
    }
}