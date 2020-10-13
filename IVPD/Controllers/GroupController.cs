using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Newtonsoft.Json;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Groups")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService _groupService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;

        public GroupController(
            IGroupService groupService,
            IMapper mapper,
            IOptions<AppSettings> appSettings, IAuditLogService auditLogService)
        {
            _groupService = groupService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;
        }


        [HttpPost("Create/{id:int?}")]
        public APIResponse CreateGroup([FromBody] NewGroup newGroup, long id)
        {
            try
            {
                Group group = new Group();
                if (id != 0)
                {
                    group.Id = id;
                }
                if (newGroup.Id != 0)
                {
                    group.Id = newGroup.Id;
                }
                group.Name = newGroup.GroupName;
                group.Descriptions = newGroup.GroupDescription;
                if (newGroup.Active == true)
                {
                    group.Status = 1;
                }
                else
                {
                    group.Status = 2;

                }

                var result = new Group();
                bool updates = false;
                if (newGroup.Id == 0)
                {

                    result = _groupService.Create(group);

                    var claimsIdentity = this.User.Identity as ClaimsIdentity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                    if (!long.TryParse(userId, out long i))
                    {
                        userId = "0";
                    }
                    AuditLog obj = new AuditLog();
                    obj.Activity = "Create";
                    obj.OldRecord = "";
                    obj.Type = "Group";
                    obj.UserID = Convert.ToInt64(userId);
                    obj.NewRecord = JsonConvert.SerializeObject(newGroup);
                    _auditlogService.Create(obj);
                    group.CreatedAt = DateTime.Now;
                    group.DeletedAt = DateTime.Now;
                    return new APIResponse(true, "", "Group Created Successfully");
                }
                else
                {
                    var claimsIdentity = this.User.Identity as ClaimsIdentity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                    if (!long.TryParse(userId, out long i))
                    {
                        userId = "0";
                    }
                    AuditLog obj = new AuditLog();
                    obj.Activity = "Update";
                    obj.OldRecord = JsonConvert.SerializeObject(_groupService.getByID(id));
                    obj.Type = "Group";
                    obj.UserID = Convert.ToInt64(userId);
                    obj.NewRecord = JsonConvert.SerializeObject(newGroup);
                    _auditlogService.Create(obj);
                    group.UpdatedAt = DateTime.Now;
                    updates = _groupService.Update(group);
                    return new APIResponse(true, "", "Group Updated Successfully");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        //[HttpGet("groups")]
        //public IActionResult Groups([FromQuery] string pageno, string pagesize)

        //{
        //    try
        //    {



        //        // set current page number, must be >= 1 (ideally this value will be passed to this logic/function from outside)

        //        var result = _groupService.GetByPage(pageno, pagesize);
        //        if(result.Count!=0)
        //        {
        //            return Ok(new
        //            {
        //                Success = "false",
        //                Data= result,
        //                message = "No data in Groups"
        //            });
        //        }
        //        else
        //        {
        //            return Ok(new
        //            {
        //                Success = "false",
        //                Data="",
        //                message = "Not able to create group"
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new
        //        {
        //            Success = "false",
        //            message = ex.Message
        //        }); ;
        //    }
        //}

        [HttpPost("UsersByGroupID/{id}")]
        public APIResponse UserByGroupID(Int64 id)
        {
            // map model to entity


            try
            {
                // create user
                ForUserByGroupID FUBGID = new ForUserByGroupID();
                FUBGID.Users = _groupService.UserByGroupId(id).ToList();
                string GroupdescriptionName = string.Empty;
                List<ForGroupList> forGroupLists = _groupService.GetAll().Where(x => x.ID == id).ToList();

                ForGroupList forGroupList = new ForGroupList();
                if (forGroupLists.Count != 0)
                {
                    forGroupList = forGroupLists.FirstOrDefault();
                }
                if (forGroupList != null)
                {
                    GroupdescriptionName = forGroupList.GroupName;
                }
                if (GroupdescriptionName != null)
                {
                    FUBGID.GroupDescription = GroupdescriptionName;
                }



                return new APIResponse(true, FUBGID, "Users Listed By GroupID");

            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message); ;
            }
        }


        [HttpPost("AssighUserByGroupId")]
        public APIResponse AssighUserByGroupId(AssignUsertogroup assignUsertogroup)
        {
            // map model to entity


            try
            {
                // create user

                _groupService.AssignUsertoGroup(assignUsertogroup.GroupIds, assignUsertogroup.UserID);

                return new APIResponse(true, "", "User assigned to group");

            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message); ;
            }
        }


        [HttpGet("{id}")]
        public APIResponse Groups(Int64 id)

        {
            try
            {
                // set current page number, must be >= 1 (ideally this value will be passed to this logic/function from outside)
                List<ForGroupList> result = new List<ForGroupList>();
                if (id == 0)
                {

                    result = _groupService.GetAll();
                }
                else
                {
                    result = _groupService.GetAll(id);
                    result = result.Where(x => x.ID == id).ToList();
                }
                if (result.Count != 0)
                {
                    return new APIResponse(true, result, "Groups listed ");
                }
                else
                {
                    return new APIResponse(false, "", "No Data in Group Table");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
        [HttpPost("AllGroups")]
        public APIListResponse Groups([FromBody] FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<ForGroupList> AllGroups = _groupService.GetAll();
                var groupData = new List<ForGroupList>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "id";
                }
                var propertyInfo = typeof(Parcel).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllGroups = AllGroups.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                groupData = AllGroups.ToList();
                //Pagination
                long TotalCount = AllGroups.Count();
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
                    groupData = AllGroups.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, groupData, p, "Group List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

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
                obj.OldRecord = JsonConvert.SerializeObject(_groupService.getByID(id));
                obj.Type = "Group";
                obj.UserID = Convert.ToInt64(userId);
                _auditlogService.Create(obj);

                _groupService.delete(id);
                return new APIResponse(true, "", "Group Deleted");
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message);
            }
        }


        [HttpPost("AssignUsertoGroup")]
        public APIResponse AssignUsertoGroup([FromBody] AssignUsertogroup assignUsertogroup)
        {
            try
            {
                _groupService.AssignUsertoGroup(assignUsertogroup.GroupIds, assignUsertogroup.UserID);
                return new APIResponse(true, "", "assigned user to a group");
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message);
            }
        }




        [HttpPost("DeleteUsertoGroup")]
        public APIResponse DeleteUsertoGroup([FromBody] AssignUsertogroup assignUsertogroup)
        {
            try
            {
                _groupService.DeleteUsertoGroup(assignUsertogroup.GroupIds, assignUsertogroup.UserID);
                return new APIResponse(true, "", "assigned user to a group");
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message);
            }
        }
    }
}