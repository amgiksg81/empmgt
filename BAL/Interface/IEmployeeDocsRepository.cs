using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Entities;
using DomainModels.Models;

namespace BAL.Abstraction
{
    public interface IEmployeeDocsRepository : IRepository<EmployeeDocument>
    {
        void DeleteAllDocuments(int EmployeeId);

        #region [Not In Use]
        //int SaveEmpDocument(EmployeeDocument model);
        //IEnumerable<EmployeeDocument> GetEmployeeDocuments(int EmployeeId);
        //EmployeeDocument GetEmployeeDocument(int EmployeeDocId);
        //void DeleteDocument(EmployeeDocument empDocument);
        #endregion
    }
}
