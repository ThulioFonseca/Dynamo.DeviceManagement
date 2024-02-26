namespace Dynamo.DeviceManagement.Constants
{

    public static class HttpResponseMessages
    {
        public const string Ok = "Operation completed successfully";
        public const string DeviceCreated = "Device created successfully";
        public const string DeviceDeleted = "Device deleted successfully";
        public const string DeviceUpdated = "Device updated successfully";
        public const string DeviceNotFound = "Device not found";
    }

    public static class HttpErrorMessages
    {
        public const string InvalidJsonFormat = "Invalid JSON format in the request body";
        public const string DeviceObjectIsNull = "Device object is null";
        public const string ErrorDeletingDevice = "An error occurred while deleting the device";
        public const string InternalServerError = "Internal server error";
    }

}
