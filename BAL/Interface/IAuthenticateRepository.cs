using DomainModels.Entities;
using DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface IAuthenticateRepository : IRepository<User>
    {        
        UserModel ValidateUser(string Username, string Password);
    }
}
