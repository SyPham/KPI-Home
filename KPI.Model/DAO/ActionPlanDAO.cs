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
        public bool Update(ActionPlan entity)
        {

            try
            {
                var item = _dbContext.ActionPlans.Find(entity.ID);
                item.Title = entity.Title;
                item.Description = entity.Description;
                item.Deadline = entity.Deadline;
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
        public object GetAll()
        {
            var lvm = new List<ActionPlanCategoryViewModel>();
            var model = _dbContext.ActionPlanCategories.ToList();
            var model2 = _dbContext.ActionPlans.ToList().Select(x => new ActionPlanViewModel
            {
                id = x.ID,
                title = x.Title,
                description = x.Description,
                done = x.Status,
                dueDate = x.Deadline.ToSafetyString().Split(' ')[0],
                listId = x.ActionPlanCategoryID.ToSafetyString()
            }).ToList();

            foreach (var item in model)
            {
                var vm = new ActionPlanCategoryViewModel();
                vm.id =item.ID.ToSafetyString();
                vm.title = item.Title;
                vm.items = model2.Where(x => x.listId == item.ID.ToSafetyString()).ToArray();
                lvm.Add(vm);
            }
            var arr = lvm.ToArray();
            return new { lists = lvm.ToArray() };
        }
        public object GetAll(int DataID, int CommentID)
        {
            var model = _dbContext.ActionPlans
                .Where(x => x.DataID == DataID && x.CommentID == CommentID)
                .AsEnumerable()
                .Select(x => new ActionPlanGettAllViewModel
                {
                    ID = x.ID,
                    Title = x.Title,
                    Description = x.Description,
                    Content = x.Content,
                    Deadline = x.Deadline.ToString("dd/MM/yyyy")
                }).ToList();
            return new {
                status = true,
                data = model
            };
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
