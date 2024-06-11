using EmployeeDirectoryWebAPI.DTO;

namespace EmployeeDirectoryWebAPI.Interfaces
{
    public interface IEmployeeProvider
    {

        public bool IsValidEmployee(string value, string type);

        public bool Validator(EmployeeDTO employee);

    }
}
