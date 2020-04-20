using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Parameters {
    public class GetAllByTypeParameter : UserParameter {
        public short Type { get; set; }
    }
}