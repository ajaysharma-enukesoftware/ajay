using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using System.Runtime.InteropServices;

namespace IVPD.Services
{

    public interface IGroupService
    {
        Group getByID(long Groupid);
        Group Create(Group group);
        List<ForGroupList> GetAll([Optional] Int64 id);
        List<Group> GetByPage(string pageno, string pagesize);

        bool Update(Group group);

        bool delete(long id);

        List<User> UserByGroupId(long Groupid);

        bool AssignUsertoGroup(long groupids, long Userid);

        bool DeleteUsertoGroup(long groupids, long Userid);
    }
    public class GroupService : IGroupService
    {
        private IVPDContext _context;

        public GroupService(IVPDContext context)
        {
            _context = context;
        }

        public Group Create(Group group)
        {
            // validation
            if (string.IsNullOrWhiteSpace(group.Name))
                throw new AppException("Name is required");
            if (string.IsNullOrWhiteSpace(group.Descriptions))
                throw new AppException("Description is required");
            if (string.IsNullOrWhiteSpace(group.Status.ToString()))
                throw new AppException("Status is required");

            if (_context.Groups.Any(x => x.Name == group.Name))
                throw new AppException("Group Name \"" + group.Name + "\" is already taken");


            group.CreatedAt = DateTime.UtcNow;
            _context.Groups.Add(group);
            _context.SaveChanges();

            return group;
        }


        public bool Update(Group group)
        {
            // validation
            if (string.IsNullOrWhiteSpace(group.Name))
                throw new AppException("Name is required");
            if (string.IsNullOrWhiteSpace(group.Descriptions))
                throw new AppException("Description is required");
            if (string.IsNullOrWhiteSpace(group.Status.ToString()))
                throw new AppException("Status is required");


            {
                Group groupById = _context.Groups.Where(x => x.Id == group.Id).FirstOrDefault();
                if (groupById == null)
                    throw new AppException("No group Exist with the given id");

                if (groupById.CreatedAt != null)
                {
                    group.CreatedAt = groupById.CreatedAt;
                }
                if (groupById.DeletedAt != null)
                {
                    group.DeletedAt = groupById.DeletedAt;
                }
                groupById.Name = group.Name;
                groupById.Descriptions = group.Descriptions;
                groupById.Status = group.Status;

            }


            // _context.Groups.Update(group);
            _context.SaveChanges();

            return true;
        }
        public List<ForGroupList> GetAll([Optional] Int64 Id)
        {
            List<ForGroupList> forGroupLists = new List<ForGroupList>();

            List<Group> groups = _context.Groups.ToList();
            foreach (Group group in groups)
            {
                ForGroupList forGroupList = new ForGroupList();
                forGroupList.ID = group.Id;
                forGroupList.GroupName = group.Name;
                forGroupList.Obzervations = group.Descriptions;
                List<UserGroup> groupPermissions = new List<UserGroup>();
                groupPermissions = _context.UserGroups.Where(x => x.GroupId == group.Id).ToList();
                int count = groupPermissions.Count();
                if (groupPermissions.Count() > 0)
                {
                    count = 0;
                    foreach (var item in groupPermissions)
                    {
                        User u = _context.Users.Where(w => w.Id == item.UserId).FirstOrDefault();
                        if (u != null)
                        {
                            if (u.DeletedAt == null)
                            {
                                count = count + 1;
                            }
                        }
                    }

                }
                forGroupList.NoofUsers = Convert.ToString(count);
                if (group.Status == 1)
                    forGroupList.Status = "Active";
                if (group.Status == 2)
                    forGroupList.Status = "InActive";
                forGroupLists.Add(forGroupList);

            }

            return forGroupLists;
        }

        public List<Group> GetByPage(string pageno, string pagesize)
        {
            int pagesizes = 0;
            int pagenos = 0;

            if (pagesize == "")
            {
                pagesizes = 10;
            }
            if (pageno == "")
            {
                pagenos = 1;
            }
            pagesizes = int.Parse(pagesize);
            pagenos = int.Parse(pageno);
            if (pagesizes == 0)
            {
                pagesizes = 10;
            }


            // set current page number, must be >= 1 (ideally this value will be passed to this logic/function from outside)

            var skip = pagesizes * (pagenos - 1);
            int TotalGroups = _context.Groups.Count();
            var canPage = skip < TotalGroups;
            List<Group> GroupList = _context.Groups.ToList();
            var Groups = GroupList
                         .Skip(skip)
                         .Take(pagesizes)
                         .ToArray();

            return Groups.ToList();

        }


        public bool delete(long id)
        {
            Group group = _context.Groups.Where(x => x.Id == id).First();
            if (group == null)
            {
                throw new AppException("No group Exist with the given id to remove");
            }
            _context.Groups.Remove(group);
            _context.SaveChanges();
            return true;
        }

        public List<User> UserByGroupId(long Groupid)
        {
            if (Groupid == 0)
                throw new AppException("GroupId is Required");
            List<UserGroup> userGroups = _context.UserGroups.Where(x => x.GroupId == Groupid).Where(w1 => w1.DeletedAt == null).ToList();
            if (userGroups == null)
                throw new AppException("No User exist attached with user id");
            List<User> users = new List<User>();
            foreach (UserGroup userGroup in userGroups)
            {
                if (userGroup.DeletedAt == null)
                {
                    User user = _context.Users.Where(x => x.Id == userGroup.UserId).FirstOrDefault();
                    if (user != null)
                    {
                        users.Add(user);
                    }
                }
            }
            return users;

        }

        public bool AssignUsertoGroup(long groupids, long Userid)
        {
            UserGroup userGroup = new UserGroup();
            var groupcheck = _context.Groups.Where(x => x.Id == groupids);
            if (groupcheck.Count() == 0)
                throw new AppException("No Group Exist with the given id");
            var usercheck = _context.Users.Where(x => x.Id == Userid);
            if (usercheck.Count() == 0)
                throw new AppException("No User Exist with the given id");
            userGroup.GroupId = groupids;
            userGroup.UserId = Userid;
            userGroup.CreatedAt = DateTime.UtcNow;
            _context.UserGroups.Add(userGroup);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteUsertoGroup(long groupids, long Userid)
        {
            UserGroup userGroup = _context.UserGroups.Where(x => x.GroupId == groupids && x.UserId == Userid).FirstOrDefault();
            if (userGroup == null)
                throw new AppException("No user Group exist with given group id and user id");
            _context.UserGroups.Remove(userGroup);
            _context.SaveChanges();
            return true;
        }

        public Group getByID(long Groupid)
        {
            if (Groupid == 0)
                throw new AppException("Group id Should be other that zero");
            Group group = new Group();
            var groupcheck = _context.Groups.AsNoTracking().Where(x => x.Id == Groupid).FirstOrDefault();
            if (groupcheck != null)
            {
                group = groupcheck;
                return group;
            }
            else
            {
                return group;
            }
        }
    }
}
