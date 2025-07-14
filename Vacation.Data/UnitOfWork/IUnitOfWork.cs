using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation.Data.Models;
using Vacation.DataAccess.Repository.IRepositories;

namespace Vacation.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository EmployeeRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IRequestRepository RequestRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
