using KPI.Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    [Serializable()]
    public  class UserProfileVM
    {
        public User User { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
