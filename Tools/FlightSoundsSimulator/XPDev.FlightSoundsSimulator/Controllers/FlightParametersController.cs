using XPDev.FlightSoundsSimulator.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using XPDev.FlightManagement;
using System.Threading.Tasks;

namespace XPDev.FlightSoundsSimulator
{
    [ApiController]
    [Route("[controller]")]
    public class FlightParametersController : ControllerBase
    {
        private readonly ILogger<FlightParametersController> _logger;
        private readonly IFlightParameterRequestProcessor _requestProcessor;

        public FlightParametersController(ILogger<FlightParametersController> logger, IFlightParameterRequestProcessor requestProcessor)
        {
            _logger = logger;
            _requestProcessor = requestProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FlightParameters flightParameters)
        {
            _logger.LogDebug("Request received. Flight parameters: ", flightParameters);
            
            try
            {
                return Ok(new { currentFlightState = GetFlightStateName(await _requestProcessor.ProcessRequestAsync(flightParameters)) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when processing post request");
                throw;
            }
        }

        private string GetFlightStateName(FlightState flightState)
        {
            var flightStateString = flightState.ToString();
            var name = new StringBuilder();

            for (var index = 0; index < flightStateString.Length; index++)
            {
                var character = flightStateString[index];

                if (index > 0 && char.IsUpper(character))
                {
                    name.Append(' ');
                    character = char.ToLower(character);
                }

                name.Append(character);
            }

            return name.ToString();
        }
    }
}
