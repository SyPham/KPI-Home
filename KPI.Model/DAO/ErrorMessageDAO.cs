using KPI.Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
   public class ErrorMessageDAO
    {
        KPIDbContext _dbContext = null;
        public ErrorMessageDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public long Add(ErrorMessage error)
        {
            var item = new ErrorMessage();
            _dbContext.ErrorMessages.Add(error);
            _dbContext.SaveChanges();
            return item.ID;

        }
    }
}
