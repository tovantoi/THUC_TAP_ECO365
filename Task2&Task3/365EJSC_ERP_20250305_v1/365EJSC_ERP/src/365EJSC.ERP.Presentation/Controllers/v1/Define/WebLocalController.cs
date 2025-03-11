using _365EJSC.ERP.Application.Requests.Define.WebLocal;
using _365EJSC.ERP.Presentation.Abstractions;
using _365EJSC.ERP.Presentation.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _365EJSC.ERP.Presentation.Controllers.v1.Define
{
    /// <summary>
    /// Controller version 1 for sample apis
    /// </summary>
    [ApiVersion(1)]
    [Route(RouteConstant.API_PREFIX + RouteConstant.WEBLOCAL_ROUTE)]
    public class WebLocalController : ApiController
    {
        private readonly IMediator mediator;

        public WebLocalController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Api version 1 for create sample
        /// </summary>
        /// <param name="request">Request to create sample</param>
        /// <returns>Action result</returns>
        [MapToApiVersion(1)]
        [HttpPost]
        public async Task<IActionResult> CreateV1(CreateWebLocalRequest request)
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
        public async Task<IActionResult> UpdateV1(string id, [FromBody] UpdateWebLocalRequest request)
        {
            request.Id = id.ToString();
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
        public async Task<IActionResult> DeleteV1(string id)
        {
            var command = new DeleteWebLocalRequest()
            {
                Id = id.ToString(),
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
        public async Task<IActionResult> GetDetailV1(string id)
        {
            var query = new GetDetailWebLocalRequest()
            {
                Id = id.ToString(),
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
            var query = new GetAllWebLocalRequest();
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
