namespace Domain.Parameters {
    public class GetAllPagedParameter : GetAllParameter {
        public int? Take { get; set; }
        public int Skip { get; set; }
        public int SortIndex { get; set; }
        public int SortOrder { get; set; }

        public string Filter { get; set; }
    }
}