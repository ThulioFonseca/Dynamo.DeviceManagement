using Dynamo.DeviceManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace Dynamo.DeviceManagement.DTO
{
    public class DeviceDto
    {
        [Required]
        public Device? Device { get; set; }

        [Required]
        public required string User { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
