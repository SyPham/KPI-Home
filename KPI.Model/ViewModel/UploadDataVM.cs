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
        public int Value { get; set; }
        public int PeriodValue { get; set; }
        public int Year { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
