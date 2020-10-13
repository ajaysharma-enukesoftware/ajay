using IVPD.Helpers;
using IVPD.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Web_Services
{
   
    public class UpdateBillingWebService
    {
        public string PK_AllTransactions_id;
        public string FK_BillingAddress_id;
        public string AllTransactions_cashier_id;
    }
    [ServiceContract]
    public interface IUpdateBillingByEntityWebService
    {
        [OperationContract]
        APIResponse UpdateBillingByEntity(Security security,UpdateBillingWebService updateWebService);

    }
    public class UpdateBillingByEntityWebService: IUpdateBillingByEntityWebService
    {
        private RevenueContext _context; private IVPDContext _ivdpcontext;

        public UpdateBillingByEntityWebService(RevenueContext context, IVPDContext ivdpcontext)
        {
            _context = context; _ivdpcontext = ivdpcontext;

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
        public APIResponse UpdateBillingByEntity(Security security, UpdateBillingWebService updateWebService)
        {
            try
            {
                int checkAuth = Authenticate(security.username, security.password);
                if (checkAuth == 1)
                {
                    AllTransactions all = _context.AllTransactions.AsNoTracking().Where(x => x.cashier_Id == Convert.ToInt32(updateWebService.AllTransactions_cashier_id)).Where(x => x.deleted_at == null).Where(w => w.id == Convert.ToInt32(updateWebService.PK_AllTransactions_id)).ToList().FirstOrDefault();
                    if (all != null)
                    {
                        all.billing_id = Convert.ToInt64(updateWebService.FK_BillingAddress_id);
                        _context.AllTransactions.Update(all);
                        _context.SaveChanges();
                        return new APIResponse(true, "", "Data Updated Succesfully");
                    }
                    else
                    {
                        return new APIResponse(false, "", "No Data Exists!");

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
