using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.SqlServerNotifier;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class NotificationDAO
    {
        private KPIDbContext _dbContext = null;
        public NotificationDAO()
        {
            _dbContext = new KPIDbContext();
        }
        public bool UpdateRange(string listID)
        {
            if (listID == null) return false;
            if (listID.Length > 0)
            {
                var arr = listID.Split(',').Select(Int32.Parse).ToList();
                var some = _dbContext.Notifications.Where(x => arr.Contains(x.ID)).ToList();
                some.ForEach(a => a.Seen = true);
                try
                {
                    _dbContext.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;

                }
            }

            return false;
  
        }
        public bool Add(Notification entity)
        {
            _dbContext.Notifications.Add(entity);

            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public async Task<IEnumerable<Notification>> NotifyCollection() => await _dbContext.Notifications.ToListAsync();

        public object GetNotifierEntity() => _dbContext.GetNotifierEntity<Notification>(_dbContext.Notifications).ToJson();

        public object Notification(int userid, DateTime notificationRegisterTime)
        {

            var user = _dbContext.Users;
            var username = user.FirstOrDefault(x => x.ID == userid).Username.ToSafetyString();
            var notifications = _dbContext.Notifications
                .Where(x => x.Tag.Contains(username) && x.CreateTime > notificationRegisterTime)
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            var list = new List<NotificationViewModel>();
            var period = string.Empty;
            foreach (var item in notifications)
            {
                var notification = new NotificationViewModel();
                notification.Link = item.Link;

                switch (item.Period)
                {
                    case "W":
                        period = "Weekly"; break;
                    case "M":
                        period = "Monthly"; break;
                    case "Q":
                        period = "Quarterly"; break;
                    case "Y":
                        period = "Yearly"; break;
                    default:
                        period = "...";
                        break;
                }

                //notification.Content = '@' + user.FirstOrDefault(x => x.ID == item.UserID).Username + " mentioned you in a comment of " + item.KPIName + " - " + period;
                notification.Seen = item.Seen;
                list.Add(notification);
            }
            return new
            {
                data = list.ToList()
            };

        }
        public object GetNotification(int userid)
        {

            var user = _dbContext.Users;
            var username = user.FirstOrDefault(x => x.ID == userid).Username.ToSafetyString();
            var notifications = _dbContext.Notifications
                .Where(x => x.Tag.Contains(username))
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            var list = new List<NotificationViewModel>();
            var period = string.Empty;
            foreach (var item in notifications)
            {
                var notification = new NotificationViewModel();
                notification.Link = item.Link;

                switch (item.Period)
                {
                    case "W":
                        period = "Weekly"; break;
                    case "M":
                        period = "Monthly"; break;
                    case "Q":
                        period = "Quarterly"; break;
                    case "Y":
                        period = "Yearly"; break;
                    default:
                        period = "...";
                        break;
                }

                //notification.Content = '@' + user.FirstOrDefault(x => x.ID == item.UserID).Username + " mentioned you in a comment of " + item.KPIName + " - " + period;
                notification.Seen = item.Seen;
                list.Add(notification);
            }
            return new
            {
                total = list.Count,
                data = list.ToList()
            };

        }
        public List<NotificationViewModel> GetHistoryNotification(int userid)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.ID == userid);
            if (user == null) return new List<NotificationViewModel>();
            var model = from a in _dbContext.Notifications
                        join b in _dbContext.Users on a.UserID equals b.ID
                        where a.Tag.Contains(user.Username)
                        select new NotificationViewModel
                        {
                            ID = a.ID,
                            UserID = a.UserID,
                            KPIName = a.KPIName,
                            Link = a.Link,
                            Period = a.Period,
                            Seen = a.Seen,
                            CreateTime = a.CreateTime,
                            Tag = a.Tag,
                            Username = b.Username
                        };
            return model.ToList();
        }
        
    }
}
