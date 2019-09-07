using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.EF;
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
                var label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name.ToSafetyString();
                //datasets chartjs
                var model = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode);

                var unit = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID);
                if (unit == null) return new ChartVM();
                var unitName = _dbContext.Units.FirstOrDefault(x => x.ID == unit.Unit).Name.ToSafetyString();

                if (period == "W".ToUpper())
                {
                    var standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.WeeklyChecked == true).WeeklyStandard;
                    var statusFavourites = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true;

                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<int> listDatasets = new List<int>();

                    //labels của chartjs mặc định có 53 phần tử
                    List<string> listLabels = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử
                    List<string> listTargets = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử
                    List<int> listStandards = new List<int>();

                    var Dataremarks = new List<Dataremark>();
                    //Search range
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == year && x.Week >= start && x.Week <= end);

                        var listValues = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Value).ToArray();
                        var listLabelsW = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Week).ToArray();
                        var listtargetsW = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Target).ToArray();
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            listStandards.Add(standard);
                        }
                        //Convert sang list string
                        var listStringLabels = Array.ConvertAll(listLabelsW, x => x.ToSafetyString());

                        //Convert sang list string
                        var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());

                        listDatasets.AddRange(listValues);
                        listLabels.AddRange(listStringLabels);
                        listTargets.AddRange(listStringTargets);

                        Dataremarks = model
                           .Where(x => x.Period == "W")
                           .OrderBy(x => x.Week)
                           .Select(x => new Dataremark
                           {
                               ID = x.ID,
                               Value = x.Value,
                               Remark = x.Remark,
                               Week = x.Week
                           }).ToList();

                    }
                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = standard,
                        Dataremarks = Dataremarks,
                        datasets = listDatasets.ToArray(),
                        labels = listLabels.ToArray(),
                        label = label,
                        targets = listTargets.ToArray(),
                        standards = listStandards.ToArray(),
                        kpiname = kpiname,
                        period = "W",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = statusFavourites
                    };

                }
                else if (period == "M".ToUpper())
                {
                    var standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.MonthlyChecked == true).MonthlyStandard;
                    var statusFavourites = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true;

                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<int> listDatasets = new List<int>();

                    //labels của chartjs mặc định có 12 phần tử = 0
                    List<string> listLabels = new List<string>();

                    //labels của chartjs mặc định có 12 phần tử
                    List<string> listTargets = new List<string>();
                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<int> listStandards = new List<int>();
                    var Dataremarks = new List<Dataremark>();


                    //Search range
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == year && x.Month >= start && x.Month <= end);

                        var listValues = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Value).ToArray();
                        var listLabelsW = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Month).ToArray();
                        var listtargetsW = model.Where(x => x.Period == "M").OrderBy(x => x.Week).Select(x => x.Target).ToArray();
                        //Convert sang list string
                        var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());
                        listTargets.AddRange(listStringTargets);

                        for (int i = 0; i < listValues.Length; i++)
                        {
                            listStandards.Add(standard);
                        }
                        foreach (var a in listLabelsW)
                        {
                            switch (a)
                            {
                                case 1:
                                    listLabels.Add("Jan");
                                    break;
                                case 2:
                                    listLabels.Add("Feb"); break;
                                case 3:
                                    listLabels.Add("Mar"); break;
                                case 4:
                                    listLabels.Add("Apr"); break;
                                case 5:
                                    listLabels.Add("May");
                                    break;
                                case 6:
                                    listLabels.Add("Jun"); break;
                                case 7:
                                    listLabels.Add("Jul"); break;
                                case 8:
                                    listLabels.Add("Aug"); break;
                                case 9:
                                    listLabels.Add("Sep");
                                    break;
                                case 10:
                                    listLabels.Add("Oct"); break;
                                case 11:
                                    listLabels.Add("Nov"); break;
                                case 12:
                                    listLabels.Add("Dec"); break;
                            }
                        }

                        listDatasets.AddRange(listValues);

                        Dataremarks = model
                           .Where(x => x.Period == "M")
                           .OrderBy(x => x.Month)
                           .Select(x => new Dataremark
                           {
                               ID = x.ID,
                               Value = x.Value,
                               Remark = x.Remark,
                               Month = x.Month
                           }).ToList();
                    }

                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = standard,
                        Dataremarks = Dataremarks,
                        datasets = listDatasets.ToArray(),
                        labels = listLabels.ToArray(),
                        targets = listTargets.ToArray(),
                        standards = listStandards.ToArray(),
                        label = label,
                        kpiname = kpiname,
                        period = "M",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = statusFavourites
                    };
                }
                else if (period == "Q".ToUpper())
                {
                    var standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.QuarterlyChecked == true).QuarterlyStandard;
                    var statusFavourites = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true;

                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<int> listDatasets = new List<int>();

                    //labels của chartjs mặc định có 53 phần tử = 0
                    List<string> listLabels = new List<string>();

                    //labels của chartjs mặc định có 12 phần tử
                    List<string> listTargets = new List<string>();
                    //labels của chartjs mặc định có 12 phần tử
                    List<int> listStandards = new List<int>();
                    var Dataremarks = new List<Dataremark>();


                    if (year > 0 && start > 0 && end > 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == year && x.Quarter >= start && x.Quarter <= end);
                        var listValues = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Value).ToArray();
                        var listLabelsW = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Quarter).ToArray();
                        listDatasets.AddRange(listValues);
                        var listtargetsW = model.Where(x => x.Period == "M").OrderBy(x => x.Week).Select(x => x.Target).ToArray();

                        //Convert sang list string
                        var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());
                        listTargets.AddRange(listStringTargets);
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            listStandards.Add(standard);
                        }
                        foreach (var i in listLabelsW)
                        {
                            switch (i)
                            {
                                case 1:
                                    listLabels.Add("Quarter 1"); break;
                                case 2:
                                    listLabels.Add("Quarter 2"); break;
                                case 3:
                                    listLabels.Add("Quarter 3"); break;
                                case 4:
                                    listLabels.Add("Quarter 4"); break;
                            }
                        }
                        Dataremarks = model
                         .Where(x => x.Period == "Q")
                         .OrderBy(x => x.Quarter)
                         .Select(x => new Dataremark
                         {
                             ID = x.ID,
                             Value = x.Value,
                             Remark = x.Remark,
                             Quater = x.Quarter
                         }).ToList();
                    }

                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = standard,
                        Dataremarks = Dataremarks,
                        datasets = listDatasets.ToArray(),
                        labels = listLabels.ToArray(),
                        targets = listTargets.ToArray(),
                        standards = listStandards.ToArray(),
                        label = label,
                        kpiname = kpiname,
                        period = "Q",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = statusFavourites
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
                          Value = x.Value,
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

            var unit = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Unit;
            var unitName = _dbContext.Units.FirstOrDefault(x => x.ID == unit).Name;

            if (period == "W")
            {
                //Tạo ra 1 mảng tuần mặc định bằng 0
                List<int> listDatasets = new List<int>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> listLabels = new List<string>();

                var datas = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Week).Select(x => new { x.Value, x.Week }).ToList();
                foreach (var valueWeek in datas)
                {
                    listDatasets.Add(valueWeek.Value);
                    listLabels.Add(valueWeek.Week.ToString());
                }

                model.datasets = listDatasets.ToArray();
                model.labels = listLabels.ToArray();
                model.period = period;

            }
            if (period == "M")
            {
                //Tạo ra 1 mảng tuần mặc định bằng 0
                List<int> listDatasets = new List<int>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> listLabels = new List<string>();


                var datas = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Month).Select(x => new { x.Month, x.Value }).ToList();
                foreach (var monthly in datas)
                {
                    listDatasets.Add(monthly.Value);
                    switch (monthly.Month)
                    {
                        case 1:
                            listLabels.Add("Jan"); break;
                        case 2:
                            listLabels.Add("Feb"); break;
                        case 3:
                            listLabels.Add("Mar"); break;
                        case 4:
                            listLabels.Add("Apr"); break;
                        case 5:
                            listLabels.Add("May"); break;
                        case 6:
                            listLabels.Add("Jun"); break;
                        case 7:
                            listLabels.Add("Jul"); break;
                        case 8:
                            listLabels.Add("Aug"); break;
                        case 9:
                            listLabels.Add("Sep");
                            break;
                        case 10:
                            listLabels.Add("Oct"); break;
                        case 11:
                            listLabels.Add("Nov"); break;
                        case 12:
                            listLabels.Add("Dec"); break;
                    }
                }
                model.datasets = listDatasets.ToArray();
                model.labels = listLabels.ToArray();
                model.period = period;
            }
            if (period == "Q")
            {
                //Tạo ra 1 mảng tuần mặc định bằng 0
                List<int> listDatasets = new List<int>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> listLabels = new List<string>();
                var datas = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Quarter).Select(x => new { x.Quarter, x.Value }).ToList();
                foreach (var quarterly in datas)
                {
                    listDatasets.Add(quarterly.Value);
                    switch (quarterly.Quarter)
                    {
                        case 1:
                            listLabels.Add("Quarter 1"); break;
                        case 2:
                            listLabels.Add("Quarter 2"); break;
                        case 3:
                            listLabels.Add("Quarter 3"); break;
                        case 4:
                            listLabels.Add("Quarter 4"); break;
                    }
                }
                model.datasets = listDatasets.ToArray();
                model.labels = listLabels.ToArray();
                model.period = period;
                model.Unit = unitName;
                model.Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.QuarterlyChecked == true).QuarterlyStandard;
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
            return new
            {
                model = model,
                users = _dbContext.Users.ToList()
            };
        }
        public DataCompareVM Compare(string obj)
        {
            var model = new DataCompareVM();
            obj = obj.ToSafetyString();

            var value = obj.Split('-');
            model.Period = value[1].Split(',')[1];
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
        public bool UpdateRemark(int dataid, string remark)
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

        public List<User> SearchUsers()
        {
            var listComment = _dbContext.Users.ToList();
            return listComment;
        }
    }
}
