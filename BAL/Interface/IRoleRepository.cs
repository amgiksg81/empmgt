using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Entities;
using DomainModels.Models;
namespace BAL.Abstraction
{
    public interface IRoleRepository : IRepository<Role>
    {
        
    }

    public interface IUserRoleRepository : IRepository<UserRole>
    {
        IEnumerable<UserRole> GetUserRoles(int UserId);
    }
}
