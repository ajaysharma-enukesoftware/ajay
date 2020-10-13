using IVPD.Helpers;
using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Web_Services
{
   
        public class BillingAddressWebservice
    {
        public string PK_BillingAddress_id;
        public string BillingAddress_address_line1;
        public string BillingAddress_address_line2;
        public string BillingAddress_city;
        public string BillingAddress_state;
        public string BillingAddress_country;
        public string BillingAddress_pin;
        public string BillingAddress_entity_id;
    }
    [ServiceContract]
    public interface IBillingEntityWebService
    {
        [OperationContract]
        APIResponse InsertOrUpdateBilling(Security security, BillingAddressWebservice billWeb);

    }
   
    public class BillingEntityWebService: IBillingEntityWebService
    {
        private RevenueContext _context; private IVPDContext _ivdpcontext;

        public BillingEntityWebService(RevenueContext context)
        {
            _context = context;
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
        public APIResponse InsertOrUpdateBilling(Security security, BillingAddressWebservice billWeb)
        {
            try
            {
                int checkAuth = Authenticate(security.username, security.password);
                if (checkAuth == 1)
                {
                    BillingAddress bill = _context.BillingAddress.AsNoTracking().Where(w => w.id == Convert.ToInt64(billWeb.PK_BillingAddress_id)).ToList().FirstOrDefault();
                BillingAddress bi = _context.BillingAddress.AsNoTracking().ToList().LastOrDefault();

                BillingAddress b = new BillingAddress();
                if (bill == null)
                {
                    b.id = bi.id + 1;
                    b.entity_id = Convert.ToInt32(billWeb.BillingAddress_entity_id);
                //    b.city = billWeb.BillingAddress_city;
                    b.address_line1 = billWeb.BillingAddress_address_line1;
                    b.address_line2 = billWeb.BillingAddress_address_line2;
                  //  b.state = billWeb.BillingAddress_state;
                    b.pin = Convert.ToInt32(billWeb.BillingAddress_pin);
                  //  b.country = billWeb.BillingAddress_country;
                    _context.BillingAddress.Add(b);
                    _context.SaveChanges();
                     //   var client = new WebClient();
                     //   var content = client.DownloadString("https://pp-gseb2b.gerall.pt/FinancialService");
                           return new APIResponse(true, "", "Data Inserted Succesfully");
                    //    return new APIResponse(true, content, "Data Inserted Succesfully");
                    }
                else
                {
                //    bill.city = billWeb.BillingAddress_city;
                    bill.address_line1 = billWeb.BillingAddress_address_line1;
                    bill.address_line2 = billWeb.BillingAddress_address_line2;
                  //  bill.state = billWeb.BillingAddress_state;
                    bill.pin = Convert.ToInt32(billWeb.BillingAddress_pin);
                 //   bill.country = billWeb.BillingAddress_country;
                    _context.BillingAddress.Update(bill);
                    _context.SaveChanges();
                    return new APIResponse(true, "", "Data Updated Succesfully");

                }
            }
                else
                {
                    return new APIResponse(false, "", "Username or Password is Incorrect");

                }
            }
            catch(Exception e)
            {
                return new APIResponse(false, "", e.Message);

            }

        }
    }
}
