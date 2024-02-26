using Dynamo.DeviceManagement.Constants;
using Dynamo.DeviceManagement.DTO;
using Dynamo.DeviceManagement.Models;
using Dynamo.DeviceManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dynamo.DeviceManagement.Functions.EditDevice
{
    public class EditDevice(ILogger<EditDevice> logger, IRepository<Device> deviceRepository)
    {
        private readonly ILogger<EditDevice> _logger = logger;
        private readonly IRepository<Device> _deviceRepository = deviceRepository;

        [Function("EditDevice")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "devices/{deviceId}")] HttpRequestData req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var deviceDto = JsonConvert.DeserializeObject<DeviceDto>(requestBody);

                _logger.LogInformation("Received request to edit device: {DeviceId}", deviceDto);

                if (deviceDto == null)
                {
                    return new BadRequestObjectResult(new { Message = HttpErrorMessages.DeviceObjectIsNull });
                }
                else
                {
                    if (deviceDto.Device != null)
                    {
                        await _deviceRepository.UpdateAsync(deviceDto.Device);
                        return new OkObjectResult(new { Message = HttpResponseMessages.DeviceUpdated, Device = deviceDto });
                    }

                    return new BadRequestObjectResult(new { Message = HttpErrorMessages.DeviceObjectIsNull });

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
