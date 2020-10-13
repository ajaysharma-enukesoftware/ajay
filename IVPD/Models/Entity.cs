using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IVPD.Models
{
    public partial class Entity
    {
        public long Id { get; set; }
        public string EntityNo { get; set; }
        public string EntityName { get; set; }
        public string Morada { get; set; }
        public string Telemovel { get; set; }
        public string NIF { get; set; }
        public string PoastalCode { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string telephone { get; set; }
        public string RuleIds  { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
