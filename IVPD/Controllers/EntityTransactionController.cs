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
using System.Linq.Dynamic.Core;
using static IVPD.Models.RevenueModels;
using Newtonsoft.Json;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Revenue")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class EntityTransactionController : ControllerBase
    {

        private IEntityTransactionService _entityTransactionService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;

        public EntityTransactionController(
            IEntityTransactionService entityTransactionService,
            IMapper mapper,
            IOptions<AppSettings> appSettings, IAuditLogService auditLogService)
        {
            _entityTransactionService = entityTransactionService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;

        }
        [HttpPost("ProcessInvoice")]
        public APIResponse ProcessInvoice([FromBody] ProcessInvoiceDetails request)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);

                var p = _entityTransactionService.GetProcessInvoiceDetails(request.invoiceId, userId);
                if (p == null)
                {
                    return new APIResponse(false, "", "No Data Exists!");
                }
                else
                {
                    return new APIResponse(true, p, "Data returned!");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
        [HttpPost("EntityTransactionDetails")]
        public APIResponse GetById([FromBody] EntityTransactionDetailRequest request)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);

                var p = _entityTransactionService.GetEntityTransactionDetails(request.entityId, request.transId, userId);
                if (p == null)
                {
                    return new APIResponse(false, "", "No Data Exists!");
                }
                else
                {
                    return new APIResponse(true, p, "Data returned!");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
        [HttpPost("getEntityDetail")]
        public APIResponse GetEntityDetails([FromBody] EntityDetailsRequest request)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);

                EntityDetails p = _entityTransactionService.GetEntityDetails(request.entityId);
                if (p==null)
                {
                    return new APIResponse(false, "", "");
                }
                else
                {
                    return new APIResponse(true, p, "");
                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }

        [HttpPost("TransactionMethod/GetAll")]
        public APIListResponse TransactionMethod([FromBody]FilterClass fc)
        {
            Pagination p = new Pagination();

            try
            {
                IEnumerable<TransactionMethod> countries = _entityTransactionService.TransactionMethodGetAll();
                var Data = new List<TransactionMethod>();
           
                Data = countries.ToList();
                //Pagination
                long TotalCount = countries.Count();
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
                    Data = countries.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                    p.Limit = (long)fc.PageSize;
                    p.CurrentPage = (long)fc.Page;
                    p.Total = TotalCount;
                }

                return new APIListResponse(true, Data, p, "Transaction Method Listed");


            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }

        [HttpPost("EntityAllotedService")]
        public APIListResponse EntityAllotedService([FromBody] EntityAccountDetailRequest fc)
        {
            Pagination p = new Pagination();

            try
            {
                IEnumerable<AlottedServices> countries = _entityTransactionService.GetEntityAllotedService(fc.entityId,fc.startDate,fc.endDate);
                var Data = new List<AlottedServices>();
                if (countries.Any())
                {


                    Data = countries.ToList();
                    //Pagination
                    long TotalCount = countries.Count();
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
                        Data = countries.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                        p.Limit = (long)fc.PageSize;
                        p.CurrentPage = (long)fc.Page;
                        p.Total = TotalCount;
                    }

                    return new APIListResponse(true, Data, p, "Entity AllotedService Listed");
                }
                else
                {
                    return new APIListResponse(false, Data, p, "");

                }

            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }
        [HttpPost("DeleteTransaction")]
        public APIResponse Delete([FromBody]TransactionDelete p)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);

                if (!long.TryParse(userIdString, out long i))
                {
                    userIdString = "0";
                }
             //   OpeningClosedAmount last = _entityTransactionService.GetLastClosed(Convert.ToInt64(userId));
            //    if (last != null)
            //    {
            //        if (last.closed == 0)
              //      {
                        var isDeleted = _entityTransactionService.Delete(p.transId, userId);
                        if (isDeleted)
                        {
                            AuditLog obj = new AuditLog();
                            obj.Activity = "Delete";
                            obj.OldRecord = JsonConvert.SerializeObject(p.transId);
                            obj.Type = "Transaction";
                            obj.UserID = Convert.ToInt64(userId);
                            _auditlogService.Create(obj);
                            return new APIResponse(true, "", "Transaction Deleted");

                        }
                        else
                        {
                            return new APIResponse(false, "", "No Data Exists!");

                        }
                /*    }
                    else
                    {
                        return new APIResponse(false, "", "You should open cashier before proceeding this request");
                    }
                }
                else
                {
                    return new APIResponse(false, "", "You should open cashier before proceeding this request");

                }*/
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message);
            }
        }
        [HttpPost("EntityAccountDetails")]
        public APIListResponse GeEntityAccountDetails([FromBody]EntityAccountDetailRequest fc)
        {
            Pagination p = new Pagination();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);
                IEnumerable<EntityAccountDetail> AllDetailDocumentInfo = _entityTransactionService.GetEntityAccountDetails(fc.entityId,fc.startDate,fc.endDate,userId);
                if (AllDetailDocumentInfo.Any())
                {
                    var Data = new List<EntityAccountDetail>();

                    //Sorting
                    if (string.IsNullOrEmpty(fc.SortBy))
                    {
                        fc.SortBy = "nome";
                    }
                   // var propertyInfo = typeof(CollectionRevenue).GetProperty(fc.SortBy);
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

                    return new APIListResponse(true, Data, p, "Entity Account Details Listed");
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
    }
}