using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq.Dynamic.Core;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Vindima/ConsultationPledges")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ConsultationPledgesController : ControllerBase
    {
        private IConsultationPledgesService _consultationPledgesService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public ConsultationPledgesController(
            IConsultationPledgesService consultationPledgesService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _consultationPledgesService = consultationPledgesService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<ConsultationPledges> AllOutstandingPayment = _consultationPledgesService.GetAll();
                var outstandingPayment = new List<ConsultationPledges>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "ano";
                }
                var propertyInfo = typeof(ConsultationPledges).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllOutstandingPayment = AllOutstandingPayment.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                outstandingPayment = AllOutstandingPayment.ToList();
                //Pagination
                long TotalCount = AllOutstandingPayment.Count();
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
                    outstandingPayment = outstandingPayment.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, outstandingPayment, p, "Consultation Active Pledges  List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
    }
}