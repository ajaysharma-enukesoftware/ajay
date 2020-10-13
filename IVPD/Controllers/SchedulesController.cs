using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using IVPD.Models;
using IVPD.Services;
using IVPD.Helpers;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/schedules")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private IScheduleService _scheduleService;
        private IAuditLogService _auditlogService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public SchedulesController(
            IScheduleService scheduleService,
            IMapper mapper,
            IOptions<AppSettings> appSettings, IAuditLogService auditLogService)
        {
            _scheduleService = scheduleService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;
        }



        [AllowAnonymous]
        [HttpGet("{id}")]
        public APIResponse GetbyId(Int64 ID)
        {
            try
            {
                var result = _scheduleService.GetById(ID);
                if (result != null)
                {
                    return new APIResponse(true, result, "Schedule returned Successfully");
                }
                else
                {
                    return new APIResponse(false, "", "No Schedule in Database with this ID");
                }
            }
            catch (Exception e)
            {
                return new APIResponse(false, "", e.Message);
            }
        }



        [HttpPost("Create/{id:int?}")]
        public APIResponse Create(Schedules schedule, long id)
        {
            try
            {

                if (id == 0)
                {
                    APIResponse aPIResponse = _scheduleService.Create(schedule, _appSettings.AdminEmail, _appSettings.SMTPEmail, _appSettings.SMTPPWD,
                                    _appSettings.SMTPPort, _appSettings.SMTPServer);
                    var claimsIdentity = this.User.Identity as ClaimsIdentity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                    if (!long.TryParse(userId, out long i))
                    {
                        userId = "0";
                    }
                    AuditLog obj = new AuditLog();
                    obj.Activity = "Create";
                    obj.OldRecord = "";
                    obj.Type = "Schedule";
                    obj.UserID = Convert.ToInt64(userId);
                    obj.NewRecord = JsonConvert.SerializeObject(schedule);
                    _auditlogService.Create(obj);
                    return new APIResponse(aPIResponse.success, "", aPIResponse.message);
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
                    obj.OldRecord = JsonConvert.SerializeObject(_scheduleService.GetById(id));
                    obj.Type = "Schedule";
                    obj.UserID = Convert.ToInt64(userId);
                    obj.NewRecord = JsonConvert.SerializeObject(schedule);
                    _auditlogService.Create(obj);
                    schedule.ID = id;
                    APIResponse aPIResponse = _scheduleService.Update(schedule, _appSettings.AdminEmail, _appSettings.SMTPEmail, _appSettings.SMTPPWD, _appSettings.SMTPPort, _appSettings.SMTPServer);
                    return new APIResponse(aPIResponse.success, "", aPIResponse.message);
                }
            }
            catch (Exception e)
            {
                return new APIResponse(false, "", e.Message);
            }
        }


        [HttpPost("GetAll")]
        public APIListResponse GetAll([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<Schedules> result = _scheduleService.GetAll();
                var Data = new List<Schedules>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "ID";
                }
                var propertyInfo = typeof(Schedules).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                result = result.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                Data = result.ToList();
                //Pagination
                long TotalCount = result.Count();
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
                    Data = result.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }
                return new APIListResponse(true, Data, p, "Schedule returned Successfully");

            }
            catch (Exception e)
            {
                return new APIListResponse(false, "", p, e.Message);
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
                obj.OldRecord = JsonConvert.SerializeObject(_scheduleService.GetById(id));
                obj.Type = "Schedule";
                obj.UserID = Convert.ToInt64(userId);
                _auditlogService.Create(obj);
                APIResponse aPIResponse = _scheduleService.Delete(id,  _appSettings.AdminEmail, _appSettings.SMTPEmail, _appSettings.SMTPPWD, _appSettings.SMTPPort, _appSettings.SMTPServer);
                return new APIResponse(aPIResponse.success, "", aPIResponse.message);

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

    }
}