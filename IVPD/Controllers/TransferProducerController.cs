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
    [Route("api/Vindima/TransferProducer")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TransferProducerController : ControllerBase
    {

        private ITransferProducerService _Service;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public TransferProducerController(
            ITransferProducerService Service,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _Service = Service;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<TransferProducer> AllTransferProducer = _Service.GetAll();
                var TransferProducerData = new List<TransferProducer>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "n";
                }
                var propertyInfo = typeof(TransferProducer).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllTransferProducer = AllTransferProducer.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                TransferProducerData = AllTransferProducer.ToList();
                //Pagination
                long TotalCount = AllTransferProducer.Count();
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
                    TransferProducerData = TransferProducerData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, TransferProducerData, p, "Transfer Producer Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }


        [HttpPost("Tipo")]
        public APIListResponse Tipo([FromBody] FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<TransferProducer> AllTransferProducer = _Service.GetAll();
                List<string> TransferProducerData = new List<string>();
                TransferProducerData = AllTransferProducer.Select(s => s.tipo).ToList();
                //Pagination
                long TotalCount = AllTransferProducer.Count();
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
                    TransferProducerData = TransferProducerData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, TransferProducerData, p, "Tipo Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

    }
}