using DomainModels.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DomainModels.Resources;
using DomainModels.Models;

namespace DAL
{
    public class EmployeeDBInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Common.EncryptedPassword = EmployeeValidations.SuperAdminPassword;
            User newUser = new User { Username = EmployeeValidations.SuperAdminEmailID, Password = Common.EncryptedPassword, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now };

            List<Role> roles = new List<Role>();
            roles.Add(new Role { Name = UserInRoles.SUPERADMIN, Description = UserInRoles.SUPERADMIN });
            roles.Add(new Role { Name = UserInRoles.ADMIN, Description = UserInRoles.ADMIN });
            roles.Add(new Role { Name = UserInRoles.DEV, Description = UserInRoles.DEV });
            roles.Add(new Role { Name = UserInRoles.HR, Description = UserInRoles.HR });
            roles.Add(new Role { Name = UserInRoles.PM, Description = UserInRoles.PM });
            roles.Add(new Role { Name = UserInRoles.BDM, Description = UserInRoles.BDM });

            context.Roles.AddRange(roles);
            context.Users.Add(newUser);
            context.SaveChanges();

            var DefaultRole = (from e in context.Roles select e).FirstOrDefault();
            context.UserRoles.Add(new UserRole { RoleId = DefaultRole.RoleId,  UserId = newUser.UserId });
            context.Employees.Add(new Employee { EmpFullName = EmployeeValidations.SuperAdminFullName, EmpEmailID = newUser.Username, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, UserId = newUser.UserId });
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
