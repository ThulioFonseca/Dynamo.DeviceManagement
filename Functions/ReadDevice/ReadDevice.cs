using Dynamo.DeviceManagement.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dynamo.DeviceManagement.Functions.ReadDevice
{
    public class ReadDevice(ILogger<ReadDevice> logger)
    {
        /// <summary>
        /// Delete a device from DynamoDB
        /// <param name="req">data from HttpRequest where device id is passed</param>
        /// <returns>OkObjectResult</returns>
        /// </summary>

        [Function("ReadDevice")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var deviceId = JsonConvert.DeserializeObject<DeviceDto>(requestBody)!.Id;

                logger.LogInformation($"{nameof(ReadDevice)} - Received request: {JsonConvert.SerializeObject(deviceId)}");

                return new OkObjectResult("Welcome to Azure Functions!");
            }
            catch (Exception ex)
            {
                logger.LogError($"{nameof(ReadDevice)} - Error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
