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
    [Route("api/Vindima/RegisterMovements")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class RegisterMovementsController : ControllerBase
    {
        private IRegisterMovementsService _registerMovementsService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public RegisterMovementsController(
            IRegisterMovementsService registerMovementsService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _registerMovementsService = registerMovementsService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<RegisterMovements> AllRegisterMovements = _registerMovementsService.GetAll();
                var RegisterMovementsData = new List<RegisterMovements>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "n_viticultor";
                }
                var propertyInfo = typeof(RegisterMovements).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllRegisterMovements = AllRegisterMovements.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                RegisterMovementsData = AllRegisterMovements.ToList();
                //Pagination
                long TotalCount = AllRegisterMovements.Count();
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
                    RegisterMovementsData = RegisterMovementsData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, RegisterMovementsData, p, "Register Movements Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
    }
}