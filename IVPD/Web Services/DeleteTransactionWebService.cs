using IVPD.Helpers;
using IVPD.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IVPD.Web_Services
{
    public class DeleteTransactionWebservice
    {
        public string PK_AllTransactions_id;
        public string AllTransactions_Cashier_Id;
    }
    [ServiceContract]
    public interface IDeleteTransactionWebService
    {
        [OperationContract]
        APIResponse DeleteTransaction(Security security, DeleteTransactionWebservice deleteWeb);
        string getResponseFromURL(string url);

    }
    public class DeleteTransactionWebService: IDeleteTransactionWebService
    {
        private RevenueContext _context; private IVPDContext _ivdpcontext;

        public DeleteTransactionWebService(RevenueContext context, IVPDContext ivdpcontext)
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
        public APIResponse DeleteTransaction(Security security, DeleteTransactionWebservice deleteWeb)
        {
            int checkAuth = Authenticate(security.username, security.password);
            if (checkAuth == 1)
            {
                var alltrans = _context.AllTransactions.AsNoTracking().Where(x => x.cashier_Id == Convert.ToInt64(deleteWeb.AllTransactions_Cashier_Id)).Where(x => x.id == Convert.ToInt64(deleteWeb.PK_AllTransactions_id)).Where(x => x.deleted_at == null).ToList().FirstOrDefault();
                if (alltrans != null)
                {
                    alltrans.deleted_at = DateTime.UtcNow;
                    _context.AllTransactions.Update(alltrans);
                    _context.SaveChanges();
                    return new APIResponse(true, "", "Transaction Deleted");
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

        public string getResponseFromURL(string url)
        {
            var client = new WebClient();
            var content = client.DownloadString(url);
            return content;
        }

    }
}
