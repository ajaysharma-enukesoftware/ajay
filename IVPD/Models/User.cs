using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IVPD.Models
{
    public partial class User
    {

        public Int64 Id { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string ContactNumber { get; set; }
        public string Designationid { get; set; }
        public DateTime DOB { get; set; }
        public string Notes { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        public int user_type_id { get; set; }

    }
    public partial class UserTypes
    {
        public int id { get; set; }
        public string? type { get; set; }

    }

    public partial class NewUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public DateTime? LastLoginDate { get; set; }

    }

    public class UpdtePasswordModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        
        public string email { get; set; }
    }


    public class AuthenticateModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class SignupModel
    {

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Password { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public Int64 DesignationId { get; set; }

        [Required]
        public Int64[]  AssignGroup { get; set; }

        [Required]
        public string DOB { get; set; }

        [Required]
        public string Notes { get; set; }

        [Required]
        public int Status { get; set; }

    }

    public class RegisterUserModel
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        public string Password { get; set; }

        [Required]
        public string ContactNo { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Description { get; set; }
        public string Address { get; set; }

        public string AccountType { get; set; }

        public string AccountStatus { get; set; }

        [Required]
        public int Status { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        public string UserName { get; set; }



    }

    public class ForgotPassword1Model
    {
        [Required]
        public string UserName { get; set; }

        public string AdminToEmail { get; set; }

        public string AdminEmail { get; set; }

        public string SMTPEmail { get; set; }

        public string SMTPPWD { get; set; }

    }

    public class UserByID : User
    {
        public long[] AssignGroup { get; set; }
    }
}
