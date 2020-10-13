﻿using System;
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
    [Route("api/Vindima/PaymentMade")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PaymentMadeController : ControllerBase
    {

        private IPaymentMadeService _designationService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public PaymentMadeController(
            IPaymentMadeService designationService,
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
                IEnumerable<PaymentMade> AllPaymentMade = _designationService.GetAll();
                var PaymentMadeData = new List<PaymentMade>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "N_Ficheiro";
                }
                var propertyInfo = typeof(PaymentMade).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllPaymentMade = AllPaymentMade.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                PaymentMadeData = AllPaymentMade.ToList();
                //Pagination
                long TotalCount = AllPaymentMade.Count();
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
                    PaymentMadeData = PaymentMadeData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, PaymentMadeData, p, "Payment Made Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

    }
}