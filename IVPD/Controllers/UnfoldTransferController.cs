
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
    [Route("api/Vindima/UnfoldTransfer")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UnfoldTransferController : ControllerBase
    {

        //private IUnfoldTransferService _Service;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UnfoldTransferController(
            //IUnfoldTransferService Service,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            //_Service = Service;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        //[HttpPost("GetAll")]
        //public APIListResponse Get([FromBody]FilterClass fc)
        //{
        //    Pagination p = new Pagination();
        //    try
        //    {
        //        IEnumerable<UnfoldTransfer> AllUnfoldTransfer = _Service.GetAll();
        //        var UnfoldTransferData = new List<UnfoldTransfer>();

        //        //Sorting
        //        if (string.IsNullOrEmpty(fc.SortBy))
        //        {
        //            fc.SortBy = "year";
        //        }
        //        var propertyInfo = typeof(UnfoldTransfer).GetProperty(fc.SortBy);
        //        string sortType = "ASC";
        //        if ((bool)fc.IsSortTypeDESC)
        //        {
        //            sortType = "DESC";
        //        }
        //        AllUnfoldTransfer = AllUnfoldTransfer.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
        //        UnfoldTransferData = AllUnfoldTransfer.ToList();
        //        //Pagination
        //        long TotalCount = AllUnfoldTransfer.Count();
        //        p.Limit = TotalCount;
        //        p.CurrentPage = 1;
        //        p.Total = TotalCount;
        //        if ((bool)fc.IsPagination)
        //        {
        //            if (fc.Page == null)
        //            {
        //                fc.Page = 1;
        //            }
        //            if (fc.PageSize == null)
        //            {
        //                fc.PageSize = 10;
        //            }
        //            UnfoldTransferData = UnfoldTransferData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
        //            p.Limit = (long)fc.PageSize;
        //            p.CurrentPage = (long)fc.Page;
        //            p.Total = TotalCount;
        //        }

        //        return new APIListResponse(true, UnfoldTransferData, p, "UnfoldTransfer Data List Returned");

        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIListResponse(false, "", p, ex.Message);
        //    }
        //}


        [HttpPost("Create")]
        public APIResponse Create(UnfoldTransfer uf)
        {
            try
            {
                return new APIResponse(true, "", "UnfoldTransfer inserted successfully");
            }
            catch (Exception e)
            {
                return new APIResponse(false, "", e.Message);
            }
        }

    }
}