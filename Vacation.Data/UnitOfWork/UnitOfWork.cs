using Vacation.Data;
using Vacation.DataAccess.Repository;
using Vacation.DataAccess.Repository.IRepositories;

namespace Vacation.Data.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly VacationDbContext _db;
        public IEmployeeRepository EmployeeRepository { get; private set; }
        public IDepartmentRepository DepartmentRepository { get; private set; }
        public IRequestRepository RequestRepository { get; private set; }

        public UnitOfWork(VacationDbContext db)
        {
            _db = db;
            EmployeeRepository = new EmployeeRepository(_db);
            DepartmentRepository = new DepartmentRepository(_db);
            RequestRepository = new RequestRepository(_db);
        }
        public async Task<int> SaveChangesAsync()
        {
          return await _db.SaveChangesAsync();
        }
    }
    
}
