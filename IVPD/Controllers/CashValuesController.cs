using AutoMapper;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Revenue")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CashValuesController : ControllerBase
    {
        private ICashValuesServices _cashValuesService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public CashValuesController(
            ICashValuesServices cashValuesService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _cashValuesService = cashValuesService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("CashValues")]
        public APIListResponse GetAll([FromBody]APIGetCashValuesRequest fc)
        {
            Pagination p = new Pagination();

            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);
                IEnumerable<AllTransactions> AllEstatutos = _cashValuesService.GeCashValuesDetails(fc.date, userId, fc.transMethodId);
                if (AllEstatutos.Any())
                {
                    var estatutoData = new List<AllTransactions>();

                    estatutoData = AllEstatutos.ToList();
                    //Pagination
                    long TotalCount = AllEstatutos.Count();
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
                        estatutoData = AllEstatutos.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                        p.Limit = (long)fc.PageSize;
                        p.CurrentPage = (long)fc.Page;
                        p.Total = TotalCount;
                    }


                    return new APIListResponse(true, estatutoData, p, "Details Returned!");
                }
                else
                {
                    return new APIListResponse(false, "", p, "");

                }

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);

            }
        }
    }
}
