using Vacation.Data;
using Vacation.Data.Models;
using Vacation.DataAccess.Repository.IRepositories;

namespace Vacation.DataAccess.Repository
{
    public class RequestRepository: Repository<Request>, IRequestRepository
    {
        private VacationDbContext db;
        public RequestRepository(VacationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void UpdateAsync(Request Request)
        {
            dbSet.Update(Request);
        }
    }
  
}
