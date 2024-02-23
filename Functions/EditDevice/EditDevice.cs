using Dynamo.DeviceManagement.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dynamo.DeviceManagement.Functions.EditDevice
{
    public class EditDevice(ILogger<EditDevice> logger)
    {
        /// <summary>
        /// Edit an existing from DynamoDB
        /// <param name="req">data from HttpRequest where device id is passed</param>
        /// <returns>OkObjectResult</returns>
        /// </summary>

        [Function("EditDevice")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var deviceId = JsonConvert.DeserializeObject<DeviceDto>(requestBody)!.Id;

                logger.LogInformation($"{nameof(EditDevice)} - Received request: {JsonConvert.SerializeObject(deviceId)}");

                return new OkObjectResult("Ok");
            }
            catch (Exception ex)
            {
                logger.LogError($"{nameof(EditDevice)} - Error: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
