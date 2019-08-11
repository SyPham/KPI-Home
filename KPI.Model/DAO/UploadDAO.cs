using KPI.Model.EF;
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
                else if (item.Quater != null && kpilevelcode.Quater == item.Quater)
                {
                    kpilevelcode.Quater = item.Quater;
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

                    var updateW = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.KPIKind == kind && x.Week == item.PeriodValue);
                    var updateM = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.KPIKind == kind && x.Month == item.PeriodValue);
                    var updateQ = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.KPIKind == kind && x.Quater == item.PeriodValue);
                    var updateY = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.KPIKind == kind && x.Year == item.PeriodValue);

                    if (kind == "W" && updateW == null)
                    {
                        var dataW = new Data();
                        dataW.KPILevelCode = code;
                        dataW.Value = item.Value;
                        dataW.Week = item.PeriodValue;
                        dataW.CreateTime = item.CreateTime;
                        dataW.KPIKind = kind;
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
                        dataM.KPIKind = kind;
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
                        dataQ.Quater = item.PeriodValue;
                        dataQ.CreateTime = item.CreateTime;
                        dataQ.KPIKind = kind;
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
                        dataY.KPIKind = kind;
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
                        where a.KPILevelCode!=null && a.KPILevelCode!=string.Empty
                        select new
                        {
                            KPILevelCode=a.KPILevelCode,
                            KPIName = h.Name,
                            LevelName = c.Name,
                            StatusW = _dbContext.KPILevels.FirstOrDefault(x=>x.KPILevelCode== a.KPILevelCode).Weekly!=null?true:false,
                            StatusM = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Monthly != null ? true : false,
                            StatusQ = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Quaterly != null ? true : false,
                            StatusY = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Yearly != null ? true : false,
                        }).AsEnumerable();
            model = model.ToList();

            return model;
        }
    }
}
