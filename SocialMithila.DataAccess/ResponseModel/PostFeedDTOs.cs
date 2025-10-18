using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.ResponseModel
{
    //public class PostFeedDTO
    //{
    //    public long PostId { get; set; }
    //    public int UserId { get; set; }
    //    public string UserName { get; set; }
    //    public string ProfilePhoto { get; set; }
    //    public string Description { get; set; }
    //    public List<MediaDTO> MediaFiles { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //}

    public class MediaDTO
    {
        public string Url { get; set; }
        public string Type { get; set; }
    }

    public class PostFeedDTOs
    {
        public long PostId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePhoto { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<MediaDTO> MediaFiles { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }

        public string UserReaction { get; set; }
        public List<UserReactionDTO> UserReactions { get; set; } = new List<UserReactionDTO>();
    }

    public class UserReactionDTO
    {
        public string UserName { get; set; }
        public string ReactionType { get; set; }
    }
}
