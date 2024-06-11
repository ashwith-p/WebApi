using Data.Interfaces;
using Data.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Data.Repository;

namespace Data.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AshwithEmployeeDirectoryContext context) : base(context)
        {
        }
    }

}
