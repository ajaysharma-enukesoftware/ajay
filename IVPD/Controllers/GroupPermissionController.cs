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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/GroupPermission")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class GroupPermissionController : ControllerBase
    {
        private IGroupPermisssionService _groupPermisssionService;
        private IAuditLogService _auditlogService;

        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public GroupPermissionController(
            IGroupPermisssionService groupPermisssionService,
            IMapper mapper,
            IOptions<AppSettings> appSettings, IAuditLogService auditlogService)
        {
            _groupPermisssionService = groupPermisssionService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditlogService;
    }


        [HttpPost("Create")]
        public APIResponse GroupPermissionCreate([FromBody]GroupPermissionRequest groupPermissionRequest)
        {
            try
            {
                bool aPIResponse = _groupPermisssionService.Create(groupPermissionRequest);
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                AuditLog obj = new AuditLog();
                obj.Activity = "Create";
                obj.OldRecord = "";
                obj.Type = "Group Permission";
                obj.UserID = Convert.ToInt64(userId);
                obj.NewRecord = JsonConvert.SerializeObject(groupPermissionRequest);
                _auditlogService.Create(obj);
                _groupPermisssionService.Create(groupPermissionRequest);
                 return new APIResponse(true, "", "GroupPermission Created Successfully");
               
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public APIResponse Delete(Int64 id)
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
                obj.OldRecord = JsonConvert.SerializeObject(_groupPermisssionService.GetById(id));
                obj.Type = "Group Permission";
                obj.UserID = Convert.ToInt64(userId);
                _auditlogService.Create(obj);
                _groupPermisssionService.Delete(id);
                return new APIResponse(true, "", "GroupPermission Deleted Successfully");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message);
            }
        }


        [HttpGet("PermissionsByGroupID/{id}")]
        public APIResponse PermissionsByGroupID(Int64 id)
        {

            try
            {
               List<GroupPermission> groupPermissions=  _groupPermisssionService.GetByGroupID(id);
                return new APIResponse(true, groupPermissions, "GroupPermission Listed");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message);
            }
        }
    }

}