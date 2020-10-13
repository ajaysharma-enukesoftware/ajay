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
    [Route("api/Revenue")]
    [ApiController]
    public class LIBTESOUR_TSARTIFController : ControllerBase
    {
        private ILIBTESOUR_TSARTIFService _lIBTESOUR_TSARTIFService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public LIBTESOUR_TSARTIFController(
            ILIBTESOUR_TSARTIFService lIBTESOUR_TSARTIFService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _lIBTESOUR_TSARTIFService = lIBTESOUR_TSARTIFService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("Artigo/GetAll")]
        public APIListResponse LIBTESOUR_TSARTIF([FromBody] FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<LIBTESOUR_TSARTIF> countries = _lIBTESOUR_TSARTIFService.LIBTESOUR_TSARTIFGetAll();
                var Data = new List<LIBTESOUR_TSARTIF>();

                //Sorting
                /* if (string.IsNullOrEmpty(fc.SortBy))
                 {
                     fc.SortBy = "id";
                 }
                 string sortType = "ASC";
                 if ((bool)fc.IsSortTypeDESC)
                 {
                     sortType = "DESC";
                 }
                 countries = countries.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                */
                Data = countries.ToList();
                //Pagination
                long TotalCount = countries.Count();
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
                    Data = countries.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, Data, p, "LIBTESOUR_TSARTIF Listed");


            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }
    }
}
