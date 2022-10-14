using congestion.calculator;
using congestion_tax_calculator_net_core.BO;
using congestion_tax_calculator_net_core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace congestion_tax_calculator_net_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongestionTaxCalculatorController : ControllerBase
    {        
        private readonly ILogger<CongestionTaxCalculatorController> _logger;
        private CongestionTaxCalculator congestionTaxCalculator;

        public CongestionTaxCalculatorController(ILogger<CongestionTaxCalculatorController> logger, IParameterService parameterService)
        {
            _logger = logger;
            congestionTaxCalculator = new CongestionTaxCalculator(parameterService);
        }


        /// <summary>
        /// Given an array of date timestamps that the vehicle has pass tolling stations, calculates the congestiontax owed.
        /// </summary>
        /// <remarks>
        /// This endpoint requires the timestampes to be in the following format yyyy-mm-dd hh:mm:ss
        /// </remarks>
        /// <param name="requestbody"> JSON body with the vehiele type and an array of timestamps</param>
        /// <returns>The tax value as an int</returns>
        /// <response code="200">Returns the requested tax value</response>
        /// <response code="400">Returned if any of the input parameters are invalid</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [HttpPost(Name = "GetCongestionTax")]
        public IActionResult GetCongestionTax([FromBody] RequestBody requestbody)
        {                        
            var result = congestionTaxCalculator.GetTax(requestbody.Vehicle, requestbody.Dates);
            return Ok(result);
        }
    }
}
