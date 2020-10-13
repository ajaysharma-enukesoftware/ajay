using IVPD.Helpers;
using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Services
{
    public interface ICollectionRevenueService
    {
        public List<AllTransactions> GetAll(long cashierId, string currDateString, string startDate, string endDate ,string entityId);
        public BillingAddress GetBilingAddress(int entityId);
        public BillingAddress GetEntityAddressWithCodEntidade(int entityId);
        public BusEntidade GetEntityDetails(int entityId);

        public bool  UpdateBilingAddress(UpdateBilling b, long cashierId);
        public bool UpdatePaymentMethod(UpdatePaymentMethod b, long cashierId);
        public OpeningClosedAmount GetLastClosed(long cashierId);
        public void CreateAddress(BillingAddress model);
        public void UpdateAddress(BillingUpdateAddressRequest model);

    }
    public class CollectionRevenueService : ICollectionRevenueService
    {
        private RevenueContext _context; private IVPDContext _ivdpcontext;

        public CollectionRevenueService(RevenueContext context,IVPDContext ivdpcontext)
        {
            _context = context;
            _ivdpcontext = ivdpcontext;

        }
        public BillingAddress GetEntityAddressWithCodEntidade(int entityId)
        {
            BusEntidade b = _context.BusEntidade.Where(x => x.nifap == entityId.ToString()).ToList().FirstOrDefault();
            BillingAddress bill = _context.BillingAddress.Where(x => x.suggested == 1).Where(x => x.entity_id == entityId).ToList().FirstOrDefault();
            List<BusEntidadeContacto> contact = _context.BusEntidadeContacto.Where(x => x.codEntidade == b.codEntidade).ToList();
            bill.busEntidadeContacto = contact;
            return bill; 
        }

        public void CreateAddress(BillingAddress model)
        {
            BillingAddress bill = new BillingAddress();
         //   bill.address_line1 = model.address_line1;
           // bill.address_line2 = model.address_line2;
            //bill.entity_id = model.entity_id;
            //bill.country_id = model.country_id;
            //bill.distrito_id = model.distrito_id;
            //bill.fregusia_id = model.fregusia_id;
            //// bill.city = model.city;
            //  bill.country = model.country;
            // bill.state = model.state;
          //  bill = model;
            _context.BillingAddress.Add(model);
            
              _context.SaveChanges();
        }
        public void UpdateAddress(BillingUpdateAddressRequest model)
        {
            BillingAddress bill = _context.BillingAddress.Where(x => x.id == model.id).ToList().FirstOrDefault();
            if (model.address_line1 != null)
            {
                bill.address_line1 = model.address_line1;
            }
            if (model.address_line2 != null)
            {
                bill.address_line2 = model.address_line2;
            }
            if (model.fregusia_id != null)
            {
                bill.fregusia_id = model.fregusia_id;
            }
            if (model.distrito_id != null)
            {
                bill.distrito_id = model.distrito_id;
            }
            if (model.entity_id != null)
            {
                bill.entity_id = model.entity_id.Value;
            }
            if (model.country_id != null)
            {
                bill.country_id = model.country_id;
            }
            if (model.pin != null)
            {
                bill.pin = model.pin;
            }
            /*BillingAddress objuserGroup = new BillingAddress();
                objuserGroup.address_line1 = model.address_line1;
                objuserGroup.address_line2 = m;
                objuserGroup.CreatedAt = DateTime.Now;
                objuserGroup.UpdatedAt = null;
                objuserGroup.DeletedAt = null;*/
            _context.BillingAddress.Update(bill);

            _context.SaveChanges();
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
        public List<AllTransactions> GetAll(long cashierId,string currDateString, string startDate, string endDate, string entityId )
        {
            List<AllTransactions> u;
            if (startDate == null && endDate == null && entityId == null)
            {
                u = _context.AllTransactions.AsNoTracking().Where(w => w.trans_date.Date == Convert.ToDateTime(currDateString)).Where(w => w.deleted_at == null).ToList();
          
            }
            else if (startDate != null && endDate != null && entityId == null)
            {
                u = _context.AllTransactions.AsNoTracking().Where(w => w.trans_date.Date <= Convert.ToDateTime(endDate)).Where(w => w.trans_date.Date >= Convert.ToDateTime(startDate)).Where(w => w.deleted_at == null).ToList();

            }
            else if (startDate == null && endDate == null && entityId != null)
            {
                u = _context.AllTransactions.AsNoTracking().Where(w => w.entity_id == Convert.ToInt32(entityId)).Where(w => w.trans_date.Date == Convert.ToDateTime(currDateString)).Where(w => w.deleted_at == null).ToList();

            }
            else if (startDate != null && endDate == null && entityId == null)        
            {
                u = _context.AllTransactions.AsNoTracking().Where(w => w.trans_date.Date >= Convert.ToDateTime(startDate)).Where(w => w.deleted_at == null).ToList();

            }
        //---  
            else if (startDate != null && endDate == null && entityId != null)
            {
                u = _context.AllTransactions.AsNoTracking().Where(w => w.trans_date.Date >= Convert.ToDateTime(startDate)).Where(w => w.deleted_at == null).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

            }
            else if (startDate != null && endDate == null && entityId == null)
            {
                u = _context.AllTransactions.AsNoTracking().Where(w => w.trans_date.Date >= Convert.ToDateTime(startDate)).Where(w => w.deleted_at == null).ToList();

            }
            else if (startDate == null && endDate != null && entityId == null)
            {
                u = _context.AllTransactions.AsNoTracking().Where(w => w.trans_date.Date <= Convert.ToDateTime(endDate)).Where(w => w.deleted_at == null).ToList();

            }
       //     else if (startDate == null && endDate != null && entityId != null)
       else
            {
                u = _context.AllTransactions.AsNoTracking().Where(w => w.trans_date.Date <= Convert.ToDateTime(endDate)).Where(w => w.deleted_at == null).Where(w => w.entity_id == Convert.ToInt32(entityId)).ToList();

            }
           

            List<AllTransactions> allTransactions = new List<AllTransactions>();

            //   List<BoxOpening> box = new List<BoxOpening>();
            // Cashier c = _context.Cashier.AsNoTracking().Where(w => w.id == cashierId).ToList().FirstOrDefault();
            // if (c != null) { bo.place = c.place; }

            if (u != null)
            {
                foreach (AllTransactions item in u)
                {
                    AllTransactions bo = new AllTransactions();
                    BusEntidade td = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == item.entity_id.ToString()).ToList().FirstOrDefault();

                    bo = item;
                    //box.Add(item);

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
                    bo.busEntidadeDetail = td;

                    allTransactions.Add(bo);

                }
                return allTransactions;
            }
            else
            {
                return null;
            }

            /*  string sQuery = "select AllTransactions.id,nifap,nome,trans_msg,current_balance,quantity,unit_amount,trans_method_id FROM BusEntidade,AllTransactions,TransactionDetails where BusEntidade.nifap = AllTransactions.entity_id and AllTransactions.id=TransactionDetails.all_trans_id and AllTransactions.cashier_id=" + cashierId + " and deleted_at is null  ";
              List<CollectionRevenue> result = _context.CollectionRevenue
                                   .FromSqlRaw(sQuery)
                                   .ToList();
              return result;
            */

        }
        public BillingAddress GetBilingAddress(int entity_id)
        {
            string sQuery = "SELECT * from BillingAddress where entity_id=" + entity_id;
            BillingAddress result = _context.BillingAddress
                                 .FromSqlRaw(sQuery)
                                 .FirstOrDefault();
            if (result == null) { return null; }
            else { return result; }


        }
        public BusEntidade GetEntityDetails(int entity_id)
        {
            string sQuery = "SELECT * from BusEntidade where nifap=" + entity_id;
            BusEntidade result = _context.BusEntidade
                                 .FromSqlRaw(sQuery)
                                 .FirstOrDefault();
            if (result == null) { return null; }
            else { return result; }


        }
        public bool UpdateBilingAddress(UpdateBilling b, long cashierId)
        {
             List<BillingAddress> billall = _context.BillingAddress.Where(x => x.entity_id == b.entityId).Where(x => x.id != b.billingId).ToList();
            foreach(BillingAddress item in billall)
            {
                item.suggested = 0;
                _context.BillingAddress.Update(item);
                _context.SaveChanges();
            }

            var bill = _context.BillingAddress.Where(x => x.entity_id == b.entityId).Where(x => x.id == b.billingId).FirstOrDefault();
            if (bill != null)
            {
                bill.suggested =1;
                _context.BillingAddress.Update(bill);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;

            }
        }
        public bool UpdatePaymentMethod(UpdatePaymentMethod b, long cashierId)
        {
            var alltrans = _context.AllTransactions.Where(x => x.id == b.transId).Where(w => w.deleted_at == null).ToList().FirstOrDefault();
            if (alltrans != null)
            {

                alltrans.trans_method_id = b.paymentMethodId;

                _context.AllTransactions.Update(alltrans);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;

            }

        }
    }
}

