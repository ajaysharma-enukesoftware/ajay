using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

using System.IO;
using Microsoft.AspNetCore.Mvc;
using IVPD.Helpers;
using System.Configuration;

using System.Text;
using SelectPdf;
using System.Diagnostics.Eventing.Reader;

namespace IVPD.Services
{
    public interface IAddTransactionService 
    {
        public List<Currency> CurrencyGetAll();
        public string GenerateInvoiceLinkForAddMoney(IssueDocumentDetails doc, AllTransactions p);
        public string GenerateInvoiceLinkForInvoice(IssueDocumentDetails doc, AllTransactions p);

        AllTransactions Create(AllTransactions p, long cashierId);
        public OpeningClosedAmount GetLastClosed(long cashierId);
        public BillingAddress lastBillingID(int entityId);
        public List<IssueDocumentDetails> GetInvoiceLinkById(long[] invoiceId);

    }
    public class AddTransactionService : IAddTransactionService
    {
        private RevenueContext _context; 

        public AddTransactionService(RevenueContext context)
        {
            _context = context;

        }


        public List<IssueDocumentDetails> GetInvoiceLinkById(long[] invoiceId)
        {
            List<IssueDocumentDetails> issList = new List<IssueDocumentDetails>();
            foreach (long id in invoiceId)
            {
                IssueDocumentDetails iss = _context.IssueDocumentDetails.Where(w => w.id == id).ToList().FirstOrDefault();
                iss.is_doc_generated = 1;
                _context.IssueDocumentDetails.Update(iss);
                _context.SaveChanges();

                if (iss != null)
                {
                    issList.Add(iss);
                }
                
            }
           
            return issList;
        }

        public List<Currency> CurrencyGetAll()
        {
            return _context.Currency.ToList();

        }
        public BillingAddress lastBillingID(int entityId)
        {
            BillingAddress cu = _context.BillingAddress.Where(w => w.entity_id == entityId).Where(w => w.suggested == 1).ToList().FirstOrDefault();
            if(cu!=null)
            {
                return cu;
            }
            else
            {
                BillingAddress cu1 = _context.BillingAddress.Where(w => w.entity_id == entityId).ToList().FirstOrDefault();
                return cu1;
            }

        }
        public string GenerateInvoiceLinkForInvoice(IssueDocumentDetails doc, AllTransactions p)
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

            html = html.Replace("{coin}", cu.base_currency.ToString());
            html = html.Replace("Rua Ferreira Borges 27", bus.moradaFiscal);
            html = html.Replace("{issuedate}", (Convert.ToDateTime(doc.issue_date_time).Date).ToString());
            html = html.Replace("{transno}", p.trans_no.ToString());

            string pathUrl = Path.Combine(@"StaticFiles/", "Invoice" + doc.id + "_" + doc.no + ".Pdf");

            HtmlToPdf h = new HtmlToPdf();

            //  h.Options.WebPageHeight = 1000;
            h.Options.MinPageLoadTime = 2;
            h.Options.MaxPageLoadTime = 12000;

            PdfDocument pdf = h.ConvertHtmlString(html, "C:/DevelopmentIVPD/api/StaticFiles/IVDP-logo");
            pdf.RemovePageAt(0);
            pdf.Save(pathUrl);
            return pathUrl;
        }
        public string GenerateInvoiceLinkForAddMoney(IssueDocumentDetails doc, AllTransactions p)
        {
           
            BusEntidade bus = _context.BusEntidade.Where(w => w.nifap == doc.entity_id.ToString()).ToList().LastOrDefault();
            Currency cu = _context.Currency.Where(w => w.id == p.currency_id).ToList().LastOrDefault();

            var html = System.IO.File.ReadAllText("StaticFiles/AddMoneyTemplate.html");
            html = html.Replace("{entity}", bus.nome);
            html = html.Replace("{Factura}", doc.document_type);
            html = html.Replace("{no}", doc.no);
            html = html.Replace("{expirydate}", doc.deadline_date.ToString());
            html = html.Replace("{quantity}", doc.total_item.ToString());
            html = html.Replace("{unitprice}", p.total_cr.ToString());
            html = html.Replace("{iva}", doc.iva);
            html = html.Replace("{valor}", doc.total_amount.ToString());
            html = html.Replace("{sumvalue}", doc.total_amount.ToString());
            html = html.Replace("{coin}", cu.base_currency.ToString());
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
            //return File(pf, "application/pdf", pathUrl);
        }
        public AllTransactions  Create(AllTransactions p, long cashierId)
        {
           long? reqInvoiceId= p.invoice_id;
            Currency cu = _context.Currency.Where(w => w.id == p.currency_id).ToList().FirstOrDefault();

            IssueDocumentDetails issueDoc = new IssueDocumentDetails();
            issueDoc.document_type = "Credit";
            issueDoc.entity_id = p.entity_id;
            issueDoc.total_amount = (double)(p.total_cr.Value * cu.conversion_rate);
            issueDoc.extra_text = "Balance Added";
            issueDoc.no =  "2020-11-0912";
            
            issueDoc.issue_date_time = p.trans_date;
            issueDoc.is_paid = 1;
            issueDoc.local = p.local;
            //issueDoc.document_url = GenerateInvoiceLink(issueDoc,p);

            _context.IssueDocumentDetails.Add(issueDoc);
            _context.SaveChanges();
            long invoiceId = issueDoc.id;

            issueDoc.document_url = GenerateInvoiceLinkForAddMoney(issueDoc, p);
            _context.IssueDocumentDetails.Update(issueDoc);
            _context.SaveChanges();

            AllTransactions u1 = _context.AllTransactions.Where(w => w.entity_id == p.entity_id).Where(x => x.deleted_at == null).ToList().LastOrDefault();

            List<TransactionDetails> td = new List<TransactionDetails>();
            if (p.total_cr.HasValue)
            {
                double amount = (double)(p.total_cr.Value * cu.conversion_rate);
                if (u1 != null)
                {
                    p.current_balance = u1.current_balance + amount;

                }
                else
                {
                    p.current_balance = amount;
                }
                p.total_cr = amount;
            }
            p.cashier_Id = cashierId;
            p.trans_type = "CR";
            p.Created_at = DateTime.UtcNow;
            p.invoice_id = invoiceId;
            BillingAddress bi= lastBillingID(p.entity_id);
            if (bi == null)
            {
                p.billing_id = null;

            }
            else
            {
                p.billing_id = bi.id;

            }
            _context.AllTransactions.Add(p);
            _context.SaveChanges();

            if (p.trans_method_id == 2 && p.transactionDetail != null)
            {
                td = p.transactionDetail;
                foreach (TransactionDetails item in td)
                {
                    item.all_trans_id =p.id;
                    _context.TransactionDetails.Add(item);
                    _context.SaveChanges();

                }

            }
            _context.Database.OpenConnection();
            _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AllTransactions ON");
            _context.SaveChanges();
         
                //  IssueDocumentDetails issuedDoc1 = _context.IssueDocumentDetails.Where(w => w.id == reqInvoiceId).ToList().FirstOrDefault();
             //   _context.IssueDocumentDetails.Update(issueDoc);
            //    _context.SaveChanges();
            
            if (reqInvoiceId != null)
            {
            

                double amntpay=0 ;
                IssueDocumentDetails iss = _context.IssueDocumentDetails.Where(w => w.id == reqInvoiceId).ToList().FirstOrDefault();
                List<AlottedServices> alot = _context.AlottedServices.Where(w => w.invoice_id == reqInvoiceId).ToList();
                foreach (AlottedServices item in alot)
                {
                    amntpay = (double)(amntpay + item.valor+item.service_tax+item.valor_cativo);
                }
                if (iss != null)
                {
                    iss.is_paid = 1;
                    iss.amount_paid = amntpay;
                    iss.document_type = "Invoice";
                    iss.flag = "Green";
                    iss.invoice_url = GenerateInvoiceLinkForInvoice(iss, p);
                    iss.for_invoice_id = reqInvoiceId;
                    _context.IssueDocumentDetails.Update(iss);
                    _context.SaveChanges();

                }
                if (iss.amount_to_be_used == null)
                {

                    foreach (AlottedServices item in alot)
                    {
                        AllTransactions u2 = new AllTransactions();

                        AllTransactions lastTransaction = _context.AllTransactions.ToList().LastOrDefault();
                        if (lastTransaction != null)
                        {
                            u2.id = lastTransaction.id + 1;
                        }
                        else
                        {
                            u2.id = 1;
                        }
                        u2.entity_id = p.entity_id;
                        u2.total_dr = item.valor + item.service_tax + item.valor_cativo;
                        u2.total_cr = null;
                        u2.trans_type = "DR";
                        u2.trans_method_id = 5;
                        u2.vat_rate = item.tax_percent;
                        u2.trans_msg = p.trans_msg;
                        u2.Created_at = DateTime.UtcNow;
                        u2.trans_date = DateTime.UtcNow;
                        u2.current_balance = (double)(lastTransaction.current_balance - item.valor);
                        u2.invoice_id = reqInvoiceId;
                        u2.local = p.local;
                        u2.currency_id = 4;
                        u2.cashier_Id = cashierId;
                        BillingAddress bis = lastBillingID(p.entity_id);
                        if (bis == null)
                        {
                            u2.billing_id = null;

                        }
                        else
                        {
                            u2.billing_id = bis.id;

                        }
                        _context.AllTransactions.Add(u2);
                        _context.SaveChanges();
                        long transId = u2.id;

                        DebitDetails debit = new DebitDetails();
                        debit.debit_id = transId;
                        debit.quantity = item.unit;
                        debit.unit_amount = Convert.ToDecimal(item.per_services_amount);
                        _context.DebitDetails.Add(debit);
                        _context.SaveChanges();

                        item.trans_id = transId;
                        item.is_invoiced = 1;

                        _context.AlottedServices.Update(item);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    List<AlottedServices> at = _context.AlottedServices.Where(w => w.invoice_id == reqInvoiceId).ToList();
                    AllTransactions u2 = new AllTransactions();

                    AllTransactions lastTransaction = _context.AllTransactions.ToList().LastOrDefault();
                    if (lastTransaction != null)
                    {
                        u2.id = lastTransaction.id + 1;
                    }
                    else
                    {
                        u2.id = 1;
                    }
                    u2.entity_id = p.entity_id;
                    u2.total_dr = iss.total_amount-iss.amount_to_be_used;
                    u2.total_cr = null;
                    u2.trans_type = "DR";
                    u2.trans_method_id = 5;
                    u2.trans_msg = p.trans_msg;
                    u2.Created_at = DateTime.UtcNow;
                    u2.trans_date = DateTime.UtcNow;
                    u2.current_balance = (double)(lastTransaction.current_balance - u2.total_dr);
                    u2.invoice_id = reqInvoiceId;
                    u2.local = p.local;
                    u2.currency_id = 4;
                    u2.cashier_Id = cashierId;
                    BillingAddress bis = lastBillingID(p.entity_id);
                    if (bis == null)
                    {
                        u2.billing_id = null;

                    }
                    else
                    {
                        u2.billing_id = bis.id;

                    }
                    _context.AllTransactions.Add(u2);
                    _context.SaveChanges();
                    long transId = u2.id;
                    foreach(AlottedServices item in at)
                    {
                        item.trans_id = transId;
                        item.is_invoiced = 1;

                        _context.AlottedServices.Update(item);
                        _context.SaveChanges();
                    }

                }
                
               
            }
            return p;
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
    }
}
