using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
   public class ChartVM
    {
        private int?[] datasetsList = {};

        public int?[] datasets
        {
            get
            {
                return datasetsList;
            }

            set
            {
                datasetsList = value;
            }
        }
        public string[] labels { get; set; }

        public string label { get; set; }
        public string kpiname { get; set; }
        public string period { get; set; }
        public string kpilevelcode { get; set; }
        public bool statusfavorite { get; set; }
        public string url { get; set; }
    }
}
