namespace CleanArchitecture.Presentation.Controllers
{
    using CleanArchitecture.Application.Common.Interfaces;
    using CleanArchitecture.Application.Employees;

    using Microsoft.AspNetCore.Mvc;

    public class EmployeesController : ApiControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDetailsDto>> GetById(int id)
            => this.OkOrNotFound(await this.employeeService.GetById(id));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
            => this.Ok(await this.employeeService.GetAll());

        [HttpPost]
        public async Task<ActionResult<EmployeeDetailsDto>> CreateEmployee(
            CreateEmployeeDto input,
            CancellationToken cancellationToken)
            => await this.employeeService.CreateEmployee(input, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<ActionResult> FireEmployee(
            int id,
            CancellationToken cancellationToken)
            => this.NoContentOrNotFound(await this.employeeService.FireEmployee(id, cancellationToken));

        [HttpPost("{id}")]
        public async Task<ActionResult<EmployeeDetailsDto>> UpdateEmployee(
            int id,
            UpdateEmployeeDto input,
            CancellationToken cancellationToken)
            => this.OkOrNotFound(await this.employeeService.Update(id, input, cancellationToken));
    }
}
