using Vacation.DataAccess.Repository.IRepositories;
using Vacation.Data.Models;
using Vacation.Data;
namespace Vacation.DataAccess.Repository
{
    public class DepartmentRepository: Repository<Department>, IDepartmentRepository
    {
        private VacationDbContext db;
        public DepartmentRepository(VacationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void Update(Department entity)
        {
            dbSet.Update(entity);
        }

    }
}
