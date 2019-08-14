using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.helpers;
using KPI.Model.EF;

namespace KPI.Model.DAO
{
    public class LevelDAO
    {
        private KPIDbContext _dbContext = null;
        public LevelDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public bool AddOrUpdate(EF.Level entity)
        {
            entity.Code = entity.Code.ToUpper();
            List<EF.KPILevel> kpiLevelList = new List<EF.KPILevel>();
            if (entity.ID == 0)
            {
                if (_dbContext.Levels.FirstOrDefault(x => x.Code == entity.Code) != null)
                {
                    return false;
                }
                try
                {
                    var level = new EF.Level()
                    {
                        Name = entity.Name,
                        Code = entity.Code,
                        LevelNumber = entity.LevelNumber,
                        ParentCode = entity.ParentCode,
                        ParentID = entity.ParentID
                    };
                    _dbContext.Levels.Add(level);
                    _dbContext.SaveChanges();

                    IEnumerable<KPIViewModel> kpiVM = from kpi in _dbContext.KPIs
                                                      join cat in _dbContext.Categories on kpi.CategoryID equals cat.ID
                                                      select new KPIViewModel
                                                      {
                                                          KPIID = kpi.ID,
                                                      };
                    foreach (var kpi in kpiVM)
                    {
                        var kpilevel = new EF.KPILevel();
                        kpilevel.LevelID = level.ID;
                        kpilevel.KPIID = kpi.KPIID;
                        kpiLevelList.Add(kpilevel);
                    }
                    try
                    {
                        _dbContext.KPILevels.AddRange(kpiLevelList);
                        _dbContext.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return false;
                }

            }
            else
            {
                try
                {
                    var item = _dbContext.Levels.FirstOrDefault(x => x.ID == entity.ID);
                    item.Code = entity.Code;
                    item.Name = entity.Name;
                    item.LevelNumber = entity.LevelNumber;
                    item.ParentID = entity.ParentID;
                    _dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return false;
                }
            }
        }

        public EF.Level GetByID(int id)
        {
            return _dbContext.Levels.FirstOrDefault(x => x.ID == id);

        }
        public KPITreeViewModel GetListTreeForWorkplace(int userid)
        {
            var levels = new List<ViewModel.KPITreeViewModel>();
            List<ViewModel.KPITreeViewModel> hierarchy = new List<ViewModel.KPITreeViewModel>();

            var listLevels = _dbContext.Levels.OrderBy(x => x.LevelNumber).ToList();

            var user = _dbContext.Users.FirstOrDefault(x => x.ID == userid);


            var levelNumber = _dbContext.Levels.FirstOrDefault(x => x.ID == user.LevelID);

            if (levelNumber == null)
            {
                return new KPITreeViewModel();
            }

            listLevels = listLevels.Where(x => x.LevelNumber >= levelNumber.LevelNumber).OrderBy(x => x.LevelNumber).ToList();
            foreach (var item in listLevels)
            {
                var levelItem = new ViewModel.KPITreeViewModel();
                levelItem.key = item.ID;
                levelItem.title = item.Name;
                levelItem.code = item.Code;
                levelItem.state = item.State;
                levelItem.levelnumber = item.LevelNumber;
                levelItem.parentid = item.ParentID;
                levels.Add(levelItem);
            }
            var itemLevel = _dbContext.Levels.FirstOrDefault(x => x.ID == user.LevelID);
            hierarchy = levels.Where(c => c.parentid == itemLevel.ParentID)
                       .Select(c => new ViewModel.KPITreeViewModel()
                       {
                           key = c.key,
                           title = c.title,
                           code = c.code,
                           levelnumber = c.levelnumber,
                           parentid = c.parentid,
                           children = GetChildren(levels, c.key)
                       })
                       .ToList();

            HieararchyWalk(hierarchy);
            var obj = new KPITreeViewModel();
            foreach (var item in hierarchy)
            {
                if (item.key == itemLevel.ID)
                {
                    obj = item;
                    break;
                }
            }
            var model = hierarchy.Where(x => x.key == itemLevel.ID).FirstOrDefault();
            return model;
        }
        public object GetListTreeClient(int userid)
        {
            var levels = new List<ViewModel.KPITreeViewModel>();
            List<ViewModel.KPITreeViewModel> hierarchy = new List<ViewModel.KPITreeViewModel>();

            var listLevels = _dbContext.Levels.OrderBy(x => x.LevelNumber).ToList();

            var user = _dbContext.Users.FirstOrDefault(x=>x.ID==userid);


            var levelNumber = _dbContext.Levels.FirstOrDefault(x => x.ID == user.LevelID);

            if (levelNumber == null)
            {
                return new List<KPITreeViewModel>();
            }

                listLevels = listLevels.Where(x => x.LevelNumber >= levelNumber.LevelNumber).OrderBy(x => x.LevelNumber).ToList();
                foreach (var item in listLevels)
                {
                    var levelItem = new ViewModel.KPITreeViewModel();
                    levelItem.key = item.ID;
                    levelItem.title = item.Name;
                    levelItem.code = item.Code;
                    levelItem.state = item.State;
                    levelItem.levelnumber = item.LevelNumber;
                    levelItem.parentid = item.ParentID;
                    levels.Add(levelItem);
                }
                var itemLevel = _dbContext.Levels.FirstOrDefault(x => x.ID == user.LevelID);
                hierarchy = levels.Where(c => c.parentid == itemLevel.ParentID)
                           .Select(c => new ViewModel.KPITreeViewModel()
                           {
                               key = c.key,
                               title = c.title,
                               code = c.code,
                               levelnumber = c.levelnumber,
                               parentid = c.parentid,
                               children = GetChildren(levels, c.key)
                           })
                           .ToList();

                HieararchyWalk(hierarchy);
                var obj = new KPITreeViewModel();
                foreach (var item in hierarchy)
                {
                    if (item.key == itemLevel.ID)
                    {
                        obj = item;
                        break;
                    }
                }
                var model = hierarchy.Where(x => x.key == itemLevel.ID).ToList();
                return model;
        }
        public List<ViewModel.KPITreeViewModel> GetListTree()
        {
            var listLevels = _dbContext.Levels.OrderBy(x => x.LevelNumber).ToList();
            var levels = new List<ViewModel.KPITreeViewModel>();
            foreach (var item in listLevels)
            {
                var levelItem = new ViewModel.KPITreeViewModel();
                levelItem.key = item.ID;
                levelItem.title = item.Name;
                levelItem.code = item.Code;
                levelItem.state = item.State;
                levelItem.levelnumber = item.LevelNumber;
                levelItem.parentid = item.ParentID;
                levels.Add(levelItem);
            }

            List<ViewModel.KPITreeViewModel> hierarchy = new List<ViewModel.KPITreeViewModel>();

            hierarchy = levels.Where(c => c.parentid == 0)
                            .Select(c => new ViewModel.KPITreeViewModel()
                            {
                                key = c.key,
                                title = c.title,
                                code = c.code,
                                levelnumber = c.levelnumber,
                                parentid = c.parentid,
                                children = GetChildren(levels, c.key)
                            })
                            .ToList();


            HieararchyWalk(hierarchy);

            return hierarchy;
        }
        private List<ViewModel.KPITreeViewModel> GetChildren(List<ViewModel.KPITreeViewModel> levels, int parentid)
        {
            return levels
                    .Where(c => c.parentid == parentid)
                    .Select(c => new ViewModel.KPITreeViewModel()
                    {
                        key = c.key,
                        title = c.title,
                        code = c.code,
                        levelnumber = c.levelnumber,
                        parentid = c.parentid,
                        children = GetChildren(levels, c.key)
                    })
                    .ToList();
        }
        public int Total()
        {
            return _dbContext.Levels.ToList().Count();
        }
        private void HieararchyWalk(List<ViewModel.KPITreeViewModel> hierarchy)
        {
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    //Console.WriteLine(string.Format("{0} {1}", item.Id, item.Text));
                    HieararchyWalk(item.children);
                }
            }
        }
        public bool Update(int key, string title, string code)
        {
            var item = _dbContext.Levels.FirstOrDefault(x => x.ID == key);
            if(item==null)
            return false;
            else
            {
                item.Code = code;
                item.Name = title;
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
        }
    }
}
