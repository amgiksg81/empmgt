using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Abstraction;
using DomainModels.Entities;
using DomainModels.Models;
using System.Data.Entity;

namespace BAL.Implementation
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private DatabaseContext context
        {
            get
            {
                return db as DatabaseContext;
            }
        }

        public RoleRepository(DbContext db)
        {
            this.db = db;
        }
    }

    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        private DatabaseContext context
        {
            get
            {
                return db as DatabaseContext;
            }
        }

        public UserRoleRepository(DbContext db)
        {
            this.db = db;
        }

        #region [Get Selected User Roles]
        /// <summary>
        /// Get Selected User Roles
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IEnumerable<UserRole> GetUserRoles(int userId)
        {
            return (from e in context.UserRoles where e.UserId == userId select e);
        }
        #endregion        
    }
}
