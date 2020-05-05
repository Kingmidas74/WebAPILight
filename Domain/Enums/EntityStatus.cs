namespace Domain
{
    public enum EntityStatusId : int {
        Active = 1,
        Inactive = 2
    }
    public class EntityStatus {
        public EntityStatusId EntityStatusId { get; set; }
        public string Value { get; set; }
    }
}