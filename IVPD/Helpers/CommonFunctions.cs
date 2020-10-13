using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IVPD.Models;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace IVPD.Helpers
{
    public class CommonFunctions
    {
        public static string Token = "";

        public static string Generate6DigitOtp()
        {
            string PasswordLength = "6";
            string sixDigitOtp = "";

            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            //allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            //allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string IDString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                sixDigitOtp = IDString;
            }
            return sixDigitOtp;
        }
        public static void SendEmail(string firstName, string email, string secretNumber)
        {
            //string username = user.UserName;
            //string password = user.Password;
            //// to send the random password in email 
            //string strNewPassword = secretNumber;
            //user.OTP = strNewPassword;
            //db.SaveChanges();
            //if (!string.IsNullOrEmpty(password))
            //{
            //    MailMessage mm = new MailMessage("sender@gmail.com", email);
            //    mm.Subject = "Password Recovery";
            //    mm.Body = string.Format("Hi {0},<br /><br />Your password is {1}{2}.<br /><br />Thank You.", username, password, strNewPassword);
            //    mm.IsBodyHtml = true;
            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.EnableSsl = true;
            //    NetworkCredential NetworkCred = new NetworkCredential();
            //    NetworkCred.UserName = "sender@gmail.com";
            //    NetworkCred.Password = "password";
            //    smtp.UseDefaultCredentials = true;
            //    smtp.Credentials = NetworkCred;
            //    smtp.Port = 587;
            //    smtp.Send(mm);
            //    message = "Password has been sent to your email address.";
            //}
        }

        public static void SendOTP(string firstName, string lastname, string email, string otp, string AdminEmail, string SMTPEmail, string SMTPPwd, int Port, string SMTPMail)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Admin",
            AdminEmail);
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(firstName, email);
            message.To.Add(to);

            message.Subject = "OTP for " + email;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h2>Dear " + firstName + " " + lastname + "</h2>,<br><br><b>" + otp + "</b> is your OTP to change your password. Please login in to IVPD to verify it.<br><br>Thanks<br>Admin";

            message.Body = bodyBuilder.ToMessageBody();

            MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(SMTPMail, Port, true);
            client.Authenticate(SMTPEmail, SMTPPwd);

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }


        public static string SendSchedule(string content, string mailsentTO, string firstname, string subject, string AdminEmail, string SMTPEmail, string SMTPPwd,
               int Port, string SMTPMail)
        {
            try
            {

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Admin",
                AdminEmail);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(firstname, mailsentTO);
                message.To.Add(to);

                message.Subject = "Schedule for " + subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = content;

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.Connect(SMTPMail, Port, true);
                client.Authenticate(SMTPEmail, SMTPPwd);

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
                return "MailSent";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        public static void SendSignUpEmail(string firstName, string lastname, string email, string AdminToEmail, string AdminEmail, string SMTPEmail, string SMTPPwd, int Port, string SMTPMail)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Admin",
            AdminEmail);
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress("Admin", AdminToEmail);
            message.To.Add(to);

            message.Subject = "New User Signup " + email;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h2>Dear Admin</h2>,<br><br>New User: '<b>" + firstName + " " + lastname + "</b>' has SignedUp with Email ID: '" + email + "'. Please Verify and Active.<br><br>Thanks<br>Admin";

            message.Body = bodyBuilder.ToMessageBody();

            MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(SMTPMail, Port, true);
            client.Authenticate(SMTPEmail, SMTPPwd);

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        public static void SendLink(string firstName, string lastname, string email, string link, string AdminEmail, string SMTPEmail, string SMTPPwd, int Port, string SMTPMail)
        {


            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Admin",
            AdminEmail);
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(firstName, email);
            message.To.Add(to);

            message.Subject = "Link for " + email;


            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<h2>Dear " + firstName + " " + lastname + "</h2>,<br>Just to make sure that you are the one who did this action.We provide you <br><b>" + link + "</b>  link to change your password. Please login in to IVPD to verify it.<br><br>Thanks<br>Admin";

            message.Body = bodyBuilder.ToMessageBody();

            MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(SMTPMail, Port, true);
            client.Authenticate(SMTPEmail, SMTPPwd);

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }

    }
}