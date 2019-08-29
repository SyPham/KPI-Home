using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class ActionPlanViewModel2
    {
        public int UserID { get; set; }
        public int DataID { get; set; }
        public int CommentID { get; set; }

        public string Title { get; set; }
        public string KPILevelCodeAndPeriod { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public string Deadline { get; set; }
        public string SubmitDate { get; set; }
    }
}
