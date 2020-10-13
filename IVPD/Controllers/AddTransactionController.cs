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
using Newtonsoft.Json;
using static IVPD.Models.RevenueModels;
using static IVPD.Startup;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Revenue")]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    public class AddTransactionController : ControllerBase
    {
        private IAddTransactionService _addTransactionService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;
        public AddTransactionController(
                  IAddTransactionService addTransactionService,
                  IMapper mapper,
                  IOptions<AppSettings> appSettings,
                  IAuditLogService auditLogService)
        {
            _addTransactionService = addTransactionService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;

        }
        [HttpPost("Currency/GetAll")]
        public APIListResponse Currency([FromBody] FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<Currency> countries = _addTransactionService.CurrencyGetAll();
                var Data = new List<Currency>();

                //Sorting
                /* if (string.IsNullOrEmpty(fc.SortBy))
                 {
                     fc.SortBy = "id";
                 }
                 string sortType = "ASC";
                 if ((bool)fc.IsSortTypeDESC)
                 {
                     sortType = "DESC";
                 }
                 countries = countries.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                */
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

                return new APIListResponse(true, Data, p, "Moeda listada");


            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }

        [HttpPost("GetInvoiceLink")]
        public APIResponse GetInvoiceLink(InvoiceLinkRequest p)
        {
           var result= _addTransactionService.GetInvoiceLinkById(p.invoice_id);
            if (result != null)
            {             
                return new APIResponse(true,result,"link de retorno");
            }
            else
            {
                return new APIResponse(false,result,"link não retorna");

            }
        }

        [HttpPost("AddBalance")]
        public APIResponse AddBalance([FromBody] AllTransactions p)
        {
            try
            {
               
                    var claimsIdentity = this.User.Identity as ClaimsIdentity;
                    var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                    if (!long.TryParse(userId, out long i))
                    {
                        userId = "0";
                    }
                    //      OpeningClosedAmount last = _addTransactionService.GetLastClosed(Convert.ToInt64(userId));
                    //      if (last != null)
                    //      {
                    //        if (last.closed == 0)
                    //        {
                    if (p.entity_id == 0)
                    {
                        return new APIResponse(false, "", "Id da entidade é obrigatório");
                    }
                   
                    if (p.trans_no == null)
                    {
                        return new APIResponse(false, "", "A transação não é necessária");
                    }
                    if (p.trans_date == null)
                    {
                        return new APIResponse(false, "", "A data da transação é obrigatória");
                    }
                    if (p.trans_msg == null)
                    {
                        return new APIResponse(false, "", "A mensagem de transação é necessária");
                    }
                    if (p.trans_type == null)
                    {
                        return new APIResponse(false, "", "O tipo de transação é obrigatório");
                    }
                    if (p.trans_method_id == 0)
                    {
                        return new APIResponse(false, "", "O método de transação é obrigatório");
                    }
                    if (p.currency_id == 0)
                    {
                        return new APIResponse(false, "", "o id da moeda é obrigatório");
                    }
                    /*  if (p.trans_method_id == 2 && p.transactionDetail.Count==0)
                      {
                          return new APIResponse(false, "", "Os detalhes da transação são obrigatórios");
                      }*/
                    if (p.trans_method_id != 2 && p.transactionDetail.Count != 0)
                    {
                        return new APIResponse(false, "", "Os detalhes da transação não são necessários");
                    }



                
                    if (p.total_cr == 0)
                    {
                        return new APIResponse(false, "", "Cr total é necessário");
                    }
                    AllTransactions result = _addTransactionService.Create(p, Convert.ToInt64(userId));
                    if (result != null)
                    {
                        AuditLog obj = new AuditLog();
                        obj.Activity = "Create";
                        obj.OldRecord = "";
                        obj.Type = "Credit Transaction";
                        obj.UserID = Convert.ToInt64(userId);
                        obj.NewRecord = JsonConvert.SerializeObject(p);
                        _auditlogService.Create(obj);
                        return new APIResponse(true, "", "Criado com sucesso");
                    }
                    else
                    {
                        return new APIResponse(false, "", "Não Criado com Sucesso");
                    }

                    //         }
                    //        else
                    //        {
                    //            return new APIResponse(false, "", "You should open cashier before proceeding this request");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return new APIResponse(false, "", "You should open cashier before proceeding this request");

                    //    }
                
               

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
    }
}
