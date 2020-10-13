using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Services
{
    public interface IInvoiceCreationService
    {
      //    InvoiceCreation Create(InvoiceCreation obj, long cashierId);
        public OpeningClosedAmount GetLastClosed(long cashierId);

    }
    public class InvoiceCreationService : IInvoiceCreationService
    {
        private RevenueContext _context;
        public InvoiceCreationService(RevenueContext context)
        {
            _context = context;
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
     /*   public InvoiceCreation Create(InvoiceCreation obj, long cashierId)
        {

        }
         */
    }
}
