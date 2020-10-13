using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using Microsoft.AspNetCore.Cors;
using AutoMapper;
using Microsoft.Extensions.Options;
using System.Linq.Dynamic.Core;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/busentconcelho")]
    [ApiController]
    public class BusEntConcelhoController : ControllerBase
    {
        private IBusEntConcelhoService _busEntConcelhoService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public BusEntConcelhoController(
         IBusEntConcelhoService busEntConcelhoService,
        IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _busEntConcelhoService = busEntConcelhoService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [HttpPost("getall")]
        public APIListResponse BusEntConcelho([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<BusEntConcelho> busEntConcelho = _busEntConcelhoService.BusEntConcelhoGetAll();
                //var Data = new List<busEntDistrito>();

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
                //busEntConcelho = busEntConcelho.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                //Data = busEntConcelho.ToList();
                //Pagination
                long TotalCount = busEntConcelho.Count();
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
                    busEntConcelho = busEntConcelho.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, busEntConcelho, p, "BusEntConcelho Listed");


            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }


        [HttpPost("getbyid/{id}")]
        public APIListResponse BusEntConcelhoByID(int id)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<BusEntConcelho> busEntConcelho = _busEntConcelhoService.BusEntConcelhoGetByID(id);
               
                //Pagination
                long TotalCount = busEntConcelho.Count();
                p.Limit = TotalCount;
                p.CurrentPage = 1;
                p.Total = TotalCount;
                return new APIListResponse(true, busEntConcelho, p, "BusEntConcelho Listed");

            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }
        [HttpPost("getbydescon/{descon}")]
        public APIListResponse BusEntConcelhoByID(string descon)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<BusEntConcelho> busEntConcelho = _busEntConcelhoService.BusEntConcelhoGetByName(descon);

                //Pagination
                long TotalCount = busEntConcelho.Count();
                p.Limit = TotalCount;
                p.CurrentPage = 1;
                p.Total = TotalCount;
                return new APIListResponse(true, busEntConcelho, p, "BusEntConcelho Listed");

            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }

    }
}