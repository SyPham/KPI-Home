using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.EF
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class Notification :EntityBase
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string KPIName { get; set; }
        [DataMember]
        public string Period { get; set; }
        [DataMember]
        public bool Seen { get; set; }
        [DataMember]
        public string Link { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public string Tag { get; set; }
    }
}
