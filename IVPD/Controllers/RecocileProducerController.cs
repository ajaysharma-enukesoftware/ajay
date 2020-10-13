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
    [Route("api/Vindima/RecocileProducer")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class RecocileProducerController : ControllerBase
    {

        private IRecocileProducerService _recocileProducerService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public RecocileProducerController(
            IRecocileProducerService RecocileProducerService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _recocileProducerService = RecocileProducerService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<RecocileProducer> AllRecocileProducer = _recocileProducerService.GetAll();
                var RecocileProducerData = new List<RecocileProducer>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "CONCANO";
                }
                var propertyInfo = typeof(RecocileProducer).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllRecocileProducer = AllRecocileProducer.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                RecocileProducerData = AllRecocileProducer.ToList();
                //Pagination
                long TotalCount = AllRecocileProducer.Count();
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
                    RecocileProducerData = RecocileProducerData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, RecocileProducerData, p, "Reconcile Producer Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

    }
}