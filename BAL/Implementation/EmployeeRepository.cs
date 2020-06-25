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
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        #region [DatabaseContext/Constructor]
        /// <summary>
        /// [DatabaseContext/Constructor]
        /// </summary>
        private DatabaseContext context
        {
            get
            {
                return db as DatabaseContext;
            }
        }

        public EmployeeRepository(DbContext db)
        {
            this.db = db;
        }
        #endregion

        #region [Enter User Table Details for New User]
        /// <summary>
        /// Enter User Table Details for New User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int SaveUser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user.UserId;
        }
        #endregion        

        #region [Get All Roles]
        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Role> GetRoles()
        {
            return (from e in context.Roles select e).ToList();
        }
        #endregion

        #region [Save User Role]
        /// <summary>
        /// Save User Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public void SaveRole(Role role)
        {
            //context.Database.ExecuteSqlCommand("insert into ",.Roles.Add(role);
            User user = new User();
            user.Roles.Add(role);
            context.SaveChanges();
        }
        #endregion
    }
}
