using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Newtonsoft.Json;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Estatuto")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class EstatutoController : ControllerBase
    {
        private IEstatutoService _estatutoService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;

        public EstatutoController(
            IEstatutoService estatutoService,
            IMapper mapper,
            IOptions<AppSettings> appSettings, IAuditLogService auditLogService)
        {
            _estatutoService = estatutoService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;
        }


        [HttpPost("Create")]
        public APIResponse CreateEstatuto([FromBody] Estatuto p)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }

                Estatuto result = _estatutoService.Create(p);
                if (result != null)
                {
                    AuditLog obj = new AuditLog();
                    obj.Activity = "Create";
                    obj.OldRecord = "";
                    obj.Type = "Estatuto";
                    obj.UserID = Convert.ToInt64(userId);
                    obj.NewRecord = JsonConvert.SerializeObject(p);
                    _auditlogService.Create(obj);
                    return new APIResponse(true, "", "Estatuto Created Successfully");
                }
                else
                {
                    return new APIResponse(false, "", "Not able to create estatuto");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpPost("Edit/{id:int?}")]
        public APIResponse UpdateEstatuto(long id, [FromBody] Estatuto p)
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
                obj.Activity = "Update";
                //obj.OldRecord = JsonConvert.SerializeObject(_estatutoService.GetById(Convert.ToInt32(id)));
                obj.OldRecord = "";
                obj.Type = "Estatuto";
                obj.UserID = Convert.ToInt64(userId);
                obj.NewRecord = JsonConvert.SerializeObject(p);
                _auditlogService.Create(obj);
                _estatutoService.Update(p);
                return new APIResponse(true, "", "Data Updated Succesfully");
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpPost("Delete/{id}")]
        public APIResponse DeleteEstatuto(int id)
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
                obj.OldRecord = JsonConvert.SerializeObject(_estatutoService.GetById(id));
                obj.Type = "Estatuto";
                obj.UserID = Convert.ToInt64(userId);
                _auditlogService.Create(obj);
                _estatutoService.Delete(id);
                return new APIResponse(true, "", "Estatuto delete successfully!");

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpGet("{id}")]
        public APIResponse GetById(int id)
        {
            try
            {
                Estatuto obj = _estatutoService.GetById(id);
                if (obj != null)
                {
                    return new APIResponse(true, obj, "");
                }
                else
                {
                    return new APIResponse(false, obj, "");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody] FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {

                IEnumerable<Estatuto> AllEstatutos = _estatutoService.GetAll();
                var estatutoData = new List<Estatuto>();
                estatutoData = AllEstatutos.ToList();
                //Pagination
                long TotalCount = AllEstatutos.Count();
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
                    estatutoData = AllEstatutos.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, estatutoData, p, "Estatuto List Returned");
            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
    }
}