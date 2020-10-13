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
    [Route("api/Vindima/ProductionAuthorization")]
    [ApiController]
    public class ProductionAuthorizationController : ControllerBase
    {
        private IProductionAuthorizationService _productionAuthorizationService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ProductionAuthorizationController(
         IProductionAuthorizationService productionAuthorizationService,
        IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _productionAuthorizationService = productionAuthorizationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [HttpPost("getall")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<ProductionAuthorization> productionAuthorization = _productionAuthorizationService.GetAll();
                var Data = new List<ProductionAuthorization>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "NRAP";
                }
                //var propertyInfo = typeof(busEntDistrito).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                productionAuthorization = productionAuthorization.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                Data = productionAuthorization.ToList();
                //Pagination
                long TotalCount = productionAuthorization.Count();
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

                return new APIListResponse(true, Data, p, "Production Authorization Listed");


            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }
    }
}