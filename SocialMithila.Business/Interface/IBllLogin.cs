using SocialMithila.DataAccess.RequestModel;
using SocialMithila.DataAccess.ResponseModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMithila.Business.Interface
{
    public interface IBllLogin
    {
        CommonResponse Login(LoginDTO login);
        CommonResponse Registration(RegistrationDTO Registration);
    }
}
