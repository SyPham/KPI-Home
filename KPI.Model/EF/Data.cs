using KPI.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    public class Data 
    {
        public int ID { get; set; }
        public string KPILevelCode { get; set; }
        public string KPIKind { get; set; }
        public int? Value { get; set; }
        public int? Week { get; set; }
        public int? Month { get; set; }
        public int? Quater { get; set; }
        public int? Year { get; set; }
        public string DateUpload { get; set; }
        public string Remark { get; set; }
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

    }
}
