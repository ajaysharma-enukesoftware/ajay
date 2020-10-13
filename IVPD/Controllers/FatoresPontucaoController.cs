using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Route("api/FatoresPontucao")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FatoresPontucaoController : ControllerBase
    {
        private IFatoresPontucaoService _fatoresPontucaoService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;

        public FatoresPontucaoController(
            IFatoresPontucaoService fatoresPontucaoService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IAuditLogService auditLogService)
        {
            _fatoresPontucaoService = fatoresPontucaoService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;
        }
        
        [HttpPost("FatoresPontucaoCalculation")]
        public APIResponse GetById([FromBody] APIFatoresPontucaoCalculationRequest request)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                var p = _fatoresPontucaoService.GetCalculations(request.parcelId, request.geocod);
                if (p == null)
                {
                    return new APIResponse(false, "", "No Parcel Exist with the given id!");
                }
                else
                {
                    return new APIResponse(true, p, "Parcel Data returned!");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
       }
        
    }
}
