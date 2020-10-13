using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace IVPD.Services
{
    public interface ILoginService
    {
        OTPModel ForgotPassword(string username, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMTPort, string SMTPMail,string link);
        bool UpdatePassword(string username, string OTP,long isAuthenticate);
        long userAuthenticateForgotPwd(string encryptEmail);
        User userInfoForgotPwd(string encryptEmail);

        string SendSignupEmail(string username, string AdminToEmail, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMTPort, string SMTPMail);
    }
    public class LoginService : ILoginService
    {
        private IVPDContext _context;

        public LoginService(IVPDContext  context)
        {
            _context = context;
        }

        public bool UpdatePassword(string username, string password,long isAuthenticate)
        {
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;


          
            var user = _context.Users.SingleOrDefault(x => x.Id == isAuthenticate);

            if (user == null)
                    throw new AppException("User not found");

            if (user.Email == username)
            {
                if (!string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    using (AuditScope.Create("UserChange", () => _context.Users))
                    {
                        _context.Users.SingleOrDefault(x => x.Email == username);
                    }

                    CommonFunctions.CreatePasswordHash(password, out passwordHash, out passwordSalt);


                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }
                user.UpdatedAt = DateTime.UtcNow;
                _context.Users.Update(user);
                using (AuditScope.Create("Order:Update", () => _context.Users))
                {
                    _context.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;

            }
        }

        public long userAuthenticateForgotPwd(string encryptEmail)
        {
            try
            {
                var key = "E546C8DF278CD5931069B522E695D4F2";
                string dirtyemail = Encryption.MakeItDirtyAgain(encryptEmail);

                string decryptEmail = Encryption.DecryptString(dirtyemail, key);
                var user = _context.Users.Where(x => x.Email.ToLower() == decryptEmail.ToLower()).ToList().FirstOrDefault();
                if (user != null)
                {
                    return user.Id;
                }
                else
                {
                    return -1;

                }
            }
            catch (Exception ex)
            {
                return -1;
            }

        }
        public User userInfoForgotPwd(string encryptEmail)
        {
            try
            {
                var key = "E546C8DF278CD5931069B522E695D4F2";
                string dirtyemail= Encryption.MakeItDirtyAgain(encryptEmail);
                string decryptEmail = Encryption.DecryptString(dirtyemail, key);
                var user = _context.Users.Where(x => x.Email.ToLower() == decryptEmail.ToLower()).ToList().FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
               
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public OTPModel ForgotPassword(string username, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMTPort, string SMTPMail,string link)
        {
            if (string.IsNullOrEmpty(username))
               return new OTPModel { result = false, Message = "User Name can not be empty!Please provide the user name." };

            var user = _context.Users.SingleOrDefault(x => x.Email == username);

            // check if username exists
            if (user == null)
           return new OTPModel { result = false, Message = "User not found. Please provide a valid user name." };

            else
            {              
                CommonFunctions.SendLink(user.FullName.Split(' ').First(), user.FullName.Split(' ').Last(), user.Email, link, AdminEmail, SMTPEmail, SMTPPwd, SMTPort, SMTPMail);
                return new OTPModel { result = true,Message = "Link sent on registered email.Please login and verify" }; 

            }
             
        }
     



        public string SendSignupEmail(string username, string AdminToEmail, string AdminEmail, string SMTPEmail, string SMTPPwd,int SMTPort, string SMTPMail)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Email == username);
                CommonFunctions.SendSignUpEmail(user.FullName.Split(' ').First(), user.FullName.Split(' ').Last(),user.Email, AdminToEmail, AdminEmail, SMTPEmail, SMTPPwd, SMTPort,SMTPMail);
                return "Success";
            }
            catch(Exception ex)
            {
                return "Fail";
            }
            
        }
      
    }
}