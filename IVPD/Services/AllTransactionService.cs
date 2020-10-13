using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Services
{
    public interface IAllTransactionService
    {
        IEnumerable<AllTransactions> GetByEntityId(string entity_id, long userId,string local);
        IEnumerable<AllTransactions> GetByStartDate(string start_date, long userId, string local);
        IEnumerable<AllTransactions> GetByEndDate(string end_date, long userId, string local);
        IEnumerable<AllTransactions> GetByEntityIdStartDate(string entity_id, string start_date, long userId, string local);
        IEnumerable<AllTransactions> GetByStartDateEndDate(string start_date, string end_date, long userId, string local);
        IEnumerable<AllTransactions> GetByEntityIdEndDate(string entity_id, string end_date, long userId, string local);
        IEnumerable<AllTransactions> GetAll(long userId, string local);
        IEnumerable<AllTransactions> GetByEntityIdStartDateEndDate(string entity_id, string start_date, string end_date, long userId, string local);
        int Update(long invoiceId,UpdateInvoice p,long cashierId);
        public OpeningClosedAmount GetLastClosed(long cashierId);

        IEnumerable<AllTransactions> AllTransactionInvoiceByEntityId(string entity_id, long userId);
        IEnumerable<AllTransactions> AllTransactionInvoice(long userId);

    }
    public class AllTransactionService: IAllTransactionService
    {
        private RevenueContext _context; private IVPDContext _ivdpcontext;

        public AllTransactionService(RevenueContext context, IVPDContext ivdpcontext)
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
        public IEnumerable<AllTransactions> GetByEntityId(string entity_id, long userId, string local)
        {
            List<AllTransaction> allList = new List<AllTransaction>();
            AllTransaction allObject = new AllTransaction();
            List<AllTransactions> allTransactionsList = new List<AllTransactions>();
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            List<TransactionDetails> td = new List<TransactionDetails>();
            List<BusEntidade> bd = new List<BusEntidade>();
            BillingAddress ba = new BillingAddress();
            string sQuery;
            if (local == null)
            {
                 sQuery = "select * FROM AllTransactions where  entity_id LIKE '" + entity_id + "' and deleted_at is null ";
            }
            else
            {
                 sQuery = "select * FROM AllTransactions where  entity_id LIKE '" + entity_id + "' and local='"+local+"' and deleted_at is null ";
            }
            List<AllTransactions> allTransactionsresult = _context.AllTransactions.FromSqlRaw(sQuery).ToList();
            if (allTransactionsresult != null)
            {

                foreach (AllTransactions item in allTransactionsresult)
                {

                    AllTransactions aso = new AllTransactions();
                    aso = item;
                    string sQuery3 = "select * FROM Users where ID=" + item.cashier_Id + "";
                    User u = _ivdpcontext.Users.FromSqlRaw(sQuery3).ToList().FirstOrDefault();
                    aso.username = u.FullName;


                    string sQuery2 = "select * FROM IssueDocumentDetails where id=" + item.invoice_id + "";
                    List<IssueDocumentDetails> issueresult = _context.IssueDocumentDetails.FromSqlRaw(sQuery2).ToList();
                    foreach (IssueDocumentDetails item2 in issueresult)
                    {
                        iss.Add(item2);
                        aso.issueDocumentDetail = iss;

                    }
                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3);
                        aso.transactionDetail = td;

                    }
                    string sQuery5 = "select * FROM BusEntidade where nifap=" + item.entity_id + "";
                    BusEntidade bus = _context.BusEntidade.FromSqlRaw(sQuery5).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;

                    string sQuery6 = "select * FROM BillingAddress where  id = " + item.billing_id + "";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }

                    aso.billingAddressDetail = bill;
                    allTransactionsList.Add(aso);

                }
            }
            allList.Add(allObject);
            return allTransactionsList;
        }
        public IEnumerable<AllTransactions> GetByStartDate(string start_date, long userId, string local)
        {
            /* string sQuery = "select * FROM AllTransactions where cashier_Id=" + userId + " and trans_date >= '" + start_date + "' and deleted_at is null ";
             List<AllTransactions> result = _context.AllTransactions.FromSqlRaw(sQuery)
                                  .ToList();
             return result;*/
            List<TransactionDetails> td = new List<TransactionDetails>();
            List<BusEntidade> bd = new List<BusEntidade>();
            BillingAddress ba = new BillingAddress();

            List<AllTransaction> allList = new List<AllTransaction>();
            AllTransaction allObject = new AllTransaction();
            List<AllTransactions> allTransactionsList = new List<AllTransactions>(); 
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            string sQuery;

            if (local == null)
            {
                 sQuery = "select * FROM AllTransactions where trans_date >= '" + start_date + "' and deleted_at is null ";
            }
            else
            {
                 sQuery = "select * FROM AllTransactions where local='"+local+"' and trans_date >= '" + start_date + "' and  deleted_at is null ";

            }
            List<AllTransactions> allTransactionsresult = _context.AllTransactions.FromSqlRaw(sQuery).ToList();
            if (allTransactionsresult != null)
            {

                foreach (AllTransactions item in allTransactionsresult)
                {

                    AllTransactions aso = new AllTransactions();
                    aso = item;
                     string sQuery3 = "select FullName FROM Users where ID=" + item.cashier_Id + "";
                     User u = _ivdpcontext.Users.FromSqlRaw(sQuery3).ToList().FirstOrDefault();
                    aso.username=  u.FullName;
                    string sQuery2 = "select * FROM IssueDocumentDetails where id=" + item.invoice_id + "";
                    List<IssueDocumentDetails> issueresult = _context.IssueDocumentDetails.FromSqlRaw(sQuery2).ToList();
                    foreach (IssueDocumentDetails item2 in issueresult)
                    {
                        iss.Add(item2);
                        aso.issueDocumentDetail = iss;

                    }
                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3);
                        aso.transactionDetail = td;

                    }
                    string sQuery5 = "select * FROM BusEntidade where nifap=" + item.entity_id + "";
                    BusEntidade bus = _context.BusEntidade.FromSqlRaw(sQuery5).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;

                    string sQuery6 = "select * FROM BillingAddress where  id = '" + item.billing_id + "'";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }
                    aso.billingAddressDetail = bill; allTransactionsList.Add(aso);


                }
            }
            allList.Add(allObject);
            return allTransactionsList;
        }
        public IEnumerable<AllTransactions> GetByEndDate(string end_date, long userId, string local)
        {
            /*  DateTime eDate = Convert.ToDateTime(end_date);
              eDate = eDate.AddDays(1);
              List<AllTransactions> result = _context.AllTransactions.AsNoTracking().Where(w => w.cashier_Id == userId).Where(w => w.trans_date < eDate.Date).Where(w => w.deleted_at == null).ToList();
              return result;
            */
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            DateTime eDate = Convert.ToDateTime(end_date);
            eDate = eDate.AddDays(1);
            List<AllTransaction> allList = new List<AllTransaction>();
            AllTransaction allObject = new AllTransaction();
            List<AllTransactions> allTransactionsList = new List<AllTransactions>();
            List<TransactionDetails> td = new List<TransactionDetails>();
            List<BusEntidade> bd = new List<BusEntidade>();
            BillingAddress ba = new BillingAddress();
            List<AllTransactions> allTransactionsresult;
            if (local == null)
            {
                 allTransactionsresult = _context.AllTransactions.AsNoTracking().Where(w => w.trans_date < eDate.Date).Where(w => w.deleted_at == null).ToList();
            }
            else
            {
                 allTransactionsresult = _context.AllTransactions.AsNoTracking().Where(w => w.local==local).Where(w => w.trans_date < eDate.Date).Where(w => w.deleted_at == null).ToList();

            }
            if (allTransactionsresult != null)
            {

                foreach (AllTransactions item in allTransactionsresult)
                {

                    AllTransactions aso = new AllTransactions();
                    aso = item;
                    string sQuery3 = "select * FROM Users where ID=" + item.cashier_Id + "";
                    User u = _ivdpcontext.Users.FromSqlRaw(sQuery3).ToList().FirstOrDefault();
                    aso.username = u.FullName;

                    string sQuery2 = "select * FROM IssueDocumentDetails where id=" + item.invoice_id + "";
                    List<IssueDocumentDetails> issueresult = _context.IssueDocumentDetails.FromSqlRaw(sQuery2).ToList();
                    foreach (IssueDocumentDetails item2 in issueresult)
                    {
                        iss.Add(item2);
                        aso.issueDocumentDetail = iss;

                    }
                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3);
                        aso.transactionDetail = td; 

                    }
                    string sQuery5 = "select * FROM BusEntidade where nifap=" + item.entity_id + "";
                    BusEntidade bus = _context.BusEntidade.FromSqlRaw(sQuery5).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;

                    string sQuery6 = "select * FROM BillingAddress where  id = '" + item.billing_id + "'";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }

                    aso.billingAddressDetail = bill; allTransactionsList.Add(aso);

                }
            }
            allList.Add(allObject);
            return allTransactionsList;
        }
        public IEnumerable<AllTransactions> GetByEntityIdStartDate(string entity_id, string start_date, long userId, string local)
        {
            /*  string sQuery = "select * FROM AllTransactions where cashier_Id=" + userId + " and entity_id LIKE'" + entity_id + "' and trans_date >= '" + start_date + "' and deleted_at is null ";
              List<AllTransactions> result = _context.AllTransactions.FromSqlRaw(sQuery)
                                   .ToList();
              return result;*/
            List<AllTransaction> allList = new List<AllTransaction>();
            AllTransaction allObject = new AllTransaction();
            List<AllTransactions> allTransactionsList = new List<AllTransactions>();
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            List<TransactionDetails> td = new List<TransactionDetails>();
            List<BusEntidade> bd = new List<BusEntidade>();
            BillingAddress ba = new BillingAddress();
            string sQuery;
            if (local==null)
            {
                 sQuery = "select * FROM AllTransactions where  entity_id LIKE'" + entity_id + "'  and trans_date >= '" + start_date + "' and deleted_at is null ";
            }
            else
            {
                 sQuery = "select * FROM AllTransactions where local='"+local+"' and entity_id LIKE'" + entity_id + "'  and trans_date >= '" + start_date + "' and deleted_at is null ";

            }
            List<AllTransactions> allTransactionsresult = _context.AllTransactions.FromSqlRaw(sQuery).ToList();
            if (allTransactionsresult != null)
            {

                foreach (AllTransactions item in allTransactionsresult)
                {

                    AllTransactions aso = new AllTransactions();
                    aso = item;
                    string sQuery3 = "select * FROM Users where ID=" + item.cashier_Id + "";
                    User u = _ivdpcontext.Users.FromSqlRaw(sQuery3).ToList().FirstOrDefault();
                    aso.username = u.FullName;


                    string sQuery2 = "select * FROM IssueDocumentDetails where id=" + item.invoice_id + "";
                    List<IssueDocumentDetails> issueresult = _context.IssueDocumentDetails.FromSqlRaw(sQuery2).ToList();
                    foreach (IssueDocumentDetails item2 in issueresult)
                    {
                        iss.Add(item2);
                        aso.issueDocumentDetail = iss;

                    }
                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3);
                        aso.transactionDetail = td; 

                    }
                    string sQuery5 = "select * FROM BusEntidade where nifap=" + item.entity_id + "";
                    BusEntidade bus = _context.BusEntidade.FromSqlRaw(sQuery5).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;

                    //   string sQuery6 = "select * FROM BillingAddress,CONCELHO.DESCON,DISTRITO.DESDIS,FREGUESIA.DESFRG  where DISTRITO.CODDIS=BillingAddress.distrito_id and CONCELHO.CODCON=BillingAddress.country_id and FREGUESIA.CODFRG=BillingAddress.fregusia_id  and suggested=1 and  entity_id = '" + entity_id + "'";

                    // string sQuery6 = "select * FROM BillingAddress where id=" + item.billing_id + " ";
                    string sQuery6 = "select * FROM BillingAddress where  id = '" + item.billing_id + "'";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }
                    aso.billingAddressDetail = bill;
                    allTransactionsList.Add(aso);

                }
            }
            allList.Add(allObject);
            return allTransactionsList;
        }
        public IEnumerable<AllTransactions> GetByStartDateEndDate(string start_date, string end_date, long userId, string local)
        {
            /*  DateTime eDate = Convert.ToDateTime(end_date);
              DateTime sDate = Convert.ToDateTime(start_date);

              eDate = eDate.AddDays(1);

             List<AllTransactions> result = _context.AllTransactions.AsNoTracking().Where(w => w.cashier_Id == userId).Where(w => w.trans_date >= sDate).Where(w => w.trans_date < eDate.Date).Where(w => w.deleted_at == null).ToList();
              return result;*/
            List<TransactionDetails> td = new List<TransactionDetails>();
            BillingAddress ba = new BillingAddress();

            DateTime eDate = Convert.ToDateTime(end_date);
            DateTime sDate = Convert.ToDateTime(start_date);
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            List<AllTransaction> allList = new List<AllTransaction>();
            AllTransaction allObject = new AllTransaction();
            List<AllTransactions> allTransactionsList = new List<AllTransactions>();
            List<BusEntidade> bd = new List<BusEntidade>();
            List<AllTransactions> allTransactionsresult;
            if (local == null)
            {
               allTransactionsresult = _context.AllTransactions.AsNoTracking().Where(w => w.trans_date >= sDate).Where(w => w.trans_date < eDate.Date).Where(w => w.deleted_at == null).ToList();
            }
            else
            {
               allTransactionsresult = _context.AllTransactions.AsNoTracking().Where(w => w.local==local).Where(w => w.trans_date >= sDate).Where(w => w.trans_date < eDate.Date).Where(w => w.deleted_at == null).ToList();
            }
            if (allTransactionsresult != null)
            {

                foreach (AllTransactions item in allTransactionsresult)
                {

                    AllTransactions aso = new AllTransactions();
                    aso = item;
                    string sQuery3 = "select * FROM Users where ID=" + item.cashier_Id + "";
                    User u = _ivdpcontext.Users.FromSqlRaw(sQuery3).ToList().FirstOrDefault();
                    aso.username = u.FullName;


                    string sQuery2 = "select * FROM IssueDocumentDetails where id=" + item.invoice_id + "";
                    List<IssueDocumentDetails> issueresult = _context.IssueDocumentDetails.FromSqlRaw(sQuery2).ToList();
                    foreach (IssueDocumentDetails item2 in issueresult)
                    {
                        iss.Add(item2);
                        aso.issueDocumentDetail = iss;

                    }
                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3);
                        aso.transactionDetail = td;

                    }
                    string sQuery5 = "select * FROM BusEntidade where nifap=" + item.entity_id + "";
                    BusEntidade bus = _context.BusEntidade.FromSqlRaw(sQuery5).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;
                    //  string sQuery6 = "select * FROM BillingAddress where id=" + item.billing_id + " ";
                    //string sQuery6 = "select * FROM BillingAddress,CONCELHO.DESCON,DISTRITO.DESDIS,FREGUESIA.DESFRG  where DISTRITO.CODDIS=BillingAddress.distrito_id and CONCELHO.CODCON=BillingAddress.country_id and FREGUESIA.CODFRG=BillingAddress.fregusia_id  and suggested=1 and  entity_id = '" + item.entity_id + "'";

                    string sQuery6 = "select * FROM BillingAddress where id = '" + item.billing_id + "'";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }
                    aso.billingAddressDetail = bill;
                    allTransactionsList.Add(aso);

                }
            }
            allList.Add(allObject);
            return allTransactionsList;
        }
        public IEnumerable<AllTransactions> GetByEntityIdEndDate(string entity_id, string end_date, long userId, string local)
        {
            /*
              DateTime eDate = Convert.ToDateTime(end_date);
              eDate = eDate.AddDays(1);
              List<AllTransactions> result = _context.AllTransactions.AsNoTracking().Where(w => w.cashier_Id == userId).Where(w => w.entity_id == Convert.ToInt32(entity_id)).Where(w => w.trans_date < eDate.Date).Where(w => w.deleted_at == null).ToList();
              return result;
            */
            List<BusEntidade> bd = new List<BusEntidade>();
            BillingAddress ba = new BillingAddress();

           // AllTransactions aso = new AllTransactions();
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            DateTime eDate = Convert.ToDateTime(end_date);
            eDate = eDate.AddDays(1);
            List<AllTransaction> allList = new List<AllTransaction>();
            AllTransaction allObject = new AllTransaction();
            List<AllTransactions> allTransactionsList = new List<AllTransactions>();
            List<TransactionDetails> td = new List<TransactionDetails>();
            List<AllTransactions> allTransactionsresult;
            if (local == null)
            {
             allTransactionsresult = _context.AllTransactions.AsNoTracking().Where(w => w.entity_id == Convert.ToInt32(entity_id)).Where(w => w.trans_date < eDate.Date).Where(w => w.deleted_at == null).ToList();
            }
            else
            {
             allTransactionsresult = _context.AllTransactions.AsNoTracking().Where(w => w.local==local).Where(w => w.entity_id == Convert.ToInt32(entity_id)).Where(w => w.trans_date < eDate.Date).Where(w => w.deleted_at == null).ToList();
            }
            if (allTransactionsresult != null)
            {

                foreach (AllTransactions item in allTransactionsresult)
                {

                    AllTransactions aso = new AllTransactions();
                    aso = item;
                    string sQuery3 = "select * FROM Users where ID=" + item.cashier_Id + "";
                    User u = _ivdpcontext.Users.FromSqlRaw(sQuery3).ToList().FirstOrDefault();
                    aso.username = u.FullName;


                    string sQuery2 = "select * FROM IssueDocumentDetails where id=" + item.invoice_id+ "";
                    List<IssueDocumentDetails> issueresult = _context.IssueDocumentDetails.FromSqlRaw(sQuery2).ToList();
                    foreach (IssueDocumentDetails item2 in issueresult)
                    {
                        iss.Add(item2);
                        aso.issueDocumentDetail = iss;

                    }
                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3);
                        aso.transactionDetail = td;  

                    }
                    string sQuery5 = "select * FROM BusEntidade where nifap=" + item.entity_id + "";
                    BusEntidade bus = _context.BusEntidade.FromSqlRaw(sQuery5).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;

                    //string sQuery6 = "select * FROM BillingAddress where id=" + item.billing_id + " ";
                    // string sQuery6 = "select * FROM BillingAddress,CONCELHO.DESCON,DISTRITO.DESDIS,FREGUESIA.DESFRG  where DISTRITO.CODDIS=BillingAddress.distrito_id and CONCELHO.CODCON=BillingAddress.country_id and FREGUESIA.CODFRG=BillingAddress.fregusia_id  and suggested=1 and  entity_id = '" + entity_id + "'";
                    string sQuery6 = "select * FROM BillingAddress where  id = '" + item.billing_id + "'";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }

                    aso.billingAddressDetail = bill;
                    allTransactionsList.Add(aso);

                }
            }
            allList.Add(allObject);
            return allTransactionsList;
        }
        public IEnumerable<AllTransactions> GetByEntityIdStartDateEndDate(string entity_id, string start_date, string end_date, long userId, string local)
        {
            /*  string sQuery = "select * FROM AllTransactions where cashier_Id=" + userId + " and  entity_id LIKE'" + entity_id + "'and trans_date >= '" + start_date + "' and  trans_date < '" + end_date + "' and deleted_at is null ";
              List<AllTransactions> result = _context.AllTransactions.FromSqlRaw(sQuery)
                                   .ToList();
              return result;*/
            List<AllTransaction> allList = new List<AllTransaction>();
            AllTransaction allObject = new AllTransaction();

            DateTime eDate = Convert.ToDateTime(end_date);
            eDate = eDate.AddDays(1);
            string asString = eDate.ToString("yyyy/MMMM/dd hh:mm:ss");
            List<AllTransactions> allTransactionsList = new List<AllTransactions>();
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            List<TransactionDetails> td = new List<TransactionDetails>();
            List<BusEntidade> bd = new List<BusEntidade>();
            BillingAddress ba = new BillingAddress(); string sQuery;
            if (local == null)
            {
                 sQuery = "select * FROM AllTransactions where  entity_id LIKE'" + entity_id + "'and trans_date >= '" + start_date + "' and  trans_date < '" + asString + "' and deleted_at is null ";
            }
            else
            {
                 sQuery = "select * FROM AllTransactions where local='"+local+"' and entity_id LIKE'" + entity_id + "'and trans_date >= '" + start_date + "' and  trans_date < '" + asString + "' and deleted_at is null ";
            }
            List<AllTransactions> allTransactionsresult = _context.AllTransactions.FromSqlRaw(sQuery).ToList();
            if (allTransactionsresult != null)
            {

                foreach (AllTransactions item in allTransactionsresult)
                {

                    AllTransactions aso = new AllTransactions();
                    aso = item;
                    string sQuery3 = "select * FROM Users where ID=" + item.cashier_Id + "";
                    User u = _ivdpcontext.Users.FromSqlRaw(sQuery3).ToList().FirstOrDefault();
                    aso.username = u.FullName;

                    string sQuery2 = "select * FROM IssueDocumentDetails where id=" + item.invoice_id + "";
                    List<IssueDocumentDetails> issueresult = _context.IssueDocumentDetails.FromSqlRaw(sQuery2).ToList();
                    foreach (IssueDocumentDetails item2 in issueresult)
                    {
                        iss.Add(item2);
                        aso.issueDocumentDetail = iss;

                    }
                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3);
                        aso.transactionDetail = td; 

                    }
                    string sQuery5 = "select * FROM BusEntidade where nifap=" + item.entity_id + "";
                    BusEntidade bus = _context.BusEntidade.FromSqlRaw(sQuery5).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;
                    //        string sQuery6 = "select * FROM BillingAddress where id=" + item.billing_id + " ";
                    //   string sQuery6 = "select * FROM BillingAddress,CONCELHO.DESCON,DISTRITO.DESDIS,FREGUESIA.DESFRG where DISTRITO.CODDIS=BillingAddress.distrito_id and CONCELHO.CODCON=BillingAddress.country_id and FREGUESIA.CODFRG=BillingAddress.fregusia_id  and suggested=1 and  entity_id = '" + entity_id + "'";

                    string sQuery6 = "select * FROM BillingAddress where  id = '" + item.billing_id + "'";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }

                    aso.billingAddressDetail = bill;
                    allTransactionsList.Add(aso);

                }

                //allObject.Add(aso);        
            }
      //      allList.Add(allObject);
            return allTransactionsList;
        }
        public IEnumerable<AllTransactions> GetAll(long userId, string local)
        {
            List<AllTransaction> allList = new List<AllTransaction>();
            AllTransaction allObject = new AllTransaction();
            List<AllTransactions> allTransactionsList = new List<AllTransactions>();
            List<IssueDocumentDetails> issueList = new List<IssueDocumentDetails>();
            List<BusEntidade> bd = new List<BusEntidade>();
            BillingAddress ba = new BillingAddress(); List<AllTransactions> allTransactionsresult;
            if (local == null)
            {
              allTransactionsresult = _context.AllTransactions.AsNoTracking().ToList();
            }
            else
            {
              allTransactionsresult = _context.AllTransactions.AsNoTracking().Where(w=>w.local==local).ToList();
            }
            if (allTransactionsresult != null)
            {

                foreach (AllTransactions item in allTransactionsresult)
                {
                    AllTransactions aso = new AllTransactions();
                    List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
                    List<TransactionDetails> td = new List<TransactionDetails>();

                    aso = item;

                    // allTransactionsList.Add(item); 
                    string sQuery3 = "select * FROM Users where ID=" + item.cashier_Id + "";
                    User u = _ivdpcontext.Users.FromSqlRaw(sQuery3).ToList().FirstOrDefault();
                    if (u != null)
                    {
                        aso.username = u.FullName;

                    }

                    string sQuery2 = "select * FROM IssueDocumentDetails where id=" + item.invoice_id + "";
                    List<IssueDocumentDetails> issueresult = _context.IssueDocumentDetails.FromSqlRaw(sQuery2).ToList();
                    foreach (IssueDocumentDetails item2 in issueresult)
                    {

                        iss.Add(item2);
                        aso.issueDocumentDetail = iss;


                    }
                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3); aso.transactionDetail = td; 

                    }
                    string sQuery5 = "select * FROM BusEntidade where nifap=" + item.entity_id + "";
                    BusEntidade bus = _context.BusEntidade.FromSqlRaw(sQuery5).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;

                    //        string sQuery6 = "select * FROM BillingAddress where id=" + item.billing_id + " ";
                    string sQuery6 = "select * FROM BillingAddress where  id = '" + item.billing_id + "'";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }

                    aso.billingAddressDetail = bill;
                    allTransactionsList.Add(aso);

                }

            }
            return allTransactionsList;
        }
        public IEnumerable<AllTransactions> AllTransactionInvoice(long userId)
        {
            List<AllTransactionInvoice> zList = new List<AllTransactionInvoice>();
            List<AllTransactions> a = new List<AllTransactions>();
            List<BusEntidade> b = new List<BusEntidade>();
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            List<AllTransactions> allTransactionsList = new List<AllTransactions>();
            List<BusEntidade> bd = new List<BusEntidade>();
            BillingAddress ba = new BillingAddress();

            List<AllTransactions> all = _context.AllTransactions.AsNoTracking().Where(w => w.deleted_at == null).ToList();
            if (all != null)
            {
                foreach (AllTransactions item in all)
                {
                    AllTransactions aso = new AllTransactions();
                    List<TransactionDetails> td = new List<TransactionDetails>();
                    List<IssueDocumentDetails> i = new List<IssueDocumentDetails>();

                    aso = item;

                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3); aso.transactionDetail = td;


                    }
                    BusEntidade bus = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;
                    //        string sQuery6 = "select * FROM BillingAddress where id=" + item.billing_id + " ";
                    string sQuery6 = "select * FROM BillingAddress id = '" + item.billing_id + "'";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }

                    aso.billingAddressDetail = bill;
                    List<IssueDocumentDetails> result = _context.IssueDocumentDetails.AsNoTracking().Where(w => w.id == item.invoice_id).ToList();

                    if (result != null)
                    {
                        foreach(IssueDocumentDetails item3 in result)
                        {
                            i.Add(item3);
                            aso.issueDocumentDetail = i;
                        }
                    }
                    allTransactionsList.Add(aso);

                }

            }

            return allTransactionsList;


        }
        public IEnumerable<AllTransactions> AllTransactionInvoiceByEntityId(string entityId,long userId)
        {
            List<AllTransactionInvoice> zList = new List<AllTransactionInvoice>();
            AllTransactionInvoice z = new AllTransactionInvoice();
            List<IssueDocumentDetails> i = new List<IssueDocumentDetails>();
            List<AllTransactions> a = new List<AllTransactions>();
            List<BusEntidade> b = new List<BusEntidade>();
            AllTransactions aso = new AllTransactions();
            List<IssueDocumentDetails> iss = new List<IssueDocumentDetails>();
            List<AllTransactions> allTransactionsList = new List<AllTransactions>();
            List<BusEntidade> bd = new List<BusEntidade>();
            BillingAddress ba = new BillingAddress();
            List<TransactionDetails> td = new List<TransactionDetails>();

            List<AllTransactions> all = _context.AllTransactions.AsNoTracking().Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.deleted_at == null).ToList();
            if (all != null)
            {
                foreach (AllTransactions item in all)
                {
                    aso = item;
                    BusEntidade bus = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();

                    aso.busEntidadeDetail = bus;

                    string sQuery4 = "select * FROM TransactionDetails where all_trans_id=" + item.id + "";
                    List<TransactionDetails> tr = _context.TransactionDetails.FromSqlRaw(sQuery4).ToList();
                    foreach (TransactionDetails item3 in tr)
                    {
                        td.Add(item3);

                    }
                    aso.transactionDetail = td;

                    //        string sQuery6 = "select * FROM BillingAddress where id=" + item.billing_id + " ";
                    string sQuery6 = "select * FROM BillingAddress where id = '" + item.billing_id + "'";

                    BillingAddress bill = _context.BillingAddress.FromSqlRaw(sQuery6).ToList().FirstOrDefault();
                    if (bill != null)
                    {
                        BusEntDistrito d = _ivdpcontext.Distrito.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).ToList().FirstOrDefault();
                        BusEntConcelho c = _ivdpcontext.CONCELHO.AsNoTracking().Where(w => w.Coddis == bill.distrito_id).Where(w => w.Codcon == bill.country_id).ToList().FirstOrDefault();
                        BusEntFreguesia f = _ivdpcontext.Freguesia.AsNoTracking().Where(w => w.CODDIS == bill.distrito_id).Where(w => w.CODCON == bill.country_id).Where(w => w.CODFRG == bill.fregusia_id).ToList().FirstOrDefault();
                        if (d != null)
                        {
                            bill.DESDIS = d.Desdis;
                        }
                        if (c != null)
                        {
                            bill.DESCON = c.Descon;
                        }
                        if (f != null)
                        {
                            bill.DESFRG = f.DESFRG;
                        }
                    }

                    aso.billingAddressDetail = bill;
                    List<IssueDocumentDetails> result = _context.IssueDocumentDetails.AsNoTracking().Where(w => w.id == item.invoice_id).ToList();

                    if (result != null)
                    {
                        foreach (IssueDocumentDetails item3 in result)
                        {
                            i.Add(item3);
                            aso.issueDocumentDetail = i;
                        }
                    }
                }
                allTransactionsList.Add(aso);

            }

            return allTransactionsList;

        }
        public int Update(long invoiceId , UpdateInvoice p,long cashierId )
        {
           
                IssueDocumentDetails i = _context.IssueDocumentDetails.AsNoTracking().Where(w => w.id == invoiceId).ToList().FirstOrDefault();
            //AllTransactions all = _context.AllTransactions.AsNoTracking().Where(w => w.invoice_id == i.id).ToList().FirstOrDefault();
            AllTransactions last = _context.AllTransactions.AsNoTracking().Where(w => w.entity_id == i.entity_id).ToList().LastOrDefault();
            if (i.is_paid == 1)
            {
                return 2;
            }
            else
            {
                if (last.current_balance > 0)
                {
                    if (i != null)
                    {
                        if (p.remove_tax != null)
                        {
                            i.remove_tax = p.remove_tax;
                            if (i.remove_tax == 1)
                            {
                                i.total_amount = i.total_amount - (i.service_tax + i.valor_cativo);
                            }
                            if (i.remove_tax == 0)
                            {
                                i.total_amount = i.total_amount + (i.service_tax + i.valor_cativo);
                            }

                        }
                        if (p.amount_paid != null)
                        {
                            i.amount_paid = p.amount_paid;
                        }
                        if (p.cancel_invoice != null)
                        {
                            i.cancel_invoice = p.cancel_invoice;
                        }
                        if (p.date != null)
                        {
                            i.deadline_date = Convert.ToDateTime(p.date);
                        }
                        if (p.document_size != null)
                        {
                            i.document_size = p.document_size;
                        }
                        if (p.document_type != null)
                        {
                            i.document_type = p.document_type;
                        }
                        if (p.document_url != null)
                        {
                            i.document_url = p.document_url;
                        }
                        if (p.issue_date_time != null)
                        {
                            i.issue_date_time = Convert.ToDateTime(p.issue_date_time);
                        }
                        if (p.iva != null)
                        {
                            i.iva = p.iva;
                        }
                        if (p.no != null)
                        {
                            i.no = p.no;
                        }
                        if (p.pag_limit != null)
                        {
                            i.pag_limit = p.pag_limit;
                        }
                        if (p.status != null)
                        {
                            i.status = p.status;
                        }
                        if (p.extra_text != null)
                        {
                            i.extra_text = p.extra_text;
                        }
                        if (p.flag != null)
                        {
                            i.flag = p.flag;
                        }
                        if (p.flag != null)
                        {
                            i.flag = p.flag;
                        }
                        //       i.document_url=GenerateInvoiceLink(i);
                      
                        _context.IssueDocumentDetails.Update(i);
                        _context.SaveChanges();
                        return 1;

                    }
                    else
                    {
                        return 0;

                    }
                }
                else
                {
                    return -1;

                }
            }
        }
        public string GenerateInvoiceLink(IssueDocumentDetails doc)
        {
            /*  var Renderer = new IronPdf.HtmlToPdf();
              BusEntidade bus = _context.BusEntidade.Where(w => w.nifap == doc.entity_id.ToString()).ToList().LastOrDefault();

              var html = System.IO.File.ReadAllText("StaticFiles/InvoiceTemplate.html");
              var html1 = html.Replace("{entity}", bus.nome);
              var html2 = html1.Replace("{Factura}", doc.document_type);
              var html3 = html2.Replace("{no}", doc.no);
              var html4 = html3.Replace("{expirydate}", doc.deadline_date.ToString());
              var html5 = html4.Replace("{quantity}", doc.total_item.ToString());
              var html6 = html5.Replace("{unitprice}", "unitprice");
              var html7 = html6.Replace("{iva}", doc.iva);
             // var html8 = html7.Replace("{valor}", obj.total_amount.ToString());
             // var html9 = html8.Replace("{sumvalue}", obj.total_amount.ToString());
              var html8 = html7.Replace("{coin}", "EUR");

              var pdf = Renderer.RenderHtmlAsPdf(html8);
              string pathUrl = Path.Combine(@"invoice/", "Invoice" + doc.id + "_" + doc.no + ".Pdf");
              pdf.SaveAs(pathUrl);

              return Path.GetFullPath(pathUrl);*/
            BusEntidade bus = _context.BusEntidade.Where(w => w.nifap == doc.entity_id.ToString()).ToList().LastOrDefault();

            var html = System.IO.File.ReadAllText("StaticFiles/InvoiceTemplate.html");
            var html1 = html.Replace("{entity}", bus.nome);
            var html2 = html1.Replace("{Factura}", doc.document_type);
            var html3 = html2.Replace("{no}", doc.no);
            var html4 = html3.Replace("{expirydate}", doc.deadline_date.ToString());
            var html5 = html4.Replace("{quantity}", doc.total_item.ToString());
            var html6 = html5.Replace("{unitprice}", "unitprice");
            var html7 = html6.Replace("{iva}", doc.iva);
            // var html8 = html7.Replace("{valor}", obj.total_amount.ToString());
            // var html9 = html8.Replace("{sumvalue}", obj.total_amount.ToString());
            var html8 = html7.Replace("{coin}", "EUR");

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
    }
}
