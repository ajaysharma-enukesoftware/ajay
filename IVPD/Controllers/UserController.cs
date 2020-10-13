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

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IUserGroupService _userGroupService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        IConfiguration _configuration;
        private IAuditLogService _auditlogService;
        
        public UserController(
            IUserService userService,
            IUserGroupService userGroupService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IConfiguration configuration, IAuditLogService auditLogService)
        {
            _userGroupService = userGroupService;
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _configuration = configuration;
            _auditlogService = auditLogService;

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public APIResponse Authenticate([FromBody] AuthenticateModel model)
        {
            try
            {
                var user = _userService.Authenticate(model.UserName, model.Password);

                if (user == null)
                    return new APIResponse(false, "", "Username or password is incorrect");
                if (user != null && user.DeletedAt != null)
                {
                    return new APIResponse(false, "", "User not found");
                }
                string IsActive = _userService.CheckStatus(model.UserName, model.Password);
                if (IsActive=="true")
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    IdentityModelEventSource.ShowPII = true;
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    // return basic user info and authentication token
                    return new APIResponse(true,
                        new
                        {
                            user = new
                            {
                                id = user.Id,
                                username = user.Email,
                                fullName = user.FullName,
                                token = tokenString,
                                usertype = "A"
                            }
                        },
                       "Login Success"
                    );
                }
                else
                {
                    return new APIResponse(false, "", "User Status is Inactive");

                }
            }
            catch (AppException ex)
            {
                return new APIResponse(false, "", ex.Message.ToString());
            }
        }



        [AllowAnonymous]
        [HttpPost("Create/{id:long?}")]
        public APIResponse Register([FromBody] SignupModel model, Int64 id)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                if (id == 0)
                {
                    _userService.Create(user, model.Password);
                    List<User> users = _userService.GetAll().ToList();
                    _userGroupService.Create(users.Last().Id, model.AssignGroup);

                    return new APIResponse(true, "", "User Created Successfully");
                }
                else
                {
                    user.Id = id;
                    _userService.Update(user, model.Password);
                    _userGroupService.Update(id, model.AssignGroup);


                    return new APIResponse(true, "", "User Updated Successfully");
                }
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message); ;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public APIListResponse Get(int? page = null, int? pageSize = 10)
        {
            Pagination p = new Pagination();
            try
            {
                if (page == null)
                {
                    page = 1;
                }
                IEnumerable<User> AllUsers = _userService.GetAll();
                int TotalCount = AllUsers.Count();
                var UserData = AllUsers.Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize);

                List<NewUser> newUsers = new List<NewUser>();


                foreach (IVPD.Models.User user1 in UserData)
                {
                    NewUser newUser = new NewUser();
                    newUser.Id = user1.Id;
                    newUser.Name = user1.FullName;
                    newUser.Email = user1.Email;
                    newUser.Status = user1.Status;
                    newUser.LastLoginDate = user1.UpdatedAt;
                    newUsers.Add(newUser);
                }

                p.CurrentPage = (long)page;
                p.Total = TotalCount;
                p.Limit = (long)pageSize;
                return new APIListResponse(true, newUsers, p, "User Listed");
            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody] FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<User> AllUsers = _userService.GetAll();
                var UserData = new List<User>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "id";
                }
                var propertyInfo = typeof(User).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllUsers = AllUsers.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                UserData = AllUsers.ToList();
                //Pagination
                long TotalCount = AllUsers.Count();
                p.Limit = TotalCount;
                p.CurrentPage = 1;
                p.Total = TotalCount;
                if ((bool)fc.IsPagination)
                {
                    if (fc.Page == null)
                    {
                        fc.Page = 1;
                    }
                    if (fc.PageSize == null)
                    {
                        fc.PageSize = 10;
                    }
                    UserData = UserData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, UserData, p, "User Listed");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

        [HttpPost("GetAllUserType")]
        public APIListResponse GetAllUserType([FromBody] FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<UserTypes> AllUsers = _userService.GetAllUserType();
                var UserData = new List<UserTypes>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "id";
                }
                var propertyInfo = typeof(User).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllUsers = AllUsers.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                UserData = AllUsers.ToList();
                //Pagination
                long TotalCount = AllUsers.Count();
                p.Limit = TotalCount;
                p.CurrentPage = 1;
                p.Total = TotalCount;
                if ((bool)fc.IsPagination)
                {
                    if (fc.Page == null)
                    {
                        fc.Page = 1;
                    }
                    if (fc.PageSize == null)
                    {
                        fc.PageSize = 10;
                    }
                    UserData = UserData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, UserData, p, "User Type Listed");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetUsers")]
        public APIResponse GetUsers()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<User>>(users);
            return new APIResponse(true, model, "User Listed");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public APIResponse GetById(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                UserByID model = _mapper.Map<UserByID>(user);
                if (model == null)
                {
                    return new APIResponse(false, "", "User not found");
                }
                else
                {
                    return new APIResponse(true, model, "User Listed By Id");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{id}")]
        public APIResponse Update(int id, [FromBody] RegisterUserModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                AuditLog obj = new AuditLog();
                obj.Activity = "Update";
                obj.OldRecord = JsonConvert.SerializeObject(_userService.GetById(id));
                obj.Type = "User";
                obj.UserID = Convert.ToInt64(userId);
                obj.NewRecord = JsonConvert.SerializeObject(model);
                _auditlogService.Create(obj);
                // update user 
                _userService.Update(user, model.Password);
                return new APIResponse(true, new
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    DisplayName = user.DisplayName,
                    Email = user.Email
                }, "User Update");
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                AuditLog obj = new AuditLog();
                obj.Activity = "Delete";
                obj.OldRecord = JsonConvert.SerializeObject(_userService.GetById(id));
                obj.Type = "User";
                obj.UserID = Convert.ToInt64(userId);
                _auditlogService.Create(obj);
                _userService.Delete(id);
                return new APIResponse(true, "", "user Deleted");
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message);
            }
        }
    }
}
