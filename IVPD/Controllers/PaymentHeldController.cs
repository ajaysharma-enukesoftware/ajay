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
    [Route("api/Vindima/PaymentHeld")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class PaymentHeldController : ControllerBase
    {
        private IPaymentHeldService _paymentHeldService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public PaymentHeldController(
            IPaymentHeldService paymentHeldService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _paymentHeldService = paymentHeldService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<PaymentHeld> AllPaymentHeld = _paymentHeldService.GetAll();
                var PaymentHeldData = new List<PaymentHeld>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "n_ficheiro";
                }
                var propertyInfo = typeof(PaymentHeld).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllPaymentHeld = AllPaymentHeld.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                PaymentHeldData = AllPaymentHeld.ToList();
                //Pagination
                long TotalCount = AllPaymentHeld.Count();
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
                    PaymentHeldData = PaymentHeldData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, PaymentHeldData, p, "Payment Held List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }

        }
    }
}