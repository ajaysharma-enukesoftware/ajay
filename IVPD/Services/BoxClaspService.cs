using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IBoxClaspService
    {
        public List<BoxClasp> GetAll(string date, long cashierId);


    }
    public class BoxClaspService: IBoxClaspService
    {
        private RevenueContext _context;
        public BoxClaspService(RevenueContext context)
        {
            _context = context;
        }
        public List<BoxClasp> GetAll(string date, long cashierId)
        {

            //string sQuery = "select date,opening_amount,total_transaction,total_cr FROM BillingAddress,OpeningClosedAmount,AllTransactions where OpeningClosedAmount.id=AllTransactions.entity_id ";

            string sQuery = "select DisplayName,AllTransactions.id,entity_id,entityacc_Id,trans_no,Trans_type,trans_msg,trans_method_id,type,total_cr,total_dr,current_balance,trans_date,Base_Currency,trans_currency,Created_At,Comment,cashier_Id,parent_id FROM Users,AllTransactions,TransactionMethod  where Users.ID=AllTransactions.cashier_Id and trans_date='" + date+ "' and AllTransactions.trans_method_id=TransactionMethod.id  and deleted_at is null ";
            List<BoxClasp> result=  _context.BoxClasp.FromSqlRaw(sQuery)
                                 .ToList();
            return result;

        }
    }
}
