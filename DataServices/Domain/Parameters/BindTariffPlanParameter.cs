using System;

namespace Domain.Parameters {
    public class BindTariffPlanParameter : UserParameter {
        public int ParentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}