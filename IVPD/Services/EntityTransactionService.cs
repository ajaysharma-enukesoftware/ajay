using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Services
{
    public interface IEntityTransactionService
    {
        public List<EntityTransaction> GetEntityTransactionDetails(int entityId, long transactionId, long cashierId);
        public EntityDetails GetEntityDetails(int entityId);

        public List<EntityAccountDetail> GetEntityAccountDetails(int entityId, string startDate, string endDate, long cashierId);
        public ProcessInvoice GetProcessInvoiceDetails( long invoiceId, long cashierId);
        bool Delete(long id,long CashierId);
        public List<TransactionMethod> TransactionMethodGetAll();
        public OpeningClosedAmount GetLastClosed(long cashierId);
        public List<AlottedServices> GetEntityAllotedService(int entityId, string startDate, string endDate);

    }
    public class EntityTransactionService : IEntityTransactionService
    {
        public RevenueContext _context; public IVPDContext _ivdpcontext;

        public EntityTransactionService(RevenueContext context,IVPDContext ivdpcontext)
        {
            _context = context;
            _ivdpcontext = ivdpcontext;

        }
        public OpeningClosedAmount GetLastClosed(long cashierId)
        {

            OpeningClosedAmount o = _context.OpeningClosedAmount.AsNoTracking().Where(w => w.cashier_id == cashierId).ToList().LastOrDefault();
            if (o != null)
            {
                return o;
            }
            else
            {
                return null;
            }


        }
        public List<TransactionMethod> TransactionMethodGetAll()
        {
            return _context.TransactionMethod.ToList();
        }
        public ProcessInvoice GetProcessInvoiceDetails(long invoiceId, long cashierId)
        {
            ProcessInvoice processInvoice = new ProcessInvoice();
            List<BillingAddress> billingAddressList = new List<BillingAddress>();

            IssueDocumentDetails issue = _context.IssueDocumentDetails.AsNoTracking().Where(w => w.id == invoiceId).ToList().FirstOrDefault();
            if (issue != null)
            {
                processInvoice.issueDocumentDetails = issue;

                AllTransactions u = _context.AllTransactions.AsNoTracking().Where(w => w.deleted_at == null).Where(w => w.invoice_id == issue.id).ToList().FirstOrDefault();

                if (u != null)
                {
                    processInvoice.allTransactions = u;

                    BusEntidade bus = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == u.entity_id.ToString()).ToList().FirstOrDefault();
                    if (bus != null)
                    {
                        processInvoice.nome = bus.nome;
                    }
                    List<BillingAddress> b = _context.BillingAddress.AsNoTracking().Where(w => w.entity_id == u.entity_id).ToList();
                    if (b != null)
                    {
                        foreach (BillingAddress item in b)
                        {
                            if (item != null)
                            {
                                BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == item.distrito_id).ToList().FirstOrDefault();
                                BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == item.distrito_id).Where(w => w.Codcon == item.country_id).ToList().FirstOrDefault();
                                BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == item.distrito_id).Where(w => w.CODCON == item.country_id).Where(w => w.CODFRG == item.fregusia_id).ToList().FirstOrDefault();
                                if (d != null)
                                {
                                    item.DESDIS = d.Desdis;
                                }
                                if (c != null)
                                {
                                    item.DESCON = c.Descon;
                                }
                                if (f != null)
                                {
                                    item.DESFRG = f.DESFRG;
                                }

                                billingAddressList.Add(item);
                            }

                        }
                        processInvoice.billingAddress = billingAddressList;

                    }

                }
                return processInvoice;

            }
            else
            {
                return null;

            }
        }
        public List<EntityAccountDetail> GetEntityAccountDetails(int entityId, string startDate, string endDate, long cashierId)
        {
            DateTime sDate = Convert.ToDateTime(startDate);
            DateTime eDate = Convert.ToDateTime(endDate);

            List<EntityAccountDetail> entityAccountDetailList = new List<EntityAccountDetail>();
            EntityAccountDetail entityAccountDetail = new EntityAccountDetail();

            List<AllTransactions> u = _context.AllTransactions.AsNoTracking().Where(w => w.deleted_at == null).Where(w => w.trans_date >= sDate).Where(w => w.trans_date <= eDate).Where(w => w.entity_id == entityId).ToList();
            if (u != null)
            {
                foreach (AllTransactions item in u)
                {
                    BusEntidade bus = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();

                    entityAccountDetail.nome = bus.nome;
                }
                entityAccountDetail.allTransactions = u;
                entityAccountDetailList.Add(entityAccountDetail);
                return entityAccountDetailList;

            }
            else
            {
                return null;
            }
        }
        public EntityDetails GetEntityDetails(int entityId)
        {
            List<EntityDetails> entityDetailsList = new List<EntityDetails>();
                 List<AllTransactions> all = new List<AllTransactions>();
            BusEntidade bus = new BusEntidade();
            List<BillingAddress> debitDetailsList = new List<BillingAddress>();

            EntityDetails entityDetails = new EntityDetails();

            // BoxOpening boxopen = new BoxOpening();
            AllTransactions u1 = _context.AllTransactions.AsNoTracking().Where(w => w.entity_id == entityId).ToList().LastOrDefault();
            if (u1 != null)
            {
                entityDetails.currentEntityBalance = u1.current_balance;
                entityDetails.captive_balance = u1.captive_balance;
                entityDetails.accumulated_previous = u1.accumulated_previous;
            }
          
              //  foreach (AllTransactions item in u)
               // {
                   //   all.Add(item);
                   // entityDetails.captive_balance ;
                  //  entityDetails.accumulated_previous;
                    List<BillingAddress> bill = _context.BillingAddress.AsNoTracking().Where(w => w.entity_id == entityId).ToList();
            foreach (BillingAddress item in bill)
            {
                if (item != null)
                {
                    BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == item.distrito_id).ToList().FirstOrDefault();
                    BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == item.distrito_id).Where(w => w.Codcon == item.country_id).ToList().FirstOrDefault();
                    BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == item.distrito_id).Where(w => w.CODCON == item.country_id).Where(w => w.CODFRG == item.fregusia_id).ToList().FirstOrDefault();
                    if (d != null)
                    {
                        item.DESDIS = d.Desdis;
                    }
                    if (c != null)
                    {
                        item.DESCON = c.Descon;
                    }
                    if (f != null)
                    {
                        item.DESFRG = f.DESFRG;
                    }
                    entityDetails.billingAddress = bill;
                }
            }
                    // List<BillingAddress> b = _context.BillingAddress.AsNoTracking().Where(w => w.entity_id == item.entity_id).ToList();
                    BusEntidade bu = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == entityId.ToString()).ToList().FirstOrDefault();
                    // List<IssueDocumentDetails> issue = _context.IssueDocumentDetails.AsNoTracking().Where(w => w.ref_id == item.id).ToList();

                    //  foreach (IssueDocumentDetails item2 in issue)
                    //  {
                    //       iss.Add(item2); entityTransaction.issueDocumentDetails = iss;
                    //   }
                    //   TransactionMethod tm = _context.TransactionMethod.AsNoTracking().Where(w => w.id == item.trans_method_id).ToList().FirstOrDefault();

                    //   List<TransactionMethod> tm2 = _context.TransactionMethod.AsNoTracking().ToList();

                    //entityTransaction.billingAddress = b;
                    entityDetails.busEntidade = bu;
                    // entityTransaction.transactionMethod = tm;

                    //  entityTransaction.transactionMethodlist = tm2;
                //}
          
            return entityDetails;
        }
        public List<EntityTransaction> GetEntityTransactionDetails(int entityId, long transactionId, long cashierId)
        {
            List<EntityTransaction> entityTransactionList = new List<EntityTransaction>();
            List<AllTransactions> all = new List<AllTransactions>();
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            List<DebitDetails> debitDetailsList = new List<DebitDetails>();
            List<BillingAddress> billingAddressList = new List<BillingAddress>();

            EntityTransaction entityTransaction = new EntityTransaction();

            // BoxOpening boxopen = new BoxOpening();
            AllTransactions u1 = _context.AllTransactions.AsNoTracking().Where(w => w.entity_id == entityId).Where(w => w.id == transactionId).ToList().LastOrDefault();
            if (u1 != null)
            {
                entityTransaction.currentEntityBalance = u1.current_balance;

            }
            List<AllTransactions> u = _context.AllTransactions.AsNoTracking().Where(w => w.deleted_at == null).Where(w => w.entity_id == entityId).Where(w => w.id == transactionId || w.parent_id == transactionId).ToList();
            if (u != null)
            {
                foreach (AllTransactions item in u)
                {
                    all.Add(item);
                    entityTransaction.allTransaction = all;
                    List<DebitDetails> debit = _context.DebitDetails.AsNoTracking().Where(w => w.debit_id == item.id).ToList();

                    entityTransaction.debitDetail = debit;

                    List<BillingAddress> b = _context.BillingAddress.AsNoTracking().Where(w => w.entity_id == item.entity_id).ToList();
                    BusEntidade bus = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();
                    foreach (BillingAddress itemv in b)
                    {
                        if (itemv != null)
                        {
                            BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == itemv.distrito_id).ToList().FirstOrDefault();
                            BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == itemv.distrito_id).Where(w => w.Codcon == itemv.country_id).ToList().FirstOrDefault();
                            BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == itemv.distrito_id).Where(w => w.CODCON == itemv.country_id).Where(w => w.CODFRG == itemv.fregusia_id).ToList().FirstOrDefault();
                            if (d != null)
                            {
                                itemv.DESDIS = d.Desdis;
                            }
                            if (c != null)
                            {
                                itemv.DESCON = c.Descon;
                            }
                            if (f != null)
                            {
                                itemv.DESFRG = f.DESFRG;
                            }

                        }
                        billingAddressList.Add(itemv);
                    }
                    entityTransaction.billingAddress = billingAddressList;

                    List<IssueDocumentDetails> issue = _context.IssueDocumentDetails.AsNoTracking().Where(w => w.id == item.invoice_id).ToList();

                    foreach (IssueDocumentDetails item2 in issue)
                    {
                        iss.Add(item2); entityTransaction.issueDocumentDetails = iss;
                    }
                    TransactionMethod tm = _context.TransactionMethod.AsNoTracking().Where(w => w.id == item.trans_method_id).ToList().FirstOrDefault();

                    List<TransactionMethod> tm2 = _context.TransactionMethod.AsNoTracking().ToList();

                    entityTransaction.busEntidade = bus;
                    entityTransaction.transactionMethod = tm;

                    entityTransaction.transactionMethodlist = tm2;
                }
                entityTransactionList.Add(entityTransaction);
            }
            return entityTransactionList;
        }
        public bool Delete(long id, long CashierId)
        {
            var alltrans = _context.AllTransactions.Where(x => x.id == id).First();
            if (alltrans != null)
            {
                alltrans.deleted_at = DateTime.UtcNow;
                _context.AllTransactions.Update(alltrans);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;

            }
        }

        public List<AlottedServices> GetEntityAllotedService(int entityId, string startDate, string endDate)
        {
            List<AlottedServices> b = _context.AlottedServices.AsNoTracking().Where(w => w.date >= Convert.ToDateTime(startDate)).Where(w => w.date <= Convert.ToDateTime(endDate)).Where(w => w.entity_id == entityId).ToList();

            return b;
        }
    }
}
