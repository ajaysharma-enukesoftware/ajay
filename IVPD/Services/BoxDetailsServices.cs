using IVPD.Helpers;
using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Services
{
    public interface IBoxDetailsServices
    {
        public List<BoxDetails> GetBoxDetails(string date, long cashierId);
        // public CashCheck GetCheckAmount(string date, int entityId);
        // public CashCheck GetCashAmount(string date, int entityId);
        public void Create(OpeningClosedAmount req, long cashierId);
        public void Update(APIInsertUpdateAmountRequest req, long cashierId);
        public OpeningClosedAmount GetLastClosed(long cashierId);

        public OpeningClosedAmount GetByDateCaixa(string caixa , string date,long cashierId);
        public OpeningClosedAmount GetAllByDateCaixa(string caixa, string date, long cashierId);
        public List<OpeningClosedAmount> GetById(long cashierId);

    }
    public class BoxDetailsServices: IBoxDetailsServices
    {
        private RevenueContext _context;
        public BoxDetailsServices(RevenueContext context)
        {
            _context = context;
        }
        public List<OpeningClosedAmount> GetById(long cashierId)
        {
            List<OpeningClosedAmount> p = new List<OpeningClosedAmount>();
            OpeningClosedAmount p1 = _context.OpeningClosedAmount.AsNoTracking().Where(w => w.caixa == "Porto").Where(w => w.cashier_id == cashierId).ToList().LastOrDefault();
            OpeningClosedAmount p2 = _context.OpeningClosedAmount.AsNoTracking().Where(w => w.caixa == "Régua").Where(w => w.cashier_id == cashierId).ToList().LastOrDefault();
            if(p1!=null)
            {
                p.Add(p1);
            }
            if (p2 != null)
            {
                p.Add(p2);
            }
            return p;
        }
        public OpeningClosedAmount GetAllByDateCaixa(string caixa, string date,long cashierId)
        {
          return  _context.OpeningClosedAmount.AsNoTracking().Where(w => w.cashier_id == cashierId).Where(w => w.date == Convert.ToDateTime(date)).Where(w => w.caixa == caixa).ToList().FirstOrDefault();
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

        public void Create(OpeningClosedAmount req, long cashierId)
        {

            req.created_at = DateTime.UtcNow;
            req.date = Convert.ToDateTime(req.date);
            req.cashier_id = cashierId;
            req.closed_amount = req.cheques + req.numerario;
            _context.OpeningClosedAmount.Add(req);
            _context.SaveChanges();

        }
        public void Update(APIInsertUpdateAmountRequest req, long cashierId)
        {
            OpeningClosedAmount u = _context.OpeningClosedAmount.AsNoTracking().Where(w => w.cashier_id == cashierId).Where(w => w.date == Convert.ToDateTime(req.date)).Where(w => w.caixa == req.caixa).ToList().FirstOrDefault();

            if (u != null)
            {
                u.updated_at = DateTime.UtcNow;
                u.closed_amount = req.cheques + req.numerario;
                u.cheques = req.cheques;
                u.numerario = req.numerario;
               u.opening_amount = req.opening_amount;
                u.closed = req.closed;
                u.date = Convert.ToDateTime(req.date);
                _context.OpeningClosedAmount.Update(u);
                _context.SaveChanges();
            }

        }
        public OpeningClosedAmount GetByDateCaixa(string caixa,string date,long cashierId)
        {
            OpeningClosedAmount u = _context.OpeningClosedAmount.AsNoTracking().Where(w => w.cashier_id == cashierId).Where(w => w.caixa == caixa).Where(w => w.date ==Convert.ToDateTime(date)).ToList().FirstOrDefault();
            if(u!=null)
            {
                return u;
            }
            else
            {
                return null;

            }

        }
        public List<BoxDetails> GetBoxDetails(string date, long cashierId)
        {
           
 string sQuery = "select  DisplayName,AllTransactions.id,entity_id,entityacc_Id,trans_no,Trans_type,trans_msg,trans_method_id,type,total_cr,total_dr,current_balance,trans_date,Base_Currency,trans_currency,Created_At,Comment,parent_id from AllTransactions,Users,TransactionMethod where TransactionMethod.id=AllTransactions.trans_method_id and AllTransactions.cashier_Id=Users.ID and trans_date>='" + date + "' and deleted_at is null";


            List<BoxDetails> result = _context.BoxDetails
                                  .FromSqlRaw(sQuery)
                                  .ToList();
            return result;
        }
    }
}
