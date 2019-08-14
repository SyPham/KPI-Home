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
                var kpilevelcode = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode);
                if (kpilevelcode == null)
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
                else if (item.Week != null && kpilevelcode.Week == item.Week)
                {
                    kpilevelcode.Week = item.Week;
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
                else if (item.Month != null && kpilevelcode.Month == item.Month)
                {
                    kpilevelcode.Month = item.Month;
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
                else if (item.Quarter != null && kpilevelcode.Quarter == item.Quarter)
                {
                    kpilevelcode.Quarter = item.Quarter;
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
                else if (item.Year != null && kpilevelcode.Year == item.Year)
                {
                    kpilevelcode.Year = item.Year;
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
                         join kpilevel in _dbContext.KPILevels on l.ID equals kpilevel.LevelID
                         where u.ID == userid && kpilevel.Checked == true
                         select new KPIUpLoadVM
                         {
                             KPIName = _dbContext.KPIs.FirstOrDefault(x => x.ID == kpilevel.KPIID).Name,
                             StateW = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Week != null ? true : false,
                             StateM = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Month == null ? true : false,
                             StateQ = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Quarter == null ? true : false,
                             StateY = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).YearlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Year == null ? true : false
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
            var model = from u in _dbContext.Users
                        join l in _dbContext.Levels on u.LevelID equals l.ID
                        join kpilevel in _dbContext.KPILevels on l.ID equals kpilevel.LevelID
                        where u.ID == userid && kpilevel.Checked == true
                        select new DataExportVM
                        {
                            StateW = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Week != null ? true : false,
                            StateM = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Month == null ? true : false,
                            StateQ = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Quarter == null ? true : false,
                            StateY = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).YearlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Year == null ? true : false,
                            PeriodValueW = _dbContext.Datas.Where(k => k.KPILevelCode == kpilevel.KPILevelCode).Select(x => x.Week).Max(),
                            PeriodValueM = _dbContext.Datas.Where(k => k.KPILevelCode == kpilevel.KPILevelCode).Select(x => x.Month).Max(),
                            PeriodValueQ = _dbContext.Datas.Where(k => k.KPILevelCode == kpilevel.KPILevelCode).Select(x => x.Quarter).Max(),
                            PeriodValueY = _dbContext.Datas.Where(k => k.KPILevelCode == kpilevel.KPILevelCode).Select(x => x.Year).Max(),

                            Value = 0,
                            Year = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).CreateTime.Year,
                            KPILevelCode = kpilevel.KPILevelCode,
                            KPIName = _dbContext.KPIs.FirstOrDefault(x => x.ID == kpilevel.KPIID).Name,
                            Area = _dbContext.Levels.FirstOrDefault(x => x.ID == kpilevel.LevelID).Name,

                            UploadTimeW = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Weekly,
                            UploadTimeM = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Monthly,
                            UploadTimeQ = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Quarterly,
                            UploadTimeY = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Yearly,
                            Remark = string.Empty,
                        };

            var models = model.AsEnumerable().ToList();
            return model.ToList();
        }
        public object UpLoadKPILevelTrack(int userid, int page, int pageSize)
        {
            var model1 = new LevelDAO().GetListTreeForWorkplace(userid);
            var relative = ConvertHierarchicalToFlattenObject(model1);
            var model = _dbContext.Users
                        .Join(_dbContext.Levels, f => f.LevelID, p => p.ID, (f, p) =>
                            new // anonymous object
                            {
                                p,f
                            })
                         .Join(_dbContext.KPILevels, a=> a.p.ID, p2 => p2.LevelID, (a, p2) =>
                            new // anonymous object
                            {
                                a,
                                p2
                            })
                        .AsEnumerable() // database query ends here, the rest is a query in memory
                        .Select(x =>
                            new KPIUpLoadVM()
                            {
                                KPIName = _dbContext.KPIs.FirstOrDefault(k => k.ID == x.p2.KPIID).Name,
                                StateW = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == x.p2.KPILevelCode).WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(y => y.KPILevelCode == x.p2.KPILevelCode).Week != null ? true : false,
                                StateM = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == x.p2.KPILevelCode).MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(y => y.KPILevelCode == x.p2.KPILevelCode).Month == null ? true : false,
                                StateQ = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == x.p2.KPILevelCode).QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(y => y.KPILevelCode == x.p2.KPILevelCode).Quarter == null ? true : false,
                                StateY = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == x.p2.KPILevelCode).YearlyChecked == true && _dbContext.Datas.FirstOrDefault(y => y.KPILevelCode == x.p2.KPILevelCode).Year == null ? true : false

                            });
            //IEnumerable<KPIUpLoadVM> model = (from u in _dbContext.Users
            //                                  join r in relative on u.LevelID equals r.key
            //                                  join kpilevel in _dbContext.KPILevels on r.key equals kpilevel.LevelID
            //                                  where u.ID == userid && kpilevel.Checked == true
            //                                  select new KPIUpLoadVM
            //                                  {
            //                                      KPIName = _dbContext.KPIs.FirstOrDefault(x => x.ID == kpilevel.KPIID).Name,
            //                                      StateW = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Week != null ? true : false,
            //                                      StateM = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Month == null ? true : false,
            //                                      StateQ = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Quarter == null ? true : false,
            //                                      StateY = _dbContext.KPILevels.FirstOrDefault(k => k.KPILevelCode == kpilevel.KPILevelCode).YearlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode).Year == null ? true : false
            //                                  });


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
    }
}
