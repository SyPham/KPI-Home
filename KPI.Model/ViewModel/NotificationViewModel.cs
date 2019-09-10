﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class NotificationViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Link { get; set; }
        public string Period { get; set; }
        public string KPIName { get; set; }
        public string Tag { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Seen { get; set; }
        public string Username { get; set; }
        
    }
}
