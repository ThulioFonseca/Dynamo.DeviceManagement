using Dynamo.DeviceManagement.Models;
using Dynamo.DeviceManagement.Repository;
using Dynamo.DeviceManagement.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Dynamo.DeviceManagement.DTO;

namespace Dynamo.DeviceManagement.Functions.CreateDevice
{
    public class CreateDevice(ILogger<CreateDevice> logger, IRepository<Device> deviceRepository)
    {
        private readonly ILogger<CreateDevice> _logger = logger;
        private readonly IRepository<Device> _deviceRepository = deviceRepository;

        [Function("CreateDevice")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "devices")] HttpRequestData req)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var newDevice = JsonConvert.DeserializeObject<DeviceDto>(requestBody);

                _logger.LogInformation("Received request to create device: {RequestBody}", requestBody);

                if (newDevice == null)
                {
                    return new BadRequestObjectResult("Device object is null!");
                }
                else
                {
                    if (newDevice.Device != null)
                    {
                        var createdDevice = await _deviceRepository.AddAsync(newDevice.Device);

                        _logger.LogInformation("Created device: {CreatedDevice}", $"{JsonConvert.SerializeObject(newDevice)}");

                        return new OkObjectResult(new { Message = HttpResponseMessages.DeviceCreated, Device = createdDevice });
                    }

                    return new BadRequestObjectResult("Device object is null!");

                }
            }
            catch (JsonException)
            {
                return new BadRequestObjectResult(new { Message = HttpErrorMessages.InvalidJsonFormat });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating the device: {ErrorMessage}", $" {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
