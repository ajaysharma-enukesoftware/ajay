using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IVPD.Services;
using AutoMapper;
using IVPD.Helpers;
using Microsoft.Extensions.Options;
using IVPD.Models;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Vindima/RegistrationImpression")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class RegistrationImpressionController : ControllerBase
    {
        private IRegistrationImpressionService _registrationImpressionService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public RegistrationImpressionController(
            IRegistrationImpressionService registrationImpressionService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _registrationImpressionService = registrationImpressionService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<RegistrationImpression> AllRegistrationImpression = _registrationImpressionService.GetAll();
                var RegistrationImpressionData = new List<RegistrationImpression>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "RCBNUM";
                }
                var propertyInfo = typeof(ConsultationPledges).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllRegistrationImpression = AllRegistrationImpression.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                RegistrationImpressionData = AllRegistrationImpression.ToList();
                //Pagination
                long TotalCount = AllRegistrationImpression.Count();
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
                    RegistrationImpressionData = RegistrationImpressionData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, RegistrationImpressionData, p, "Registration Impression Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
    }
}