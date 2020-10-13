using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public partial class Permission
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
