using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class NotificationViewModel
    {
        public string Link { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Seen { get; set; }
    }
}
