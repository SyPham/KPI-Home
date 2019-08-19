using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.ViewModel
{
    public class CommentChartVM
    {
        public List<CommentsArray> commentsArray { get; set; }
        public List<UserArray> userArray { get; set; }
    }
    public class CommentsArray
    {
        public int id { get; set; }
        public string created { get; set; }

        public int creator { get; set; }
        public string modified { get; set; }

        public int? parent { get; set; }
        public string content { get; set; }

        public string[] pings { get; set; }
        public string fullname { get; set; }

        public string profile_picture_url { get; set; }
        public bool created_by_current_user { get; set; }

        public bool created_by_admin { get; set; }
        public int upvote_count { get; set; }

        public bool user_has_upvoted { get; set; }
        public bool is_new { get; set; }

        public int dataid { get; set; }
        public int userid { get; set; }
    }

    public class CommentsChartVM
    {
        public string id { get; set; }
        public string created { get; set; }

        public int creator { get; set; }
        public string modified { get; set; }

        public int? parent { get; set; }
        public string content { get; set; }

        public string[] pings { get; set; }
        public string fullname { get; set; }

        public string profile_picture_url { get; set; }
        public bool created_by_current_user { get; set; }

        public bool created_by_admin { get; set; }
        public int upvote_count { get; set; }

        public bool user_has_upvoted { get; set; }
        public bool is_new { get; set; }

       
    }

    public class UserArray
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string profile_picture_url { get; set; }
    }

    public class CommentsVM
    {
        public string id { get; set; }
        public int parent { get; set; }

        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public int creator { get; set; }
        public string content { get; set; }
        public string[] pings { get; set; }
        public string fullname { get; set; }
        public string profile_picture_url { get; set; }

        public bool create_by_current_user { get; set; }
        public bool created_by_admin { get; set; }
        public int upvote_count { get; set; }
        public bool user_has_upvoted { get; set; }
        public bool is_new { get; set; }
    }
}
      