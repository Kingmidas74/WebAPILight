using BaseWorkerService.Enums;

namespace BaseWorkerService.Models
{
    public partial class ExchangeSettings
    {
        public string ExchangeName {get;set;}
        public ExchangeTypes ExchangeType {get;set;}
        public bool Durable {get;set;}
    }
}