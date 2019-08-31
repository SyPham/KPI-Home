using System;
using System.Collections.Generic;
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
        public int DataID { get; set; }
        [DataMember]
        public int CommentID { get; set; }
        [DataMember]
        public bool Seen { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public string Tag { get; set; }
    }
}
