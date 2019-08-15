using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KPI.Model.DAO
{
    public class UploadDAO
    {
        public KPIDbContext _dbContext = null;
        public UploadDAO() => _dbContext = new KPIDbContext();
        public bool AddRange1(List<EF.Data> entity)
        {
            foreach (var item in entity)
            {
                var itemcode = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode);
                if (itemcode == null)
                {
                    try
                    {
                        _dbContext.Datas.Add(item);
                        _dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }
                else if (item.Week > 0 && itemcode.Week == item.Week)
                {
                    itemcode.Week = item.Week;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }
                else if (item.Month > 0 && itemcode.Month == item.Month)
                {
                    itemcode.Month = item.Month;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }
                else if (item.Quarter > 0 && itemcode.Quarter == item.Quarter)
                {
                    itemcode.Quarter = item.Quarter;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }
                else if (item.Year > 0 && itemcode.Year == item.Year)
                {
                    itemcode.Year = item.Year;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }

            }
            return true;
        }

        public bool Add(List<ViewModel.UploadDataVM> entity)
        {
            try
            {
                foreach (var item in entity)
                {
                    var value = item.KPILevelCode;
                    var code = value.Substring(0, value.Length - 1);
                    var kind = value.Substring(value.Length - 1, 1);

                    var updateW = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.Period == kind && x.Week == item.PeriodValue);
                    var updateM = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.Period == kind && x.Month == item.PeriodValue);
                    var updateQ = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.Period == kind && x.Quarter == item.PeriodValue);
                    var updateY = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.Period == kind && x.Year == item.PeriodValue);

                    if (kind == "W" && updateW == null)
                    {
                        var dataW = new Data();
                        dataW.KPILevelCode = code;
                        dataW.Value = item.Value;
                        dataW.Week = item.PeriodValue;
                        dataW.CreateTime = item.CreateTime;
                        dataW.Period = kind;
                        _dbContext.Datas.Add(dataW);
                        _dbContext.SaveChanges();
                    }
                    else if (kind == "W" && updateW != null)
                    {
                        updateW.Value = item.Value;
                        _dbContext.SaveChanges();
                    }
                    else if (kind == "M" && updateM == null)
                    {
                        var dataM = new Data();
                        dataM.KPILevelCode = code;
                        dataM.Value = item.Value;
                        dataM.Month = item.PeriodValue;
                        dataM.CreateTime = item.CreateTime;
                        dataM.Period = kind;
                        _dbContext.Datas.Add(dataM);
                        _dbContext.SaveChanges();
                    }
                    else if (kind == "M" && updateM != null)
                    {
                        updateM.Value = item.Value;
                        _dbContext.SaveChanges();
                    }
                    else if (kind == "Q" && updateM == null)
                    {
                        var dataQ = new Data();
                        dataQ.KPILevelCode = code;
                        dataQ.Value = item.Value;
                        dataQ.Quarter = item.PeriodValue;
                        dataQ.CreateTime = item.CreateTime;
                        dataQ.Period = kind;
                    }
                    else if (kind == "Q" && updateM != null)
                    {
                        updateQ.Value = item.Value;
                        _dbContext.SaveChanges();
                    }
                    else if (kind == "Y" && updateY != null)
                    {
                        var dataY = new Data();
                        dataY.KPILevelCode = code;
                        dataY.Value = item.Value;
                        dataY.CreateTime = item.CreateTime;
                        dataY.Year = item.Year;
                        dataY.Period = kind;
                    }
                    else if (kind == "Y" && updateM != null)
                    {
                        updateY.Value = item.Value;
                        _dbContext.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }
        public bool Update(EF.Data entity)
        {
            var item = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == entity.KPILevelCode);
            try
            {
                item = entity;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }

        public object UploadData()
        {
            var model = (from a in _dbContext.KPILevels
                         join h in _dbContext.KPIs on a.KPIID equals h.ID
                         join c in _dbContext.Levels on a.LevelID equals c.ID
                         where a.KPILevelCode != null && a.KPILevelCode != string.Empty
                         select new
                         {
                             KPILevelCode = a.KPILevelCode,
                             KPIName = h.Name,
                             LevelName = c.Name,
                             StatusW = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Weekly != null ? true : false,
                             StatusM = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Monthly != null ? true : false,
                             StatusQ = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Quarterly != null ? true : false,
                             StatusY = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Yearly != null ? true : false,
                         }).AsEnumerable();
            model = model.ToList();

            return model;
        }


        public object UpLoadKPILevel(int userid, int page, int pageSize)
        {
            var model = (from u in _dbContext.Users
                         join l in _dbContext.Levels on u.LevelID equals l.ID
                         join item in _dbContext.KPILevels on l.ID equals item.LevelID
                         where u.ID == userid && item.Checked == true
                         select new KPIUpLoadVM
                         {
                             KPIName = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Name,
                             StateW = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == item.KPILevelCode).WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode).Week > 0 ? true : false,
                             StateM = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == item.KPILevelCode).MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode).Month > 0 ? true : false,
                             StateQ = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == item.KPILevelCode).QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode).Quarter > 0 ? true : false,
                             StateY = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == item.KPILevelCode).YearlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode).Year > 0 ? true : false
                         });
            int totalRow = model.Count();
            model = model.OrderByDescending(x => x.KPIName)
              .Skip((page - 1) * pageSize)
              .Take(pageSize);
            var vm = new WorkplaceVM()
            {
                KPIUpLoads = model.ToList(),
                total = totalRow
            };
            return vm;
        }

        public List<DataExportVM> DataExport(int userid)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();
            var model = (from l in _dbContext.Levels
                         join u in _dbContext.Users on l.ID equals u.LevelID
                         join item in _dbContext.KPILevels on l.ID equals item.LevelID
                         join d in _dbContext.Datas on item.KPILevelCode equals d.KPILevelCode into JoinItem
                         from joi in JoinItem.DefaultIfEmpty()
                         join k in _dbContext.KPIs on item.KPIID equals k.ID
                         where u.ID == userid && item.Checked == true
                         group new { u, l, item, joi, k } by new
                         {
                             u.Username,
                             Area = l.Name,
                             KPIName = k.Name,
                             item.KPILevelCode,
                             item.Checked,
                             item.WeeklyChecked,
                             item.MonthlyChecked,
                             item.QuarterlyChecked,
                             item.YearlyChecked,

                         } into g
                         select new
                         {
                             g.Key.Username,
                             Area = g.Key.Area,
                             KPIName = g.Key.KPIName,
                             g.Key.KPILevelCode,
                             g.Key.Checked,
                             g.Key.WeeklyChecked,
                             g.Key.MonthlyChecked,
                             g.Key.QuarterlyChecked,
                             g.Key.YearlyChecked,
                             
                             PeriodValueW = g.Select(x => x.joi.Week).Max(),
                             PeriodValueM = g.Select(x => x.joi.Month).Max(),
                             PeriodValueQ = g.Select(x => x.joi.Quarter).Max(),
                             PeriodValueY = g.Select(x => x.joi.Year).Max(),
                         }).AsEnumerable()
                         .Select(x => new DataExportVM
                          {
                              Value = 0,
                              Year =currentYear,
                              KPILevelCode = x.KPILevelCode,
                              KPIName = x.KPIName,
                              Area = x.Area,
                              Remark = string.Empty,
                              PeriodValueW = x.PeriodValueW,
                              PeriodValueM = x.PeriodValueM,
                              PeriodValueQ = x.PeriodValueQ,
                              PeriodValueY = x.PeriodValueY,
                          });

            return model.ToList();
        }
        public object UpLoadKPILevelTrack(int userid, int page, int pageSize)
        {
            var model1 = new LevelDAO().GetListTreeForWorkplace(userid);
            var relative = ConvertHierarchicalToFlattenObject(model1);
            var itemuser = _dbContext.Users.FirstOrDefault(x => x.ID == userid).LevelID;
            var level = _dbContext.Levels.Select(
                x => new LevelVM
                {
                    ID = x.ID,
                    Name = x.Name,
                    Code = x.Code,
                    ParentID = x.ParentID,
                    ParentCode = x.ParentCode,
                    LevelNumber = x.LevelNumber,
                    State = x.State,
                    CreateTime = x.CreateTime
                }).ToList();
            // here you get your list
            var itemlevel = _dbContext.Levels.FirstOrDefault(x => x.ID == itemuser);
            var tree = GetTree(level, itemuser).FirstOrDefault();

            var relative2 = ConvertHierarchicalToFlattenObject2(tree);
            //var KPILevels = _dbContext.KPILevels.Where(x => x.Checked == true).ToList();

            var list = new List<KPIUpLoadVM>();
            var userKPIlevel = _dbContext.KPILevels.Where(x => x.Checked == true && x.LevelID == itemuser).ToList();
            foreach (var item in userKPIlevel)
            {
                var data = new KPIUpLoadVM()
                {
                    KPIName = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Name,
                    Area = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name,
                    StateW = item.WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Week > 0) != null ? true : false,
                    StateM = item.MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Month > 0) != null ? true : false,
                    StateQ = item.QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Quarter > 0) != null ? true : false,
                    StateY = item.YearlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Year > 0) != null ? true : false
                };
                list.Add(data);
            }
            foreach (var a in relative2)
            {
                var KPILevels = _dbContext.KPILevels.Where(x => x.Checked == true && x.LevelID == a.ID).ToList();
                foreach (var item in KPILevels)
                {
                    var data = new KPIUpLoadVM()
                    {
                        KPIName = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Name,
                        Area = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name,
                        StateW = item.WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Week > 0) != null ? true : false,
                        StateM = item.MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Month > 0) != null ? true : false,
                        StateQ = item.QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Quarter > 0) != null ? true : false,
                        StateY = item.YearlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Year > 0) != null ? true : false
                    };
                    list.Add(data);
                }
            }
            var total = list.Count();
            list = list.OrderBy(x => x.KPIName).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new
            {
                model = list,
                total
            };
        }
        /// <summary>
        /// Convert the nested hierarchical object to flatten object
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerable<KPITreeViewModel> ConvertHierarchicalToFlattenObject(KPITreeViewModel parent)
        {
            yield return parent;
            foreach (KPITreeViewModel child in parent.children) // check null if you must
                foreach (KPITreeViewModel relative in ConvertHierarchicalToFlattenObject(child))
                    yield return relative;
        }
        public IEnumerable<LevelVM> ConvertHierarchicalToFlattenObject2(LevelVM parent)
        {
            yield return parent;
            foreach (LevelVM child in parent.Levels) // check null if you must
                foreach (LevelVM relative in ConvertHierarchicalToFlattenObject2(child))
                    yield return relative;
        }
        public List<LevelVM> GetTree(List<LevelVM> list, int parent)
        {
            return list.Where(x => x.ParentID == parent).Select(x => new LevelVM
            {
                ID = x.ID,
                Name = x.Name,
                Levels = GetTree(list, x.ID)
            }).ToList();
        }

        public object MyWorkplace(int levelId, int page, int pageSize)
        {
            var obj = _dbContext.KPILevels.Where(x => x.LevelID == levelId && x.Checked == true).ToList();
            var list = new List<KPIUpLoadVM>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    var data = new KPIUpLoadVM()
                    {
                        KPIName = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Name,
                        StateW = item.WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Week > 0) != null ? true : false,
                        StateM = item.MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Month > 0) != null ? true : false,
                        StateQ = item.QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Quarter > 0) != null ? true : false,
                        StateY = item.YearlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Year > 0) != null ? true : false

                    };
                    list.Add(data);
                }
                var total = list.Count();
                list = list.OrderBy(x => x.KPIName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return new
                {
                    model = list,
                    total,
                    status = true
                };
            }
            return new
            {
                status = false
            };
        }
    }
}
