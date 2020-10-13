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
    [Route("api/Vindima/SidePanel")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SidePanelController : ControllerBase
    {

        //private ISidePanelService _Service;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public SidePanelController(
            //ISidePanelService Service,
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
        //        IEnumerable<SidePanel> AllSidePanel = _Service.GetAll();
        //        var SidePanelData = new List<SidePanel>();

        //        //Sorting
        //        if (string.IsNullOrEmpty(fc.SortBy))
        //        {
        //            fc.SortBy = "year";
        //        }
        //        var propertyInfo = typeof(SidePanel).GetProperty(fc.SortBy);
        //        string sortType = "ASC";
        //        if ((bool)fc.IsSortTypeDESC)
        //        {
        //            sortType = "DESC";
        //        }
        //        AllSidePanel = AllSidePanel.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
        //        SidePanelData = AllSidePanel.ToList();
        //        //Pagination
        //        long TotalCount = AllSidePanel.Count();
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
        //            SidePanelData = SidePanelData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
        //            p.Limit = (long)fc.PageSize;
        //            p.CurrentPage = (long)fc.Page;
        //            p.Total = TotalCount;
        //        }

        //        return new APIListResponse(true, SidePanelData, p, "SidePanel Data List Returned");

        //    }
        //    catch (Exception ex)
        //    {
        //        return new APIListResponse(false, "", p, ex.Message);
        //    }
        //}


        [HttpPost("Create")]
        public APIResponse Create(SidePanel uf)
        {
            try
            {
                return new APIResponse(true, "", "SidePanel inserted successfully");
            }
            catch (Exception e)
            {
                return new APIResponse(false, "", e.Message);
            }
        }

    }
}