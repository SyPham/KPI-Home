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
            var user = _dbContext.Users;
            var itemActionPlanDetail = new ActionPlanDetail();
            try
            {
                _dbContext.ActionPlans.Add(entity);
                _dbContext.SaveChanges();

                if (entity.Tag.IsNullOrEmpty())
                {
                    var tag = new Tag();
                    if (entity.Tag.IndexOf(",") == -1)
                    {
                        tag.ActionPlanID = entity.ID;
                        tag.UserID = (int?)user.FirstOrDefault(x => x.Username == entity.Tag).ID ?? 0;
                        _dbContext.Tags.Add(tag);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        var list = entity.Tag.Split(',');
                        foreach (var item in list)
                        {
                            tag.UserID = (int?)user.FirstOrDefault(x => x.Username == item).ID ?? 0;
                            tag.ActionPlanID = entity.ID;
                            var username = item.ToSafetyString();
                            itemActionPlanDetail.Seen = false;
                            itemActionPlanDetail.USerID = (int?)user.FirstOrDefault(x => x.Username == username).ID ?? 0;
                            itemActionPlanDetail.ActionPlanID = entity.ID;
                            _dbContext.Tags.Add(tag);
                            _dbContext.ActionPlanDetails.Add(itemActionPlanDetail);
                            _dbContext.SaveChanges();
                        }
                    }
                }
                
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
                item.Tag = entity.Tag;
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
        public bool Approve(ActionPlan entity)
        {

            try
            {
                var item = _dbContext.ActionPlans.Find(entity.ID);
                item.ApprovedBy = entity.ApprovedBy;
                item.ApprovedStatus = entity.ApprovedStatus;
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
        public ActionPlanViewModel2 GetByID(int id)
        {
            var modal = _dbContext.ActionPlans.FirstOrDefault(x => x.ID == id);
            var vm = new ActionPlanViewModel2();
            vm.ID = modal.ID;
            vm.Title = modal.Title;
            vm.Deadline = modal.Deadline.ToString("yyyy-MM-dd");
            vm.Description = modal.Description;
            vm.Tag = modal.Tag;
            return vm;

        }
        public bool Approval(int id, int aproveBy)
        {
            var model = _dbContext.ActionPlans.FirstOrDefault(x => x.ID == id);
            model.ApprovedBy = aproveBy;
           model.ApprovedStatus = !model.ApprovedStatus;
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           

        }
        public bool Done(int id)
        {
            var model = _dbContext.ActionPlans.FirstOrDefault(x => x.ID == id);
            model.Status = !model.Status;
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public object GetAll(int DataID, int CommentID,int userid)
        {
            var userModel = _dbContext.Users.FirstOrDefault(x => x.ID == userid);
            var permission = _dbContext.Permissions;
            var model = _dbContext.ActionPlans
                .Where(x => x.DataID == DataID && x.CommentID == CommentID)
                .AsEnumerable()
                .Select(x => new ActionPlanGettAllViewModel
                {
                    ID = x.ID,
                    Title = x.Title,
                    Description = x.Description,
                    Tag = x.Tag,
                    ApprovedStatus = x.ApprovedStatus,
                    Deadline = x.Deadline.ToString("dd/MM/yyyy"),
                    Status = x.Status,
                    IsBoss = (int?)permission.FirstOrDefault(a => a.ID == userModel.Permission).ID < 3 ? true : false
                }).ToList();
            return new
            {
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
