using Dynamo.DeviceManagement.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dynamo.DeviceManagement.Functions.DeleteDevice
{
    public class DeleteDevice(ILogger<DeleteDevice> logger)
    {
        /// <summary>
        /// Delete a device from DynamoDB
        /// <param name="req">data from HttpRequest where device id is passed</param>
        /// <returns>OkObjectResult</returns>
        /// </summary>

        [Function("DeleteDevice")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var deviceId = JsonConvert.DeserializeObject<DeviceDto>(requestBody)!.Id;

                logger.LogInformation($"{nameof(DeleteDevice)} - Received request: {JsonConvert.SerializeObject(deviceId)}");

                return new OkObjectResult("Welcome to Azure Functions!");
            }
            catch (Exception ex)
            {
                logger.LogError($"{nameof(DeleteDevice)} - Exception: {ex.Message}");
                return new BadRequestResult();
            }
        }
    }
}
