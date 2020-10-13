using IVPD.Helpers;
using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Web_Services
{
   
    public class InvoiceService 
    {
        public string PK_IssueDocumentDetails_id;
        public string FK_AllTransactions_transaction_id;
        public string IssueDocumentDetails_document_type;
        public string IssueDocumentDetails_document_url;
        public string IssueDocumentDetails_document_size;
        public string IssueDocumentDetails_issue_date_time;
        public string IssueDocumentDetails_deadline_date;
        public string IssueDocumentDetails_status;
        public string IssueDocumentDetails_cancel_invoice;
        public string IssueDocumentDetails_flag;
        public string IssueDocumentDetails_extra_text;
    }


    [ServiceContract]
    public interface IInvoiceWebService
    {
        [OperationContract]
        APIResponse InsertOrUpdateInvoice(Security security, InvoiceService invoiceWebService);

    }
    public class InvoiceWebService: IInvoiceWebService
    {
        private RevenueContext _context; private IVPDContext _ivdpcontext;

        public InvoiceWebService(RevenueContext context, IVPDContext ivdpcontext)
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
        public APIResponse InsertOrUpdateInvoice(Security security, InvoiceService invoiceWebService)
        {
            int checkAuth = Authenticate(security.username, security.password);
            if (checkAuth == 1)
            {
                IssueDocumentDetails bill = _context.IssueDocumentDetails.Where(w => w.id == Convert.ToInt64(invoiceWebService.PK_IssueDocumentDetails_id)).ToList().FirstOrDefault();
                IssueDocumentDetails bi = _context.IssueDocumentDetails.ToList().LastOrDefault();

                IssueDocumentDetails b = new IssueDocumentDetails();
                if (bill == null)
                {
                    _context.Database.OpenConnection();
                    _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.IssueDocumentDetails ON");

                    b.id = bi.id + 1;
                    b.flag = invoiceWebService.IssueDocumentDetails_flag;
                    b.document_type = invoiceWebService.IssueDocumentDetails_document_type;
                    b.cancel_invoice = Convert.ToInt32(invoiceWebService.IssueDocumentDetails_cancel_invoice);
                    b.deadline_date = DateTime.Parse(invoiceWebService.IssueDocumentDetails_deadline_date);
                    b.document_size = Convert.ToInt32(invoiceWebService.IssueDocumentDetails_document_size);
                    b.document_type = invoiceWebService.IssueDocumentDetails_document_type;
                    b.document_url = invoiceWebService.IssueDocumentDetails_document_url;
                    b.status = Convert.ToInt32(invoiceWebService.IssueDocumentDetails_status);
                    b.issue_date_time = DateTime.Parse(invoiceWebService.IssueDocumentDetails_issue_date_time);
                    b.ref_table = "Alltransaction";
                    b.ref_id = Convert.ToInt64(invoiceWebService.FK_AllTransactions_transaction_id);
                    b.extra_text = invoiceWebService.IssueDocumentDetails_extra_text;

                    _context.IssueDocumentDetails.Add(b);
                    _context.SaveChanges();
                    _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.IssueDocumentDetails OFF");
                    return new APIResponse(true, "", "Data Inserted Succesfully");

                }
                else
                {
                    bill.flag = invoiceWebService.IssueDocumentDetails_document_type;
                    bill.document_type = invoiceWebService.IssueDocumentDetails_document_type;
                    bill.cancel_invoice = Convert.ToInt32(invoiceWebService.IssueDocumentDetails_cancel_invoice);
                    bill.deadline_date = DateTime.Parse(invoiceWebService.IssueDocumentDetails_deadline_date);
                    bill.document_size = Convert.ToInt32(invoiceWebService.IssueDocumentDetails_document_size);
                    bill.document_type = invoiceWebService.IssueDocumentDetails_document_type;
                    bill.document_url = invoiceWebService.IssueDocumentDetails_document_url;
                    bill.status = Convert.ToInt32(invoiceWebService.IssueDocumentDetails_status);
                    bill.issue_date_time = DateTime.Parse(invoiceWebService.IssueDocumentDetails_issue_date_time);
                    bill.ref_table = "Alltransaction";
                    bill.ref_id = Convert.ToInt64(invoiceWebService.FK_AllTransactions_transaction_id);
                    bill.extra_text = invoiceWebService.IssueDocumentDetails_extra_text;

                    _context.IssueDocumentDetails.Update(bill);
                    _context.SaveChanges();
                    return new APIResponse(true, "", "Data Updated Succesfully");

                }
            }
            else
            {
                return new APIResponse(false, "", "Username or Password is Incorrect");

            }

        }
    }
}
