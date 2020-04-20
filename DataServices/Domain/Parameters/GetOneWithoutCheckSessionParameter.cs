using System;

namespace Domain.Parameters {
    public class GetOneWithoutCheckSessionParameter : GetOneBaseParameter<Guid> {
        public bool CheckSession { get; set; }
    }
}