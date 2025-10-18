using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.RequestModel
{
    public class CommentsRequestModel
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int? ParentCommentId { get; set; }
        public string CommentText { get; set; }

    }

    



}
