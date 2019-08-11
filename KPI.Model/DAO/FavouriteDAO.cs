using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.helpers;
using KPI.Model.ViewModel;

namespace KPI.Model.DAO
{
    public class FavouriteDAO
    {
        KPIDbContext _dbContext = null;
        public FavouriteDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public bool Add(EF.Favourite entity)
        {
            try
            {
               
                _dbContext.Favourites.Add(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var item = _dbContext.Favourites.FirstOrDefault(x => x.ID == id);
                _dbContext.Favourites.Remove(item);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }
        public object LoadData(int userid, int page, int pageSize)
        {

            try
            {

                var model = _dbContext.Favourites
               .Where(x => x.UserID == userid)
               .Select(x => new FavouriteVM
               {
                   KPIName = _dbContext.KPIs.FirstOrDefault(k => k.ID == _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == x.KPILevelCode).KPIID).Name,
                   Username = _dbContext.Users.FirstOrDefault(u => u.ID == x.UserID).Username,
                   TeamName = _dbContext.Levels.FirstOrDefault(l => l.ID == _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == x.KPILevelCode).LevelID).Name,
                   Level = _dbContext.KPIs.FirstOrDefault(k => k.ID == _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == x.KPILevelCode).KPIID).LevelID,
                   CreateTime = x.CreateTime,
                   KPILevelCode = x.KPILevelCode,
                   Period = x.Period,
                   ID=x.ID
               })
               .Distinct()
               .OrderByDescending(x => x.CreateTime)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToList();
                int totalRow = model.Count();
                return new
                {
                    status = true,
                    data = model,
                    total = totalRow
                };

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }

        }
    }
}
