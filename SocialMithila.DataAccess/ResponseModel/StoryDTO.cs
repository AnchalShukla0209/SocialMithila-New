using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.ResponseModel
{
    public class StoryDTO
    {
        public int UserId { get; set; }
        public int StoryId { get; set; }
        public string UserName { get; set; }
        public string ProfilePic { get; set; }
        public string MediaUrl { get; set; }
        public string MediaType { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
    }

}
