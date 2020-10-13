using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Audit.Core;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace IVPD.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        string CheckStatus(string username, string password);

        IEnumerable<User> GetAll();
        IEnumerable<UserTypes> GetAllUserType();

        UserByID GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
        User ForgotPassword(string username);
    }
    public class UserService : IUserService
    {
        private IVPDContext _context;

        public UserService(IVPDContext context)
        {
            _context = context;
        }



        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Email == username);

            // check if username exists
            if (user == null)
                return user;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;


            // authentication successful
            return user;
        }
        public string CheckStatus(string username, string password)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Email == username);
                if (user != null)
                {
                    if (user.Status == 1)
                    {
                        return "true";
                    }
                    else
                    {
                        return "false";
                    }
                }
                else
                {
                    return "false";

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.Where(w => w.DeletedAt == null).ToList();
        }
        public IEnumerable<UserTypes> GetAllUserType()
        {
            return _context.UserTypes.ToList();
        }



        public UserByID GetById(int id)
        {
            User u = _context.Users.AsNoTracking().Where(w => w.Id == id).ToList().FirstOrDefault();
            UserByID userByID = new UserByID();
            if (u != null)
            {
                userByID = JsonConvert.DeserializeObject<UserByID>(JsonConvert.SerializeObject(u));
                try
                {
                    userByID.AssignGroup = _context.UserGroups.Where(w => w.UserId == id).Where(w1 => w1.DeletedAt == null).Select(s => s.GroupId).ToList().ToArray();
                }
                catch (Exception)
                {
                    userByID.AssignGroup = null;
                }

                if (userByID.AssignGroup.Count() > 0)
                {
                    List<long> gids = new List<long>();
                    foreach (long item in userByID.AssignGroup)
                    {
                        Models.Group sd = _context.Groups.Where(w => w.Id == item).Select(s => s).FirstOrDefault();
                        if (sd != null && sd.DeletedAt == null)
                        {
                            gids.Add(item);
                        }
                    }
                    userByID.AssignGroup = gids.ToArray();
                }
                return userByID;
            }
            else
            {
                return null;
            }
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new AppException("FullName is required");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new AppException("Email is required");

            if (string.IsNullOrWhiteSpace(user.DisplayName))
                throw new AppException("Display Name is required");

            if (string.IsNullOrWhiteSpace(user.ContactNumber))
                throw new AppException("Contact Number is required");

            if (string.IsNullOrWhiteSpace(user.DOB.ToString()))
                throw new AppException("Date of Birth is required");

            if (string.IsNullOrWhiteSpace(user.Notes.ToString()))
                throw new AppException("Notes is required");

            if (user.Status == 0)
                throw new AppException("Status is required");
            if (user.user_type_id == 0)
                throw new AppException("Type is required");

            if (!Regex.Match(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z").Success)
                throw new AppException("Email is not in correct format");



            if (string.IsNullOrWhiteSpace(user.Designationid) || user.Designationid.ToString() == 0.ToString())
                throw new AppException("Designationid is required");


            if (_context.Users.Any(x => x.FullName == user.FullName))
                throw new AppException("Username \"" + user.FullName + "\" is already taken");

            if (_context.Users.Any(x => x.Email == user.Email))
                throw new AppException("Email \"" + user.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            //user.Status = 1;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = null;
            user.DeletedAt = null;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            User user = _context.Users.Where(x => x.Id == userParam.Id).ToList().First();

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Status == 0)
                throw new AppException("Status is required");

            //if (!string.IsNullOrWhiteSpace(userParam.FullName))
            user.FullName = userParam.FullName;

            // update user properties if provided
            //if (!string.IsNullOrWhiteSpace(userParam.DisplayName))
            user.DisplayName = userParam.DisplayName;

            //if (!string.IsNullOrWhiteSpace(userParam.ContactNumber))
            user.ContactNumber = userParam.ContactNumber;

            //if (!string.IsNullOrWhiteSpace(userParam.ContactNumber))
            user.ContactNumber = userParam.ContactNumber;
            //if (!string.IsNullOrWhiteSpace(userParam.Designationid))
            user.Designationid = userParam.Designationid;
            //if (!string.IsNullOrWhiteSpace(userParam.DOB.ToString()))
            user.DOB = userParam.DOB;
            //if (!string.IsNullOrWhiteSpace(userParam.Notes))
            user.Notes = userParam.Notes;
            //if (!string.IsNullOrWhiteSpace(userParam.Status.ToString()))
            user.Status = userParam.Status;
            // update password if provided
            user.user_type_id = userParam.user_type_id;
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            _context.SaveChanges();

        }

        public void Delete(int id)
        {
            var user = _context.Users.Where(x => x.Id == id).First();
            if (user != null)
            {
                user.DeletedAt = DateTime.UtcNow;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
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

        public User ForgotPassword(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Email == username);


            // check if username exists
            if (user == null)
                return null;
            else
                return user;
        }
    }
}
