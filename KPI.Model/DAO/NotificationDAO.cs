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
                var some = _dbContext.NotificationDetails.Where(x => arr.Contains(x.ID)).ToList();
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
        public object Update(int ID)
        {
            var some = _dbContext.NotificationDetails.FirstOrDefault(x => x.ID == ID);
            var user = _dbContext.Users;
            try
            {
                some.Seen = true;
                _dbContext.SaveChanges();
                //var detail = _dbContext.Notifications.FirstOrDefault(x => x.ID == some.NotificationID);
              
                var detail = _dbContext.Notifications.FirstOrDefault(x => x.ID == some.NotificationID);
                var tag = _dbContext.Tags.Where(x => x.CommentID == detail.CommentID).Select(x => x.UserID).ToList();
                var listArr = string.Empty;
                if (tag.Count > 0)
                {
                    listArr = string.Join(",", user.Where(x => tag.Contains(x.ID)).Select(x => x.FullName).ToArray()); ;
                }
                var vm = new NotificationViewModel();
                vm.ID = detail.ID;
                vm.Title = detail.Title;
                vm.CreateTime = detail.CreateTime;
                vm.UserID = detail.UserID;
                vm.Link = detail.Link;
                vm.Tag = listArr;
                vm.FullNameBy = user.Find(detail.UserID).FullName;
                vm.FullName = listArr;
                vm.Content = detail.Content;
                vm.Title = detail.Title;
                if (detail.ActionplanID > 0)
                {
                    vm.DueDate = _dbContext.ActionPlans.FirstOrDefault(x => x.ID == detail.ActionplanID).Deadline.ToString("MM/dd/yyyy");
                }

                return new { status = true, data = vm };

            }
            catch (Exception)
            {
                return new { status = false, data = "" }; ;
            }

        }
        public bool Add(Notification entity)
        {
            try
            {
                entity.CreateTime = DateTime.Now;
                _dbContext.Notifications.Add(entity);

                _dbContext.SaveChanges();

                

                var listUserID = new List<int>();
                var listDetails = new List<NotificationDetail>();
                var user = _dbContext.Users;
                var tag = _dbContext.Tags;
                //Neu thong bao cua comment thi vao bang tag lay tat ca user dc tag ra
                if (entity.CommentID > 0)
                {
                    listUserID.AddRange(tag.Where(x => x.CommentID == entity.CommentID).Select(x => x.UserID).ToArray());
                }

                if (entity.ActionplanID > 0)
                {
                    listUserID.AddRange(tag.Where(x => x.ActionPlanID == entity.ActionplanID).Select(x => x.UserID).ToArray());
                }
                var detail2 = new NotificationDetail();
                detail2.UserID = entity.UserID;
                detail2.Seen = false;
                detail2.NotificationID = entity.ID;
                listDetails.Add(detail2);

                foreach (var item in listUserID)
                {
                    
                    var detail = new NotificationDetail();
                    detail.UserID = item;
                    detail.Seen = false;
                    detail.NotificationID = entity.ID;
                    listDetails.Add(detail);
                }
                  

                _dbContext.NotificationDetails.AddRange(listDetails);
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

        public List<NotificationViewModel> ListNotifications(int userid)
        {
            var user = _dbContext.Users;
            var tag = _dbContext.Tags;
            var model = from a in _dbContext.Notifications
                        join b in _dbContext.NotificationDetails on a.ID equals b.NotificationID
                        where b.UserID == userid
                        join c in _dbContext.Users on b.UserID equals c.ID
                        join d in _dbContext.Users on a.UserID equals d.ID
                        select new NotificationViewModel
                        {
                            ID = b.ID,
                            Title = a.Title,
                            KPIName = a.KPIName,
                            Period = a.Period,
                            CreateTime = b.CreateTime,
                            UserID = a.UserID,
                            Username = c.Username,
                            Link = a.Link,
                            Seen = b.Seen,
                            Tag = a.Tag,
                            UsernameBy = d.Username,
                            FullNameBy = d.FullName,
                            FullName = c.FullName,

                            Content = a.Content
                        };
            var model1 = model.OrderByDescending(x=>x.CreateTime).ToList();
            return model1;
        }
        public List<NotificationViewModel> ListNotifications2(int userid)
        {
            var detail = _dbContext.NotificationDetails;
            var model =
                        from b in _dbContext.Notifications
                        join a in _dbContext.Users on b.UserID equals a.ID
                        select new NotificationViewModel
                        {
                            ID = a.ID,
                            Title = b.Title,
                            KPIName = b.KPIName,
                            Period = b.Period,
                            CreateTime = b.CreateTime,
                            UserID = b.UserID,
                            Username = a.Username,
                            Link = b.Link,
                            Seen = detail.FirstOrDefault(x => x.UserID == userid && x.NotificationID == b.ID).Seen,
                            Tag = b.Tag
                        };
            var model1 = model.Where(x => x.UserID == userid).ToList();
            return model1;
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
            //var user = _dbContext.Users.FirstOrDefault(x => x.ID == userid);
            //if (user == null) return new List<NotificationViewModel>();
            //var model = from a in _dbContext.Notifications
            //            join b in _dbContext.Users on a.UserID equals b.ID
            //            where a.Tag.Contains(user.Username)
            //            select new NotificationViewModel
            //            {
            //                ID = a.ID,
            //                UserID = a.UserID,
            //                KPIName = a.KPIName,
            //                Link = a.Link,
            //                Period = a.Period,
            //                Seen = a.Seen,
            //                CreateTime = a.CreateTime,
            //                Tag = a.Tag,
            //                Title = a.Title,
            //                Username = b.Username
            //            };
            //return model.ToList();

            var user = _dbContext.Users;
            var tag = _dbContext.Tags;
            var model = from a in _dbContext.Notifications
                        join b in _dbContext.NotificationDetails on a.ID equals b.NotificationID
                        where b.UserID == userid
                        join c in _dbContext.Users on b.UserID equals c.ID
                        join d in _dbContext.Users on a.UserID equals d.ID
                        select new NotificationViewModel
                        {
                            ID = b.ID,
                            Title = a.Title,
                            KPIName = a.KPIName,
                            Period = a.Period,
                            CreateTime = b.CreateTime,
                            UserID = a.UserID,
                            Username = c.Username,
                            Link = a.Link,
                            Seen = b.Seen,
                            Tag = a.Tag,
                            UsernameBy = d.Username,
                            FullNameBy = d.FullName,
                            FullName = c.FullName,

                            Content = a.Content
                        };
            var model1 = model.OrderByDescending(x => x.CreateTime).ToList();
            return model1;
        }

    }
}
