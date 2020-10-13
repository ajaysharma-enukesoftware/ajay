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
    [Route("api/Vindima/PaymentDicoFre")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PaymentDicoFreController : ControllerBase
    {

        private IPaymentDicoFreService _designationService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public PaymentDicoFreController(
            IPaymentDicoFreService designationService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _designationService = designationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<PaymentDicoFre> AllPaymentDicoFre = _designationService.GetAll();
                var PaymentDicoFreData = new List<PaymentDicoFre>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "n_entidade";
                }
                var propertyInfo = typeof(PaymentDicoFre).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllPaymentDicoFre = AllPaymentDicoFre.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                PaymentDicoFreData = AllPaymentDicoFre.ToList();
                //Pagination
                long TotalCount = AllPaymentDicoFre.Count();
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
                    PaymentDicoFreData = PaymentDicoFreData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, PaymentDicoFreData, p, "Payment Dico Fre Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

    }
}