using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System;
using Newtonsoft.Json;

namespace IVPD.Controllers
{

    [EnableCors("AllowAll")]
    [Route("api/auditlog")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AuditLogController : ControllerBase
    {

        private IAuditLogService _auditLogService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuditLogController(
            IAuditLogService auditLogService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _auditLogService = auditLogService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }



        [HttpPost("getall")]
        public APIListResponse GetAll([FromBody] APIRequest fc)
        {
            Pagination p = new Pagination();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                if (_auditLogService.CheckPermission(Convert.ToInt64(userId), "audit", "isread"))
                {

                    IEnumerable<AuditLogList> auditlogs = _auditLogService.GetAll();
                    var Data = new List<AuditLogList>();
                    Data = auditlogs.ToList();
                    if (!string.IsNullOrEmpty(fc.modulename))
                    {
                        Data = auditlogs.Where(w => w.Type.ToLower() == fc.modulename.ToLower()).ToList();
                    }
                    if (fc.id > 0)
                    {
                        var Data2 = new List<AuditLogList>();
                        switch (fc.modulename.ToLower())
                        {
                            case "parcel":
                                foreach (var item in Data)
                                {
                                    try
                                    {
                                        MainParcel results = JsonConvert.DeserializeObject<MainParcel>(item.NewRecord);
                                        if (fc.id == results.parcel.NUMPARC)
                                        {
                                            Data2.Add(item);
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                Data = Data2;
                                break;
                            default:
                                break;
                        }
                    }

                    //Pagination
                    long TotalCount = Data.Count();
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
                        Data = Data.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                        p.Limit = (long)fc.PageSize;
                        p.CurrentPage = (long)fc.Page;
                        p.Total = TotalCount;
                    }

                    return new APIListResponse(true, Data, p, "Audit Log Listed");
                }
                else
                {
                    return new APIListResponse(false, "", p, "Permission is denied.Please contact Administrator!");
                }
            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }


        [HttpPost("Comment/{id:int?}")]
        public APIResponse Create(APIAuditRequest auditRequest, long id)
        {
            try
            {
                if (id > 0)
                {
                    bool b = _auditLogService.CreateComment(auditRequest.comment, id);
                    if (b)
                    {
                        return new APIResponse(b, "", "Comment inserted successfully!");
                    }
                    else
                    {
                        return new APIResponse(b, "", "Not insert comment. Please try again!");
                    }
                }
                else
                {

                    return new APIResponse(false, "", "Please send Audit Log ID!");
                }
            }
            catch (Exception e)
            {
                return new APIResponse(false, "", e.Message);
            }
        }

    }
}
