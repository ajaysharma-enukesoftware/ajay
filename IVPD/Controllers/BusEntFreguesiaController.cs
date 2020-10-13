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

namespace IVPD.Controllers
{

    [EnableCors("AllowAll")]
    [Route("api/BusEntFreguesia")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BusEntFreguesiaController : ControllerBase
    {

        private IBusEntFreguesiaService _Service;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public BusEntFreguesiaController(
            IBusEntFreguesiaService objService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _Service = objService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<BusEntFreguesia> AllBusEntFreguesia = _Service.GetAll();
               // var BusEntFreguesiaData = new List<BusEntFreguesia>();

                //Sorting
                //if (string.IsNullOrEmpty(fc.SortBy))
                //{
                //    fc.SortBy = "IDSUBREG";
                //}
                //var propertyInfo = typeof(BusEntFreguesia).GetProperty(fc.SortBy);
                //string sortType = "ASC";
                //if ((bool)fc.IsSortTypeDESC)
                //{
                //    sortType = "DESC";
                //}
                //AllBusEntFreguesia = AllBusEntFreguesia.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                //BusEntFreguesiaData = AllBusEntFreguesia.ToList();
                //Pagination
                long TotalCount = AllBusEntFreguesia.Count();
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
                    AllBusEntFreguesia = AllBusEntFreguesia.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, AllBusEntFreguesia, p, "BusEntFreguesiaData List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }


        [HttpGet("GetByID/{id}")]
        public APIListResponse GetByID(int id)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<BusEntFreguesia> AllBusEntFreguesia = _Service.GetByID(id);
                //Pagination
                long TotalCount = AllBusEntFreguesia.Count();
                p.Limit = TotalCount;
                p.CurrentPage = 1;
                p.Total = TotalCount;               
                return new APIListResponse(true, AllBusEntFreguesia, p, "BusEntFreguesiaData List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

    }
}