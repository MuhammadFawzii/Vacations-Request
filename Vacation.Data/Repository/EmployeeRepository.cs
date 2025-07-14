using Vacation.Data;
using Vacation.Data.Models;
using Vacation.DataAccess.Repository.IRepositories;
namespace Vacation.DataAccess.Repository
{
    public class EmployeeRepository: Repository<Employee>, IEmployeeRepository
    {
        private VacationDbContext db;
        public EmployeeRepository(VacationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void Update(Employee entity)
        {
            dbSet.Update(entity);
        }
    }
    
}
