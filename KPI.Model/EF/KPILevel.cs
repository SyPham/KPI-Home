using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.Abstract;
namespace KPI.Model.EF
{
    public class KPILevel
    {
        public int ID { get; set; }
        public string KPILevelCode { get; set; }
        public string UserCheck { get; set; }
        public int KPIID { get; set; }
        public int LevelID { get; set; }
        public int? TeamID { get; set; }
        public string Period { get; set; }

        public int? Weekly { get; set; }
        public DateTime? Monthly { get; set; }
        public DateTime? Quaterly { get; set; }
        public DateTime? Yearly { get; set; }

        public bool? Checked { get; set; }
        public bool? WeeklyChecked { get; set; }
        public bool? MonthlyChecked { get; set; }
        public bool? QuaterlyChecked { get; set; }
        public bool? YearlyChecked { get; set; }
        public bool? CheckedPeriod { get; set; }

        public bool? WeeklyPublic { get; set; }
        public bool? MonthlyPublic { get; set; }
        public bool? QuaterlyPublic { get; set; }
        public bool? YearlyPublic { get; set; }

        public DateTime? TimeCheck { get; set; }

        public DateTime? CreateTime { get; set; }

        public int LevelNumber { get; set; }
    }
}
