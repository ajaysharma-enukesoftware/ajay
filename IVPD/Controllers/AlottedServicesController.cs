using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IVPD.Helpers;
using IVPD.Models;
using IVPD.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static IVPD.Models.RevenueModels;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IVPD.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/Revenue")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


    [ApiController]
    public class AlottedServicesController : ControllerBase
    {
        private IAlottedServicesService _alottedServicesService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;

        public AlottedServicesController(
            IAlottedServicesService alottedServicesService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IAuditLogService auditLogService)
        {
            _alottedServicesService = alottedServicesService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;

        }

        [HttpPost("Create")]
        public APIResponse CreateAllotedService([FromBody] AlottedServicesCreateRequest p)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                //  OpeningClosedAmount last = _alottedServicesService.GetLastClosed(Convert.ToInt64(userId));
                // if (last != null)
                // {
                //     if (last.closed == 0)
                //     {
                if (p.entity_id == null)
                {
                    return new APIResponse(false, "", "Id da entidade é obrigatório");
                }
                if (p.services.Count == 0)
                {
                    return new APIResponse(false, "", "Os serviços alocados à entidade não podem estar vazios");
                }
                if(p.future_payment==1 && p.deadline_date == null)
                {
                    return new APIResponse(false, "", "a data limite deve ser passada em pagamentos futuros");
                }
                if (p.future_payment == 0 && p.deadline_date != null)
                {
                    return new APIResponse(false, "", "a data limite não deve ser ultrapassada");
                }
                
                if (p.total_amount == null)
                {
                    return new APIResponse(false, "", "O montante total é necessário");
                }
                if (p.total_services_amount == null)
                {
                    return new APIResponse(false, "", "O valor total do serviço é necessário");
                }
                if (p.total_service_tax == null)
                {
                    return new APIResponse(false, "", "Taxa de serviço total é necessária");
                }
                if (p.total_valor_cativo_tax == null)
                {
                    return new APIResponse(false, "", "O imposto valor cativo total é obrigatório");
                }
                AllTransactions previousEntityBalance = _alottedServicesService.CheckPreviousBalance(p.entity_id.Value);
                if (previousEntityBalance != null && previousEntityBalance.current_balance >= p.total_amount)
                {
                    AlottedServices result = _alottedServicesService.Create(p, Convert.ToInt64(userId), previousEntityBalance);
                    if (result != null)
                    {
                        AuditLog obj = new AuditLog();
                        obj.Activity = "Create";
                        obj.OldRecord = "";
                        obj.Type = "Allocated Services";
                        obj.UserID = Convert.ToInt64(userId);
                        obj.NewRecord = JsonConvert.SerializeObject(p);
                        _auditlogService.Create(obj);
                        return new APIResponse(true, "", "Criado com sucesso");
                    }
                    else
                    {
                        return new APIResponse(false, "", "Não Criado com Sucesso");
                    }
                }
                else
                {
                    return new APIResponse(false, "", "Verifique o seu saldo anterior");
                }
                //     }
                //     else
                //     {
                //         return new APIResponse(false, "", "You should open cashier before proceeding this request");
                //     }
                //  }
                //    else
                //    {
                //                    return new APIResponse(false, "", "You should open cashier before proceeding this request");

                //   }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
        [HttpPost("GetAlottedServicesByInvoice")]
        public APIListResponse GetAlottedServicesByInvoice([FromBody] FilterAlottedServicesByInvoice fc)
        {
            Pagination p = new Pagination();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);

              

                IEnumerable<AlottedServices> alottedServices = _alottedServicesService.GetAlottedServicesByInvoice(userId,fc.invoiceId);
                if (alottedServices.Any())
                {
                    if (string.IsNullOrEmpty(fc.SortBy))
                    {
                        fc.SortBy = "id";
                    }
                    string sortType = "ASC";
                    if ((bool)fc.IsSortTypeDESC)
                    {
                        sortType = "DESC";
                    }
                    var Data = new List<AlottedServices>();
                    alottedServices = alottedServices.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                    Data = alottedServices.ToList();
                    //Pagination
                    long TotalCount = alottedServices.Count();
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
                        Data = alottedServices.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                        p.Limit = (long)fc.PageSize;
                        p.CurrentPage = (long)fc.Page;
                        p.Total = TotalCount;
                    }

                    return new APIListResponse(true, Data, p, "Alloted Services Listed By Invoice");
                }
                else
                {
                    return new APIListResponse(false, "", p, " ");

                }

            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }
        [HttpPost("GetAlottedServices")]
        public APIListResponse GetAlottedServices([FromBody] FilterAlottedServices fc)
        {
            Pagination p = new Pagination();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);

                string entityId = fc.Filters["entity_id"];
                string startDate = fc.Filters["start_date"];
                string endDate = fc.Filters["end_date"];
                string isInvoiced = fc.Filters["is_invoiced"];
                string local;
                if (fc.Filters.ContainsKey("local"))
                {
                    local = fc.Filters["local"];
                }
                else
                {
                    local = null;

                }
                var currDateString = DateTime.Now.ToString("yyyy-MM-dd");

                IEnumerable<AlottedServices> alottedServices = _alottedServicesService.GetAlottedService(userId,startDate,endDate,entityId,isInvoiced, currDateString, local);
                var Data = new List<AlottedServices>();

                if (alottedServices.Any())
                {
                    if (string.IsNullOrEmpty(fc.SortBy))
                    {
                        fc.SortBy = "id";
                    }
                    string sortType = "ASC";
                    if ((bool)fc.IsSortTypeDESC)
                    {
                        sortType = "DESC";
                    }
                    alottedServices = alottedServices.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                    Data = alottedServices.ToList();
                    //Pagination
                    long TotalCount = alottedServices.Count();
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
                        Data = alottedServices.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                        p.Limit = (long)fc.PageSize;
                        p.CurrentPage = (long)fc.Page;
                        p.Total = TotalCount;
                    }

                    return new APIListResponse(true, Data, p, "Alloted Services Listed");
                }
                else
                {
                    return new APIListResponse(true, Data, p," ");

                }

            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }
        [HttpPost("Tax/GetAll")]
        public APIListResponse Currency([FromBody] FilterClass fc)
        {
            Pagination p = new Pagination();
            try
            {
                IEnumerable<TaxType> countries = _alottedServicesService.TaxGetAll();
                var Data = new List<TaxType>();

              
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

                return new APIListResponse(true, Data, p, "Tax Listed");


            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }
        [HttpPost("GetInvoiceList")]
        public APIListResponse GetInvoiceList([FromBody] FilterAlottedServices fc)
        {
            Pagination p = new Pagination();
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userIdString = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                long userId = Convert.ToInt32(userIdString);

                string entityId = fc.Filters["entity_id"];
                string startDate = fc.Filters["start_date"];
                string endDate = fc.Filters["end_date"];
                string isPaid = fc.Filters["is_paid"];
                string docType = fc.Filters["type"];
                string forInvoice = fc.Filters["for_invoice"];

                string local;
                string is_doc_generated = fc.Filters["is_doc_generated"];

                if (fc.Filters.ContainsKey("local"))
                {
                    local = fc.Filters["local"];

                }
                else
                {
                    local = null;

                }
                var currDateString = DateTime.Now.ToString("yyyy-MM-dd");
                IEnumerable<IssueDocumentDetails> alottedServices;
                if (forInvoice == null)
                {
                     alottedServices = _alottedServicesService.GetInvoiceList(userId, startDate, endDate, entityId, currDateString, isPaid, docType, local, is_doc_generated);

                }
                else
                {
                     alottedServices = _alottedServicesService.GetInvoiceListForInvoice(userId, startDate, endDate, entityId, currDateString, isPaid, docType, local, is_doc_generated,forInvoice);

                }
                var Data = new List<IssueDocumentDetails>();

                if (alottedServices.Any())
                {   //Sorting
                     if (string.IsNullOrEmpty(fc.SortBy))
                     {
                         fc.SortBy = "id";
                     }
                     string sortType = "ASC";
                     if ((bool)fc.IsSortTypeDESC)
                     {
                         sortType = "DESC";
                     }
                    alottedServices = alottedServices.AsQueryable().OrderBy($"{fc.SortBy} {sortType}");
                    
                    Data = alottedServices.ToList();
                    //Pagination
                    long TotalCount = alottedServices.Count();
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
                        Data = alottedServices.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
                        p.Limit = (long)fc.PageSize;
                        p.CurrentPage = (long)fc.Page;
                        p.Total = TotalCount;
                    }

                    return new APIListResponse(true, Data, p, "Invoice Listed");
                }
                else
                {
                    return new APIListResponse(true, Data, p, " ");

                }

            }
            catch (AppException ex)
            {
                return new APIListResponse(false, "", p, ex.Message); ;
            }
        }
    }
}

