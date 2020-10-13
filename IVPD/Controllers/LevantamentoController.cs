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
    [Route("api/Levantamento")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LevantamentoController : ControllerBase
    {

        private ILevantamentoService _LevantamentoService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public LevantamentoController(
            ILevantamentoService LevantamentoService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _LevantamentoService = LevantamentoService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<Levantamento> AllLevantamento = _LevantamentoService.GetAll(fc, out p);
                return new APIListResponse(true, AllLevantamento, p, "Levantamento Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

    }
}