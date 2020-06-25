using DAL;
using BAL.Abstraction;
using DomainModels.Entities;
using System.Data.Entity;

namespace BAL.Implementation
{
    public class ClientRepository : Repository<Client>, IClientRepository
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

        public ClientRepository(DbContext db)
        {
            this.db = db;
        }
        #endregion
    }
}