using Dynamo.DeviceManagement.Constants;
using Dynamo.DeviceManagement.Models;
using Dynamo.DeviceManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Dynamo.DeviceManagement.Functions.GetManyDevice
{
    public class GetManyDevice(ILogger<GetManyDevice> logger, IRepository<Device> deviceRepository)
    {
        private readonly ILogger<GetManyDevice> _logger = logger;
        private readonly IRepository<Device> _deviceRepository = deviceRepository;

        [Function("GetManyDevice")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "devices")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("Received request to get device: {deviceId}", req);

                var devices = await _deviceRepository.GetAllAsync();

                if (devices != null)
                {
                    return new OkObjectResult(devices);
                }
                else
                {
                    return new NotFoundObjectResult(new { Message = HttpResponseMessages.DeviceNotFound });
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
