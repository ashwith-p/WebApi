using Data.Interfaces;
using Data.Models;

namespace Data.Repository
{
    public class RoleDetailsRepository : GenericRepository<RoleDetail>, IRoleDetailRepository
    {
        public RoleDetailsRepository(AshwithEmployeeDirectoryContext context) : base(context)
        {
        }
    }
}
