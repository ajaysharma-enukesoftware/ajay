using System;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Security.Cryptography;
namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Login")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;

        public LoginController(
            ILoginService loginService,
            IMapper mapper,
            IOptions<AppSettings> appSettings, IAuditLogService auditLogService)
        {
            _loginService = loginService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;
        }


        [AllowAnonymous]
        [HttpPost("forgotpassword")]
        public APIResponse ForgotPassword([FromBody]ForgotPasswordModel userInfo)
        {
            try
            {
                string email = userInfo.UserName;
                var key = "E546C8DF278CD5931069B522E695D4F2";
               
                string encr1 = Encryption.EncryptString(email, key);
                string encr = Encryption.cleanUpEncription(encr1);
                string a= $"{this.Request.Scheme}://{this.Request.Host}/api/Login/forgotpassword/";
                string link = a + encr;
                var result = _loginService.ForgotPassword(userInfo.UserName, _appSettings.AdminEmail, _appSettings.SMTPEmail, _appSettings.SMTPPWD, _appSettings.SMTPPort, _appSettings.SMTPServer,link);
             
                if (result != null)
                {
                    return new APIResponse(result.result, "", result.Message);

                }
                else
                {
                    return new APIResponse(false, "", "No UserName Exist");
                }
            }
            catch (Exception e)
            {
                return new APIResponse(false, "", e.Message.ToString());
            }
        }
        
        [AllowAnonymous]
        [HttpGet("forgotpassword/{email}")]
        public APIResponse ForgotPasswordURL(string email)
        {
            try
            {

                User userInformation = new User();
                userInformation = _loginService.userInfoForgotPwd(email);
                if (userInformation != null)
                {
                    return new APIResponse ( true,userInformation, "User Information with details." );

                }
                else
                {
                    return new APIResponse ( false,"", "User Information not found." );
                }
            }
            catch (Exception e)
            {
                return new APIResponse (false, "", e.Message );
            }
        }

        [AllowAnonymous]
        [HttpPost("SendSignupEmail")]
        public APIResponse SendSignupEmail([FromBody]ForgotPassword1Model userInfo)
        {
            try
            {
                var result = _loginService.SendSignupEmail(userInfo.UserName, userInfo.AdminToEmail, userInfo.AdminEmail, userInfo.SMTPEmail, userInfo.SMTPPWD, _appSettings.SMTPPort, _appSettings.SMTPServer);

                if (result != "Fail")
                {
                    return new APIResponse(true, "", result);

                }
                else
                {
                    return new APIResponse(true, "", result);
                }
            }
            catch (Exception e)
            {
                return new APIResponse(true, "", e.Message);
            }
        }

        //[Authorize(Users = "Alice,Bob")]
        [HttpPost("updatepassword")]
        public APIResponse UpdatePassword([FromBody]UpdtePasswordModel userPwd)
        {
            bool result = false;long isAuthenticate;
            try
            {
                if (!string.IsNullOrEmpty(userPwd.email))
                {
                    isAuthenticate = _loginService.userAuthenticateForgotPwd(userPwd.email);
                    if (isAuthenticate!=-1)
                    {
                        result = _loginService.UpdatePassword(userPwd.UserName, userPwd.Password,isAuthenticate);
                        if (!result)
                            return new APIResponse(false, "", "Username is incorrect");
                        else
                        {

                            var claimsIdentity = this.User.Identity as ClaimsIdentity;
                            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                            if (!long.TryParse(userId, out long i))
                            {
                                userId = "0";
                            }
                            AuditLog obj = new AuditLog();
                            obj.Activity = "Update Password";
                            obj.OldRecord = "";
                            obj.Type = "Login";
                            obj.UserID = Convert.ToInt64(userId);
                            obj.NewRecord = JsonConvert.SerializeObject(userPwd);
                            _auditlogService.Create(obj);
                            return new APIResponse(true, "", "Password Updated Successfully");
                        }
                    }
                    else
                    {
                        return new APIResponse(false, "", "Username is not authenticated");

                    }

                }
                else
                {
                    return new APIResponse(false, "","Email Null");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

       


    }
}