using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Loginsso")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LoginSSOController : ControllerBase
    {
        private ILoginService _loginService;
        private IMapper _mapper;
        private AppSettings _appSettings;
        private IAuditLogService _auditlogService;
        IConfiguration _configuration;

        public LoginSSOController(
            ILoginService loginService,
            IMapper mapper,
            IOptions<AppSettings> appSettings, IConfiguration configuration, IAuditLogService auditLogService)
        {
            _loginService = loginService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public APIResponse Authenticate()
        {
            try
            {
                string URL = "https://login.microsoftonline.com/" + _appSettings.TenantID + "/oauth2/authorize?client_id=" + _appSettings.ClientID + "&response_type=code&response_mode=query&prompt=admin_consent&resource_id=" + _appSettings.ResourceID + "";
                return new APIResponse(true, URL, "Login SSO URL successfully generated!");
            }
            catch (AppException ex)
            {
                return new APIResponse(false, "", ex.Message.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public APIResponse Post(string code, string session_state, bool admin_consent)
        {
            try
            {
                string URL = "https://login.microsoftonline.com/" + _appSettings.TenantID + "/oauth2/token";
                using (HttpClient client = new HttpClient())
                {
                    var parameters = new Dictionary<string, string> { { "code", code }, { "grant_type", "authorization_code" },
                    { "client_id", _appSettings.ClientID }, { "client_secret", _appSettings.ClientSecret }};

                    var encodedContent = new FormUrlEncodedContent(parameters);
                    try
                    {
                        HttpResponseMessage response = client.PostAsync("https://login.microsoftonline.com/" + _appSettings.TenantID + "/oauth2/token", encodedContent).Result;
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().Result;

                        if (responseBody.Contains("token_type"))
                        {
                            Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
                            string tokenSSO = "";
                            if (keyValuePairs.TryGetValue("access_token", out tokenSSO))
                            {

                                var jwt = tokenSSO;
                                var tokenHandler = new JwtSecurityTokenHandler();
                                var token = tokenHandler.ReadJwtToken(jwt);
                                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                                
                                var credentials = new SigningCredentials
                                               (key, SecurityAlgorithms.HmacSha256Signature);
                                var header = new JwtHeader(credentials);
                                var payload = token.Payload;


                                var secToken = new JwtSecurityToken(header, payload);

                                var tokenString = tokenHandler.WriteToken(secToken);
                                return new APIResponse(true, tokenString, "Token generated successfully!");
                            }
                            else
                            {
                                return new APIResponse(false, "", "Token not generated!");  
                            }
                        }
                        else if (responseBody.Contains("error"))
                        {
                            Dictionary<string, string> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
                            string errorSSO = "";
                            if (keyValuePairs.TryGetValue("error_description", out errorSSO))
                            {
                                return new APIResponse(false, "", errorSSO);
                            }
                            else
                            {
                                return new APIResponse(false, "", "Something went wrong!");
                            }
                        }
                        else
                        {
                            return new APIResponse(false, "", "Something went wrong!");
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        return new APIResponse(false, "", Convert.ToString(e.Message));
                    }
                }
            }
            catch (AppException ex)
            {
                return new APIResponse(false, "", ex.Message.ToString());
            }
        }
    }
}