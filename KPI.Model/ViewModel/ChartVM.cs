﻿using KPI.Model.EF;
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
        public int Standard { get; set; }
        public string Unit { get; set; }
        public int[] dataids { get; set; }

      
        public List<Dataremark> Dataremarks { get; set; }
    }
    public  class Dataremark 
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
    }
}
