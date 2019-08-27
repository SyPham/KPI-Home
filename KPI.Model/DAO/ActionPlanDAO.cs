using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class ActionPlanDAO
    {
        KPIDbContext _dbContext = null;

        public ActionPlanDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public bool Add(ActionPlan entity)
        {

            try
            {
                _dbContext.ActionPlans.Add(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }

        public int Total()
        {
            return _dbContext.ActionPlans.Count();
        }

        public List<ActionPlan> GetActionPlanCode()
        {
            return _dbContext.ActionPlans.ToList();
        }
        public List<ActionPlan> GetActionPlanByCommentID(int commentID)
        {
            return _dbContext.ActionPlans.Where(x => x.CommentID == commentID).ToList();
        }
        public bool Delete(int id)
        {

            try
            {
                var category = _dbContext.ActionPlans.Find(id);
                _dbContext.ActionPlans.Remove(category);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }
        public IEnumerable<ActionPlan> GetAll()
        {
            return _dbContext.ActionPlans.ToList();
        }
        public ActionPlan GetbyID(int ID)
        {
            return _dbContext.ActionPlans.FirstOrDefault(x => x.ID == ID);
        }
        public object ListActionPlan()
        {
            return _dbContext.ActionPlans.ToList();
        }
    }
}
