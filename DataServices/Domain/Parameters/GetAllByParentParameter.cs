using System;

namespace Domain.Parameters {
    public class GetAllByParentParameter : UserParameter {
        public Guid ParentId { get; set; }
    }
}