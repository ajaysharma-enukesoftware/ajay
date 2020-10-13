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
    [Route("api/Vindima/PaymentNotSent")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentNotSentController : ControllerBase
    {
        private IPaymentNotSentService _paymentNotSentService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public PaymentNotSentController(
            IPaymentNotSentService paymentNotSentService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _paymentNotSentService = paymentNotSentService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<PaymentNotSent> AllPaymentNotSent = _paymentNotSentService.GetAll();
                var PaymentNotSentData = new List<PaymentNotSent>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "data_ficheiro";
                }
                var propertyInfo = typeof(PaymentNotSent).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllPaymentNotSent = AllPaymentNotSent.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                PaymentNotSentData = AllPaymentNotSent.ToList();
                //Pagination
                long TotalCount = AllPaymentNotSent.Count();
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
                    PaymentNotSentData = PaymentNotSentData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, PaymentNotSentData, p, "Payment Not Sent to DGT Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }

        }
    }
}