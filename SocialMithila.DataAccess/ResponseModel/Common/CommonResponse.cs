using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.ResponseModel.Common
{
    public class CommonResponse
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public int? id { get; set; }
        public string UserName { get; set; }
        public string UserProfile { get; set; }
    }

    public class CommonResponse2
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
        public int? id { get; set; }
        public int? UserId { get; set; }
        public string ReactionType { get; set; }
        public List<UserReactionDTO> UserReactions { get; set; }
    }

}
