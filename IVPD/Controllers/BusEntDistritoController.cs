using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq.Dynamic.Core;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/busentdistrito")]
    [ApiController]
    public class BusEntDistritoController : ControllerBase
    {
        private IBusEntDistritoService _busEntDistritoService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public BusEntDistritoController(
         IBusEntDistritoService busEntDistritoService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
        {
            _busEntDistritoService = busEntDistritoService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("getall")]
        public APIListResponse BusEntDistrito([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<BusEntDistrito> busEntDistrito = _busEntDistritoService.BusEntDistritoGetAll();
                //var Data = new List<BusEntDistrito>();

                //Sorting
                //if (string.IsNullOrEmpty(fc.SortBy))
                //{
                //    fc.SortBy = "Coddis";
                //}
                //var propertyInfo = typeof(busEntDistrito).GetProperty(fc.SortBy);
                //string sortType = "ASC";
                //if ((bool)fc.IsSortTypeDESC)
                //{
                //    sortType = "DESC";
                //}
                //busEntDistrito = busEntDistrito.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                //Data = busEntDistrito.ToList();
                //Pagination
                long TotalCount = busEntDistrito.Count();
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
                    busEntDistrito = busEntDistrito.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, busEntDistrito, p, "BusEntDistrito Listed");


            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }
    }
}