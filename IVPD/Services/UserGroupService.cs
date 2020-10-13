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
    public interface IUserGroupService
    {
        bool Create(Int64 Userid, Int64[] Groupids);
        bool Update(Int64 Userid, Int64[] Groupids);
        public bool DeleteUserGroups(Int64 UserID);
    }

    public class UserGroupService : IUserGroupService
    {
        private IVPDContext _context;

        public UserGroupService(IVPDContext context)
        {
            _context = context;
        }

        public bool Create(Int64 Userid, Int64[] Groupids)
        {
            DeleteUserGroups(Userid);
            foreach (Int64 Groupid in Groupids)
            {
                UserGroup objuserGroup = new UserGroup();
                objuserGroup.UserId = Userid;
                objuserGroup.GroupId = Groupid;
                objuserGroup.CreatedAt = DateTime.Now;
                objuserGroup.UpdatedAt = null;
                objuserGroup.DeletedAt = null;
                _context.UserGroups.Add(objuserGroup);
            }
            _context.SaveChanges();
            return true;
        }

        public bool Update(Int64 Userid, Int64[] Groupids)
        {
            long[] existIDs = _context.UserGroups.Where(w => w.UserId == Userid).Select(s => s.GroupId).ToArray();
            List<long> checkIDs = new List<long>();
            foreach (Int64 Groupid in Groupids)
            {
                UserGroup objuserGroup = new UserGroup();
                objuserGroup = _context.UserGroups.Where(w => w.UserId == Userid).Where(w1 => w1.GroupId == Groupid).FirstOrDefault();
                if (objuserGroup == null)
                {
                    objuserGroup = new UserGroup();
                    objuserGroup.UserId = Userid;
                    objuserGroup.GroupId = Groupid;
                    objuserGroup.CreatedAt = DateTime.Now;
                    objuserGroup.UpdatedAt = null;
                    objuserGroup.DeletedAt = null;
                    _context.UserGroups.Add(objuserGroup);
                }
                else
                {
                    objuserGroup.UserId = Userid;
                    objuserGroup.GroupId = Groupid;
                    objuserGroup.UpdatedAt = DateTime.UtcNow;
                    objuserGroup.DeletedAt = null;
                    _context.UserGroups.Update(objuserGroup);
                }
                checkIDs.Add(Groupid);
            }
            foreach (long item in existIDs)
            {
                long id = checkIDs.Where(w => w == item).Select(s => s).FirstOrDefault();
                if (id == 0)
                {
                    UserGroup objuserGroup = new UserGroup();
                    objuserGroup = _context.UserGroups.Where(w => w.UserId == Userid).Where(w1 => w1.GroupId == item).FirstOrDefault();
                    objuserGroup.DeletedAt = DateTime.UtcNow;
                    _context.UserGroups.Update(objuserGroup);
                }
            }
            _context.SaveChanges();
            return true;
        }

        public bool DeleteUserGroups(Int64 UserID)
        {
            var items = _context.UserGroups.Where(item => item.UserId == UserID);
            foreach (var item in items)
            {
                _context.UserGroups.Remove(item);
            }
            return true;
        }

    }
}
