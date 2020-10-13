using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Services
{
    public interface ICashValuesServices
    {
        public List<AllTransactions> GeCashValuesDetails(string date, long cashierId,int transMethodId);
     

    }
    public class CashValuesServices : ICashValuesServices
    {
        private RevenueContext _context;
        public CashValuesServices(RevenueContext context)
        {
            _context = context;
        }
        public List<AllTransactions> GeCashValuesDetails(string date, long cashierId,int transMethodId)
        {
            List<AllTransactions> allTransactions = new List<AllTransactions>();

           DateTime oDate = Convert.ToDateTime(date);
            //   List<BoxOpening> box = new List<BoxOpening>();
            // Cashier c = _context.Cashier.AsNoTracking().Where(w => w.id == cashierId).ToList().FirstOrDefault();
            // if (c != null) { bo.place = c.place; }

            List<AllTransactions> u = _context.AllTransactions.AsNoTracking().Where(w => w.deleted_at == null).Where(w => w.trans_date.Date == oDate.Date).Where(w => w.trans_method_id == transMethodId).ToList();
            if (u != null)
            {
                foreach (AllTransactions item in u)
                {
                    AllTransactions bo = new AllTransactions();
                    BusEntidade td = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();
                    // bo.id = item.id;
                    //box.Add(item);
                    bo = item;

                    List<TransactionDetails> b = _context.TransactionDetails.AsNoTracking().Where(w => w.all_trans_id == item.id).ToList();
                    if (b.Count() != 0)
                    {
                        List<TransactionDetails> transactionDetails = new List<TransactionDetails>();

                        foreach (TransactionDetails item2 in b)
                        {

                            transactionDetails.Add(item2);
                            bo.transactionDetail = transactionDetails;

                        }

                    }
                    bo.nome = td.nome;

                    allTransactions.Add(bo);

                }
                return allTransactions;
            }
            else
            {
                return null;
            }

        }
    }
}
