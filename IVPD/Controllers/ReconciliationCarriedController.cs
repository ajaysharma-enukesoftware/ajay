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
    [Route("api/Vindima/ReconcileCarried")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ReconciliationCarriedController : ControllerBase
    {
        private IReconciliationCarriedService _reconcileCarriedService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public ReconciliationCarriedController(
            IReconciliationCarriedService reconcileCarriedService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _reconcileCarriedService = reconcileCarriedService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<ReconciliationCarried> AllReconcileCarried = _reconcileCarriedService.GetAll();
                var ReconcilecarriedData = new List<ReconciliationCarried>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "data";
                }
                var propertyInfo = typeof(ReconciliationCarried).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllReconcileCarried = AllReconcileCarried.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                ReconcilecarriedData = AllReconcileCarried.ToList();
                //Pagination
                long TotalCount = AllReconcileCarried.Count();
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
                    ReconcilecarriedData = ReconcilecarriedData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, ReconcilecarriedData, p, "Reconcile Carried Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
    }
}