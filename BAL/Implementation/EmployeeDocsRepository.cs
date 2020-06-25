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
    public class EmployeeDocsRepository : Repository<EmployeeDocument>, IEmployeeDocsRepository
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

        public EmployeeDocsRepository(DbContext db)
        {
            this.db = db;
        }
        #endregion

        #region [Delete All Documents By EmployeeId]
        /// <summary>
        /// Delete All Documents By EmployeeId
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public void DeleteAllDocuments(int EmployeeId)
        {
            context.EmployeeDocuments.RemoveRange(context.EmployeeDocuments.Where(x => x.EmployeeId == EmployeeId));
            db.SaveChanges();
        }
        #endregion

        #region [NOT IN USE]
        //#region [Enter User Table Details for New User]
        ///// <summary>
        ///// Enter User Table Details for New User
        ///// </summary>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //public int SaveUser(User user)
        //{
        //    context.Users.Add(user);
        //    context.SaveChanges();
        //    return user.UserId;
        //}
        //#endregion

        //#region [Upload Employee's Document ]
        ///// <summary>
        ///// Upload Employee's Document 
        ///// </summary>
        ///// <param name="empDocument"></param>
        //public int SaveEmpDocument(EmployeeDocument empDocument)
        //{
        //    context.EmployeeDocuments.Add(empDocument);
        //    context.SaveChanges();
        //    return empDocument.EmployeeDocId;
        //}
        //#endregion

        //#region [Get All Documents of Particular Employee by EmployeeDocId]
        ///// <summary>
        ///// Get All Documents of Particular Employee by EmployeeDocId
        ///// </summary>
        ///// <param name="EmployeeId"></param>
        ///// <returns></returns>
        //public IEnumerable<EmployeeDocument> GetEmployeeDocuments(int EmployeeId)
        //{
        //    var empDocuments = (from e in context.EmployeeDocuments
        //                 where e.EmployeeId == EmployeeId
        //                 select e).ToList();

        //    return empDocuments;
        //}
        //#endregion

        //#region [Get Document Detail of Particular Employee Document by EmployeeDocId]
        ///// <summary>
        ///// Get Document Detail of Particular Employee Document by EmployeeDocId
        ///// </summary>
        ///// <param name="EmployeeDocId"></param>
        ///// <returns></returns>
        //public EmployeeDocument GetEmployeeDocument(int EmployeeDocId)
        //{
        //    var empDocument = (from e in context.EmployeeDocuments
        //                 where e.EmployeeDocId == EmployeeDocId
        //                 select e).FirstOrDefault();

        //    return empDocument;
        //}
        //#endregion

        //#region [Delete particular Empoloyee Document Record]
        ///// <summary>
        ///// Delete particular Empoloyee Document Record
        ///// </summary>
        ///// <param name="empDocument"></param>
        ///// <returns></returns>
        //public void DeleteDocument(EmployeeDocument empDocument)
        //{
        //    context.EmployeeDocuments.Remove(empDocument);
        //    context.SaveChanges();
        //}
        //#endregion   
        #endregion
    }
}
