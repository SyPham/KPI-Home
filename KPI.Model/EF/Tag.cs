using KPI.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
  public  class Tag 
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CommentID { get; set; }
        public int ActionPlanID { get; set; }
    }
}
