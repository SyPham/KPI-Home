using KPI.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    public class Data 
    {
        public int ID { get; set; }
        public string KPILevelCode { get; set; }
        [Column("Period")]
        public string Period { get; set; }
        public int Value { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        [Column("Quarter")]
        public int Quarter { get; set; }
        public int Year { get; set; }
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
