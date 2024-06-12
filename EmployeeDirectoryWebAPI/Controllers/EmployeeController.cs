using Data;
using Data.Interfaces;
using Data.Models;
using Data.Repository;
using EmployeeDirectoryWebAPI.DTO;
using EmployeeDirectoryWebAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EmployeeDirectoryWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController(IEmployeeRepository employeeRepository,IEmployeeProvider employeeProvider) : Controller
    {
        private readonly  IEmployeeRepository _employeeRepository=employeeRepository;
        private readonly IEmployeeProvider _employeeProvider=employeeProvider;

        [HttpGet]
        [Route("All", Name = "GetEmployees")]
        public ActionResult<List<Employee>> GetEmployees()
        {
            List<Employee> employees = _employeeRepository.GetAll().Result;
            return Ok(employees);
        }

        [HttpGet("{id}", Name = "GetEmployeeById")]
        public ActionResult<List<Employee>> GetEmployeeById(string id)
        {
            if(String.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid Employee id");
            }
            Employee? employee = _employeeRepository.GetById(id).Result;
            if (employee == null)
            {
                return NotFound("Employee Id does not exist");
            }
            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteEmployee(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            Employee? employee = _employeeRepository.GetById(id).Result;
            if (employee == null)
            {
                return NotFound($"Employee with id:{id} not found");
            }
            return Ok(_employeeRepository.Delete(employee));
        }

        [HttpPost]
        [Route("Create")]

        public async Task<ActionResult<EmployeeDTO>> AddEmployee([FromBody]EmployeeDTO employeeDTO)
        {
            if (employeeDTO == null)
            {
                return BadRequest();
            }
            if(_employeeProvider.Validator(employeeDTO))
            {
                string id;
                if (_employeeRepository.GetAll().Result.LastOrDefault()!=null)
                {
                    id = "TZ000" + int.Parse(_employeeRepository.GetAll().Result.LastOrDefault()!.Id[3..]) + 1;
                }
                else
                {
                    id = "TZ0001";
                }

                Employee employee = new()
                {
                    Id=id,
                    FirstName=employeeDTO.FirstName,
                    LastName=employeeDTO.LastName,
                    DateOfBirth= employeeDTO.DateOfBirth!=null ? DateOnly.ParseExact(employeeDTO.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                    JoiningDate = DateOnly.ParseExact(employeeDTO.JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    RoleId=employeeDTO.RoleId,
                    DepartmentId=employeeDTO.DepartmentId,
                    LocationId=employeeDTO.LocationId,
                    ProjectId=employeeDTO.ProjectId,
                    ManagerId=employeeDTO.ManagerId,
                    Email=employeeDTO.Email,
                    MobileNumber=employeeDTO.MobileNumber!=null?long.Parse(employeeDTO.MobileNumber):null,
                    
                };
                await _employeeRepository.Add(employee);
                return Ok(employeeDTO);
            }
            return BadRequest("InValid Employee");
        }

        [HttpPut("Edit/{id}")]

        public async Task<ActionResult<EmployeeDTO>> EditEmployee(string id,[FromBody] EmployeeDTO employeeDTO)
        {
            Employee? employee=_employeeRepository.GetById(id).Result;
            if(employee==null)
            {
                return BadRequest();
            }
            if(_employeeProvider.Validator(employeeDTO))
            {
                employee!.FirstName = employeeDTO.FirstName;
                employee.LastName = employeeDTO.LastName;
                employee.DateOfBirth = employeeDTO.DateOfBirth != null ? DateOnly.ParseExact(employeeDTO.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture) : null;
                employee.JoiningDate = DateOnly.ParseExact(employeeDTO.JoiningDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                employee.RoleId = employeeDTO.RoleId;
                employee.DepartmentId = employeeDTO.DepartmentId;
                employee.LocationId = employeeDTO.LocationId;
                employee.ProjectId = employeeDTO.ProjectId;
                employee.ManagerId = employeeDTO.ManagerId;
                employee.Email = employeeDTO.Email;
                employee.MobileNumber = employeeDTO.MobileNumber != null ? long.Parse(employeeDTO.MobileNumber) : null;
                await _employeeRepository.Update(employee);
                return Ok(employeeDTO);
            }
            return BadRequest();

        }



    }

    
}
