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

namespace IVPD.Controllers
{

    [EnableCors("AllowAll")]
    [Route("api/Revenue")]
    [ApiController]
    public class InvoiceCreationController : ControllerBase
    {
        private IInvoiceCreationService _invoiceCreationService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private IAuditLogService _auditlogService;
        public InvoiceCreationController(
                  IInvoiceCreationService invoiceCreationService,
                  IMapper mapper,
                  IOptions<AppSettings> appSettings,
                  IAuditLogService auditLogService)
        {
            _invoiceCreationService = invoiceCreationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _auditlogService = auditLogService;

        }
        [HttpPost("InvoiceCreationWithoutAmount")]
        public APIResponse InvoiceCreationWithoutAmount([FromBody] InvoiceCreation p)
        {
           try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                if (!long.TryParse(userId, out long i))
                {
                    userId = "0";
                }
                OpeningClosedAmount last = _invoiceCreationService.GetLastClosed(Convert.ToInt64(userId));
                if (last != null)
                {
                    if (last.closed == 0)
                    {
                        /*InvoiceCreation result = _invoiceCreationService.Create(p, Convert.ToInt64(userId));
                        if (result != null)
                        {
                            AuditLog obj = new AuditLog();
                            obj.Activity = "Create";
                            obj.OldRecord = "";
                            obj.Type = "Invoice Creation without amount";
                            obj.UserID = Convert.ToInt64(userId);
                            obj.NewRecord = JsonConvert.SerializeObject(p);
                            _auditlogService.Create(obj);
                            return new APIResponse(true, "", "Created Successfully");
                        }
                        else
                        {
                            return new APIResponse(false, "", "");
                        }
                        */
                        return new APIResponse(false, "", "");

                    }
                    else
                    {
                        return new APIResponse(false, "", "You should open cashier before proceeding this request");
                    }
                }
                else
                {
                    return new APIResponse(false, "", "You should open cashier before proceeding this request");

                }

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
    }
}
