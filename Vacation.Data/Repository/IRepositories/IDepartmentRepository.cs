using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation.Data.Models;
namespace Vacation.DataAccess.Repository.IRepositories
{
    public interface IDepartmentRepository:IRepository<Department>
    {
        public void Update(Department department);
    }
}
