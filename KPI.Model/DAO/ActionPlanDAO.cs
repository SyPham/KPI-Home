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
        public AddCommentVM Add(ActionPlan entity, string subject)
        {
            var user = _dbContext.Users;
            var itemActionPlanDetail = new ActionPlanDetail();
            var listEmail = new List<string[]>();
            var listUserID = new List<int>();
            var listFullNameTag = new List<string>();
            var listTags = new List<Tag>();
            var itemTag = _dbContext.Tags;


            try
            {
                _dbContext.ActionPlans.Add(entity);
                _dbContext.SaveChanges();


                if (!entity.Tag.IsNullOrEmpty())
                {
                    string[] arrayString = new string[5];
                  

                    if (entity.Tag.IndexOf(",") == -1)
                    {
                        var userItem = user.FirstOrDefault(x => x.Username == entity.Tag);

                        if (userItem != null)
                        {
                            var tag = new Tag();
                            tag.ActionPlanID = entity.ID;
                            tag.UserID = (int?)userItem.ID ?? 0;
                            _dbContext.Tags.Add(tag);
                            _dbContext.SaveChanges();

                            arrayString[0] = user.FirstOrDefault(x => x.ID == entity.UserID).FullName;
                            arrayString[1] = userItem.Email;
                            arrayString[2] = entity.Link;
                            arrayString[3] = entity.Title;
                            arrayString[4] = entity.Description;
                            listFullNameTag.Add(userItem.FullName);
                            listEmail.Add(arrayString);
                        }
                    }
                    else
                    {
                        var list = entity.Tag.Split(',');
                        var listUsers = _dbContext.Users.Where(x => list.Contains(x.Username)).ToList();
                        foreach (var item in listUsers)
                        {
                            var tag = new Tag();
                            tag.ActionPlanID = entity.ID;
                            tag.UserID = item.ID;
                            listTags.Add(tag);

                            arrayString[0] = user.FirstOrDefault(x => x.ID == entity.UserID).FullName;
                            arrayString[1] = item.Email;
                            arrayString[2] = entity.Link;
                            arrayString[3] = entity.Title;
                            arrayString[4] = entity.Description;
                            listFullNameTag.Add(item.FullName);
                            listEmail.Add(arrayString);
                        }
                        _dbContext.Tags.AddRange(listTags);
                        _dbContext.SaveChanges();
                    }
                }

                //Add vao Notification
                var notify = new Notification();
                notify.ActionplanID = entity.ID;
                notify.Content = entity.Description;
                notify.UserID = entity.UserID;
                notify.Title = entity.Title;
                notify.Link = entity.Link;
                notify.Tag = string.Join(",", listFullNameTag);
                notify.Title = subject;
                new NotificationDAO().Add(notify);



                return new AddCommentVM
                {
                    Status = true,
                    ListEmails = listEmail
                };
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return new AddCommentVM
                {
                    Status = true,
                    ListEmails = listEmail
                };
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
        public object GetAll(int DataID, int CommentID, int userid)
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
        public List<ActionPlanVM> CheckDeadline()
        {
            var currentDate = DateTime.Now;
            var timeSpan = new TimeSpan(24, 00, 00);
            var date = currentDate - timeSpan;
            var listAc = new List<ActionPlanVM>();
            var itemAc = new ActionPlanVM();
            var list = from a in _dbContext.ActionPlans
                       join b in _dbContext.Tags on a.ID equals b.ActionPlanID into ab
                       from c in ab.DefaultIfEmpty()
                       join d in _dbContext.Users on c.UserID equals d.ID
                       select new ActionPlanVM
                       {
                           ActionplanID = a.ID,
                           UserID = c.UserID,
                           Email = d.Email,
                           Deadline = a.Deadline
                       };
            var model = list.ToList();
            foreach (var item in model)
            {
                if (DateTime.Compare(date, item.Deadline) == 0)
                {
                    itemAc.ActionplanID = item.ActionplanID;
                    itemAc.UserID = item.ActionplanID;
                    itemAc.Deadline = item.Deadline;
                    itemAc.Email = item.Email;
                    listAc.Add(itemAc);
                }
            }
            return listAc;
        }
    }
}
