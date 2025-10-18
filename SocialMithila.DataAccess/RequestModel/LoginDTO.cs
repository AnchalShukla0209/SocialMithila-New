using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.DataAccess.RequestModel
{
    public class LoginDTO
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
    }


    public class RegistrationDTO
    {
        public string FirstName { get; set; }
        public string EmailId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
