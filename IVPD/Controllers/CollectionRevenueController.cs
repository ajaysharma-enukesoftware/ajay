using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Razor.Language;

namespace IVPD.Controllers
{

    [EnableCors("AllowAll")]
    [Route("api/Revenue")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CollectionRevenueController : ControllerBase
    {
        private ICollectionRevenueService _collectionRevenueService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;


        public CollectionRevenueController(
            ICollectionRevenueService collectionRevenueService,
            IMapper mapper,
            IOptions<AppSettings> appSettings, IAuditLogService auditLogService)
        {
            _collectionRevenueService = collectionRevenueService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;

        }
        [HttpPost("CollectionRevenue/GetAll")]
        public APIListResponse GetAll([FromBody] FilterCollectionRevenue fc)
        {
            Pagination p = new Pagination();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);
                IEnumerable<AllTransactions> AllDetailDocumentInfo;
               
                string entityId = fc.Filters["entity_id"];
                string startDate = fc.Filters["start_date"];
                string endDate = fc.Filters["end_date"];
               
                var currDateString = DateTime.Now.ToString("yyyy-MM-dd");
                //DateTime currdate = Convert.ToDateTime(dateAsString);

               
                   AllDetailDocumentInfo = _collectionRevenueService.GetAll(userId, currDateString, startDate, endDate, entityId);

                var Data = new List<AllTransactions>();

                if (AllDetailDocumentInfo.Any())
                {

                    //Sorting
                    if (string.IsNullOrEmpty(fc.SortBy))
                    {
                        fc.SortBy = "id";
                    }
                    var propertyInfo = typeof(AllTransactions).GetProperty(fc.SortBy);
                    string sortType = "DESC";
                   /* if ((bool)fc.IsSortTypeDESC)
                    {
                        sortType = "DESC";
                    }*/
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

                    return new APIListResponse(true, Data, p, "Receita de coleção listada");
                }
                else
                {
                    return new APIListResponse(true, Data, p, "");

                }

            }
            catch (Exception ex)
            {
                return new APIListResponse(false, "", p, ex.Message);
            }
        }
        [HttpPost("CollectionRevenue/GetBillingAddess")]
        public APIResponse GetBillingAddress([FromBody]APIBilingAddressRequest request)
        {
            try
            {
                var p = _collectionRevenueService.GetBilingAddress(request.entity_id);
                if (p == null)
                {
                    return new APIResponse(false, "", "Nenhum endereço existe com o ID de entidade fornecido!");
                }
                else
                {
                    return new APIResponse(true, p, "Endereço de cobrança devolvido!");
                }
            }
            catch(Exception ex)
            {
                return new APIResponse(false, "", ex.Message);

            }
        }

        [HttpPost("GetEntityDetails")]
        public APIResponse GetEntityDetails([FromBody] APIBilingAddressRequest request)
        {
            try
            {
                var p = _collectionRevenueService.GetEntityDetails(request.entity_id);
                if (p == null)
                {
                    return new APIResponse(false, "", "Nenhuma entidade existe com o id de entidade fornecido!");
                }
                else
                {
                    return new APIResponse(true, p, "Detalhes da entidade devolvidos!");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);

            }
        }

        [HttpPost("UpdateBillingAddess")]
        public APIResponse UpdateBillingAddress([FromBody]UpdateBilling b)
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
            //    OpeningClosedAmount last = _collectionRevenueService.GetLastClosed(Convert.ToInt64(userId));
           //     if (last != null)
          //      {
           //         if (last.closed == 0)
           //         {
                        var isUpdated=_collectionRevenueService.UpdateBilingAddress(b, userId);
                         if(isUpdated)
                         {
                           AuditLog obj = new AuditLog();
                           obj.Activity = "Update";
                           //obj.OldRecord = JsonConvert.SerializeObject(_parcelService.GetById(Convert.ToInt32(id)));
                          obj.OldRecord = "";
                          obj.Type = "BillingAddress";
                          obj.UserID = Convert.ToInt64(userIdString);
                          obj.NewRecord = JsonConvert.SerializeObject(b);
                          _auditlogService.Create(obj);
                          return new APIResponse(true, "", "Dados atualizados com sucesso");
                         }
                         else
                         {
                          return new APIResponse(false, "", "Dados não atualizados");

                         }
            //        }
            //        else
            //        {
             //           return new APIResponse(false, "", "You should open cashier before proceeding this request");
             //       }
             //   }
             //   else
             //   {
             //       return new APIResponse(false, "", "You should open cashier before proceeding this request");

             //   }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);

            }
        }
        [HttpPost("UpdatePaymentMethod")]
        public APIResponse UpdatePaymentMethod([FromBody]UpdatePaymentMethod b)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);
           //     OpeningClosedAmount last = _collectionRevenueService.GetLastClosed(Convert.ToInt64(userId));
            //    if (last != null)
            //    {
             //       if (last.closed == 0)
            //        {
                        var isUpdated= _collectionRevenueService.UpdatePaymentMethod(b, userId);
                          if (isUpdated)
                          {
                            AuditLog obj = new AuditLog();
                            obj.Activity = "Update";
                            //obj.OldRecord = JsonConvert.SerializeObject(_parcelService.GetById(Convert.ToInt32(id)));
                             obj.OldRecord = "";
                            obj.Type = "Payment Method";
                            obj.UserID = Convert.ToInt64(userIdString);
                            obj.NewRecord = JsonConvert.SerializeObject(b);
                            _auditlogService.Create(obj);
                            return new APIResponse(true, "", "Dados atualizados com sucesso");
                          }
                         else
                         {
                            return new APIResponse(false, "", "Não existem dados!");

                         }
            //        }
         //           else
          //          {
         //               return new APIResponse(false, "", "You should open cashier before proceeding this request");
        //            }
        //        }
         //       else
          //      {
            //        return new APIResponse(false, "", "You should open cashier before proceeding this request");

            //    }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);

            }
        }

        [HttpPost("CreateBillingAddress")]
        public APIResponse Create([FromBody] BillingAddress model)
        {
            try
            {               
                _collectionRevenueService.CreateAddress(model);
                return new APIResponse(true, "", "Endereço criado com sucesso");
                          
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message); ;
            }
        }
        [HttpPost("UpdateBillingAddress/{id:long?}")]
        public APIResponse Update([FromBody] BillingUpdateAddressRequest model, Int64 id)
        {
            try
            {

                model.id = id;
                _collectionRevenueService.UpdateAddress(model);
                return new APIResponse(true, "", "Endereço AtualizadoCom sucesso");

            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return new APIResponse(false, "", ex.Message); ;
            }
        }

        [HttpPost("GetEntityAddress")]
        public APIResponse GetEntityAddress([FromBody] APIBilingAddressRequest request)
        {
            try
            {
                var p = _collectionRevenueService.GetEntityAddressWithCodEntidade(request.entity_id);
                if (p == null)
                {
                    return new APIResponse(false, "", "Nenhuma entidade existe com o id de entidade fornecido!");
                }
                else
                {
                    return new APIResponse(true, p, "Detalhes da entidade devolvidos!");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);

            }
        }
    }
}