using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using static IVPD.Models.RevenueModels;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Revenue")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BoxDetailsController : ControllerBase
    {
        private IBoxDetailsServices _boxDetailsService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public BoxDetailsController(
            IBoxDetailsServices boxDetailsService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _boxDetailsService = boxDetailsService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        [HttpPost("BoxDetails")]
        public APIListResponse GetAll([FromBody]APIPreviousBalanceRequest fc)
        {
            Pagination p = new Pagination();

            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);
                IEnumerable<BoxDetails> AllEstatutos = _boxDetailsService.GetBoxDetails(fc.date, userId);
                if (AllEstatutos.Any())
                {
                    var estatutoData = new List<BoxDetails>();

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
        [HttpPost("OpenCloseAmount/GetByDateCaixa")]
        public APIResponse GetAllByDateCaixa([FromBody] APIGetAmountRequest fc)
        {

            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);
                OpeningClosedAmount AllEstatutos = _boxDetailsService.GetAllByDateCaixa(fc.caixa,fc.date, userId);
                if(AllEstatutos!=null)
                { 
                    return new APIResponse(true, AllEstatutos,  "Details Returned!");
                }
                else
                {
                    return new APIResponse(false, "", "");

                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);

            }
        }

        [HttpPost("InsertOpenCloseAmount")]
        public APIResponse InsertUpdateAmount(APIInsertUpdateAmountRequest req)
        {

            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);
                var dateAsString = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime currdate = Convert.ToDateTime(dateAsString);
         //       OpeningClosedAmount last = _boxDetailsService.GetLastClosed( userId);
         //       if (last != null)
        //        {
          //          if (last.closed == 0 && Convert.ToDateTime(req.date) == last.date)
         //           {
                        OpeningClosedAmount o = _boxDetailsService.GetByDateCaixa(req.caixa, req.date, userId);
                        if (o == null)
                       {
                            OpeningClosedAmount op = new OpeningClosedAmount();
                            op.caixa = req.caixa;
                            op.cheques = req.cheques;
                            op.date = Convert.ToDateTime(req.date);
                            op.opening_amount = req.opening_amount;
                            op.numerario = req.numerario;
                            _boxDetailsService.Create(op, userId);
                            return new APIResponse(true, "", "Created Successfully");
                        }
                        else
                        {
                            _boxDetailsService.Update(req, userId);
                            return new APIResponse(true, "", "Updated Successfully");
                        }
               //     }
                  //  else if (last.closed == 1 && Convert.ToDateTime(req.date) == last.date)
                   // {
                  //      return new APIResponse(false, "", "You should open cashier before proceeding this request");

                  //  }
                 //   else if (last.closed == 0 && Convert.ToDateTime(req.date) != last.date)
                   // {
                     //   return new APIResponse(false, "", "You should close cashier before proceeding this request");

                   // }
                  /*  else if (last.closed == 1 && Convert.ToDateTime(req.date) != last.date)
                    {
                        OpeningClosedAmount op = new OpeningClosedAmount();
                        op.caixa = req.caixa;
                        op.cheques = req.cheques;
                        op.date = Convert.ToDateTime(req.date);
                        op.opening_amount = req.opening_amount;
                        op.numerario = req.numerario;
                        _boxDetailsService.Create(op, userId);
                        return new APIResponse(true, "", "Created Successfully");

                    }
                    else
                    {
                        return new APIResponse(false, "", "");

                    }
            }
                else
                {
                    OpeningClosedAmount o = _boxDetailsService.GetByDateCaixa(req.caixa, req.date, userId);
                    if (o == null)
                    {
                        OpeningClosedAmount op = new OpeningClosedAmount();
                        op.caixa = req.caixa;
                        op.cheques = req.cheques;
                        op.date = Convert.ToDateTime(req.date);
                        op.opening_amount = req.opening_amount;
                        op.numerario = req.numerario;
                        _boxDetailsService.Create(op, userId);
                        return new APIResponse(true, "", "Created Successfully");
                    }
                    else
                    {
                        _boxDetailsService.Update(req, userId);
                        return new APIResponse(true, "", "Updated Successfully");
                    }

                }
*/
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message); ;
            }
        }



        [HttpPost("LastWorkingDate")]
        public APIResponse GetById()
        { Pagination p = new Pagination();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);
              
                List<OpeningClosedAmount> obj = _boxDetailsService.GetById(userId);

                if (obj.Any())
                {
                    return new APIResponse(true, obj,"");
                }
                else
                {
                    return new APIResponse(false, "", "");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
    }
}