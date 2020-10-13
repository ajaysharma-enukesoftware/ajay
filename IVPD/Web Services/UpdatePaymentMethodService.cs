using IVPD.Helpers;
using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
public class Security
{
    public string username; 
    public string password;

}
namespace IVPD.Web_Services
{
    public class UpdatePaymentWebService
    {
        public string PK_AllTransactions_id;
        public string AllTransactions_Cashier_Id;
        public string AllTransactions_TransactionMethod_Id;
    }
    [ServiceContract]
    public interface IUpdatePaymentMethodService
    {

        [OperationContract]
        APIResponse UpdatePaymentMethod(Security security,UpdatePaymentWebService updateObj);

    }
    public class UpdatePaymentMethodService: IUpdatePaymentMethodService
    {
        private RevenueContext _context; private IVPDContext _ivdpcontext;

        public UpdatePaymentMethodService(RevenueContext context, IVPDContext ivdpcontext)
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
           
            /*
             * var key = "E546C8DF278CD5931069B522E695D4F2";
            string dirtyemail = Encryption.MakeItDirtyAgain(username);
            string decryptEmail = Encryption.DecryptWebService(dirtyemail, key);         
            string dirtypass = Encryption.MakeItDirtyAgain(password);
            string decryptpass = Encryption.DecryptWebService(dirtypass, key);
            */
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
        public APIResponse UpdatePaymentMethod(Security security,UpdatePaymentWebService updateObj)
        {

            try
            {
                int checkAuth = Authenticate(security.username, security.password);


                if (checkAuth == 1)
                {
                   // var user = _context.Users.AsNoTracking().Where(w => w.Email == security.username).ToList().FirstOrDefault();
                    
                        var alltrans = _context.AllTransactions.AsNoTracking().Where(x => x.cashier_Id == Convert.ToInt64(updateObj.AllTransactions_Cashier_Id)).Where(x => x.id == Convert.ToInt64(updateObj.PK_AllTransactions_id)).Where(x => x.deleted_at == null).ToList().FirstOrDefault();
                    if (alltrans != null)
                    {
                        alltrans.trans_method_id = Convert.ToInt32(updateObj.AllTransactions_TransactionMethod_Id);
                        _context.AllTransactions.Update(alltrans);
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
            catch (Exception e)
            {
                return new APIResponse(false, "", e.Message);

            }
        }
    }
}
