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
    [Route("api/Vindima/ImportedPaymentFiles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ImportedPaymentFilesController : ControllerBase
    {

        private IImportedPaymentFilesService _Service;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ImportedPaymentFilesController(
            IImportedPaymentFilesService Service,
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
                IEnumerable<ImportedPaymentFiles> AllImportedPaymentFiles = _Service.GetAll();
                var ImportedPaymentFilesData = new List<ImportedPaymentFiles>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "n_ficheiro";
                }
                var propertyInfo = typeof(ImportedPaymentFiles).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                AllImportedPaymentFiles = AllImportedPaymentFiles.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                ImportedPaymentFilesData = AllImportedPaymentFiles.ToList();
                //Pagination
                long TotalCount = AllImportedPaymentFiles.Count();
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
                    ImportedPaymentFilesData = ImportedPaymentFilesData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, ImportedPaymentFilesData, p, "ImportedPaymentFiles Data List Returned");

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }

    }
}