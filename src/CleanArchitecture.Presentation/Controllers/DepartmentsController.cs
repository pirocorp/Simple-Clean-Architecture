namespace CleanArchitecture.Presentation.Controllers
{
    using Application.Common.Interfaces;
    using Application.Departments;
    using Microsoft.AspNetCore.Mvc;

    public class DepartmentsController : ApiControllerBase
    {
        private readonly IDepartmentService departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentFullDetailsDto>> GetById(int id)
            => this.OkOrNotFound(await this.departmentService.GetById(id));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentListingDto>>> GetDepartments()
            => this.Ok(await this.departmentService.GetAll());

        [HttpPost]
        public async Task<ActionResult<DepartmentDetailsDto>> CreateDepartment(
            CreateDepartmentDto input,
            CancellationToken cancellationToken)
            => await this.departmentService.CreateDepartment(input, cancellationToken);

        [HttpPatch("{id}")]
        public async Task<ActionResult<DepartmentDetailsDto>> RenameDepartment(
            int id,
            string name,
            CancellationToken cancellationToken)
            => this.OkOrNotFound(await this.departmentService.RenameDepartment(id, name, cancellationToken));

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
            => this.NoContentOrNotFound(await this.departmentService
                .DeleteDepartment(id, cancellationToken));
    }
}
