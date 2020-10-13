using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using System;

namespace IVPD.Services
{
    public interface IPermisssionService
    {
        public List<Permission> PermissionsByGroup(Int64 GroupId);

        public List<Permission> GetAll();
    }
    public class PermissionsService: IPermisssionService
    {
        private IVPDContext _context;

        public PermissionsService(IVPDContext context)
        {
            _context = context;
        }
        public List<Permission> PermissionsByGroup(Int64 GroupId)
        {
            List<Permission> permissions = new List<Permission>();
            var PermissionCount=_context.Permissions.Where(Y => _context.GroupPermissions.Where(x => x.Groupid == GroupId).Select(x => x.Permissionid).Contains(Y.Id)).ToList();
            if (PermissionCount.Count != 0)
            {
                permissions = PermissionCount;
            }
            return permissions;
        }


        public List<Permission> GetAll()
        {
            List<Permission> permissions=   _context.Permissions.ToList();
            return permissions;
        }
    }
}
