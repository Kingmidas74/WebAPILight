using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Parameters {
    public class CreatePartnerParameter : UserParameter {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}