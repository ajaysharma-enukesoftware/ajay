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
    [Route("api/Vindima/FileDetail")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FileDetailController : ControllerBase
    {
        private IFileDetailService _fileDetailService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public FileDetailController(
            IFileDetailService fileDetailService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _fileDetailService = fileDetailService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<FileDetail> AllFileDetail = _fileDetailService.GetAll();
                var FileDetailData = new List<FileDetail>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "nome";
                }
                var propertyInfo = typeof(FileDetail).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllFileDetail = AllFileDetail.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                FileDetailData = AllFileDetail.ToList();
                //Pagination
                long TotalCount = AllFileDetail.Count();
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
                    FileDetailData = FileDetailData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, FileDetailData, p, "File Detail Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
    }
}