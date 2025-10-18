using SocialMithila.Business.Interface;
using SocialMithila.DataAccess.RequestModel;
using SocialMithila.DataAccess.ResponseModel.Common;
using SocialMithila.SharedDataAccess.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data.SqlClient;


namespace SocialMithila.Business.Business
{
    public class BllLogin: IBllLogin
    {
        private readonly AppDbContext _db;
        public BllLogin()
        {
            _db = new AppDbContext();
        }
        public CommonResponse Login(LoginDTO login)
        {
            try
            {
                CommonResponse response = new CommonResponse();
                if (string.IsNullOrEmpty(login.EmailId) || string.IsNullOrEmpty(login.Password))
                {
                    response.success = false;
                    response.msg = "Email and Password are required.";
                    return response;
                }


                var user = (from u in _db.TblUser
                            join p in _db.TbleUserPassword on u.Id equals p.UserId
                            where u.Email == login.EmailId && p.NewPassword == login.Password
                            orderby p.Id descending
                            select new
                            {
                                User = u,
                                Password = p,
                                Id = u.Id,
                                UserName = u.FirstName ?? "" + " " + u.LastName ?? "",
                                UserProfile= string.IsNullOrEmpty(u.ProfilePhoto)? "https://cdn-icons-png.flaticon.com/512/149/149071.png" : u.ProfilePhoto
                            }).FirstOrDefault();

                if (user == null)
                {
                    response.success = false;
                    response.msg = "Invalid Email Id.";
                    return response;
                }

                if (user.Password.IsBlocked)
                {
                    response.success = false;
                    response.msg = "Your account is blocked. Please contact support.";
                    return response;
                }

                if (user.Password.NewPassword != login.Password)
                {
                    response.success = false;
                    response.msg = "Invalid Password.";
                    return response;
                }

                response.success = true;
                response.msg = "Login successful!";
                response.id = user.Id;
                response.UserName = user.UserName;
                response.UserProfile = user.UserProfile;
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public CommonResponse Registration(RegistrationDTO Registration)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var existingUser = db.TblUser.FirstOrDefault(u => u.Email == Registration.EmailId);
                    if (existingUser != null)
                    {
                        return new CommonResponse
                        {
                            success = false,
                            msg = "Email already registered."
                        };
                    }

                   
                    var user = new TblUser
                    {
                        FirstName = Registration.FirstName,
                        Email = Registration.EmailId,
                        IsDeleted= false,
                        IsActive= true
                    };
                    db.TblUser.Add(user);
                    db.SaveChanges();

                    var pwd = new TbleUserPassword
                    {
                        UserId = user.Id,
                        OldPassword =  "",
                        NewPassword = Registration.NewPassword,
                        IsBlocked = false,
                        AddedOn = DateTime.Now
                    };
                    db.TbleUserPassword.Add(pwd);
                    db.SaveChanges();

                    return new CommonResponse
                    {
                        success = true,
                        msg = "Registration successful."
                    };
                }
            }
            catch (Exception ex)
            {
                return new CommonResponse
                {
                    success = false,
                    msg = "Error: " + ex.Message
                };
            }
        }

    }
}
