using BAL.Abstraction;
using System.Data.Entity;
using DAL;
using BAL.Implementation;

namespace BAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext db;
        public UnitOfWork()
        {
            db = new DatabaseContext();
        }
        private IAuthenticateRepository _AuthenticateRepo;
        public IAuthenticateRepository AuthenticateRepo
        {
            get
            {
                if (_AuthenticateRepo == null)
                    _AuthenticateRepo = new AuthenticateRepository(db);

                return _AuthenticateRepo;
            }
        }

        private IEmployeeRepository _EmployeeRepo;
        public IEmployeeRepository EmployeeRepo
        {
            get
            {
                if (_EmployeeRepo == null)
                    _EmployeeRepo = new EmployeeRepository(db);

                return _EmployeeRepo;
            }
        }

        private IEmployeeDocsRepository _EmployeeDocsRepo;
        public IEmployeeDocsRepository EmployeeDocsRepo
        {
            get
            {
                if (_EmployeeDocsRepo == null)
                    _EmployeeDocsRepo = new EmployeeDocsRepository(db);

                return _EmployeeDocsRepo;
            }
        }

        private IClientRepository _ClientRepo;
        public IClientRepository ClientRepo
        {
            get
            {
                if (_ClientRepo == null)
                    _ClientRepo = new ClientRepository(db);

                return _ClientRepo;
            }
        }

        private ICategoryRepository _CategoryRepo;
        public ICategoryRepository CategoryRepo
        {
            get
            {
                if (_CategoryRepo == null)
                    _CategoryRepo = new CategoryRepository(db);

                return _CategoryRepo;
            }
        }

        private IProductRepository _ProductRepo;
        public IProductRepository ProductRepo
        {
            get
            {
                if (_ProductRepo == null)
                    _ProductRepo = new ProductRepository(db);

                return _ProductRepo;
            }
        }
        private IOrderRepository _OrderRepo;
        public IOrderRepository OrderRepo
        {
            get
            {
                if (_OrderRepo == null)
                    _OrderRepo = new OrderRepository(db);

                return _OrderRepo;
            }
        }

        private IRoleRepository _RoleRepo;
        public IRoleRepository RoleRepo
        {
            get
            {
                if (_RoleRepo == null)
                    _RoleRepo = new RoleRepository(db);

                return _RoleRepo;
            }
        }

        private IUserRoleRepository _UserRoleRepo;
        public IUserRoleRepository UserRoleRepo
        {
            get
            {
                if (_UserRoleRepo == null)
                    _UserRoleRepo = new UserRoleRepository(db);

                return _UserRoleRepo;
            }
        }

        public int SaveChanges()
        {            
            return db.SaveChanges();
        }
    }
}
