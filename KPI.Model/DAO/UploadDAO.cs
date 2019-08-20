using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Configuration;

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

        public SendMailVM UploadData(List<UploadDataVM> entity)
        {
            var listAdd = new List<Data>();
            var listUpdate = new List<Data>();
            var listDataUpload = new List<UploadKPIVM>();
            var model = _dbContext.Datas;
            try
            {
                foreach (var item in entity)
                {
                    var value = item.KPILevelCode;
                    var code = value.Substring(0, value.Length - 1);
                    var period = value.Substring(value.Length - 1, 1);

                    var updateW = model.FirstOrDefault(x => x.KPILevelCode == code && x.Period == period && x.Week == item.PeriodValue);
                    var updateM = model.FirstOrDefault(x => x.KPILevelCode == code && x.Period == period && x.Month == item.PeriodValue);
                    var updateQ = model.FirstOrDefault(x => x.KPILevelCode == code && x.Period == period && x.Quarter == item.PeriodValue);
                    var updateY = model.FirstOrDefault(x => x.KPILevelCode == code && x.Period == period && x.Year == item.PeriodValue);

                    if (period == "W" && updateW == null)
                    {
                        var dataAdd = new Data();
                        dataAdd.KPILevelCode = code;
                        dataAdd.Value = item.Value;
                        dataAdd.Week = item.PeriodValue;
                        dataAdd.CreateTime = item.CreateTime;
                        dataAdd.Period = period;
                        listAdd.Add(dataAdd);

                    }
                    else if (period == "W" && updateW != null)
                    {
                        var dataUpdate = new Data();
                        dataUpdate.KPILevelCode = code;
                        dataUpdate.Value = item.Value;
                        dataUpdate.Week = item.PeriodValue;
                        dataUpdate.CreateTime = item.CreateTime;
                        dataUpdate.Period = period;

                        listUpdate.Add(dataUpdate);
                    }
                    else if (period == "M" && updateM == null)
                    {
                        var dataAdd = new Data();
                        dataAdd.KPILevelCode = code;
                        dataAdd.Value = item.Value;
                        dataAdd.Month = item.PeriodValue;
                        dataAdd.CreateTime = item.CreateTime;
                        dataAdd.Period = period;
                        listAdd.Add(dataAdd);

                    }
                    else if (period == "M" && updateM != null)
                    {
                        var dataUpdate = new Data();
                        dataUpdate.KPILevelCode = code;
                        dataUpdate.Value = item.Value;
                        dataUpdate.Month = item.PeriodValue;
                        dataUpdate.CreateTime = item.CreateTime;
                        dataUpdate.Period = period;

                        listUpdate.Add(dataUpdate);
                    }
                    else if (period == "Q" && updateM == null)
                    {
                        var dataAdd = new Data();
                        dataAdd.KPILevelCode = code;
                        dataAdd.Value = item.Value;
                        dataAdd.Quarter = item.PeriodValue;
                        dataAdd.CreateTime = item.CreateTime;
                        dataAdd.Period = period;
                        listAdd.Add(dataAdd);


                    }
                    else if (period == "Q" && updateM != null)
                    {
                        var dataUpdate = new Data();
                        dataUpdate.KPILevelCode = code;
                        dataUpdate.Value = item.Value;
                        dataUpdate.Quarter = item.PeriodValue;
                        dataUpdate.CreateTime = item.CreateTime;
                        dataUpdate.Period = period;

                        listUpdate.Add(dataUpdate);
                    }
                    else if (period == "Y" && updateY == null)
                    {
                        var dataAdd = new Data();
                        dataAdd.KPILevelCode = code;
                        dataAdd.Value = item.Value;
                        dataAdd.Year = item.PeriodValue;
                        dataAdd.CreateTime = item.CreateTime;
                        dataAdd.Period = period;
                        listAdd.Add(dataAdd);
                    }
                    else if (period == "Y" && updateM != null)
                    {
                        var dataUpdate = new Data();
                        dataUpdate.KPILevelCode = code;
                        dataUpdate.Value = item.Value;
                        dataUpdate.Year = item.PeriodValue;
                        dataUpdate.CreateTime = item.CreateTime;
                        dataUpdate.Period = period;

                        listUpdate.Add(dataUpdate);
                    }
                }
                if (listAdd.Count() > 0)
                {
                    _dbContext.Datas.AddRange(listAdd);
                    _dbContext.SaveChanges();
                    //Gui mail list nay khi update
                    var modelKPILevel = _dbContext.KPILevels;
                    var kpis = _dbContext.KPIs;
                    var levels = _dbContext.Levels;
                    foreach (var item in listAdd)
                    {
                        var standard = modelKPILevel.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode);
                        if (item.Value < standard.WeeklyStandard)
                        {
                            var dataUploadKPIVM = new UploadKPIVM()
                            {
                                KPILevelCode = item.KPILevelCode,
                                Area = levels.FirstOrDefault(x => x.ID == standard.LevelID).Name,
                                KPIName = kpis.FirstOrDefault(x => x.ID == standard.KPIID).Name,
                                Week = item.Week,
                                Month = item.Month,
                                Quarter = item.Quarter,
                                Year = item.Year
                            };
                            listDataUpload.Add(dataUploadKPIVM);
                        }

                    }
                }

                if (listUpdate.Count() > 0)
                {
                    foreach (var item in listUpdate)
                    {
                        if (item.Period == "W")
                        {
                            var dataW = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Period == item.Period && x.Week == item.Week);
                            dataW.Value = item.Value;
                            _dbContext.SaveChanges();
                        }
                        if (item.Period == "M")
                        {
                            var dataW = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Period == item.Period && x.Month == item.Month);
                            dataW.Value = item.Value;
                            _dbContext.SaveChanges();
                        }
                        if (item.Period == "Q")
                        {
                            var dataW = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Period == item.Period && x.Quarter == item.Quarter);
                            dataW.Value = item.Value;
                            _dbContext.SaveChanges();
                        }
                        if (item.Period == "Y")
                        {
                            var dataW = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Period == item.Period && x.Year == item.Year);
                            dataW.Value = item.Value;
                            _dbContext.SaveChanges();
                        }
                    }
                    /////Gui mail khi update
                    var modelKPILevel = _dbContext.KPILevels;
                    var kpis = _dbContext.KPIs;
                    var levels = _dbContext.Levels;
                    foreach (var item in listUpdate)
                    {
                        var standard = modelKPILevel.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode);
                        if (item.Value < standard.WeeklyStandard)
                        {
                            var dataUploadKPIVM = new UploadKPIVM()
                            {
                                KPILevelCode = item.KPILevelCode,
                                Area = levels.FirstOrDefault(x => x.ID == standard.LevelID).Name,
                                KPIName = kpis.FirstOrDefault(x => x.ID == standard.KPIID).Name,
                                Week = item.Week,
                                Month = item.Month,
                                Quarter = item.Quarter,
                                Year = item.Year
                            };
                            listDataUpload.Add(dataUploadKPIVM);
                        }

                    }
                }
                if (listDataUpload.Count > 0)
                {
                    return new SendMailVM
                    {
                        ListUploadKPIVMs = listDataUpload,
                        Status = true,
                    };
                }
                else
                {
                    return new SendMailVM
                    {
                        ListUploadKPIVMs = listDataUpload,
                        Status = true,
                    };
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return new SendMailVM
                {
                    ListUploadKPIVMs = listDataUpload,
                    Status = false,
                };
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
                             StateW = item.WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode).Week > 0 ? true : false,
                             StateM = item.MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode).Month > 0 ? true : false,
                             StateQ = item.QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode).Quarter > 0 ? true : false,
                             StateY = item.YearlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode).Year > 0 ? true : false
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
            var model = (from u in _dbContext.Users
                         join l in _dbContext.Levels on u.LevelID equals l.ID
                         join item in _dbContext.KPILevels on l.ID equals item.LevelID
                         where u.ID == userid && item.Checked == true
                         select new DataExportVM
                         {
                             Area = l.Name,
                             KPILevelCode = item.KPILevelCode,
                             KPIName = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Name,
                             StateW = (bool?)item.WeeklyChecked ?? false,
                             StateM = (bool?)item.MonthlyChecked ?? false,
                             StateQ = (bool?)item.QuarterlyChecked ?? false,
                             StateY = (bool?)item.YearlyChecked ?? false,

                             PeriodValueW = (int?)_dbContext.Datas.Where(x=>x.KPILevelCode==item.KPILevelCode).Max(x => x.Week) ?? 0,
                             PeriodValueM = (int?)_dbContext.Datas.Where(x=>x.KPILevelCode==item.KPILevelCode).Max(x => x.Month) ?? 0,
                             PeriodValueQ = (int?)_dbContext.Datas.Where(x=>x.KPILevelCode==item.KPILevelCode).Max(x => x.Quarter) ?? 0,
                             PeriodValueY = (int?)_dbContext.Datas.Where(x => x.KPILevelCode == item.KPILevelCode).Max(x => x.Year) ?? 0,

                             UploadTimeW = item.Weekly,
                             UploadTimeM = item.Monthly,
                             UploadTimeQ = item.Quarterly,
                             UploadTimeY = item.Yearly,
                         });
            return model.ToList();
        }
        //group by, join sample
        public List<DataExportVM> DataExport2(int userid)
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
                             Year = currentYear,
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
            var total = 0;
            if (relative2 != null)
            {
                var KPILevels = new List<KPILevel>();
                foreach (var aa in relative2)
                {
                    if (aa != null)
                    {
                         KPILevels = _dbContext.KPILevels.Where(x => x.Checked == true && x.LevelID == aa.ID)
                        .Select(a => new
                        {
                            a.KPIID,
                            a.LevelID,
                            a.WeeklyChecked,
                            a.MonthlyChecked,
                            a.QuarterlyChecked,
                            a.YearlyChecked,
                            a.KPILevelCode
                        }).ToList()
                        .Select(x => new KPILevel
                        {
                            KPIID = x.KPIID,
                            LevelID = x.LevelID,
                            WeeklyChecked = x.WeeklyChecked,
                            MonthlyChecked = x.MonthlyChecked,
                            QuarterlyChecked = x.QuarterlyChecked,
                            YearlyChecked = x.YearlyChecked,
                            KPILevelCode = x.KPILevelCode
                        }).ToList();
                    }
                    
                    if (KPILevels != null)
                    {
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

                }
                total = list.Count();
                list = list.OrderBy(x => x.KPIName).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            }

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
            if (parent == null)
                parent = new LevelVM();
            if (parent.Levels == null)
                 parent.Levels = new List<LevelVM>();
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
