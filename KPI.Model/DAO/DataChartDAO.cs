﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.helpers;
using KPI.Model.ViewModel;

namespace KPI.Model.DAO
{
    public class DataChartDAO
    {
        KPIDbContext _dbContext = null;
        public DataChartDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        /// <summary>
        /// Lấy dữ liệu cho chart js.
        /// </summary>
        /// <param name="kpiid"></param>
        /// <param name="kpilevel"></param>
        /// <param name="period"></param>
        /// <returns>Danh sách value theo thời gian</returns>
        public ChartVM ListDatas(string kpilevelcode, string period, int? year, int? start, int? end)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();
            string url = string.Empty;
            if (!string.IsNullOrEmpty(kpilevelcode) && !string.IsNullOrEmpty(period))
            {
                //label chartjs
                var item = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
                var kpi = _dbContext.KPIs.Find(item.KPIID);
                var kpiname = kpi.Name;
                var label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name;
                //datasets chartjs
                var model = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode);
                var unit = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Unit;
                var unitName = _dbContext.Units.FirstOrDefault(x => x.ID == unit).Name;
                if (period == "W".ToUpper())
                {
                    if (year == null && start == null && end == null || year == 0 && start == 0 && end == 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == currentYear && x.Week >= 1 && x.Week <= currentWeek);
                    }
                    if (year > 0 && start > 0 && end > 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == year && x.Week >= start && x.Week <= end);

                    }

                    var datasets = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Value).ToArray();
                    var Dataremarks = model
                        .Where(x => x.Period == "W")
                        .OrderBy(x => x.Week)
                        .Select(x => new Dataremark
                        {
                            ID =x.ID,
                            Value =x.Value.Value,
                            Remark =x.Remark,
                            Week=x.Week
                        }).ToList();
                    //data: labels chartjs
                    var listlabels = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Week).ToArray();
                    var labels = Array.ConvertAll(listlabels, x => x.ToSafetyString());

                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x=>x.KPILevelCode==kpilevelcode && x.WeeklyChecked==true).WeeklyStandard,
                        Dataremarks=Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        kpiname = kpiname,
                        period = "W",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true
                    };
                }
                else if (period == "M".ToUpper())
                {
                    if (year == null && start == null && end == null || year == 0 && start == 0 && end == 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == currentYear && x.Month >= 1 && x.Month <= currentMonth);
                    }
                    if (year > 0 && start > 0 && end > 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == year && x.Month >= start && x.Month <= end);
                    }
                    //model = model.Where(x => x.CreateTime.Year == year && x.Month >= 1 && x.Month <= currentMonth);
                    var Dataremarks = model
                       .Where(x => x.Period == "M")
                       .OrderBy(x => x.Month)
                       .Select(x => new Dataremark
                       {
                           ID = x.ID,
                           Value = x.Value.Value,
                           Remark = x.Remark,
                           Month = x.Month
                       }).ToList();
                    var datasets = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var listlabels = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Month).ToArray();
                    string[] labels = new string[listlabels.Length];

                    for (int i = 0; i < listlabels.Length; i++)
                    {
                        switch (listlabels[i])
                        {
                            case 1:
                                labels[i] = "Jan";
                                break;
                            case 2:
                                labels[i] = "Feb"; break;
                            case 3:
                                labels[i] = "Mar"; break;
                            case 4:
                                labels[i] = "Apr"; break;
                            case 5:
                                labels[i] = "May";
                                break;
                            case 6:
                                labels[i] = "Jun"; break;
                            case 7:
                                labels[i] = "Jul"; break;
                            case 8:
                                labels[i] = "Aug"; break;
                            case 9:
                                labels[i] = "Sep";
                                break;
                            case 10:
                                labels[i] = "Oct"; break;
                            case 11:
                                labels[i] = "Nov"; break;
                            case 12:
                                labels[i] = "Dec"; break;
                        }
                    }
                    return new ChartVM
                    {
                        Unit= unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.MonthlyChecked == true).MonthlyStandard,
                        Dataremarks = Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        kpiname = kpiname,
                        period = "M",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true
                    };
                }
                else if (period == "Q".ToUpper())
                {
                    if (year == null && start == null && end == null || year == 0 && start == 0 && end == 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == currentYear && x.Quarter >= 1 && x.Quarter <= currentQuarter);
                    }
                    if (year > 0 && start > 0 && end > 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == year && x.Quarter >= start && x.Quarter <= end);
                    }
                    //model = model.Where(x => x.CreateTime.Year == year && x.Quater >= 1 && x.Quater <= currentQuarter);
                    var datasets = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Value).ToArray();
                    var Dataremarks = model
                      .Where(x => x.Period == "Q")
                      .OrderBy(x => x.Quarter)
                      .Select(x => new Dataremark
                      {
                          ID = x.ID,
                          Value = x.Value.Value,
                          Remark = x.Remark,
                          Quater = x.Quarter
                      }).ToList();
                    //data: labels chartjs
                    var listlabels = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Quarter).ToArray();
                    //var labels = Array.ConvertAll(listlabels, x => x.ToSafetyString());
                    string[] labels = new string[listlabels.Length];
                    for (int i = 0; i < listlabels.Length; i++)
                    {
                        switch (listlabels[i])
                        {
                            case 1:
                                labels[i] = "Quarter 1"; break;
                            case 2:
                                labels[i] = "Quarter 2"; break;
                            case 3:
                                labels[i] = "Quarter 3"; break;
                            case 4:
                                labels[i] = "Quarter 4"; break;
                        }
                    }
                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.QuarterlyChecked == true).QuarterlyStandard,
                        Dataremarks = Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        kpiname = kpiname,
                        period = "Q",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true
                    };
                }
                else if (period == "Y".ToUpper())
                {
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Year >= start && x.Year <= end);
                    }
                    var datasets = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Value).ToArray();
                    var Dataremarks = model
                      .Where(x => x.Period == "Y")
                      .OrderBy(x => x.Year)
                      .Select(x => new Dataremark
                      {
                          ID = x.ID,
                          Value = x.Value.Value,
                          Remark = x.Remark,
                          Year = x.Year
                      }).ToList();
                    //data: labels chartjs
                    var listlabels = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                    var labels = Array.ConvertAll(listlabels, x => x.ToSafetyString());
                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.YearlyChecked == true).YearlyStandard,
                        Dataremarks = Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        kpiname = kpiname,
                        period = "Y",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true
                    };
                }
                else
                {
                    return new ChartVM();
                }
            }
            else
            {
                return new ChartVM();
            }
        }

        public ChartVM Compare(string kpilevelcode, string period)
        {
            var model = new ChartVM();
            var item = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
            model.kpiname = _dbContext.KPIs.Find(item.KPIID).Name;
            model.label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name;
            model.kpilevelcode = kpilevelcode;
            if (period == "W")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Week).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Week).Select(x => x.Week).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
            }
            if (period == "M")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Month).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Month).Select(x => x.Month).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
            }
            if (period == "Q")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Quarter).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Quarter).Select(x => x.Quarter).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
            }
            if (period == "Y")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Year).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
            }
            return model;
        }
        public object Remark(int dataid)
        {
            var model = _dbContext.Datas.FirstOrDefault(x => x.ID == dataid);
           
            return model;
        }
        public DataCompareVM Compare(string obj)
        {
            var model = new DataCompareVM();
            obj = obj.ToSafetyString();
            var value = obj.Split('-');
            var size = value.Length;
            if (size == 2)
            {
                var kpilevelcode1 = value[0].Split(',')[0];
                var period1 = value[1].Split(',')[1];
                var kpilevelcode2 = value[1].Split(',')[0];
                var period2 = value[1].Split(',')[1];
                model.list1 = Compare(kpilevelcode1, period1);
                model.list2 = Compare(kpilevelcode2, period2);
                return model;
            }
            else if (size == 3)
            {
                var kpilevelcode1 = value[0].Split(',')[0];
                var period1 = value[1].Split(',')[1];

                var kpilevelcode2 = value[1].Split(',')[0];
                var period2 = value[1].Split(',')[1];

                var kpilevelcode3 = value[2].Split(',')[0];
                var period3 = value[2].Split(',')[1];
                model.list1 = Compare(kpilevelcode1, period1);
                model.list2 = Compare(kpilevelcode2, period2);
                model.list3 = Compare(kpilevelcode3, period3);
                return model;

            }
            else if (size == 4)
            {
                var kpilevelcode1 = value[0].Split(',')[0];
                var period1 = value[1].Split(',')[1];

                var kpilevelcode2 = value[1].Split(',')[0];
                var period2 = value[1].Split(',')[1];

                var kpilevelcode3 = value[2].Split(',')[0];
                var period3 = value[2].Split(',')[1];

                var kpilevelcode4 = value[3].Split(',')[0];
                var period4 = value[3].Split(',')[1];
                model.list1 = Compare(kpilevelcode1, period1);
                model.list2 = Compare(kpilevelcode2, period2);
                model.list3 = Compare(kpilevelcode3, period3);
                model.list4 = Compare(kpilevelcode4, period4);
                return model;
            }
            else
            {
                return new DataCompareVM();
            }
        }
        public bool UpdateRemark(int dataid,string remark)
        {
            var model = _dbContext.Datas.FirstOrDefault(x => x.ID == dataid);
            try
            {
                model.Remark = remark.ToSafetyString();
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
