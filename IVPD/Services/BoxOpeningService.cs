using IVPD.Controllers;
using IVPD.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Services
{
    public interface IBoxOpeningService
    {
        public List<AllTransactions> GetPreviousBalanceByDate(string date, long cashierId);
         public Cashier GetDetails(int cashierId);
        // public CashCheck GetCashAmount(string date, int entityId);


    }
    public class BoxOpeningService : IBoxOpeningService
    {
        private RevenueContext _context;
        public BoxOpeningService(RevenueContext context)
        {
            _context = context;
        }
        public Cashier GetDetails(int cashierId)
        {
            Cashier c = _context.Cashier.AsNoTracking().Where(w => w.id == cashierId).ToList().FirstOrDefault();
            return c;

        }

        public List<AllTransactions> GetPreviousBalanceByDate(string date, long cashierId)
        {
            List<AllTransactions> allTransactions = new List<AllTransactions>();

            DateTime oDate = Convert.ToDateTime(date);
            // BoxOpening boxopen = new BoxOpening();
            Cashier c = _context.Cashier.AsNoTracking().Where(w => w.id == cashierId).ToList().FirstOrDefault();

            List<AllTransactions> u = _context.AllTransactions.AsNoTracking().Where(w => w.deleted_at == null).Where(w => w.trans_date == oDate).ToList();
            if (u != null)
            {

                foreach (AllTransactions item in u)
                {

                    AllTransactions bo = new AllTransactions();
                    BusEntidade td = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();


                    // bo.id = item.id;

                    bo = item;
                    List<TransactionDetails> b = _context.TransactionDetails.AsNoTracking().Where(w => w.all_trans_id == item.id).ToList();
                    if (b.Count()!=0)
                    {
                        List<TransactionDetails> transactionDetails = new List<TransactionDetails>();

                        foreach (TransactionDetails item2 in b)
                        {

                            transactionDetails.Add( item2);
                            bo.transactionDetail = transactionDetails;

                        }

                    }
                    bo.cashier_place = c.place;
                    bo.nome = td.nome;

                    allTransactions.Add(bo);

                }
                return allTransactions;
            }
            else
            {
                return null;
            }


            /*  sQuery = "select  AllTransactions.id,entity_id,entityacc_Id,trans_no,trans_type,trans_msg,trans_method,total_cr,total_dr,current_balance,trans_date,base_currency,trans_currency,Created_at,Comment,parent_id from AllTransactions where   cashier_Id=" + cashierId + " and trans_date='" + date + "'";

                               var result = _context.BoxOpening
                                                     .FromSqlRaw(sQuery)
                                                     .ToList();
                            return result; */


            //  DateTime oDate = Convert.ToDateTime(date);

            /*  var data = (from ep in _context.AllTransactions
                          join e in _context.TransactionDetails on ep.id equals e.all_trans_id
                          where ep.cashier_Id == cashierId && ep.trans_date == oDate
                          select new BoxOpening()
                          {

                              id = ep.id,
                              entity_id = ep.entity_id,
                              entityacc_Id =ep.entityacc_Id,
                              trans_no = ep.trans_no,
                              trans_type = ep.trans_type,
                              trans_msg = ep.trans_msg,
                              trans_method = ep.trans_method,
                              total_cr = ep.total_cr,
                              total_dr = ep.total_dr,
                              current_balance = ep.current_balance,
                              trans_date = ep.trans_date,
                              base_currency = ep.base_currency,
                              trans_currency = ep.trans_currency,
                              Created_at = ep.Created_at,
                              Comment = ep.Comment,
                              TeamPlayers = ep.TeamPlayers.Select(z => new CashCheck
                              {

                                  tax_id = e.tax_id,
                                  all_trans_id = e.all_trans_id,
                                  unit_amount = e.unit_amount,
                                  quantity = e.quantity
                              })


                          }).ToList();
              */

            // return data;
        }

            /* public CashCheck GetCheckAmount(string date,int entityId)
             {


                  string  sQuery = "select id,total_cr from AllTransactions where trans_date='" + date + "' and entity_id=" + entityId + " and trans_method='Cheques'";


                 CashCheck result = _context.CashCheck
                                         .FromSqlRaw(sQuery)
                                         .FirstOrDefault();
                 return result;
             }
             public CashCheck GetCashAmount(string date, int entityId)
             {


                 string sQuery = "select id,total_cr from AllTransactions where trans_date='" + date + "' and entity_id=" + entityId + " and trans_method='Numerário'";


                 CashCheck result = _context.CashCheck
                                         .FromSqlRaw(sQuery)
                                         .FirstOrDefault();
                 return result;
             }*/
        }
    }

