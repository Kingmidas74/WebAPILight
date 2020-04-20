using System;

namespace Domain.Parameters {
    public class CreatePromocodeParameter : UserParameter {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TotalNumber { get; set; }
        public int UsedNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PartnerId { get; set; }
        public short Type { get; set; }
        public int[] SubscriptionsId { get; set; }
    }
}