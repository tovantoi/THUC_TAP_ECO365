using _365EJSC.ERP.Application.Requests.HRM.EmployeeRole;
using _365EJSC.ERP.Presentation.Abstractions;
using _365EJSC.ERP.Presentation.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _365EJSC.ERP.Presentation.Controllers.v1.HRM
{
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.EMPLOYEE_ROLE_ROUTE)]
    public class EmployeeRoleController : ApiController
    {
        private readonly IMediator mediator;

        public EmployeeRoleController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [MapToApiVersion(1)]
        [HttpPost]
        public async Task<IActionResult> CreateV1(CreateEmployeeRoleRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update sample
        /// </summary>
        /// <param name="id">Id of sample need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [MapToApiVersion(1)]
        [HttpPut]
        public async Task<IActionResult> UpdateV1(int id, [FromBody] UpdateEmployeeRoleRequest request)
        {
            request.Id = id;
            var result = await mediator.Send(request);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete sample
        /// </summary>
        /// <param name="id">id of sample</param>
        /// <returns>Action result</returns>
        [MapToApiVersion(1)]
        [HttpDelete]
        public async Task<IActionResult> DeleteV1(int id)
        {
            var command = new DeleteEmployeeRoleRequest()
            {
                Id = id,
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get sample by id
        /// </summary>
        /// <param name="id">ID of sample</param>
        /// <returns>Action result with sample as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailV1(int id)
        {
            var query = new GetDetailEmployeeRoleRequest()
            {
                Id = id,
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all samples
        /// </summary>
        /// <returns>Action result with list of samples as data</returns>
        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> GetAllV1()
        {
            var query = new GetAllEmployeeRoleRequest();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
