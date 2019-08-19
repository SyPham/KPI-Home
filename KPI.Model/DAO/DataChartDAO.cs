using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;

namespace KPI.Model.DAO
{
    public class DataChartDAO
    {
        KPIDbContext _dbContext = null;
        public DataChartDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        /// <summary>
        /// Lấy dữ liệu cho chart js.
        /// </summary>
        /// <param name="kpiid"></param>
        /// <param name="kpilevel"></param>
        /// <param name="period"></param>
        /// <returns>Danh sách value theo thời gian</returns>
        public ChartVM ListDatas(string kpilevelcode, string period, int? year, int? start, int? end)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();
            string url = string.Empty;
            if (!string.IsNullOrEmpty(kpilevelcode) && !string.IsNullOrEmpty(period))
            {
                //label chartjs
                var item = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
                var kpi = _dbContext.KPIs.Find(item.KPIID);
                var kpiname = kpi.Name;
                var label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name.ToSafetyString();
                //datasets chartjs
                var model = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode);

                var unit = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Unit;
                var unitName = _dbContext.Units.FirstOrDefault(x => x.ID == unit).Name.ToSafetyString();
                if (period == "W".ToUpper())
                {
                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    int[] datasets = new int[53];

                    //labels của chartjs mặc định có 53 phần tử = 0
                    string[] labels = new string[53];

                    var Dataremarks = new List<Dataremark>();

                    if (year > 0 && start == 0 && end == 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == currentYear && x.Week >= 1 && x.Week <= currentWeek);
                    }
                    if (year > 0 && start > 0 && end > 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == year && x.Week >= start && x.Week <= end);


                        var listValueWeekly = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => new { x.Week, x.Value }).ToList();
                        foreach (var weekly in listValueWeekly)
                        {
                            int valueWeek = weekly.Week;
                            datasets[valueWeek - 1] = weekly.Value;
                        }

                        Dataremarks = model
                           .Where(x => x.Period == "W")
                           .OrderBy(x => x.Week)
                           .Select(x => new Dataremark
                           {
                               ID = x.ID,
                               Value = x.Value,
                               Remark = x.Remark,
                               Week = x.Week
                           }).ToList();
                    }
                    for (int i = 0; i < labels.Length; i++)
                    {
                        labels[i] = (i + 1).ToString();
                    }
                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.WeeklyChecked == true).WeeklyStandard,
                        Dataremarks = Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        kpiname = kpiname,
                        period = "W",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true
                    };
                }
                else if (period == "M".ToUpper())
                {
                    int[] datasets = new int[12];
                    //data: labels chartjs
                    string[] labels = new string[12];

                    var Dataremarks = new List<Dataremark>();
                    if (year >= 0 && start == 0 && end == 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == currentYear && x.Month >= 1 && x.Month <= currentMonth);
                    }
                    if (year > 0 && start > 0 && end > 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == year && x.Month >= start && x.Month <= end);

                        Dataremarks = model
                          .Where(x => x.Period == "M")
                          .OrderBy(x => x.Month)
                          .Select(x => new Dataremark
                          {
                              ID = x.ID,
                              Value = x.Value,
                              Remark = x.Remark,
                              Month = x.Month
                          }).ToList();

                        var datasetsM = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => new { x.Value, x.Month }).ToList();

                        foreach (var month in datasetsM)
                        {
                            int monthValue = month.Month;
                            datasets[monthValue - 1] = month.Value;
                        }
                    }


                    for (int i = 1; i <= 12; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                labels[i - 1] = "Jan";
                                break;
                            case 2:
                                labels[i - 1] = "Feb"; break;
                            case 3:
                                labels[i - 1] = "Mar"; break;
                            case 4:
                                labels[i - 1] = "Apr"; break;
                            case 5:
                                labels[i - 1] = "May";
                                break;
                            case 6:
                                labels[i - 1] = "Jun"; break;
                            case 7:
                                labels[i - 1] = "Jul"; break;
                            case 8:
                                labels[i - 1] = "Aug"; break;
                            case 9:
                                labels[i - 1] = "Sep";
                                break;
                            case 10:
                                labels[i - 1] = "Oct"; break;
                            case 11:
                                labels[i - 1] = "Nov"; break;
                            case 12:
                                labels[i - 1] = "Dec"; break;
                        }
                    }


                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.MonthlyChecked == true).MonthlyStandard,
                        Dataremarks = Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        kpiname = kpiname,
                        period = "M",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true
                    };
                }
                else if (period == "Q".ToUpper())
                {
                    var Dataremarks = new List<Dataremark>();
                    int[] datasets = new int[4];


                    if (year == null && start == null && end == null || year == 0 && start == 0 && end == 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == currentYear && x.Quarter >= 1 && x.Quarter <= currentQuarter);
                    }
                    if (year > 0 && start > 0 && end > 0)
                    {
                        model = model.Where(x => x.CreateTime.Year == year && x.Quarter >= start && x.Quarter <= end);

                        //model = model.Where(x => x.CreateTime.Year == year && x.Quater >= 1 && x.Quater <= currentQuarter);
                        var datasetsQ = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => new { x.Quarter, x.Value }).ToList();
                        foreach (var quarter in datasetsQ)
                        {
                            int quarterValue = quarter.Quarter;
                            datasets[quarterValue - 1] = quarter.Value;
                        }
                        Dataremarks = model
                         .Where(x => x.Period == "Q")
                         .OrderBy(x => x.Quarter)
                         .Select(x => new Dataremark
                         {
                             ID = x.ID,
                             Value = x.Value,
                             Remark = x.Remark,
                             Quater = x.Quarter
                         }).ToList();
                    }
                    string[] labels = new string[4];
                    for (int i = 1; i <= 4; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                labels[i - 1] = "Quarter 1"; break;
                            case 2:
                                labels[i - 1] = "Quarter 2"; break;
                            case 3:
                                labels[i - 1] = "Quarter 3"; break;
                            case 4:
                                labels[i - 1] = "Quarter 4"; break;
                        }
                    }
                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.QuarterlyChecked == true).QuarterlyStandard,
                        Dataremarks = Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        kpiname = kpiname,
                        period = "Q",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true
                    };
                }
                else if (period == "Y".ToUpper())
                {
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Year >= start && x.Year <= end);
                    }
                    var datasets = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Value).ToArray();
                    var Dataremarks = model
                      .Where(x => x.Period == "Y")
                      .OrderBy(x => x.Year)
                      .Select(x => new Dataremark
                      {
                          ID = x.ID,
                          Value = x.Value,
                          Remark = x.Remark,
                          Year = x.Year
                      }).ToList();
                    //data: labels chartjs
                    var listlabels = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                    var labels = Array.ConvertAll(listlabels, x => x.ToSafetyString());

                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.YearlyChecked == true).YearlyStandard,
                        Dataremarks = Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        kpiname = kpiname,
                        period = "Y",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true
                    };
                }
                else
                {
                    return new ChartVM();
                }
            }
            else
            {
                return new ChartVM();
            }
        }

        public ChartVM Compare(string kpilevelcode, string period)
        {
            var model = new ChartVM();
            var item = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
            model.kpiname = _dbContext.KPIs.Find(item.KPIID).Name;
            model.label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name;
            model.kpilevelcode = kpilevelcode;

            var unit = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Unit;
            var unitName = _dbContext.Units.FirstOrDefault(x => x.ID == unit).Name;

            if (period == "W")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Week).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Week).Select(x => x.Week).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
                //model.Unit = unitName;
                //model.Standard = _dbContext.KPILevels.FirstOrDefault(x=>x.KPILevelCode==kpilevelcode && x.WeeklyChecked==true).WeeklyStandard;
            }
            if (period == "M")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Month).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Month).Select(x => x.Month).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
                //model.Unit = unitName;
                //model.Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.MonthlyChecked == true).MonthlyStandard;
            }
            if (period == "Q")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Quarter).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Quarter).Select(x => x.Quarter).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
                model.Unit = unitName;
                model.Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.QuarterlyChecked == true).QuarterlyStandard;
            }
            if (period == "Y")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Year).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
                //model.Unit = unitName;
                //model.Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.YearlyChecked == true).YearlyStandard;
            }
            return model;
        }
        public object Remark(int dataid)
        {
            var model = _dbContext.Datas.FirstOrDefault(x => x.ID == dataid);
            return new
            {
               model= model,
               users= _dbContext.Users.ToList()
             };
        }
        public DataCompareVM Compare(string obj)
        {
            var model = new DataCompareVM();
            obj = obj.ToSafetyString();

            var value = obj.Split('-');
            model.Period = value[1].Split(',')[1];
            var size = value.Length;
            if (size == 2)
            {
                var kpilevelcode1 = value[0].Split(',')[0];
                var period1 = value[1].Split(',')[1];
                var kpilevelcode2 = value[1].Split(',')[0];
                var period2 = value[1].Split(',')[1];
                model.list1 = Compare(kpilevelcode1, period1);
                model.list2 = Compare(kpilevelcode2, period2);

                return model;
            }
            else if (size == 3)
            {
                var kpilevelcode1 = value[0].Split(',')[0];
                var period1 = value[1].Split(',')[1];

                var kpilevelcode2 = value[1].Split(',')[0];
                var period2 = value[1].Split(',')[1];

                var kpilevelcode3 = value[2].Split(',')[0];
                var period3 = value[2].Split(',')[1];
                model.list1 = Compare(kpilevelcode1, period1);
                model.list2 = Compare(kpilevelcode2, period2);
                model.list3 = Compare(kpilevelcode3, period3);
                return model;

            }
            else if (size == 4)
            {
                var kpilevelcode1 = value[0].Split(',')[0];
                var period1 = value[1].Split(',')[1];

                var kpilevelcode2 = value[1].Split(',')[0];
                var period2 = value[1].Split(',')[1];

                var kpilevelcode3 = value[2].Split(',')[0];
                var period3 = value[2].Split(',')[1];

                var kpilevelcode4 = value[3].Split(',')[0];
                var period4 = value[3].Split(',')[1];
                model.list1 = Compare(kpilevelcode1, period1);
                model.list2 = Compare(kpilevelcode2, period2);
                model.list3 = Compare(kpilevelcode3, period3);
                model.list4 = Compare(kpilevelcode4, period4);
                return model;
            }
            else
            {
                return new DataCompareVM();
            }
        }
        public bool UpdateRemark(int dataid, string remark)
        {
            var model = _dbContext.Datas.FirstOrDefault(x => x.ID == dataid);
            try
            {
                model.Remark = remark.ToSafetyString();
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public object SearchUser()
        {
            return "";
        }
        public object LoadCommentChart(int dataid)
        {
            var commentVM = new CommentChartVM();
            var userArray = _dbContext.Users.Select(x => new UserArray
            {
                id = x.ID,
                fullname = x.FullName,
                email = x.Email,
                profile_picture_url = "/Scripts/plugins/jquery-comments/user-icon.png"
            }).ToList();
            commentVM.userArray = userArray;

            var commentChart = _dbContext.CommentCharts
                .Where(x => x.DataID == dataid).FirstOrDefault(x => x.DataID == dataid);

            var commentsArray = _dbContext.CommentChartDetails
            .Select(x => new CommentsArray
            {
                //id = x.ID,
                //parent = x.Parent,
                //content = x.Content,
                //pings = x.Pings.Split(','),
                //creator = x.Creator,
                //created_by_admin = x.CreatedByAdmin,
                //created_by_current_user = x.CreatedByCurrentUser,
                //upvote_count = x.UpvoteCount,
                //user_has_upvoted = x.userHasUpvoted,
                //is_new = x.IsNew
            }).ToList();
            commentVM.commentsArray = commentsArray;

            return commentVM;
        }

        public List<User> SearchUsers()
        {
            var listComment = _dbContext.Users.ToList();
            return listComment;
        }
        public object GetComments(int dataid)
        {
            string[] pings = { };
            var listComment = _dbContext.CommentChartDetails
            .Where(x => x.DataID == dataid).Select(x => new
            {
                id = x.ID,
                created = x.Created,
                creator = x.Creator,
                modified = x.Modified,
                parent = x.Parent,
                content = x.Content,
                pings = x.Pings,
                fullname = x.FullName,
                profile_picture_url = x.ProfilePictureUrl,
                created_by_current_user = x.CreatedByCurrentUser,
                created_by_admin = x.CreatedByAdmin,
                upvote_count = x.UpvoteCount,
                user_has_upvoted = x.userHasUpvoted,
                is_new = x.IsNew,
            }).AsEnumerable().Select(a => new CommentsArray
            {
                id = a.id,
                created = a.created.ToString("MM/dd/yyyy"),
                creator = a.creator,
                modified = a.modified.ToString("MM/dd/yyyy"),
                parent = a.parent,
                content = a.content,
                pings = a.pings == "" ? new string[0] { } : a.pings.Split(';').Select(n => Convert.ToString(n)).ToArray(),
                fullname = a.fullname,
                profile_picture_url = a.profile_picture_url,
                created_by_current_user = a.created_by_current_user,
                created_by_admin = a.created_by_admin,
                upvote_count = a.upvote_count,
                user_has_upvoted = a.user_has_upvoted,
                is_new = a.is_new,
            });

            return listComment.ToList();
        }
        public bool PostComment(CommentsChartVM commentsChartVM, int userid, int dataid)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.ID == userid);
            var item = new CommentChartDetail();
            item.UserID = userid;
            item.DataID = dataid;
            //item.ID = commentsChartVM.id.ToInt();

            item.FullName = user.FullName.ToSafetyString();
            item.ProfilePictureUrl = "/Scripts/plugins/jquery-comments/user-icon.png";
            item.Parent = commentsChartVM.parent.ToInt();
            item.Content = commentsChartVM.content.ToSafetyString();
            item.Pings = string.Join(",", commentsChartVM.pings.ToSafetyString());
            item.Creator = commentsChartVM.creator.ToInt();
            item.CreatedByAdmin = commentsChartVM.created_by_admin;
            item.CreatedByCurrentUser = commentsChartVM.created_by_current_user.ToBool();
            item.UpvoteCount = commentsChartVM.upvote_count.ToInt();
            item.userHasUpvoted = commentsChartVM.user_has_upvoted.ToBool();

            item.Modified = Convert.ToDateTime(commentsChartVM.modified).Date;
            item.Created = Convert.ToDateTime(commentsChartVM.created).Date;
            item.IsNew = commentsChartVM.is_new;

            try
            {
                _dbContext.CommentChartDetails.Add(item);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool PutComment(CommentsChartVM commentsChartVM, int userid, int dataid)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.ID == userid);
            var ID = commentsChartVM.id.ToInt();
            var item = _dbContext.CommentChartDetails.FirstOrDefault(x => x.ID == ID && x.UserID == userid && x.DataID == dataid);
            //var item = new CommentChartDetail();
            item.UserID = userid;
            item.DataID = dataid;
            //item.ID = commentsChartVM.id.ToInt();

            item.FullName = user.FullName.ToSafetyString();
            item.ProfilePictureUrl = "/Scripts/plugins/jquery-comments/user-icon.png";
            item.Parent = commentsChartVM.parent.ToInt();
            item.Content = commentsChartVM.content.ToSafetyString();
            item.Pings = string.Join(",", commentsChartVM.pings.ToSafetyString());
            item.Creator = commentsChartVM.creator.ToInt();
            item.CreatedByAdmin = commentsChartVM.created_by_admin;
            item.CreatedByCurrentUser = commentsChartVM.created_by_current_user.ToBool();
            item.UpvoteCount = commentsChartVM.upvote_count.ToInt();
            item.userHasUpvoted = commentsChartVM.user_has_upvoted.ToBool();

           // item.Modified = Convert.ToDateTime(commentsChartVM.modified).ToDateTime();
            item.Created = Convert.ToDateTime(commentsChartVM.created).ToDateTime();
            item.IsNew = commentsChartVM.is_new;

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
        public bool DeleteComment(int id)
        {
            try
            {
                var item = _dbContext.Datas.FirstOrDefault(x => x.ID == id);
                _dbContext.Datas.Remove(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public bool UpvoteComment()
        {
            return false;
        }
        public bool UploadAttachments()
        {
            return false;
        }
    }
}
