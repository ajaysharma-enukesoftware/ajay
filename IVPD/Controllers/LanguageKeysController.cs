using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Options;
using IVPD.Models;
using IVPD.Services;
using IVPD.Helpers;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using System.Linq.Dynamic.Core;


namespace IVPD.Controllers
{

    [EnableCors("AllowAll")]
    [Route("api/language")]
    [ApiController]
    public class LanguageKeysController : ControllerBase
    {
        private ILanguageKeysService _languageKeysService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public LanguageKeysController(
               ILanguageKeysService languageKeysService,
               IMapper mapper,
               IOptions<AppSettings> appSettings)
        {
            _languageKeysService = languageKeysService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse GetAll([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<LanguageKeys> result = _languageKeysService.GetAll();
                var Data = new List<LanguageKeys>();

                //Sorting
                if (string.IsNullOrEmpty(fc.SortBy))
                {
                    fc.SortBy = "ID";
                }
                var propertyInfo = typeof(LanguageKeys).GetProperty(fc.SortBy);
                string sortType = "ASC";
                if ((bool)fc.IsSortTypeDESC)
                {
                    sortType = "DESC";
                }
                result = result.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                Data = result.ToList();
                //Pagination
                long TotalCount = result.Count();
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
                    Data = result.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                foreach (LanguageKeys item in Data)
                {
                    keyValues.Add(item.English, item.Portuguese);
                }
              /*  List<string> keyValues = new List<string>();
                foreach (LanguageKeys item in Data)
                {
                    keyValues.Add(item.English + "=>" + item.Portuguese);
                }*/
                return new APIListResponse(true, keyValues, p, "Languages returned Successfully");

            }
            catch (Exception e)
            {
                return new APIListResponse(false, "", p, e.Message);
            }
        }
    }
}