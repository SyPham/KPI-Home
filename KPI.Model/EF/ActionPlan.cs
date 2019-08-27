using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
   public class ActionPlan
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int DataID { get; set; }
        public int CommentID { get; set; }

        [Column("Title")]
        public string Title { get; set; }
        public string KPILevelCodeAndPeriod { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int ApprovedBy { get; set; }

        [Column("CreatedTime")]
        public DateTime CreatedTime { get; set; }
        [Column("Deadline")]
        public DateTime Deadline { get; set; }
        public DateTime SubmitDate { get; set; }

        public bool Status { get; set; }
        public bool ApprovedStatus { get; set; }
    }
}
