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
    public class KPILevelDAO
    {
        KPIDbContext _dbContext = null;
        public KPILevelDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public KPILevel GetByID(int id)
        {
            return _dbContext.KPILevels.Where(x => x.ID == id).FirstOrDefault();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Update các cột WeeklyChecked, MonthlyChecked, QuarterlyChecked, YearlyChecked</returns>
        public bool Update(EF.KPILevel entity)
        {
            var comparedt = new DateTime(2001, 1, 1);
            var kpiLevel = _dbContext.KPILevels.FirstOrDefault(x => x.KPIID == entity.KPIID && x.LevelID == entity.LevelID);
            if (entity.Weekly != null)
            {
                kpiLevel.Weekly = entity.Weekly;
            }
            if (DateTime.Compare(entity.Monthly.Value, comparedt) != 0)
            {
                kpiLevel.Monthly = entity.Monthly;
            }
            if (DateTime.Compare(entity.Quarterly.Value, comparedt) != 0)
            {
                kpiLevel.Quarterly = entity.Quarterly;
            }
            if (DateTime.Compare(entity.Yearly.Value, comparedt) != 0)
            {
                kpiLevel.Yearly = entity.Yearly;
            }
            if (entity.WeeklyChecked != null)
            {
                kpiLevel.WeeklyChecked = entity.WeeklyChecked;
            }
            if (entity.MonthlyChecked != null)
            {
                kpiLevel.MonthlyChecked = entity.MonthlyChecked;
            }
            if (entity.QuarterlyChecked != null)
            {
                kpiLevel.QuarterlyChecked = entity.QuarterlyChecked;
            }
            if (entity.MonthlyChecked != null)
            {
                kpiLevel.MonthlyChecked = entity.MonthlyChecked;
            }
            if (entity.YearlyChecked != null)
            {
                kpiLevel.YearlyChecked = entity.YearlyChecked;
            }
            if (entity.WeeklyPublic != null)
            {
                kpiLevel.WeeklyPublic = entity.WeeklyPublic;
            }
            if (entity.MonthlyPublic != null)
            {
                kpiLevel.MonthlyPublic = entity.MonthlyPublic;
            }
            if (entity.QuarterlyPublic != null)
            {
                kpiLevel.QuarterlyPublic = entity.QuarterlyPublic;
            }
            if (entity.YearlyPublic != null)
            {
                kpiLevel.YearlyPublic = entity.YearlyPublic;
            }
            if (entity.Checked != null)
            {
                kpiLevel.Checked = entity.Checked;
                kpiLevel.KPILevelCode = entity.KPILevelCode;
            }
            if (entity.WeeklyStandard != 0)
            {
                kpiLevel.WeeklyStandard = entity.WeeklyStandard;
            }
            if (entity.MonthlyStandard != 0)
            {
                kpiLevel.MonthlyStandard = entity.MonthlyStandard;
            }
            if (entity.QuarterlyStandard != 0)
            {
                kpiLevel.QuarterlyStandard = entity.QuarterlyStandard;
            }
            if (entity.YearlyStandard != 0)
            {
                kpiLevel.YearlyStandard = entity.YearlyStandard;
            }
            kpiLevel.UserCheck = entity.UserCheck;
            kpiLevel.TimeCheck = entity.TimeCheck;
            //kpiLevel.Code = entity.Code;

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
        public int Total()
        {
            return _dbContext.KPILevels.Where(x => x.Checked == true).ToList().Count();
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns>Lấy danh sách các KPILevel</returns>
        public IEnumerable<EF.KPILevel> GetAll()
        {
            return _dbContext.KPILevels.ToList();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns>Dnah sách các level theo điều kiện</returns>
        public IEnumerable<EF.KPILevel> GetAllByID(int levelID)
        {
            return _dbContext.KPILevels.Where(x => x.LevelID == levelID).ToList();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Tìm kiếm KPILevel theo ID</returns>
        public EF.KPILevel GetbyID(int ID)
        {
            return _dbContext.KPILevels.FirstOrDefault(x => x.ID == ID);
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns>Danh sách tất cả các record trong bảng Category</returns>
        public IEnumerable<EF.Category> GetAllCategory()
        {
            return _dbContext.Categories.ToList();
        }
        /// <summary>
        /// Lấy ra danh sách tất cả các KPILevel
        /// </summary>
        /// <param name="levelID"></param>
        /// <param name="categoryID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>Danh sách KPI theo điều kiện</returns>
        public object LoadData(int levelID, int categoryID, int page, int pageSize = 3)
        {
            var model = from kpiLevel in _dbContext.KPILevels
                        where kpiLevel.LevelID == levelID
                        join kpi in _dbContext.KPIs on kpiLevel.KPIID equals kpi.ID
                        join unit in _dbContext.Units on kpi.Unit equals unit.ID
                        join level in _dbContext.Levels on kpiLevel.LevelID equals level.ID
                        select new ViewModel.KPILevelVM
                        {
                            ID = kpiLevel.ID,
                            KPILevelCode = kpiLevel.KPILevelCode,
                            UserCheck = kpiLevel.KPILevelCode,
                            KPIID = kpiLevel.KPIID,
                            KPICode = kpi.Code,
                            LevelID = kpiLevel.LevelID,
                            LevelNumber = kpi.LevelID,
                            Period = kpiLevel.Period,

                            Weekly = kpiLevel.Weekly,
                            Monthly = kpiLevel.Monthly,
                            Quarterly = kpiLevel.Quarterly,
                            Yearly = kpiLevel.Yearly,

                            Checked = kpiLevel.Checked,
                            WeeklyChecked = kpiLevel.WeeklyChecked,
                            MonthlyChecked = kpiLevel.MonthlyChecked,
                            QuarterlyChecked = kpiLevel.QuarterlyChecked,
                            YearlyChecked = kpiLevel.YearlyChecked,
                            CheckedPeriod = kpiLevel.CheckedPeriod,

                            TimeCheck = kpiLevel.TimeCheck,

                            CreateTime = kpiLevel.CreateTime,
                            Unit = unit.Name,
                            CategoryID = kpi.CategoryID,
                            KPIName = kpi.Name,
                            LevelCode = level.Code,
                        };
            if (categoryID != 0)
            {
                model = model.Where(x => x.CategoryID == categoryID);
            }

            int totalRow = model.Count();

            model = model.OrderByDescending(x => x.CreateTime)
              .Skip((page - 1) * pageSize)
              .Take(pageSize);


            return new
            {
                data = model,
                total = totalRow,
                status = true,
                page,
                pageSize
            };
        }
        /// <summary>
        /// Lấy ra danh sách những KPI có checked bằng true.
        /// </summary>
        /// <param name="levelID"></param>
        /// <param name="categoryID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>Danh sách KPILevel có checked bằng true</returns>
        public object LoadDataForUser(int levelID, int categoryID, int page, int pageSize = 3)
        {
            //Lấy các tuần tháng quý năm hiện tại
            var weekofyear = DateTime.Now.GetIso8601WeekOfYear();
            var monthofyear = DateTime.Now.Month;
            var quarterofyear = DateTime.Now.GetQuarterOfYear();
            var year = DateTime.Now.Year;
            var currentweekday = DateTime.Now.DayOfWeek.ToSafetyString().ToUpper().ConvertStringDayOfWeekToNumber();
            var currentdate = DateTime.Now.Date;
            var dt = new DateTime(2019, 8, 1);
            var value = DateTime.Compare(currentdate, dt);
            try
            {
                //Lấy ra danh sách data từ trong db
                var datas = _dbContext.Datas;
                var model = from kpiLevel in _dbContext.KPILevels
                            where kpiLevel.LevelID == levelID && kpiLevel.Checked == true
                            join kpi in _dbContext.KPIs on kpiLevel.KPIID equals kpi.ID
                            join level in _dbContext.Levels on kpiLevel.LevelID equals level.ID
                            select new KPILevelVM
                            {
                                ID = kpiLevel.ID,
                                KPILevelCode = kpiLevel.KPILevelCode,
                                UserCheck = kpiLevel.KPILevelCode,
                                KPIID = kpiLevel.KPIID,
                                KPICode = kpi.Code,
                                LevelID = kpiLevel.LevelID,
                                LevelNumber = kpi.LevelID,
                                Period = kpiLevel.Period,

                                Weekly = kpiLevel.Weekly,
                                Monthly = kpiLevel.Monthly,
                                Quarterly = kpiLevel.Quarterly,
                                Yearly = kpiLevel.Yearly,

                                Checked = kpiLevel.Checked,

                                WeeklyChecked = kpiLevel.WeeklyChecked,
                                MonthlyChecked = kpiLevel.MonthlyChecked,
                                QuarterlyChecked = kpiLevel.QuarterlyChecked,
                                YearlyChecked = kpiLevel.YearlyChecked,
                                CheckedPeriod = kpiLevel.CheckedPeriod,

                                //true co du lieu false khong co du lieu
                                StatusEmptyDataW = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.WeeklyChecked == true ? "W" : "")) != null ? true : false,
                                StatusEmptyDataM = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.MonthlyChecked == true ? "M" : "")) != null ? true : false,
                                StatusEmptyDataQ = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.QuarterlyChecked == true ? "Q" : "")) != null ? true : false,
                                StatusEmptyDataY = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.YearlyChecked == true ? "Y" : "")) != null ? true : false,

                                TimeCheck = kpiLevel.TimeCheck,
                                CreateTime = kpiLevel.CreateTime,

                                CategoryID = kpi.CategoryID,
                                KPIName = kpi.Name,
                                LevelCode = level.Code,
                                //Nếu tuần hiện tại - tuần MAX trong bảng DATA > 1 return false,
                                //ngược lại nếu == 1 thi kiểm tra thứ trong bảng KPILevel,
                                //Nếu thứ nhỏ hơn thứ hiện tại thì return true,
                                //ngược lại reutrn false
                                StatusUploadDataW = weekofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W").Max(x => x.Week) > 1 ? false : ((weekofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W").Max(x => x.Week)) == 1 ? (kpiLevel.Weekly < currentweekday ? true : false) : false),

                                StatusUploadDataM = monthofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "M").Max(x => x.Month) > 1 ? false : monthofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "M").Max(x => x.Month) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Monthly.Value) < 0 ? true : false) : false,

                                StatusUploadDataQ = quarterofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Q").Max(x => x.Quarter) > 1 ? false : quarterofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Q").Max(x => x.Quarter) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Quarterly.Value) < 0 ? true : false) : false, //true dung han flase tre han

                                StatusUploadDataY = year - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Y").Max(x => x.Year) > 1 ? false : year - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Y").Max(x => x.Year) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Yearly.Value) < 0 ? true : false) : false,

                            };



                if (categoryID != 0)
                {
                    model = model.Where(x => x.CategoryID == categoryID);
                }

                int totalRow = model.Count();

                model = model.OrderByDescending(x => x.CreateTime)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize);


                return new
                {
                    data = model,
                    total = totalRow,
                    status = true
                };
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return "";
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kpilevelcode"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public object ListDatas(string kpilevelcode, string period)
        {
            if (!string.IsNullOrEmpty(kpilevelcode) && !string.IsNullOrEmpty(period))
            {
                //label chartjs
                var item = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
                var label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name;
                //datasets chartjs
                var model = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode);

                if (period == "W".ToUpper())
                {

                    var datasets = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Week).ToArray();


                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else if (period == "M".ToUpper())
                {

                    var datasets = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Month).ToArray();
                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else if (period == "Q".ToUpper())
                {
                    var datasets = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Quarter).ToArray();
                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else if (period == "Y".ToUpper())
                {

                    var datasets = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public bool AddComment(AddCommentViewModel entity)
        {
            try
            {
                //add vao comment
                var comment = new Comment();
                comment.CommentMsg = entity.CommentMsg;
                comment.DataID = entity.DataID;
                comment.UserID = entity.UserID;
                _dbContext.Comments.Add(comment);
                _dbContext.SaveChanges();

                //Add vao seencomment
                var seenComment = new SeenComment();
                seenComment.CommentID = comment.ID;
                seenComment.UserID = entity.UserID;
                seenComment.Status = true;
                _dbContext.SeenComments.Add(seenComment);
                _dbContext.SaveChanges();
                //Add vao Tag
               
                if (entity.Tag.IsNullOrEmpty())
                {
                    var itemtag = new Tag();
                    var user = _dbContext.Users;
                    if (entity.Tag.IndexOf(',') == -1)
                    {
                        itemtag = new Tag();
                        itemtag.UserID = (int?)user.FirstOrDefault(x => x.Username == entity.Tag).ID ?? 0;
                        itemtag.CommentID = comment.ID;
                        _dbContext.Tags.Add(itemtag);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        var list = entity.Tag.Split(',');
                        var commentID = comment.ID;
                        foreach (var item in list)
                        {
                            var username = item.ToSafetyString();
                            itemtag.UserID = (int?)user.FirstOrDefault(x => x.Username == username).ID ?? 0;
                            itemtag.CommentID = commentID;
                            //listTag.Add(itemtag);
                            _dbContext.Tags.Add(itemtag);
                            _dbContext.SaveChanges();

                        }
                    }
                }
                
               
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool AddCommentHistory(int userid, int dataid)
        {
            try
            {
                var comments = _dbContext.Comments.Where(x => x.DataID == dataid).ToList();
                foreach (var comment in comments)
                {
                    var item = _dbContext.SeenComments.FirstOrDefault(x => x.UserID == userid && x.CommentID == comment.ID);
                    if (item == null)
                    {
                        var seencmt = new SeenComment();
                        seencmt.CommentID = comment.ID;
                        seencmt.UserID = userid;
                        seencmt.Status = true;
                        _dbContext.SeenComments.Add(seencmt);
                        _dbContext.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Lấy ra các danh sách comment theo từng Value của KPILevel
        /// </summary>
        /// <param name="dataid">Là giá trị của KPILevel upload</param>
        /// <returns>Trả về các comment theo dataid</returns>
        public object ListComments(int dataid, int userid)
        {

            var actionPlan = _dbContext.ActionPlans;
            //Cat chuoi
            //lay tat ca comment cua kpi
            var listcmts = _dbContext.Comments.Where(x => x.DataID == dataid).ToList();

            //Tong tat ca cac comment cua kpi
            var totalcomment = listcmts.Count();

            //Lay ra tat ca lich su cmt
            var seenCMT = _dbContext.SeenComments;

            //Lay ra tat ca lich su cmt
            var user = _dbContext.Users;

            //Lay ra tat ca cac comment cua kpi(userid nao post comment len thi mac dinh userid do da xem comment cua chinh minh roi)
            var data = _dbContext.Comments.Where(x => x.DataID == dataid)
               .Select(x => new CommentVM
               {
                   CommentID = x.ID,
                   UserID = x.UserID,
                   CommentMsg = x.CommentMsg,
                   //KPILevelCode = x.KPILevelCode,
                   CommentedDate = x.CommentedDate,
                   FullName = user.FirstOrDefault(a => a.ID == x.UserID).FullName,
                   //Period = x.Period,
                   Read = seenCMT.FirstOrDefault(a => a.CommentID == x.ID && a.UserID == userid) == null ? true : false,
                   IsHasTask = actionPlan.FirstOrDefault(a => a.DataID == dataid && a.CommentID == x.ID) == null ? false : true,
                   Task = actionPlan.FirstOrDefault(a => a.DataID == dataid && a.CommentID == x.ID) == null ? false : true
               })
               .OrderByDescending(x => x.CommentedDate)
               .ToList();

            return new
            {
                data,
                total = _dbContext.Comments.Where(x => x.DataID == dataid).Count()
            };

        }

        //public object LoadData(string obj)
        //{
        //    var value = obj.ToSafetyString().Split(',');
        //    var code = value[0].Substring(0, value[0].Length - 1).ToSafetyString();
        //    var period = value[0].Substring(value[0].Length - 1, 1).ToUpper().ToSafetyString();
        //    var userid = value[1].ToInt();
        //    var data = _dbContext.Comments
        //       .Where(x => x.KPILevelCode == code)
        //       .Select(x => new CommentVM
        //       {
        //           CommentMsg = x.CommentMsg,
        //           KPILevelCode = x.KPILevelCode,
        //           CommentedDate = x.CommentedDate,
        //           FullName = _dbContext.Users.FirstOrDefault(a => a.ID == x.UserID).FullName,
        //           Period = x.Period,
        //           Read = _dbContext.SeenComments.FirstOrDefault(b => b.CommentID == x.ID && b.UserID == userid).Status
        //       })
        //       .OrderByDescending(x => x.CommentedDate)
        //       .Take(4).ToList();

        //    return new
        //    {
        //        data,
        //        total = _dbContext.Comments.Where(x => x.KPILevelCode == code).Count()
        //    };

        //}
        /// <summary>
        /// Lấy ra danh sách để so sánh chart với nhau.
        /// </summary>
        /// <param name="obj">Chuỗi dữ liệu gồm KPIlevelcode, Period của các KPILevel</param>
        /// <returns>Trả về danh sách so sánh dữ liệu cùng cấp. So sánh tối đa 4 KPILevel</returns>
        public object LoadDataProvide(string obj, int page, int pageSize)
        {
            var listCompare = new List<CompareVM>();
            var value = obj.ToSafetyString().Split(',');
            var kpilevelcode = value[0].ToSafetyString();
            var period = value[1].ToSafetyString();

            var itemkpilevel = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
            var itemlevel = _dbContext.Levels.FirstOrDefault(x => x.ID == itemkpilevel.LevelID).LevelNumber;
            var kpiid = itemkpilevel.KPIID;
            //Lay ra tat ca kpiLevel cung levelNumber

            if (period == "W")
            {

                listCompare = _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.WeeklyChecked == true && !x.KPILevelCode.Contains(kpilevelcode))
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true,
                        StatusPublic = (bool?)x.WeeklyPublic ?? false
                    }).
                    Where(c => c.LevelNumber == itemlevel).ToList();

                int totalRow = listCompare.Count();
                listCompare = listCompare.OrderByDescending(x => x.LevelNumber)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return new
                {
                    total = totalRow,
                    listCompare
                };
            }

            if (period == "M")
            {
                listCompare = _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.MonthlyChecked == true && !x.KPILevelCode.Contains(kpilevelcode))
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true,
                        StatusPublic = (bool?)x.MonthlyPublic ?? false
                    }).
                    Where(c => c.LevelNumber == itemlevel)
                    .ToList();

                int totalRow = listCompare.Count();
                listCompare = listCompare.OrderByDescending(x => x.LevelNumber)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return new
                {
                    total = totalRow,
                    listCompare
                };
            }

            if (period == "Q")
            {
                listCompare = _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.QuarterlyChecked == true && !x.KPILevelCode.Contains(kpilevelcode))
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true,
                        StatusPublic = (bool?)x.QuarterlyPublic ?? false
                    }).
                    Where(c => c.LevelNumber == itemlevel)
                    .ToList();

                int totalRow = listCompare.Count();
                listCompare = listCompare.OrderByDescending(x => x.LevelNumber)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return new
                {
                    total = totalRow,
                    listCompare
                };
            }

            if (period == "Y")
            {
                listCompare = _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.YearlyChecked == true && !x.KPILevelCode.Contains(kpilevelcode))
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true,
                        StatusPublic = (bool?)x.YearlyPublic ?? false
                    }).
                    Where(c => c.LevelNumber == itemlevel)
                    .ToList();

                int totalRow = listCompare.Count();
                listCompare = listCompare.OrderByDescending(x => x.LevelNumber)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return new
                {
                    total = totalRow,
                    listCompare
                };
            }
            //Lay tat ca kpilevel cung period

            return new
            {
                listCompare
            };
        }

    }
}

