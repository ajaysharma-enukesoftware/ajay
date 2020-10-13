using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;

namespace IVPD.Services
{
    public interface IGroupPermisssionService
    {
        bool Create(GroupPermissionRequest groupPermissionRequest);
        bool Delete(Int64 ID);

        public GroupPermission GetById(Int64 Id);

        List<GroupPermission> GetByGroupID(long GroupID);
    }

    public class GroupPermisssionService : IGroupPermisssionService
    {
        private IVPDContext _context;

        public GroupPermisssionService(IVPDContext context)
        {
            _context = context;
        }
        public bool Create(GroupPermissionRequest groupPermissionRequest)
        {
            var Groupcheck = _context.Groups.Where(x => x.Id == groupPermissionRequest.Groupid);
            if (Groupcheck.Count() == 0)
                throw new AppException("group not found with id "+groupPermissionRequest.Groupid.ToString());
            foreach (long groupids in groupPermissionRequest.AnotherGroup)
            {
                var Groupcheck1 = _context.Groups.Where(x => x.Id == groupids);
                if (Groupcheck1.Count() == 0)
                    throw new AppException("group not found with id " + groupids.ToString());
            }

            foreach (PermissionPermissions permissionPermissions in groupPermissionRequest.permissionPermissions)
            {
                var Permissioncheck = _context.Permissions.Where(x => x.Id== permissionPermissions.Permissionid);
                if (Permissioncheck.Count() == 0)
                    throw new AppException("Permission not found with id " + permissionPermissions.Permissionid.ToString());
            }

                foreach (PermissionPermissions permissionPermissions in groupPermissionRequest.permissionPermissions)
            {
                GroupPermission groupPermission = _context.GroupPermissions.Where(x => x.Groupid == groupPermissionRequest.Groupid && x.Permissionid == permissionPermissions.Permissionid).FirstOrDefault();
                if (groupPermission != null)
                {
                    _context.GroupPermissions.Remove(groupPermission);
                }
                foreach (Int64 anothergroupid in groupPermissionRequest.AnotherGroup)
                {
                    GroupPermission groupPermission1 = _context.GroupPermissions.Where(x => x.Groupid == anothergroupid && x.Permissionid == permissionPermissions.Permissionid).FirstOrDefault();
                    if (groupPermission1 != null)
                    {
                        _context.GroupPermissions.Remove(groupPermission1);
                    }
                }
            }

            foreach (PermissionPermissions permissionPermissions in groupPermissionRequest.permissionPermissions)
            {
                GroupPermission groupPermission = new GroupPermission();
                groupPermission.Permissionid = permissionPermissions.Permissionid;
                groupPermission.Groupid = groupPermissionRequest.Groupid;
                groupPermission.IsRead = permissionPermissions.IsRead;
                groupPermission.IsDelete = permissionPermissions.IsDelete;
                groupPermission.IsUpdate = permissionPermissions.IsUpdate;
                groupPermission.IsWrite = permissionPermissions.IsWrite;
                groupPermission.CreatedAt = DateTime.Now;
                _context.Add(groupPermission);
                foreach (Int64 anothergroupid in groupPermissionRequest.AnotherGroup)
                {
                    GroupPermission groupPermission1 = new GroupPermission();
                    groupPermission1.Permissionid = permissionPermissions.Permissionid;
                    groupPermission1.Groupid = anothergroupid;
                    groupPermission1.IsRead = permissionPermissions.IsRead;
                    groupPermission1.IsDelete = permissionPermissions.IsDelete;
                    groupPermission1.IsUpdate = permissionPermissions.IsUpdate;
                    groupPermission1.IsWrite = permissionPermissions.IsWrite;
                    groupPermission1.CreatedAt = DateTime.Now;
                    _context.Add(groupPermission1);
                }
            }

            _context.SaveChanges();
            return true;
        }


        public bool  Delete(Int64 ID)
        {
            var GPTDCheck = _context.GroupPermissions.Where(x => x.Id == ID);
            if (GPTDCheck.Count() == 0)
            {
                throw new AppException("GroupPermission not found with id " + ID.ToString());
            }

            var GPTD = _context.GroupPermissions.Where(x => x.Id == ID).First();

            _context.GroupPermissions.Remove(GPTD);

            _context.SaveChanges();
            return true;
        }

        public List<GroupPermission> GetByGroupID(long GroupID)
        {
             List<GroupPermission> groupPermissions= _context.GroupPermissions.Where(x => x.Groupid == GroupID).ToList() ;
            return groupPermissions;
        }
        public GroupPermission GetById(Int64 Id)
        {
            return _context.GroupPermissions.Where(x => x.Id == Id).First();
        }


    }

}
