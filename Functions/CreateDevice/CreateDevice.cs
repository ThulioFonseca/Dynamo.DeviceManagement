using Dynamo.DeviceManagement.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dynamo.DeviceManagement.Functions.CreateDevice
{
    public class CreateDevice(ILogger<CreateDevice> logger)
    {
        /// <summary>
        /// Create a device from DynamoDB
        /// <param name="req">data from HttpRequest where device data is passed</param>
        /// <returns>OkObjectResult</returns>
        /// </summary>
        [Function("CreateDevice")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var newDevice = JsonConvert.DeserializeObject<DeviceDto>(requestBody);

                logger.LogInformation($"{nameof(CreateDevice)} - Received request: + {JsonConvert.SerializeObject(newDevice)}");

                return new OkObjectResult("Welcome to Azure Functions!");
            }
            catch (Exception ex)
            {
                logger.LogError($"{nameof(CreateDevice)} - Error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
