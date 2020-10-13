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
using System.Security.Claims;
using static IVPD.Models.RevenueModels;
using Newtonsoft.Json;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Revenue")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AllTransactionController : ControllerBase
    {
        
        private IAllTransactionService _allTransactionService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;

        public AllTransactionController(
            IAllTransactionService allTransactionService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
             IAuditLogService auditLogService)
        {
            _allTransactionService = allTransactionService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;

        }
        [HttpPost("AllTransactionInvoice/GetAll")]
        public APIListResponse Get([FromBody] FilterAllTransaction fc)
        {
            Pagination p = new Pagination();
            IEnumerable<AllTransactions> issueDocData;

            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);

                string entity_id = fc.Filters["entity_id"];
                if (entity_id != null)
                {
                    issueDocData = _allTransactionService.AllTransactionInvoiceByEntityId(entity_id,userId).ToList();
                }
                else
                {
                    issueDocData = _allTransactionService.AllTransactionInvoice(userId).ToList();

                }

                //Sorting
                if (issueDocData.Any())
                {
                   
                    long TotalCount = issueDocData.Count();
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
                        issueDocData = issueDocData.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                        p.Limit = (long)fc.PageSize;
                        p.CurrentPage = (long)fc.Page;
                        p.Total = TotalCount;

                    }

                    return new APIListResponse(true, issueDocData, p, "All Transaction List Returned");
                }
                else
                {
                    return new APIListResponse(true, issueDocData, p, "");

                }

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }

        }
        [HttpPost("AllTransaction/GetAll")]
        public APIListResponse GetAll([FromBody] FilterAllTransaction fc)
        {
            Pagination p = new Pagination();
            IEnumerable<AllTransactions> FinalEntityData;

            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);
               
                    string entity_id = fc.Filters["entity_id"];
                    string start_date = fc.Filters["start_date"];
                    string end_date = fc.Filters["end_date"];
                string local;
                if (fc.Filters.ContainsKey("local"))
                {
                     local = fc.Filters["local"];

                }
                else
                {
                     local =null;

                }

                //  string searchString = "18";
                if (entity_id != null && start_date == null && end_date == null)
                {
                    FinalEntityData = _allTransactionService.GetByEntityId(entity_id, userId,local).ToList();

                }
                else if (start_date != null && entity_id == null && end_date == null)
                {
                    FinalEntityData = _allTransactionService.GetByStartDate(start_date, userId, local).ToList();
                }
                else if (end_date != null && entity_id == null && start_date == null)

                {
                    FinalEntityData = _allTransactionService.GetByEndDate(end_date, userId, local).ToList();

                }
                else if (entity_id != null && start_date != null && end_date == null)
                {
                    FinalEntityData = _allTransactionService.GetByEntityIdStartDate(entity_id, start_date, userId, local).ToList();

                }
                else if (start_date != null && end_date != null && entity_id == null)
                {
                    FinalEntityData = _allTransactionService.GetByStartDateEndDate(start_date, end_date, userId, local).ToList();

                }
                else if (entity_id != null && end_date != null && end_date==null)
                {
                    FinalEntityData = _allTransactionService.GetByEntityIdEndDate(entity_id, end_date, userId, local).ToList();

                }
                else if (entity_id != null && end_date != null && start_date != null)
                {
                    FinalEntityData = _allTransactionService.GetByEntityIdStartDateEndDate(entity_id, start_date, end_date, userId, local).ToList();

                }
                else
                {
                    FinalEntityData = _allTransactionService.GetAll(userId, local).ToList();

                }
                //Sorting
                if (FinalEntityData.Any())
                {
                   /* if (string.IsNullOrEmpty(fc.SortBy))
                    {
                        fc.SortBy = "id";
                    }
                    //    var propertyInfo = typeof(Parcel).GetProperty(fc.SortBy);
                    string sortType = "ASC";
                    if ((bool)fc.IsSortTypeDESC)
                    {
                        sortType = "DESC";
                    }
                    FinalEntityData = FinalEntityData.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                    //Pagination*/
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

                    return new APIListResponse(true, FinalEntityData, p, "All Transaction List Returned");
                }
                else
                {
                    return new APIListResponse(true, FinalEntityData, p, "");

                }

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }

        }
        [HttpPost("UpdateInvoice/{id:int?}")]
        public APIResponse UpdateInvoice(long id, [FromBody] UpdateInvoice p)
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

              //  OpeningClosedAmount last = _allTransactionService.GetLastClosed(Convert.ToInt64(userId));
              //  if (last != null)
              //  {
                //    if (last.closed == 0)
                //    {
                        int isUpdated = _allTransactionService.Update(id, p, userId);
                        if (isUpdated == 1)
                        {
                            AuditLog obj = new AuditLog();
                            obj.Activity = "Update";
                            obj.OldRecord = "";
                            obj.Type = "Invoice";
                            obj.UserID = Convert.ToInt64(userIdString);
                            obj.NewRecord = JsonConvert.SerializeObject(p);
                            _auditlogService.Create(obj);
                            return new APIResponse(true, "", "Data Updated Succesfully");

                        }
                        else if (isUpdated == 0)
                        {
                            return new APIResponse(false, "", "Data Not Updated");

                        }
                        else if (isUpdated == 2)
                        {
                            return new APIResponse(false, "", "Already Paid");

                        }
                        else
                        {
                            return new APIResponse(false, "", "o saldo atual da entidade não é suficiente para esta fatur");

                        }
                 //   }
                 //   else
                //    {
                 //       return new APIResponse(false, "", "You should open cashier before proceeding this request");
                 //   }
               // }
               // else
               // {
               //     return new APIResponse(false, "", "You should open cashier before proceeding this request");

               // }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
    }
}