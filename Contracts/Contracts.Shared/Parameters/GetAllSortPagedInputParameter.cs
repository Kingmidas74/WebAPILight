using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Shared.Parameters {
    public class GetAllSortPagedInputParameter : GetSortedInputParameter {
        public int? Take { get; set; }
        public int Skip { get; set; }
    }
}