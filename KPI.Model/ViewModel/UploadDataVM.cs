using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class UploadDataVM
    {
        public string KPILevelCode { get; set; }
        public string KPIName { get; set; }
        public int Value { get; set; }
        public int PeriodValue { get; set; }
        public int Year { get; set; }
        public string  Area { get; set; }
        public object UpdateTime { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
