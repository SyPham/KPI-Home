using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class AdminCategoryDAO
    {
        KPIDbContext _dbContext = null;

        public AdminCategoryDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public bool Add(EF.Category entity)
        {
            entity.Code = entity.Code.ToUpper();

            try
            {
                _dbContext.Categories.Add(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }

        public int Total()
        {
            return _dbContext.Categories.Count();
        }
        public bool Update(EF.Category entity)
        {
            entity.Code = entity.Code.ToUpper();
            try
            {
                var iteam = _dbContext.Categories.FirstOrDefault(x => x.ID == entity.ID);
                iteam.Name = entity.Name;
                iteam.Code = entity.Code;
                iteam.LevelID = entity.LevelID;
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
                var category = _dbContext.Categories.Find(id);
                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }
        public IEnumerable<EF.Category> GetAll()
        {
            return _dbContext.Categories.ToList();
        }
        public EF.Category GetbyID(int ID)
        {
            return _dbContext.Categories.FirstOrDefault(x => x.ID == ID);
        }
        public object ListCategory()
        {
            return _dbContext.Categories.ToList();
        }

        public object LoadData(string name, int page, int pageSize = 3)
        {
            name = name.ToSafetyString();
            var model = _dbContext.Categories.ToList();
            if (!string.IsNullOrEmpty(name))
            {
                model = model.Where(x => x.Name.Contains(name)).ToList();
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
                return _dbContext.Categories.Where(x => x.Name.Contains(search)).Select(x => x.Name).Take(5).ToList();
            else
                return "";
        }
    }
}
