namespace Dynamo.DeviceManagement.Models
{
    public class Device
    {
        public required string Id { get; set; }

        public required string Alias { get; set; }

        public required string MacAddress { get; set; }
    }
}
