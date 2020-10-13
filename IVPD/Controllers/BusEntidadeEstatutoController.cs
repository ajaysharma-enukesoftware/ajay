using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace IVPD.Controllers
{

    [EnableCors("AllowAll")]
    [Route("api/BusEntidadeEstatuto")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BusEntidadeEstatutoController : ControllerBase
    {

        private IBusEntidadeEstatutoService _Service;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public BusEntidadeEstatutoController(
            IBusEntidadeEstatutoService objService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _Service = objService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody] FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<BusEntidadeEstatutoList> AllBusEntidadeEstatuto = _Service.GetAllName();
                // var BusEntidadeEstatutoData = new List<BusEntidadeEstatuto>();

                //Sorting
                //if (string.IsNullOrEmpty(fc.SortBy))
                //{
                //    fc.SortBy = "IDSUBREG";
                //}
                //var propertyInfo = typeof(BusEntidadeEstatuto).GetProperty(fc.SortBy);
                //string sortType = "ASC";
                //if ((bool)fc.IsSortTypeDESC)
                //{
                //    sortType = "DESC";
                //}
                //AllBusEntidadeEstatuto = AllBusEntidadeEstatuto.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                //BusEntidadeEstatutoData = AllBusEntidadeEstatuto.ToList();
                //Pagination
                long TotalCount = AllBusEntidadeEstatuto.Count();
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
                    AllBusEntidadeEstatuto = AllBusEntidadeEstatuto.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, AllBusEntidadeEstatuto, p, "BusEntidadeEstatutoData List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

        [HttpPost("Create")]
        public APIResponse Create([FromBody] BusEntidadeEstatutoList p)
        {
            try
            {
                APIResponse result = _Service.Create(p);
                if (result != null)
                {
                    return new APIResponse(result.success, result.data, result.message);
                }
                else
                {
                    return new APIResponse(false, "", "Something went wrong. Please try again later!");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }


        [HttpPost("Edit")]
        public APIResponse Edit([FromBody] BusEntidadeEstatutoList p)
        {
            try
            {
                APIResponse result = _Service.Update(p);
                if (result != null)
                {
                    return new APIResponse(result.success, result.data, result.message);
                }
                else
                {
                    return new APIResponse(false, "", "Something went wrong. Please try again later!");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpGet("GetByID/{id:int?}")]
        public APIResponse GetByID(int id)
        {
            try
            {
                BusEntidadeEstatutoList AllBusEntidadeEstatuto = _Service.GetByID(id);
                if (AllBusEntidadeEstatuto != null)
                {
                    return new APIResponse(true, AllBusEntidadeEstatuto, "BusEntidadeEstatutoData List Returned");
                }
                else
                {
                    return new APIResponse(false, AllBusEntidadeEstatuto, "Something went wrong. Please try again later!");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpPost("Delete/{id:int?}")]
        public APIResponse Delete(int id)
        {
            try
            {
                APIResponse result = _Service.Delete(id);
                if (result != null)
                {
                    return new APIResponse(result.success, result.data, result.message);
                }
                else
                {
                    return new APIResponse(false, "", "Something went wrong. Please try again later!");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
    }
}