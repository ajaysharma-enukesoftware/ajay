using IVPD.Helpers;
using IVPD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static IVPD.Models.RevenueModels;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace IVPD.Web_Services
{

    public class CreditWebservice
    {
        public string AllTransactions_entity_id;
        public string AllTransactions_entityacc_Id;
        public string AllTransactions_trans_no;
        public string AllTransactions_trans_type;
        public string AllTransactions_trans_msg;
        public string AllTransactions_total_cr;
        public string AllTransactions_total_dr;
        public string AllTransactions_base_currency;
        public string AllTransactions_trans_currency;
        public string AllTransactions_trans_Comment;
        public string AllTransactions_trans_method_id;
        public string AllTransactions_bank;
        public string AllTransactions_useful_cashier;
        public string AllTransactions_vat_rate;
        public string AllTransactions_vat_no;
        public string AllTransactions_Cashier_Id;
        public string FK_AllTransactions_billing_id;
        public string AllTransactions_trans_date;

        //  public Items Items;
        public TransactiondetailsList[] TransactiondetailsLists;

        // public Dictionary<string, string>  transactiondetailsList;

    }
    //  [XmlRoot(ElementName = "transactiondetailsList")]

  /*  public class Items
    {
        public TransactiondetailsList[] TransactiondetailsLists;

    }
  */

    public  class  TransactiondetailsList
    {
        public string TransactionsDetails_tax_id;

        public string TransactionsDetails_unit_amount;
        
        public string TransactionsDetails_quantity;
    }

        [ServiceContract]
        public interface ICreditWebService
        {
          [OperationContract]
            APIResponse Credit(Security security, CreditWebservice creditObject);
        }
    public class CreditWebService : ICreditWebService
        {
            private RevenueContext _context; private IVPDContext _ivdpcontext;

        public CreditWebService(RevenueContext context, IVPDContext ivdpcontext)
         {
                _context = context; _ivdpcontext = ivdpcontext;

        }

        [HttpPost]
        public APIResponse Credit(Security security, CreditWebservice creditObject)
            {
            try
                {
              /*  CreditWebservice creditObject = new CreditWebservice();
                creditObject = new XmlSerializer(typeof(object),
        new Type[] { CreditWebservice });*/
                //      var a= creditObject.transactiondetailsList["TransactionsDetails_tax_id"];
                /////////////////
                int checkAuth = Authenticate(security.username, security.password);
                    if (checkAuth == 1)
                    {
                  //  User u=GetId(security.username);
                  //  OpeningClosedAmount last = GetLastClosed(Convert.ToInt64(u.Id));
                  //  if (last != null)
                  //  {
                  //      if (last.closed == 0)
                  //      {

                            _context.Database.OpenConnection();
                            _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AllTransactions ON");

                            _context.SaveChanges();


                            AllTransactions alltrans = _context.AllTransactions.Where(x => x.cashier_Id == Convert.ToInt64(creditObject.AllTransactions_Cashier_Id)).ToList().LastOrDefault();

                            AllTransactions previousEntityBalance = _context.AllTransactions.Where(x => x.cashier_Id == Convert.ToInt64(creditObject.AllTransactions_Cashier_Id)).Where(x => x.entity_id == Convert.ToInt32(creditObject.AllTransactions_entity_id)).ToList().LastOrDefault();

                            TransactionDetails transDetails = new TransactionDetails();
                            DebitDetails debitDetails = new DebitDetails();

                            AllTransactions newTransModel = new AllTransactions();
                            if (alltrans != null)
                            {
                                newTransModel.id = alltrans.id + 1;

                            }
                            else
                            {
                                newTransModel.id = 1;

                            }
                            if (creditObject.AllTransactions_trans_type == "CR")
                            {
                                if (previousEntityBalance != null)
                                {
                                    newTransModel.current_balance = previousEntityBalance.current_balance + Convert.ToInt32(creditObject.AllTransactions_total_cr);

                                }
                                else
                                {
                                    newTransModel.current_balance = Convert.ToInt32(creditObject.AllTransactions_total_cr);
                                }
                                newTransModel.total_cr = Convert.ToInt32(creditObject.AllTransactions_total_cr);
                                newTransModel.trans_currency = creditObject.AllTransactions_trans_currency;
                                newTransModel.base_currency = creditObject.AllTransactions_base_currency;
                                newTransModel.Created_at = DateTime.UtcNow;
                                newTransModel.Comment = creditObject.AllTransactions_trans_Comment;

                                newTransModel.trans_method_id = Convert.ToInt32(creditObject.AllTransactions_trans_method_id);
                                newTransModel.trans_msg = creditObject.AllTransactions_trans_msg;
                                newTransModel.trans_no = Convert.ToInt64(creditObject.AllTransactions_trans_no);
                                newTransModel.trans_type = creditObject.AllTransactions_trans_type;
                                newTransModel.useful_cashier = creditObject.AllTransactions_useful_cashier;
                                newTransModel.vat_no = Convert.ToInt64(creditObject.AllTransactions_vat_no);
                                newTransModel.vat_rate = Convert.ToDecimal(creditObject.AllTransactions_vat_rate);
                                newTransModel.cashier_Id = Convert.ToInt64(creditObject.AllTransactions_Cashier_Id);
                                newTransModel.bank = creditObject.AllTransactions_bank;
                                newTransModel.entityacc_Id = Convert.ToInt64(creditObject.AllTransactions_entityacc_Id);
                                newTransModel.entity_id = Convert.ToInt32(creditObject.AllTransactions_entity_id);
                                newTransModel.billing_id = Convert.ToInt32(creditObject.FK_AllTransactions_billing_id);
                                newTransModel.trans_date = Convert.ToDateTime(creditObject.AllTransactions_trans_date);

                                _context.AllTransactions.Add(newTransModel);

                                _context.SaveChanges();
                                _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AllTransactions OFF");
                                if (creditObject.AllTransactions_trans_method_id == "2" && creditObject.AllTransactions_trans_type == "CR")
                                {
                                    //    var q = JsonConvert.SerializeObject(creditObject.TransactiondetailsLists);
                                    _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Transactiondetails ON");

                                    foreach (TransactiondetailsList item in creditObject.TransactiondetailsLists)
                                    {
                                        TransactionDetails td = _context.TransactionDetails.ToList().LastOrDefault();
                                        if (td != null)
                                        {
                                            transDetails.id = td.id + 1;
                                        }
                                        else
                                        {
                                            transDetails.id = 1;

                                        }
                                        transDetails.tax_id=Convert.ToInt64(item.TransactionsDetails_tax_id);
                                            transDetails.unit_amount = Convert.ToDecimal(item.TransactionsDetails_unit_amount);
                                            transDetails.quantity = Convert.ToInt32(item.TransactionsDetails_quantity);
                                            transDetails.all_trans_id = newTransModel.id;

                                        _context.TransactionDetails.Add(transDetails);
                                        _context.SaveChanges();

                                    }
                                    _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Transactiondetails OFF");

                                }


                                return new APIResponse(true, "", "Data Inserted Succesfully");
                            }
                            else
                            {
                                if (previousEntityBalance.current_balance >= Convert.ToDouble(creditObject.AllTransactions_total_dr))
                                {
                                    newTransModel.current_balance = previousEntityBalance.current_balance - Convert.ToInt32(creditObject.AllTransactions_total_dr);


                                    newTransModel.total_dr = Convert.ToInt32(creditObject.AllTransactions_total_dr);

                                    newTransModel.trans_currency = creditObject.AllTransactions_trans_currency;
                                    newTransModel.base_currency = creditObject.AllTransactions_base_currency;
                              //      newTransModel.trans_date = DateTime.UtcNow;
                                    newTransModel.Created_at = DateTime.UtcNow;
                                    newTransModel.Comment = creditObject.AllTransactions_trans_Comment;

                                    newTransModel.trans_method_id = Convert.ToInt32(creditObject.AllTransactions_trans_method_id);
                                    newTransModel.trans_msg = creditObject.AllTransactions_trans_msg;
                                    newTransModel.trans_no = Convert.ToInt64(creditObject.AllTransactions_trans_no);
                                    newTransModel.trans_type = creditObject.AllTransactions_trans_type;
                                    newTransModel.useful_cashier = creditObject.AllTransactions_useful_cashier;
                                    newTransModel.vat_no = Convert.ToInt64(creditObject.AllTransactions_vat_no);
                                    newTransModel.vat_rate = Convert.ToDecimal(creditObject.AllTransactions_vat_rate);
                                    newTransModel.cashier_Id = Convert.ToInt64(creditObject.AllTransactions_Cashier_Id);
                                    newTransModel.bank = creditObject.AllTransactions_bank;
                                    newTransModel.entityacc_Id = Convert.ToInt64(creditObject.AllTransactions_entityacc_Id);
                                    newTransModel.entity_id = Convert.ToInt32(creditObject.AllTransactions_entity_id);
                                    newTransModel.billing_id = Convert.ToInt32(creditObject.FK_AllTransactions_billing_id);
                                    newTransModel.trans_date = Convert.ToDateTime(creditObject.AllTransactions_trans_date);

                                    _context.AllTransactions.Add(newTransModel);

                                    _context.SaveChanges();
                                    _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AllTransactions OFF");
                                    if (creditObject.AllTransactions_trans_method_id == "2" && creditObject.AllTransactions_trans_type == "DR")
                                    {
                                        _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DebitDetails ON");

                                        foreach (TransactiondetailsList item in creditObject.TransactiondetailsLists)
                                        {
                                            DebitDetails td = _context.DebitDetails.ToList().LastOrDefault();
                                            if (td != null)
                                            {
                                                debitDetails.id = td.id + 1;
                                            }
                                            else
                                            {
                                                debitDetails.id = 1;

                                            }

                                            debitDetails.unit_amount = Convert.ToDecimal(item.TransactionsDetails_unit_amount);
                                            debitDetails.quantity = Convert.ToInt32(item.TransactionsDetails_quantity);
                                            debitDetails.debit_id = newTransModel.id;
                                            _context.DebitDetails.Add(debitDetails);
                                            _context.DebitDetails.Add(debitDetails);
                                            _context.SaveChanges();
                                        }
                                        _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AllTransactions OFF");

                                    }



                                    return new APIResponse(true, "", "Data Inserted Succesfully");
                                }
                                else
                                {
                                    return new APIResponse(false, "", "Insufficient Balance For Debit");

                                }
                            }
                       }
                   //     else
                 //       {
                 //           return new APIResponse(false, "", "You should open cashier before proceeding this request");

                //        }
          //          }
           //         else
           //         {
            //            return new APIResponse(false, "", "You should open cashier before proceeding this request");

            //        }


         //       }
                    else
                    {
                        return new APIResponse(false, "", "Username or Password is Incorrect");

                    }
                }
                catch (Exception e)
                {
                    return new APIResponse(false, "", e.Message);

                }

            }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        public int Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return -1;

            var user = _ivdpcontext.Users.SingleOrDefault(x => x.Email == username);

            // check if username exists
            if (user == null)
                return -1;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return -1;


            // authentication successful
            return 1;
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
        public User GetId(string username)
        {
            User u = _ivdpcontext.Users.Where(w => w.Email == username).ToList().FirstOrDefault();
            return u;
        }
    }
    }

