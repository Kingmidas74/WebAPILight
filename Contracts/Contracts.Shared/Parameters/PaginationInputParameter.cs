using Contracts.Shared.Enums;

namespace Contracts.Shared.Parameters {
    public class PaginationInputParameter : UserInputParameter {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? Filter { get; set; }
        public SortOrderTypes SortOrder { get; set; }
    }
}