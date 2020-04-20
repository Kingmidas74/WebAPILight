using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Shared.Parameters {
    public class GetAllSortPagedFilterInputParameter : GetAllSortPagedInputParameter {
        public FilterInputParameter Filter { get; set; }
    }
}