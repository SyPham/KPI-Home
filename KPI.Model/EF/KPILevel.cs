using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column("Quarterly")]
        public DateTime? Quarterly { get; set; }
        public DateTime? Yearly { get; set; }

        public bool? Checked { get; set; }
        public bool? WeeklyChecked { get; set; }
        public bool? MonthlyChecked { get; set; }

        [Column("QuarterlyChecked")]
        public bool? QuarterlyChecked { get; set; }
        public bool? YearlyChecked { get; set; }
        public bool? CheckedPeriod { get; set; }

        public bool? WeeklyPublic { get; set; }
        public bool? MonthlyPublic { get; set; }
        [Column("QuarterlyPublic")]
        public bool? QuarterlyPublic { get; set; }
        public bool? YearlyPublic { get; set; }

        public DateTime? TimeCheck { get; set; }

        private DateTime? createTime = null;
        public DateTime CreateTime
        {
            get
            {
                return this.createTime.HasValue
                   ? this.createTime.Value
                   : DateTime.Now;
            }

            set { this.createTime = value; }
        }

        public int LevelNumber { get; set; }
        [Column("WeeklyStandard")]
        public int WeeklyStandard { get; set; }
        public int MonthlyStandard { get; set; }
        [Column("QuarterlyStandard")]
        public int QuarterlyStandard { get; set; }
        public int YearlyStandard { get; set; }
    }
}
