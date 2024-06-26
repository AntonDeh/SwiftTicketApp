﻿namespace SwiftTicketApp.Utilities
{
    public class ServiceResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public ServiceResponse()
        {
            Success = true;
        }
    }
}
