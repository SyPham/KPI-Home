using KPI.Model.EF;
using System;
using System.Collections.Generic;
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

        public object Notification(int userid, DateTime currentime)
        {
            var model = _dbContext.Notifications
                .Where(x => x.UserID == userid && x.CreateTime > currentime)
                .OrderByDescending(x=>x.CreateTime)
                .ToList();
            return model;
        }
    }
}
