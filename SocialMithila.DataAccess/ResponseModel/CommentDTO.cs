using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.ResponseModel
{
    
    public class PostCommentResponseDTO
    {
        public long PostId { get; set; }
        public long PosterId { get; set; }
        public string PosterName { get; set; }
        public string PosterProfile { get; set; }
        public int TotalPostCommentCount { get; set; }
        public int TotalPostLikeCount { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        
    }

    public class CommentDTO
    {
        public long CommentId { get; set; }
        public long PostId { get; set; }
        public long UserId { get; set; }
        public long? ParentCommentId { get; set; }

        public string UserName { get; set; }
        public string ProfilePhoto { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedOn { get; set; }

        public int CommentLikeCount { get; set; }
        public int CommentReplyCount { get; set; }
        public int SubReplyCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public List<CommentDTO> SubReply { get; set; } = new List<CommentDTO>();
    }


    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

}
