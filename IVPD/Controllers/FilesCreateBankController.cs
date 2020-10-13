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
    [Route("api/Vindima/FilesCreateBank")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FilesCreateBankController : ControllerBase
    {

        private IFilesCreateBankService Service;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public FilesCreateBankController(
            IFilesCreateBankService designationService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            Service = designationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<FilesCreateBank> AllFilesCreateBank = Service.GetAll();
                var FilesCreateBankData = new List<FilesCreateBank>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "n_ficheiro";
                }
                var propertyInfo = typeof(FilesCreateBank).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllFilesCreateBank = AllFilesCreateBank.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                FilesCreateBankData = AllFilesCreateBank.ToList();
                //Pagination
                long TotalCount = AllFilesCreateBank.Count();
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
                    FilesCreateBankData = FilesCreateBankData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, FilesCreateBankData, p, "FilesCreateBank Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

    }
}