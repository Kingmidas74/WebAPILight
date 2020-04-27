using System;

namespace WebAPIService.Models {
    public class ApplicationOptions {
        public String IdentityServiceURI { get; set; }
        public String RabbitMQSeriveURI { get; set; }
    }
}