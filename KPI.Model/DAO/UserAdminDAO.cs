using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class UserAdminDAO
    {
        KPIDbContext _dbContext = null;

        public UserAdminDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public int Add(EF.User entity)
        {
            entity.Code = entity.Code.ToSafetyString().ToUpper();
            List<EF.KPILevel> kpiLevelList = new List<EF.KPILevel>();

            try
            {
                entity.Password = entity.Password.SHA256Hash();
                entity.State = true;
                entity.IsActive = true;
                _dbContext.Users.Add(entity);
                _dbContext.SaveChanges();
                IEnumerable<KPIViewModel> kpiVM = from kpi in _dbContext.KPIs
                                                  join cat in _dbContext.Categories on kpi.CategoryID equals cat.ID
                                                  select new KPIViewModel
                                                  {
                                                      KPIID = kpi.ID,
                                                  };
                foreach (var kpi in kpiVM)
                {
                    var kpilevel = new EF.KPILevel();
                    kpilevel.LevelID = entity.ID;
                    kpilevel.KPIID = kpi.KPIID;
                    kpiLevelList.Add(kpilevel);
                }

                _dbContext.KPILevels.AddRange(kpiLevelList);
                _dbContext.SaveChanges();

                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public object GetListAllPermissionsForUser(int userid)
        {
            var model = (from u in _dbContext.Users
                         join p in _dbContext.Permissions on u.Permission equals p.ID
                         join m in _dbContext.Menus on u.Permission equals m.Permission
                         where u.ID == userid
                         select new
                         {
                             UserID = u.ID,
                             PermissionID = p.ID,
                             MenuID = m.ID
                         }).AsEnumerable().ToList();
            return model;
        }
        public bool Checkpermisson(int userid)
        {
            var model = (_dbContext.Permissions.Join(
                _dbContext.Users,
               p => p.ID,
               u => u.Permission,
               (p, u) => new
               {
                   UserID = u.ID,
                   PermissionID = p.ID,

               })).Where(x => x.UserID == userid).FirstOrDefault();

            return model != null ? true : false;
        }
        public object GetListAllPermissions(int userid)
        {
            var model = _dbContext.Permissions.Select(x => new
            {
                x.ID,
                x.PermissionName,
                State = _dbContext.Users.FirstOrDefault(a => a.ID == userid && a.Permission == x.ID) != null ? true : false
            }).ToList();
            return model;
        }
        public object GetAllMenusByPermissionID(int id)
        {
            return _dbContext.Menus.Where(x => x.Permission == id).Select(x => new
            {
                x.ID,
                x.Link,
                x.Name,
                x.Permission,
                State = _dbContext.Resources.FirstOrDefault(a => a.Menu == x.ID) != null ? true : false
            }).ToList();
        }
        public object GetAllMenus()
        {
            return _dbContext.Menus.Select(x => new
            {
                x.ID,
                x.Link,
                x.Name,
                x.Permission
            }).ToList();
        }
        public object LoadDetailMenu(int id)
        {
            return _dbContext.Menus.Where(x => x.Permission == id).ToList();
        }

        public bool Update(EF.User entity)
        {
            var code = entity.Code.ToSafetyString().ToUpper();
            var item = _dbContext.Users.FirstOrDefault(x => x.ID == entity.ID);

            item.Username = entity.Username;
            item.Code = code;
            item.FullName = entity.FullName;
            item.Permission = entity.Permission;
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //logging
                return false;
            }
        }

        public bool LockUser(int id)
        {
            var item = _dbContext.Users.FirstOrDefault(x => x.ID == id);

            item.IsActive = !item.IsActive;
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //logging
                return false;
            }
        }

        public bool ChangePassword(string username, string newpass)
        {
            var item = _dbContext.Users.FirstOrDefault(x => x.Username == username);

            try
            {
                var pass = newpass.ToSafetyString().SHA256Hash();
                item.Password = pass;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //logging
                return false;
            }
        }
        public bool AddUserToLevel(int id, int levelid)
        {
            var itemUser = _dbContext.Users.FirstOrDefault(x => x.ID == id);
            if (itemUser != null)
                itemUser.LevelID = levelid;

            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //logging
                return false;
            }

        }
        public bool Delete(int ID)
        {
            var findUser = _dbContext.Users.FirstOrDefault(x => x.ID == ID);
            try
            {
                _dbContext.Users.Remove(findUser);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }
        public IEnumerable<EF.User> GetAll()
        {
            return _dbContext.Users.Where(x => x.State == true).ToList();
        }
        public EF.User GetbyID(int ID)
        {
            return _dbContext.Users.FirstOrDefault(x => x.ID == ID);
        }


        public object GetPaggedData(int pageNumber = 1, int pageSize = 20)
        {
            List<EF.User> listData = _dbContext.Users.ToList();
            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            return pagedData;
        }
        public object LoadData(string search, int page, int pageSize)
        {
            var model = _dbContext.Users.Select(x => new
            {
                x.ID,
                x.Username,
                x.FullName,
                x.Code,
                PermissionName = _dbContext.Permissions.FirstOrDefault(a => a.ID == x.Permission).PermissionName,
                x.State,
                x.Permission

            });

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().ToSafetyString();
                model = model.Where(x => x.Code.Contains(search) || x.Username == search);
            }

            int totalRow = model.Count();

            model = model.OrderBy(x => x.Permission)
              .Skip((page - 1) * pageSize)
              .Take(pageSize);


            return new
            {
                data = model.ToList(),
                total = totalRow,
                status = true
            };
        }
        public object LoadDataUser(string code, int page, int pageSize)
        {
            IQueryable<EF.User> model = _dbContext.Users.Where(x => x.State == true);
            if (!string.IsNullOrEmpty(code))
            {
                model = model.Where(a => a.Username.Contains(code));
            }
            int totalRow = model.Count();

            model = model.OrderBy(x => x.LevelID)
              .Skip((page - 1) * pageSize)
              .Take(pageSize);


            return new
            {
                data = model,
                total = totalRow,
                status = true
            };
        }
        public int Total()
        {
            return _dbContext.Users.ToList().Count();
        }
        public object LoadDataUser(int teamid, string code, int page, int pageSize)
        {
            IQueryable<EF.User> model = _dbContext.Users.Where(x => x.State == true && x.TeamID == teamid);
            if (!string.IsNullOrEmpty(code))
            {
                model = model.Where(a => a.Username.Contains(code));
            }
            int totalRow = model.Count();

            model = model.OrderBy(x => x.LevelID)
              .Skip((page - 1) * pageSize)
              .Take(pageSize);


            return new
            {
                data = model,
                total = totalRow,
                status = true
            };
        }
        public List<ViewModel.MenuVM> GetListMenuTree()
        {
            var listLevels = _dbContext.Menus.OrderBy(x => x.Name).ToList();
            var menus = new List<ViewModel.MenuVM>();
            foreach (var item in listLevels)
            {
                var menuItem = new ViewModel.MenuVM();
                menuItem.ID = item.ID;
                menuItem.Name = item.Name;
                menuItem.Link = item.Link;
                menuItem.FontAwesome = item.FontAwesome;
                menuItem.BackgroudColor = item.BackgroudColor;
                menuItem.ParentID = item.ParentID;
                menus.Add(menuItem);
            }

            List<ViewModel.MenuVM> hierarchy = new List<ViewModel.MenuVM>();

            hierarchy = menus.Where(c => c.ParentID == 0)
                            .Select(c => new ViewModel.MenuVM()
                            {
                                ID = c.ID,
                                Link = c.Link,
                                Name = c.Name,
                                BackgroudColor = c.BackgroudColor,
                                FontAwesome = c.FontAwesome,
                                children = GetChildren(menus, c.ID)
                            })
                            .ToList();


            HieararchyWalk(hierarchy);

            return hierarchy;
        }
        private List<ViewModel.MenuVM> GetChildren(List<ViewModel.MenuVM> menus, int parentid)
        {
            return menus
                    .Where(c => c.ParentID == parentid)
                    .Select(c => new ViewModel.MenuVM()
                    {
                        ID = c.ID,
                        Link = c.Link,
                        Name = c.Name,
                        BackgroudColor = c.BackgroudColor,
                        FontAwesome = c.FontAwesome,
                        children = GetChildren(menus, c.ID)
                    })
                    .ToList();
        }
        private void HieararchyWalk(List<ViewModel.MenuVM> hierarchy)
        {
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    //Console.WriteLine(string.Format("{0} {1}", item.Id, item.Text));
                    HieararchyWalk(item.children);
                }
            }
        }
    }
}
