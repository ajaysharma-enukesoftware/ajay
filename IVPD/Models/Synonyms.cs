using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class Synonyms
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public partial class ForSynonyms
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
