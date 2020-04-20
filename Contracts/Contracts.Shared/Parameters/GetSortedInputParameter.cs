using System;
using System.Collections.Generic;
using System.Text;
using Contracts.Shared.Enums;

namespace Contracts.Shared.Parameters {

    public class GetSortedInputParameter : UserInputParameter {
        public int SortIndex { get; set; }
        public SortOrderTypes SortOrder { get; set; }
    }
}