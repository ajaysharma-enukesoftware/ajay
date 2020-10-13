using IVPD.Helpers;
using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using static IVPD.Models.RevenueModels;

namespace IVPD.Services
{
    public interface IAlottedServicesService
    {
        public List<AlottedServices> GetAlottedService(long cashierId, string startDate, string endDate, string entityId, string is_invoiced, string currDateString,string local);
        AlottedServices Create(AlottedServicesCreateRequest p, long cashierId, AllTransactions previousEntityBalance);
        public OpeningClosedAmount GetLastClosed(long cashierId);
        public List<TaxType> TaxGetAll();
        public AllTransactions CheckPreviousBalance(int entity_id);
        public string getById(long id);
        public List<IssueDocumentDetails> GetInvoiceList(long cashierId, string startDate, string endDate, string entityId, string currDateString,string isnotpaid, string docType, string local, string is_doc_generated);
        public List<IssueDocumentDetails> GetInvoiceListForInvoice(long cashierId, string startDate, string endDate, string entityId, string currDateString, string isnotpaid, string docType, string local, string is_doc_generated,string forInvoice);

        public List<AlottedServices> GetAlottedServicesByInvoice(long cashierId, long invoiceId);
        public BillingAddress lastBillingID(int entityId);
        public string GenerateInvoiceLink(IssueDocumentDetails doc, AllTransactions p);

    }
    public class AlottedServicesService : IAlottedServicesService
    {
        private RevenueContext _context;
        public AlottedServicesService(RevenueContext context)
        {
            _context = context;
        }
       public string GenerateInvoiceLink(IssueDocumentDetails doc, AllTransactions p)
        {
           
            BusEntidade bus = _context.BusEntidade.Where(w => w.nifap == doc.entity_id.ToString()).ToList().LastOrDefault();
            Currency cu = _context.Currency.Where(w => w.id == p.currency_id).ToList().LastOrDefault();

            var html = System.IO.File.ReadAllText("StaticFiles/InvoiceTemplate.html");
            html = html.Replace("{entity}", bus.nome);
            html = html.Replace("{Factura}", doc.document_type);
            html = html.Replace("{no}", doc.no);
            html = html.Replace("{expirydate}", doc.deadline_date.ToString());
            html = html.Replace("{quantity}", doc.total_item.ToString());
            html = html.Replace("{unitprice}", p.total_cr.ToString());
            html = html.Replace("{iva}", doc.iva);
            html = html.Replace("{valor}", doc.total_amount.ToString());
            html = html.Replace("{sumvalue}", doc.total_amount.ToString());
            if (cu!= null)
            {
                html = html.Replace("{coin}", cu.base_currency.ToString());
            }
            else
            {
                html = html.Replace("{coin}","EUR");
            }
            html = html.Replace("Rua Ferreira Borges 27", bus.moradaFiscal);
            html = html.Replace("{issuedate}", (Convert.ToDateTime(doc.issue_date_time).Date).ToString());
            html = html.Replace("{transno}", p.trans_no.ToString());


            string pathUrl = Path.Combine(@"StaticFiles/", "Invoice" + doc.id + "_" + doc.no + ".Pdf");

            HtmlToPdf h = new HtmlToPdf();
         
            h.Options.MinPageLoadTime = 2;
            h.Options.MaxPageLoadTime = 12000;

            PdfDocument pdf = h.ConvertHtmlString(html, "C:/DevelopmentIVPD/api/StaticFiles/IVDP-logo");

            pdf.RemovePageAt(0);
            pdf.Save(pathUrl);
            return pathUrl;
        }
        public BillingAddress lastBillingID(int entityId)
        {
            BillingAddress cu = _context.BillingAddress.Where(w => w.entity_id == entityId).Where(w => w.suggested == 1).ToList().FirstOrDefault();
            if (cu != null)
            {
                return cu;
            }
            else
            {
                BillingAddress cu1 = _context.BillingAddress.Where(w => w.entity_id == entityId).ToList().FirstOrDefault();
                return cu1;
            }

        }

        public string getById(long id)
        {
            LIBTESOUR_TSARTIF service = _context.LIBTESOUR_TSARTIF.Where(x => x.id == id).ToList().FirstOrDefault();
            return service.CODART;

        }
        public List<TaxType> TaxGetAll()
        {
            return _context.TaxType.ToList();
        }
        public AllTransactions CheckPreviousBalance(int entity_id)
        {
            AllTransactions previousEntityBalance = _context.AllTransactions.Where(x => x.entity_id == entity_id).Where(x => x.deleted_at == null).ToList().LastOrDefault();

            return previousEntityBalance;
        }

        public OpeningClosedAmount GetLastClosed(long cashierId)
        {

            OpeningClosedAmount o = _context.OpeningClosedAmount.Where(w => w.cashier_id == cashierId).ToList().LastOrDefault();
            if (o != null)
            {
                return o;
            }
            else
            {
                return null;
            }

        }
        public AlottedServices Create(AlottedServicesCreateRequest obj, long cashierId, AllTransactions previousEntityBalance)
        {
            double totalTax = 0.0;
            // double totalValorCativo=0.0;
            IssueDocumentDetails iss = new IssueDocumentDetails();
            AlottedServices alloted = new AlottedServices();
            AllTransactions all = new AllTransactions();
            
                totalTax = (double)(obj.total_service_tax);
            
            
            if (obj.deadline_date != null)
            {
                iss.deadline_date = Convert.ToDateTime(obj.deadline_date);

            }
            iss.local = obj.local;
      //     iss.document_type = "Invoice";
            iss.total_item = obj.services.Count();
            iss.total_amount = obj.total_amount;
           // iss.document_size = 50;           
            iss.extra_text = "Added";
            iss.no = "2020-11-0912";
            iss.service_tax = totalTax;
            iss.issue_date_time = DateTime.UtcNow; 
            iss.entity_id = obj.entity_id.Value;
            iss.valor_cativo = obj.total_valor_cativo_tax;
            if (obj.future_payment == 0)
            {
                iss.amount_paid = obj.total_services_amount+obj.total_service_tax+obj.total_valor_cativo_tax;
                iss.is_paid = 1;
                iss.flag = "Green";


            }
            else
            {
                if (obj.amount_to_be_used == null)
                {
                    iss.amount_paid = 0;
                }
                else
                {
                    if (obj.amount_to_be_used > obj.total_amount)
                    {
                        iss.amount_paid = obj.total_amount;
                    }
                    else
                    {
                        iss.amount_paid = obj.amount_to_be_used;
                    }
                }
                iss.is_paid = 0;
                iss.amount_to_be_used = obj.amount_to_be_used;

            }
            _context.IssueDocumentDetails.Add(iss);
            _context.SaveChanges();
            
            _context.Database.OpenConnection();
            _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AllTransactions ON");

            _context.SaveChanges();
            foreach (AlottedServices item in obj.services)
            {
                if (obj.future_payment == 0)
                {

                    long insertedInvoiceId = iss.id;
                    AllTransactions previousData = _context.AllTransactions.ToList().LastOrDefault();
                    if (previousData != null)
                    {
                        all.id = previousData.id + 1;
                    }
                    else
                    {
                        all.id = 1;
                    }
                    all.invoice_id = insertedInvoiceId;
                    all.local = obj.local;
                    //    all.total_dr = obj.total_amount;
                    all.total_dr = item.valor+item.service_tax+item.valor_cativo;
                    all.vat_rate = item.tax_percent;
                    all.vat_no = previousEntityBalance.vat_no;
                    AllTransactions previousEntityBalance1 = CheckPreviousBalance(obj.entity_id.Value);

                    //previousEntityBalance1.current_balance = (double)(previousEntityBalance1.current_balance - item.valor);
                    all.current_balance = (double)(previousEntityBalance1.current_balance - all.total_dr);
                    all.cashier_Id = cashierId;
                    all.entity_id = obj.entity_id.Value;
                    all.Created_at = DateTime.UtcNow;
                    all.trans_date = DateTime.UtcNow;
                    all.trans_type = "DR";
                    all.useful_cashier = previousEntityBalance.useful_cashier;
                    all.billing_id = previousEntityBalance.billing_id;
                    all.currency_id = 4;
                    all.trans_method_id = 5;
                    string serviceName = getById(item.service_id);
                    all.trans_msg = serviceName;
                    BillingAddress bis = lastBillingID(obj.entity_id.Value);
                    if (bis == null)
                    {
                        all.billing_id = null;

                    }
                    else
                    {
                        all.billing_id = bis.id;

                    }
                 

                    //  all.bank = previousEntityBalance.bank;
                    _context.AllTransactions.Add(all);
                    _context.SaveChanges();
                    long transId = all.id;

                    DebitDetails debit = new DebitDetails();
                    debit.debit_id = transId;
                    debit.quantity = item.unit;
                    debit.unit_amount = Convert.ToDecimal(item.per_services_amount);
                    _context.DebitDetails.Add(debit);
                    _context.SaveChanges();

                    alloted = item;
                    alloted.local = obj.local;
                    alloted.invoice_id = insertedInvoiceId;
                    alloted.entity_id = obj.entity_id.Value;
                    alloted.trans_id = transId;
                    alloted.cashier_id = cashierId;
                 //   alloted.service_tax = item.service_tax;
                    alloted.is_invoiced =1;
                    _context.AlottedServices.Add(alloted);
                    _context.SaveChanges();
                }

                else
                {

                    long insertedInvoiceId = iss.id;

                    if (obj.amount_to_be_used != null)
                    {
                        if (obj.amount_to_be_used > 0)
                        {


                            AllTransactions previousData = _context.AllTransactions.ToList().LastOrDefault();
                            if (previousData != null)
                            {
                                all.id = previousData.id + 1;
                            }
                            else
                            {
                                all.id = 1;
                            }
                            all.invoice_id = insertedInvoiceId;
                            all.local = obj.local;
                            all.vat_rate = item.tax_percent;
                            all.vat_no = previousEntityBalance.vat_no;
                            AllTransactions previousEntityBalance1 = CheckPreviousBalance(obj.entity_id.Value);

                            if (obj.amount_to_be_used >= obj.total_amount)
                            {
                                all.total_dr = item.valor + item.service_tax + item.valor_cativo;
                                obj.amount_to_be_used -= all.total_dr;
                            }
                            else if (obj.amount_to_be_used < obj.total_amount && obj.amount_to_be_used > (item.valor + item.service_tax + item.valor_cativo))
                            {
                                all.total_dr = item.valor + item.service_tax + item.valor_cativo;
                                obj.amount_to_be_used -= all.total_dr;
                                // all.total_dr = obj.amount_to_be_used;
                                //    obj.amount_to_be_used -= all.total_dr;

                            }
                            else
                            {
                                all.total_dr = obj.amount_to_be_used;
                                obj.amount_to_be_used -= all.total_dr;
                            }

                            all.current_balance = (double)(previousEntityBalance1.current_balance - all.total_dr);
                            all.cashier_Id = cashierId;
                            all.entity_id = obj.entity_id.Value;
                            all.Created_at = DateTime.UtcNow;
                            all.trans_date = DateTime.UtcNow;
                            all.trans_type = "DR";
                            all.useful_cashier = previousEntityBalance.useful_cashier;
                            all.billing_id = previousEntityBalance.billing_id;
                            all.currency_id = 4;
                            all.trans_method_id = 5;
                            string serviceName = getById(item.service_id);
                            all.trans_msg = serviceName;
                            BillingAddress bis = lastBillingID(obj.entity_id.Value);
                            if (bis == null)
                            {
                                all.billing_id = null;

                            }
                            else
                            {
                                all.billing_id = bis.id;

                            }
                            _context.AllTransactions.Add(all);
                            _context.SaveChanges();
                        }
                        long transId = all.id;

                        alloted = item;
                            alloted.invoice_id = insertedInvoiceId;
                            alloted.entity_id = obj.entity_id.Value;
                            alloted.is_invoiced = 0;
                            alloted.trans_id = transId;
                            alloted.cashier_id = cashierId;
                            alloted.local = obj.local;
                            //   alloted.service_tax = item.service_tax;

                            _context.AlottedServices.Add(alloted);
                            _context.SaveChanges();
                        
                    }
                    else
                    {

                        alloted = item;
                        alloted.invoice_id = insertedInvoiceId;
                        alloted.entity_id = obj.entity_id.Value;
                        alloted.is_invoiced = 0;
                        alloted.cashier_id = cashierId;
                        alloted.local = obj.local;
                        //   alloted.service_tax = item.service_tax;

                        _context.AlottedServices.Add(alloted);
                        _context.SaveChanges();
                    }
                }

            }
           
            if(obj.future_payment==0 || (obj.future_payment == 1 && obj.amount_to_be_used!=null))
            {
                IssueDocumentDetails issuedDoc2 = _context.IssueDocumentDetails.Where(w => w.id == iss.id).ToList().FirstOrDefault();
                issuedDoc2.invoice_url = GenerateInvoiceLink(issuedDoc2, all);
                issuedDoc2.document_type = "Invoice";
                _context.IssueDocumentDetails.Update(issuedDoc2);
                _context.SaveChanges(); 
            }
            return alloted;

        }
        public List<AlottedServices> GetAlottedService(long cashierId, string startDate, string endDate, string entityId, string is_invoiced, string currDateString, string local)
        {
            List<AlottedServices> u;
            if (local == null)
            {
                if (startDate == null && endDate == null && entityId == null && is_invoiced == null)
                {
                    u = _context.AlottedServices.Where(w => w.date == Convert.ToDateTime(currDateString)).ToList();

                }
                else if (startDate != null && endDate != null && entityId == null && is_invoiced == null)
                {
                    u = _context.AlottedServices.Where(w => w.date <= Convert.ToDateTime(endDate)).Where(w => w.date >= Convert.ToDateTime(startDate)).ToList();

                }
                else if (startDate == null && endDate == null && entityId != null && is_invoiced == null)
                {
                    u = _context.AlottedServices.Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.date == Convert.ToDateTime(currDateString)).ToList();

                }
                else if (startDate == null && endDate == null && entityId == null && is_invoiced != null)
                {
                    u = _context.AlottedServices.Where(w => w.is_invoiced == Convert.ToInt64(is_invoiced)).Where(w => w.date == Convert.ToDateTime(currDateString)).ToList();

                }
                else if (startDate != null && endDate != null && entityId != null && is_invoiced == null)
                {
                    u = _context.AlottedServices.Where(w => w.date <= Convert.ToDateTime(endDate)).Where(w => w.date >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                }
                //---  
                else if (startDate != null && endDate != null && entityId == null && is_invoiced != null)
                {
                    u = _context.AlottedServices.Where(w => w.date >= Convert.ToDateTime(startDate)).Where(w => w.date <= Convert.ToDateTime(endDate)).Where(w => w.is_invoiced == Convert.ToInt64(is_invoiced)).ToList();

                }
                else if (startDate == null && endDate == null && entityId != null && is_invoiced != null)
                {
                    u = _context.AlottedServices.Where(w => w.date >= Convert.ToDateTime(currDateString)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_invoiced == Convert.ToInt64(is_invoiced)).ToList();

                }

                //     else if (startDate == null && endDate != null && entityId != null)
                else
                {
                    u = _context.AlottedServices.Where(w => w.date <= Convert.ToDateTime(endDate)).Where(w => w.date >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_invoiced == Convert.ToInt64(is_invoiced)).ToList();

                }
            }
            else
            {
                if (startDate == null && endDate == null && entityId == null && is_invoiced == null && local != null)
                {
                    u = _context.AlottedServices.Where(w => w.local == local).Where(w => w.date == Convert.ToDateTime(currDateString)).ToList();

                }
                else if (startDate != null && endDate != null && entityId == null && is_invoiced == null && local != null)
                {
                    u = _context.AlottedServices.Where(w => w.local == local).Where(w => w.date <= Convert.ToDateTime(endDate)).Where(w => w.date >= Convert.ToDateTime(startDate)).ToList();

                }
                else if (startDate == null && endDate == null && entityId != null && is_invoiced == null && local != null)
                {
                    u = _context.AlottedServices.Where(w => w.local == local).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.date == Convert.ToDateTime(currDateString)).ToList();

                }
                else if (startDate == null && endDate == null && entityId == null && is_invoiced != null && local != null)
                {
                    u = _context.AlottedServices.Where(w => w.local == local).Where(w => w.is_invoiced == Convert.ToInt64(is_invoiced)).Where(w => w.date == Convert.ToDateTime(currDateString)).ToList();

                }
                else if (startDate != null && endDate != null && entityId != null && is_invoiced == null && local != null)
                {
                    u = _context.AlottedServices.Where(w => w.local == local).Where(w => w.date <= Convert.ToDateTime(endDate)).Where(w => w.date >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                }
                //---  
                else if (startDate != null && endDate != null && entityId == null && is_invoiced != null && local != null)
                {
                    u = _context.AlottedServices.Where(w => w.local == local).Where(w => w.date >= Convert.ToDateTime(startDate)).Where(w => w.date <= Convert.ToDateTime(endDate)).Where(w => w.is_invoiced == Convert.ToInt64(is_invoiced)).ToList();

                }
                else if (startDate == null && endDate == null && entityId != null && is_invoiced != null && local != null)
                {
                    u = _context.AlottedServices.Where(w => w.local == local).Where(w => w.date >= Convert.ToDateTime(currDateString)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_invoiced == Convert.ToInt64(is_invoiced)).ToList();

                }
                else
                {
                    u = _context.AlottedServices.Where(w => w.local == local).Where(w => w.date <= Convert.ToDateTime(endDate)).Where(w => w.date >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_invoiced == Convert.ToInt64(is_invoiced)).ToList();

                }
            }
           
            List<AlottedServices> AlottedServices = new List<AlottedServices>();
            if (u != null)
            {
                foreach (AlottedServices item in u)
                {
                    AlottedServices bo = new AlottedServices();
                    BusEntidade td = _context.BusEntidade.Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();

                    LIBTESOUR_TSARTIF lb = _context.LIBTESOUR_TSARTIF.Where(w => w.id == item.service_id).ToList().FirstOrDefault();
                    bo = item;
                    //box.Add(item);
                    bo.service = lb;
                    bo.busEntidadeDetail = td;

                    AlottedServices.Add(bo);

                }
                return AlottedServices;
            }
            else
            {
                return null;
            }

        }

        public List<AlottedServices> GetAlottedServicesByInvoice(long cashierId, long invoiceId)
        {
            List<AlottedServices> u;
          
          u = _context.AlottedServices.Where(w => w.invoice_id == invoiceId).ToList();


      
            List<AlottedServices> AlottedServices = new List<AlottedServices>();
            if (u != null)
            {
                foreach (AlottedServices item in u)
                {
                    AlottedServices bo = new AlottedServices();
                    BusEntidade td = _context.BusEntidade.Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();

                    LIBTESOUR_TSARTIF lb = _context.LIBTESOUR_TSARTIF.Where(w => w.id == item.service_id).ToList().FirstOrDefault();
                    bo = item;
                    //box.Add(item);
                    bo.service = lb;
                    bo.busEntidadeDetail = td;

                    AlottedServices.Add(bo);

                }
                return AlottedServices;
            }
            else
            {
                return null;
            }

        }

        public List<IssueDocumentDetails> GetInvoiceList(long cashierId, string startDate, string endDate, string entityId, string currDateString, string isPaid,string docType,string local,string is_doc_generated)
        {
            List<IssueDocumentDetails> u;
            if (is_doc_generated == null)
            {
                if (docType == null)
                {

                    if (local == null)
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time == Convert.ToDateTime(currDateString)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                    else
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                }
                else
                {
                    if (local != null)
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                    else
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                }
            }
            else
            {
                if (docType == null)
                {

                    if (local == null)
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Take(100).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                    else
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                }
                else
                {
                    if (local != null)
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                    else
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                }
            }
            List<IssueDocumentDetails> issList = new List<IssueDocumentDetails>();
            List<AllTransactions> transactionList = new List<AllTransactions>();

            if (u != null)
            {
                foreach (IssueDocumentDetails item in u)
                {
                    IssueDocumentDetails iss = new IssueDocumentDetails();
                    BusEntidade td = _context.BusEntidade.Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();
                  List<AllTransactions> all = _context.AllTransactions.Where(w => w.invoice_id == item.id).ToList();
                    //  LIBTESOUR_TSARTIF lb = _context.LIBTESOUR_TSARTIF.Where(w => w.id == item.service_id).ToList().FirstOrDefault();
                    iss = item;

                    transactionList = all;
                    //box.Add(item);
                    //  iss.service = lb;
                    iss.busEntidade = td;
                    iss.allTransaction = transactionList;
                    issList.Add(iss);

                }
                return issList;
            }
            else
            {
                return null;
            }

        }
        public List<IssueDocumentDetails> GetInvoiceListForInvoice(long cashierId, string startDate, string endDate, string entityId, string currDateString, string isPaid, string docType, string local, string is_doc_generated,string forInvoice)
        {
            List<IssueDocumentDetails> u;
            if (is_doc_generated == null)
            {
                if (docType == null)
                {

                    if (local == null)
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.issue_date_time == Convert.ToDateTime(currDateString)).Where(w => w.for_invoice_id !=null).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                    else
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                }
                else
                {
                    if (local != null)
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && local != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                    else
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                }
            }
            else
            {
                if (docType == null)
                {

                    if (local == null)
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Take(100).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                    else
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                }
                else
                {
                    if (local != null)
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && local != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.local == local).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                    else
                    {
                        if (startDate == null && endDate == null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Take(100).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        //---  
                        else if (startDate == null && endDate != null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate != null && endDate != null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        /// 3 nulls
                        else if (startDate == null && endDate == null && entityId == null && isPaid != null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                        else if (startDate == null && endDate == null && entityId != null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

                        }
                        else if (startDate == null && endDate != null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).ToList();

                        }
                        else if (startDate != null && endDate == null && entityId == null && isPaid == null && is_doc_generated != null)
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).ToList();

                        }
                        else
                        {
                            u = _context.IssueDocumentDetails.Where(w => w.for_invoice_id != null).Where(w => w.is_doc_generated == Convert.ToInt32(is_doc_generated)).Where(w => w.document_type == docType).Where(w => w.issue_date_time <= Convert.ToDateTime(endDate)).Where(w => w.issue_date_time >= Convert.ToDateTime(startDate)).Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.is_paid == Convert.ToInt32(isPaid)).ToList();

                        }
                    }
                }
            }
            List<IssueDocumentDetails> issList = new List<IssueDocumentDetails>();
            List<AllTransactions> transactionList = new List<AllTransactions>();

            if (u != null)
            {
                foreach (IssueDocumentDetails item in u)
                {
                    IssueDocumentDetails iss = new IssueDocumentDetails();
                    BusEntidade td = _context.BusEntidade.Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();
                    List<AllTransactions> all = _context.AllTransactions.Where(w => w.invoice_id == item.id).ToList();
                    //  LIBTESOUR_TSARTIF lb = _context.LIBTESOUR_TSARTIF.Where(w => w.id == item.service_id).ToList().FirstOrDefault();
                    iss = item;

                    transactionList = all;
                    //box.Add(item);
                    //  iss.service = lb;
                    iss.busEntidade = td;
                    iss.allTransaction = transactionList;
                    issList.Add(iss);

                }
                return issList;
            }
            else
            {
                return null;
            }

        }

    }
}
