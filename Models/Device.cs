using Newtonsoft.Json;

namespace Dynamo.DeviceManagement.Models
{
    public class Device
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public required string Alias { get; set; }

        public required string MacAddress { get; set; }

        public Device()
        {
            if (string.IsNullOrEmpty(Id))
            {
                this.Id = Guid.NewGuid().ToString();
            }
        }
    }
}
