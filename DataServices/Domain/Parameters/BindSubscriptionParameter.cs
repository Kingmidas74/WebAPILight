using System;

namespace Domain.Parameters {
    public class BindSubscriptionParameter : UserParameter {
        public DateTime EndDate { get; set; }
        public int ProductId { get; set; }
        public DateTime StartDate { get; set; }
        public int ParentId { get; set; }
    }
}