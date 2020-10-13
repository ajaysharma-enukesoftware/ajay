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

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Revenue/ConsultCurrentAccount")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    public class ConsultCurrentAccountController : ControllerBase
    {
        private IConsultCurrentAccountService _recocileProducerService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ConsultCurrentAccountController(
            IConsultCurrentAccountService RecocileProducerService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _recocileProducerService = RecocileProducerService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("GetByEntity")]
        public APIListResponse Get([FromBody] APIConsultCurrentAccountRequest request)
        {
            Pagination p = new Pagination();
            try
            {
                // IEnumerable<ConsultCurrentAccount> AllRecocileProducer = _recocileProducerService.GetAllPeriodMovements(request.txtNUMENT,request.dateInicial,request.dateFinal);
                List<ConsultCurrentAccount> AllRecocileProducer = _recocileProducerService.GetAllPeriodMovements(request.txtNUMENT, request.dateInicial, request.dateFinal).ToList();
                if (AllRecocileProducer.Any())
                {
                    return new APIListResponse(true, AllRecocileProducer, p, "ConsultCurrentAccount List Returned");
                }
                else
                {
                    return new APIListResponse(false, "", p, "No List Returned");


                }

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
    }
}