using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.ResponseModel
{
    public class PostDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string ContentText { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<PostMediaDto> Media { get; set; }
    }

    public class PostMediaDto
    {
        public string MediaUrl { get; set; }
        public string MediaType { get; set; }
    }

}
