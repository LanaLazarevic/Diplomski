using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFM.Api.Validation;
using PFM.Application.Result;
using PFM.Application.UseCases.Analytics.Queries.GetSpendingAnalytics;
using Swashbuckle.AspNetCore.Annotations;

namespace PFM.Api.Controllers
{
    [Route("spending-analytics")]
    [ApiController]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnalyticsController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet]
        [SwaggerOperation(
            OperationId = "Spendings_Get",
            Summary = "Retrieve spending analytics by category or by subcategories within category",
            Description = "Retrieves spending analytics by category or by subcategories within category"
        )]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] GetSpendingsAnalyticsQuery request)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState
                    .SelectMany(kvp => kvp.Value?.Errors
                    .Select(err =>
                    {
                        var raw = err.ErrorMessage ?? "";
                        var idx = raw.IndexOf(':');
                        var code = idx > 0 ? raw.Substring(0, idx) : "invalid-format";
                        var message = idx > 0 ? raw.Substring(idx + 1) : raw;
                        var tag = kvp.Key;
                        if (string.IsNullOrEmpty(tag))
                            tag = "query";
                        return new ValidationError
                        {
                            Tag = tag,
                            Error = code,
                            Message = message
                        };
                    }) ?? [])
                    .ToList();
                return BadRequest(new { errors = modelErrors });
            }

            var op = await _mediator.Send(request);
            if (!op.IsSuccess)
            {
                object? errors = null;
                if (op.code == 503)
                {
                    errors = op.Error!
                    .OfType<ServerError>()
                    .Select(e => new
                    {
                        message = e.Message
                    })
                    .ToList();
                } else if (op.code == 440)
                {
                    errors = op.Error!
                    .OfType<BusinessError>()
                    .Select(e => new
                    {
                        problem = e.Problem,
                        message = e.Message,
                        details = e.Details
                    })
                    .First();
                } else
                {
                    errors = op.Error!
                    .OfType<ValidationError>()
                    .Select(e => new
                    {
                        tag = e.Tag,
                        error = e.Error,
                        message = e.Message
                    })
                    .ToList();
                    return StatusCode(op.code, new { errors });
                }

                return StatusCode(op.code, errors);

            }

            return Ok(op.Value);
        }
    }
}
