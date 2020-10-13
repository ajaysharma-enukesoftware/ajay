using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class Parish
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }


    public partial class ForParishList
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
