using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Application.Requests.HRM.TrainingMajor;
using _365EJSC.ERP.Presentation.Abstractions;
using _365EJSC.ERP.Presentation.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _365EJSC.ERP.Presentation.Controllers.v1.HRM
{
    /// <summary>
    /// Controller version 1 for training major apis
    /// </summary>
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.TRAININGMAJOR_ROUTE)]
    public class TrainingMajorController : ApiController
    {
        private readonly IMediator mediator;

        public TrainingMajorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create training major
        /// </summary>
        /// <param name="request">Request to create training major</param>
        /// <returns>Action result</returns>
        [MapToApiVersion(1)]
        [HttpPost]
        public async Task<IActionResult> CreateV1(CreateTrainingMajorRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for update training major
        /// </summary>
        /// <param name="id">Id of training major need to be updated</param>
        /// <param name="request">Request body contains content to update</param>
        /// <returns></returns>
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateV1(int id, [FromBody] UpdateTrainingMajorRequest request)
        {
            request.Id = id;
            var result = await mediator.Send(request);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for delete training major
        /// </summary>
        /// <param name="id">id of training major</param>
        /// <returns>Action result</returns>
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteV1(int id)
        {
            var command = new DeleteTrainingMajorRequest()
            {
                Id = id,
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get training major by id
        /// </summary>
        /// <param name="id">ID of training major</param>
        /// <returns>Action result with training major as data</returns>
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailV1(int id)
        {
            var query = new GetDetailTrainingMajorRequest()
            {
                Id = id,
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Api version 1 for get all training majors
        /// </summary>
        /// <returns>Action result with list of training majors as data</returns>
        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> GetAllV1()
        {
            var query = new GetAllTrainingMajorRequest();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
