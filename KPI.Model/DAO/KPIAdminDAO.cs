using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class KPIAdminDAO
    {
        KPIDbContext _dbContext = null;

        public KPIAdminDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public bool Add(EF.KPI entity)
        {
            try
            {
                for (int i = 1; i < 10000; i++)
                {
                    string code = i.ToString("D4");
                    if (_dbContext.KPIs.FirstOrDefault(x => x.Code == code) == null)
                    {
                        entity.Code = code;
                        break;
                    }
                }
                _dbContext.KPIs.Add(entity);
                _dbContext.SaveChanges();

                List<EF.KPILevel> kpiLevelList = new List<EF.KPILevel>();
                var levels = _dbContext.Levels.ToList();
                foreach (var level in levels)
                {
                    var kpilevel = new EF.KPILevel();
                    kpilevel.LevelID = level.ID;
                    kpilevel.KPIID = entity.ID;
                    kpiLevelList.Add(kpilevel);
                }
                _dbContext.KPILevels.AddRange(kpiLevelList);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool AddKPILevel(EF.KPILevel entity)
        {
            _dbContext.KPILevels.Add(entity);
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
        public int Total()
        {
            return _dbContext.KPIs.ToList().Count();
        }
        public bool Update(EF.KPI entity)
        {
            entity.Code = entity.Code.ToSafetyString().ToUpper();
            try
            {
                var iteam = _dbContext.KPIs.FirstOrDefault(x => x.ID == entity.ID);
                iteam.Name = entity.Name;
                iteam.Code = entity.Code;
                iteam.LevelID = entity.LevelID;
                iteam.CategoryID = entity.CategoryID;
                iteam.Unit = entity.Unit;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //logging
                return false;
            }

        }
        public List<EF.Category> GetCategoryCode()
        {
            return _dbContext.Categories.ToList();
        }
        public bool Delete(int id)
        {

            try
            {
                var kpi = _dbContext.KPIs.Find(id);
                _dbContext.KPIs.Remove(kpi);

                var kpiLevel = _dbContext.KPILevels.Where(x=>x.KPIID==id).ToList();
                _dbContext.KPILevels.RemoveRange(kpiLevel);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }
        public object GetAll()
        {
            return _dbContext.KPIs.Select(x=>new {
                x.ID,
                x.Code,
                x.Name,
                x.LevelID,
                CategoryName = _dbContext.Categories.FirstOrDefault(a=>a.ID==x.CategoryID),
                Unit = _dbContext.Units.FirstOrDefault(u=>u.ID==x.Unit)

            }).ToList();
        }
        public EF.KPI GetbyID(int ID)
        {
            return _dbContext.KPIs.FirstOrDefault(x => x.ID == ID);
        }
        public List<Unit> GetAllUnit()
        {
            return _dbContext.Units.ToList();
        }
        public object ListCategory()
        {
            return _dbContext.Categories.ToList();
        }

        public object LoadData(int? categoryID, string name, int page, int pageSize = 3)
        {
            categoryID = categoryID.ToInt();
            name = name.ToSafetyString();
            var model = _dbContext.KPIs.Select(
                x => new KPIVM
                {
                    ID = x.ID,
                    Name = x.Name,
                    Code = x.Code,
                    LevelID = x.LevelID,
                    CategoryID = x.CategoryID,
                    CategoryName = _dbContext.Categories.FirstOrDefault(c => c.ID == x.CategoryID).Name,
                    Unit = _dbContext.Units.FirstOrDefault(u => u.ID == x.Unit).Name,
                    CreateTime = x.CreateTime
                }
                ).ToList();
            if (!string.IsNullOrEmpty(name))
            {
                model = model.Where(x => x.Name.Contains(name)).ToList();
            }

            if (categoryID != 0)
            {
                model = model.Where(x => x.CategoryID == categoryID).ToList();
            }
            int totalRow = model.Count();

            model = model.OrderByDescending(x => x.CreateTime)
              .Skip((page - 1) * pageSize)
              .Take(pageSize).ToList();
            return new
            {
                data = model,
                total = totalRow,
                status = true
            };
        }

        public object Autocomplete(string search)
        {
            if (search != "")
                return _dbContext.KPIs.Where(x => x.Name.Contains(search)).Select(x => x.Name).Take(5).ToList();
            else
                return "";
        }
    }
}
