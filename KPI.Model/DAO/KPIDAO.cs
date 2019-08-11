using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.helpers;
namespace KPI.Model.DAO
{
    public class KPIDAO
    {
        KPIDbContext _dbContext = null;
        public KPIDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        
    }
}
