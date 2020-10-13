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

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Permission")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermisssionService _PermisssionService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public PermissionController(
            IPermisssionService PermisssionService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _PermisssionService = PermisssionService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        [HttpGet("PermissionsByGroup/{id}")]
        public IActionResult PermissionsByGroup(Int64 ID)
        {
            try
            {
                var result = _PermisssionService.PermissionsByGroup(ID);
                if (result != null)
                {
                    // return basic user info and authentication token
                    return Ok(new
                    {
                        Success = true,
                        Data= result,
                        message = "Permissions return by Group Successfully "
                    }); ;
                }
                else
                {
                    return Ok(new
                    {
                        Success = true,
                        Data = "",
                        message = "No Permissions associated with the group id"

                    }); ;
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    Success = false,
                    Data = "",
                    message = e.Message.ToString()
                });
            }
        }


        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<Permission> AllPermission = _PermisssionService.GetAll();
                var PermissionData = new List<Permission>();

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
                AllPermission = AllPermission.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                PermissionData = AllPermission.ToList();
                //Pagination
                long TotalCount = AllPermission.Count();
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
                    PermissionData = PermissionData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, PermissionData, p, "Permission List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }


    }
}