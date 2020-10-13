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
    [Route("api/Revenue/DetailDocumentInfo")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DetailDocumentInfoController : ControllerBase
    {
        private IDetailDocumentInfoService _detailDocumentInfoService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public DetailDocumentInfoController(
            IDetailDocumentInfoService detailDocumentInfoService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _detailDocumentInfoService = detailDocumentInfoService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("GetAll")]
        public APIListResponse GetAll([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<DetailDocumentInfo> AllDetailDocumentInfo = _detailDocumentInfoService.GetAll().ToList();
                if (AllDetailDocumentInfo.Any())
                {
                    var Data = new List<DetailDocumentInfo>();

                    //Sorting
                    if (string.IsNullOrEmpty(fc.SortBy))
                    {
                        fc.SortBy = "LINDOL";
                    }
                    var propertyInfo = typeof(DetailDocumentInfo).GetProperty(fc.SortBy);
                    string sortType = "ASC";
                    if ((bool)fc.IsSortTypeDESC)
                    {
                        sortType = "DESC";
                    }
                    AllDetailDocumentInfo = AllDetailDocumentInfo.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                    Data = AllDetailDocumentInfo.ToList();
                    //Pagination
                    long TotalCount = AllDetailDocumentInfo.Count();
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
                        Data = Data.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                        p.Limit = (long)fc.PageSize;
                        p.CurrentPage = (long)fc.Page;
                        p.Total = TotalCount;
                    }

                    return new APIListResponse(true, Data, p, "BusEntConcelho Listed");
                }
                else
                {
                    return new APIListResponse(false, "", p, "No List");

                }

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

        [HttpPost("Get")]
        public APIResponse Get()
        {
            var info = _detailDocumentInfoService.GetInfo();
            if (info != null)
            {
                return new APIResponse(true, info, "Info Listed");

            }
            else
            {
                return new APIResponse(false, "", "No Listed");


            }
        }

    }
}