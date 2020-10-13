using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class GroupPermission
    {
        public Int64 Id { get; set; }
        public Int64 Permissionid { get; set; }
        public Int64 Groupid { get; set; }
        public bool IsRead { get; set; }
        public bool IsWrite { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
    }


    public partial class PermissionPermissions
    {
        public Int64 Permissionid { get; set; }
        public bool IsRead { get; set; }
        public bool IsWrite { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
    }

    public partial class GroupPermissionRequest
    {
        public GroupPermissionRequest()
        {
            permissionPermissions = new List<PermissionPermissions>();
        }
        public List<PermissionPermissions> permissionPermissions { get; set; }
      
        public Int64 Groupid { get; set; }
        public Int64[] AnotherGroup { get; set; }
    }



}
