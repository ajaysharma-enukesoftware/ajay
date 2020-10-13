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
    [Route("api/Vindima/PaymentFileDetail")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PaymentFileDetailController : ControllerBase
    {
        private IPaymentFileDetailService _paymentFileDetailService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public PaymentFileDetailController(
            IPaymentFileDetailService paymentFileDetailService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _paymentFileDetailService = paymentFileDetailService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<PaymentFileDetail> AllPaymentFileDetail = _paymentFileDetailService.GetAll();
                var PaymentFileDetailData = new List<PaymentFileDetail>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "n_vit";
                }
                var propertyInfo = typeof(PaymentFileDetail).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllPaymentFileDetail = AllPaymentFileDetail.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                PaymentFileDetailData = AllPaymentFileDetail.ToList();
                //Pagination
                long TotalCount = AllPaymentFileDetail.Count();
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
                    PaymentFileDetailData = PaymentFileDetailData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, PaymentFileDetailData, p, "Payment File Detail Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
    }
}