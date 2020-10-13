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
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Parcel")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ParcelController : ControllerBase
    {
        private IParcelService _parcelService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;

        public ParcelController(
            IParcelService parcelService,
            IMapper mapper,
            IOptions<AppSettings> appSettings, IAuditLogService auditLogService)
        {
            _parcelService = parcelService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;
        }

        [HttpPost("Create")]
        public APIResponse CreateParcel([FromBody] MainParcel p)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                if (_auditlogService.CheckPermission(Convert.ToInt64(userId), "parcel", "iswrite"))
                {
                    if (ModelState.IsValid)
                    {
                        if (!_parcelService.CheckID(p.parcel.NUMPARC))
                        {
                            Parcel result = _parcelService.Create(p);
                            if (result != null)
                            {
                                AuditLog obj = new AuditLog();
                                obj.Activity = "Create";
                                obj.OldRecord = "";
                                obj.Type = "Parcel";
                                obj.UserID = Convert.ToInt64(userId);
                                obj.NewRecord = JsonConvert.SerializeObject(p);
                                _auditlogService.Create(obj);
                                return new APIResponse(true, "", "Parcel Created Successfully");
                            }
                            else
                            {
                                return new APIResponse(false, "", "Not able to create parcel");
                            }
                        }
                        else
                        {
                            return new APIResponse(false, "", "NUMPARC is already exist!");
                        }
                    }
                    else
                    {
                        return new APIResponse(false, "", "NUMPARC is already exist!");
                    }
                }
                else
                {
                    return new APIResponse(false, "", "Permission is denied.Please contact Administrator!");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpPost("Edit")]
        public APIResponse UpdateParcel([FromBody] MainParcel p)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                if (_auditlogService.CheckPermission(Convert.ToInt64(userId), "parcel", "isupdate"))
                {
                    AuditLog obj = new AuditLog();
                    obj.Activity = "Update";
                    //obj.OldRecord = JsonConvert.SerializeObject(_parcelService.GetById(Convert.ToInt32(id)));
                    obj.OldRecord = "";
                    obj.Type = "Parcel";
                    obj.UserID = Convert.ToInt64(userId);
                    obj.NewRecord = JsonConvert.SerializeObject(p);
                    _auditlogService.Create(obj);
                    _parcelService.Update(p);
                    return new APIResponse(true, "", "Data Updated Succesfully");
                }
                else
                {
                    return new APIResponse(false, "", "Permission is denied.Please contact Administrator!");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpPost("Delete")]
        public APIResponse DeleteParcel([FromBody] APIParcelRequest request)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                if (_auditlogService.CheckPermission(Convert.ToInt64(userId), "parcel", "isdelete"))
                {
                    AuditLog obj = new AuditLog();
                    obj.Activity = "Delete";
                    obj.OldRecord = JsonConvert.SerializeObject(_parcelService.GetById(request.id, request.versao));
                    obj.Type = "Parcel";
                    obj.UserID = Convert.ToInt64(userId);
                    _auditlogService.Create(obj);
                    _parcelService.Delete(request.id, request.versao);
                    return new APIResponse(true, "", "Parcel delete successfully!");
                }
                else
                {
                    return new APIResponse(false, "", "Permission is denied.Please contact Administrator!");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpPost("GetById")]
        public APIResponse GetById([FromBody] APIParcelRequest request)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                if (_auditlogService.CheckPermission(Convert.ToInt64(userId), "parcel", "isread"))
                {
                    var p = _parcelService.GetByAll(request.id, request.versao);
                    var model = _mapper.Map<MainParcelByID>(p);
                    if (model.parcel == null)
                    {
                        return new APIResponse(false, "", "No Parcel Exist with the given id!");
                    }
                    else
                    {
                        return new APIResponse(true, model, "Parcel Data returned!");
                    }
                }
                else
                {
                    return new APIResponse(false, "", "Permission is denied.Please contact Administrator!");
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
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                if (_auditlogService.CheckPermission(Convert.ToInt64(userId), "parcel", "isread"))
                {
                    IEnumerable<ParcelList> AllParcels = _parcelService.GetAll(fc, out p);
                    var parcelData = new List<ParcelList>();
                    parcelData = AllParcels.ToList();
                    return new APIListResponse(true, parcelData, p, "Parcel List Returned");
                }
                else
                {
                    return new APIListResponse(false, "", p, "Permission is denied.Please contact Administrator!");
                }
            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
    }
}