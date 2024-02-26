using Dynamo.DeviceManagement.Constants;
using Dynamo.DeviceManagement.Models;
using Dynamo.DeviceManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Dynamo.DeviceManagement.Functions.GetDevice
{
    public class GetDevice(ILogger<GetDevice> logger, IRepository<Device> deviceRepository)
    {
        private readonly ILogger<GetDevice> _logger = logger;
        private readonly IRepository<Device> _deviceRepository = deviceRepository;

        [Function("GetDevice")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "devices/{deviceId}")] HttpRequestData req, string deviceId)
        {
            try
            {
                if (!string.IsNullOrEmpty(deviceId))
                {
                    _logger.LogInformation("Received request to get device: {deviceId}", deviceId);

                    var device = await _deviceRepository.GetByIdAsync(deviceId);

                    if (device != null)
                    {
                        return new OkObjectResult(device);
                    }
                    else
                    {
                        return new NotFoundObjectResult(new { Message = HttpResponseMessages.DeviceNotFound });
                    }
                }
                else
                {
                    return new BadRequestObjectResult(new { Message = HttpErrorMessages.DeviceObjectIsNull });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating the device: {ErrorMessage}", $" {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
