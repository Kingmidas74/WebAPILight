using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;

namespace DataAccess
{
    public abstract class DataBaseEntity<T>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public T Id { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }        
        
        public EntityStatusId StatusId {get;set;}

        [ForeignKey(nameof(EntityStatusId))]
        public virtual EntityStatus Status {get;set;}
    }
}