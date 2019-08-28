using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class ActionPlanViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string dueDate { get; set; }
        public bool done { get; set; }
        public string listId { get; set; }
    }
    public class ActionPlanCategoryViewModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public IList<ActionPlanViewModel> items { get; set; }
    }
}
