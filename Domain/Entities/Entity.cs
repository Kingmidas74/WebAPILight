using System;

namespace Domain
{
    public partial class Entity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual EntityStatusId Status { get; set; }
    }
}