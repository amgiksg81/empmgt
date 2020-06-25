using BAL.Abstraction;
using DAL;
using DomainModels.Entities;
using DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Implementation
{
    public class AuthenticateRepository : Repository<User>, IAuthenticateRepository
    {
        private DatabaseContext context
        {
            get
            {
                return this.db as DatabaseContext;
            }
        }
        public AuthenticateRepository(DbContext _db)
        {
            this.db = _db;
        }
               
        public UserModel ValidateUser(string Username, string Password)
        {
            Common.EncryptedPassword = Password;
            User data = context.Users.Include("Employees").Include("UserRoles").Where(u => u.Username == Username && u.Password == Common.EncryptedPassword).FirstOrDefault();
            if (data != null)
            {
                UserModel user = new UserModel();
                user.UserId = data.UserId;
                Common.DecryptedPassword = data.Password;
                //user.Password = Common.DecryptedPassword;
                user.Name = data.Employees.Select(e => e.EmpFullName).FirstOrDefault();
                user.Roles = data.UserRoles.Select(r => r.Role.Name).ToArray();
                user.Username = data.Username;
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
