using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public partial class Group
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }


    public partial class NewGroup
    {

        public Int64 Id { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public bool Active { get; set; }
    }
    public partial class ForGroupList
    {
        public Int64 ID { get; set; }

        public string GroupName { get; set; }

        public string NoofUsers { get; set; }

        public string Obzervations { get; set; }

        public string Status { get; set; }

    }

    public partial class AssignUsertogroup
    {

        public long GroupIds { get; set; }
        public long UserID { get; set; }

    }

    public partial class ForUserByGroupID
    {

        public string GroupDescription { get; set; }
        public List<User> Users { get; set; }

    }


}
