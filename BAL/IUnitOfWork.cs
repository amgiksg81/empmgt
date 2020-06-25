using BAL.Abstraction;

namespace BAL
{
    public interface IUnitOfWork
    {
        IAuthenticateRepository AuthenticateRepo { get; }        
        
        IEmployeeRepository EmployeeRepo { get; }
        IEmployeeDocsRepository EmployeeDocsRepo { get; }

        IClientRepository ClientRepo { get; }

        ICategoryRepository CategoryRepo { get; }
        IProductRepository ProductRepo { get; }
        IOrderRepository OrderRepo { get; }

        IRoleRepository RoleRepo { get; }
        IUserRoleRepository UserRoleRepo { get; }

        int SaveChanges();
    }
}
