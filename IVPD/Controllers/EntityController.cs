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
    [Route("api/Entity")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class EntityController : ControllerBase
    {

        private IBusEntidadeService _entityService;
        private IBusEntidadeEstatutoService _entityEstatutoService;
        private IEstatutoService _rulesService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public EntityController(
            IBusEntidadeService entityService,
            IEstatutoService rulesService, IBusEntidadeEstatutoService entityEstatutoService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _entityEstatutoService = entityEstatutoService;
            _entityService = entityService;
            _rulesService = rulesService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("GetAll")]
        public APIListResponse Get([FromBody] FilterClassEntity fc)
        {
                Pagination p = new Pagination();
            IEnumerable<BusEntidade>  FinalEntityData;
            List<BusEntityList> list = new List<BusEntityList>();

            try
            {
              // string isPermission= GetP(fc.modulename,fc.Id);
             //   if (isPermission == "true")
            //    {
                    string searchString = fc.Filters["searchString"];
                    string searchStringNIF = fc.Filters["searchStringNIF"];

                    //  string searchString = "18";
                    if (searchString == null)
                    {
                        FinalEntityData = _entityService.GetAll(searchStringNIF).ToList();

                    }
                    else
                    {
                        FinalEntityData = _entityService.GetAllFilters(searchString).ToList();

                    }
                    //Sorting
                    if (string.IsNullOrEmpty(fc.SortBy))
                    {
                        fc.SortBy = fc.SortBy;
                    }
                    var propertyInfo = typeof(Parcel).GetProperty(fc.SortBy);
                    string sortType = "ASC";
                    if ((bool)fc.IsSortTypeDESC)
                    {
                        sortType = "DESC";
                    }
                    FinalEntityData = FinalEntityData.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                    //Pagination
                    long TotalCount = FinalEntityData.Count();
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
                        FinalEntityData = FinalEntityData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                        p.Limit = (long)fc.PageSize;
                        p.CurrentPage = (long)fc.Page;
                        p.Total = TotalCount;

                    }
                    foreach (BusEntidade item in FinalEntityData)
                    {
                        BusEntityList l = new BusEntityList();
                        l.codEntidade = item.codEntidade;
                        l.codPais2nif = item.codPais2nif;
                        l.nif = item.nif;
                        l.nifap = item.nifap;
                        l.nome = item.nome;
                        list.Add(l);
                    }
                    return new APIListResponse(true, list, p, "Entity List Returned");
            //    }
            //    else
            //    {
              //      return new APIListResponse(true, "", p, "Permission is not allowed!");

              //  }
            }
                catch (Exception ex)
                {
                    return new APIListResponse(false, "", p, ex.Message);
                }
            
        }
       

        [HttpPost("CheckPermission")]
        public APIResponse Get([FromBody] APIRequest fc)
        {
            try
            {
                List<BusEntidadeEstatuto> FinalEntityData = _entityEstatutoService.CheckPermission(Convert.ToInt32(fc.id));
                List<BusEntidadeEstatuto> chk = new List<BusEntidadeEstatuto>();
                bool chkpermission = false;
                List<Estatuto> allrules = new List<Estatuto>();
                if (!string.IsNullOrEmpty(fc.modulename))
                {
                    switch (fc.modulename.ToLower())
                    {
                        case "parcel":
                            allrules = _rulesService.GetAllParcels();
                            break;
                        default:
                            chkpermission = false;
                            break;
                    }
                }

                if (allrules != null && allrules.Count() > 0)
                {
                    long[] ids = FinalEntityData.Select(s => Convert.ToInt64(s.codEstatuto)).ToArray();
                    long co = allrules.Select(s => Convert.ToInt64(s.CodEstatuto)).ToArray().Intersect(FinalEntityData.Select(s => Convert.ToInt64(s.codEstatuto))).Count();

                    if (co > 0)
                    {
                        chkpermission = true;
                    }
                }
                if (chkpermission)
                {
                    return new APIResponse(true, chkpermission, "Permission is allowed!");
                }
                else
                {
                    return new APIResponse(false, chkpermission, "Permission is not allowed!");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

    }
}