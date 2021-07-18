﻿using Common.Hash;
using Common.Web;
using Data.DAL;
using Data.DTO;
using MSSQL_Lite.Access;
using MSSQL_Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class RoleBLL : BusinessLogicLayer
    {
        private bool disposed;

        public RoleBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public RoleBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            SetDefault();
            disposed = false;
        }

        private RoleInfo ToRoleInfo(Role role)
        {
            if (role == null)
                return null;

            RoleInfo roleInfo = new RoleInfo();
            roleInfo.ID = role.ID;
            roleInfo.name = role.name;

            if (includeTimestamp)
            {
                roleInfo.createAt = role.createAt;
                roleInfo.updateAt = role.updateAt;
            }

            return roleInfo;
        }

        private Role ToRole(RoleCreation roleCreation)
        {
            if (roleCreation == null)
                throw new Exception("@'roleCreation' must be not null");

            return new Role
            {
                ID = roleCreation.ID,
                name = roleCreation.name,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private Role ToRole(RoleUpdate roleUpdate)
        {
            if (roleUpdate == null)
                throw new Exception("@'roleUpdate' must be not null");

            return new Role
            {
                ID = roleUpdate.ID,
                name = roleUpdate.name,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<RoleInfo>> GetRolesAsync()
        {
            List<RoleInfo> roles = null;
            if (includeTimestamp)
                roles = (await db.Roles.ToListAsync()).Select(c => ToRoleInfo(c)).ToList();
            else
                roles = (await db.Roles.ToListAsync(c => new { c.ID, c.name }))
                    .Select(c => ToRoleInfo(c)).ToList();

            return roles;
        }

        public async Task<RoleInfo> GetRoleAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new Exception("");
            Role role = null;
            if (includeTimestamp)
                role = (await db.Roles.SingleOrDefaultAsync(c => c.ID == roleId));
            else
                role = (await db.Roles.SingleOrDefaultAsync(c => new { c.ID, c.name }, c => c.ID == roleId));

            return ToRoleInfo(role);
        }

        public RoleInfo GetRole(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new Exception("");

            Role role = null;
            if (includeTimestamp)
                role = (db.Roles.SingleOrDefault(c => c.ID == roleId));
            else
                role = (db.Roles.SingleOrDefault(c => new { c.ID, c.name }, c => c.ID == roleId));

            return ToRoleInfo(role);
        }

        public PagedList<RoleInfo> GetRoles(int pageIndex, int pageSize)
        {
            SqlPagedList<Role> pagedList = null;
            Expression<Func<Role, object>> orderBy = c => new { c.ID };
            if (includeTimestamp)
                pagedList = db.Roles.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Roles.ToPagedList(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<RoleInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToRoleInfo(c)).ToList()
            };
        }

        public async Task<PagedList<RoleInfo>> GetRolesAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Role> pagedList = null;
            Expression<Func<Role, object>> orderBy = c => new { c.ID };
            if (includeTimestamp)
                pagedList = await db.Roles.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Roles.ToPagedListAsync(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<RoleInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToRoleInfo(c)).ToList()
            };
        }

        public async Task<CreationState> CreateRoleAsync(RoleCreation roleCreation)
        {
            Role role = ToRole(roleCreation);
            if (role.name == null)
                throw new Exception("");

            int checkExists = (int)await db.Roles.CountAsync(r => r.name == role.name);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            Random random = new Random();
            HashFunction hash = new HashFunction();
            role.ID = hash.MD5_Hash(random.NextString(10));
            int affected = await db.Roles.InsertAsync(role);
            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<UpdateState> UpdateRoleAsync(RoleUpdate roleUpdate)
        {
            Role role = ToRole(roleUpdate);
            if (role.name == null)
                throw new Exception("");

            int affected = await db.Roles
                .UpdateAsync(role, r => new { r.name, r.updateAt }, r => r.ID == role.ID);

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<DeletionState> DeleteRoleAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new Exception("");

            long userNumberOfRoleId = await db.Users.CountAsync(r => r.roleId == roleId);
            if (userNumberOfRoleId > 0)
                return DeletionState.ConstraintExists;

            int affected = await db.Roles.DeleteAsync(r => r.ID == roleId);
            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Roles.CountAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
