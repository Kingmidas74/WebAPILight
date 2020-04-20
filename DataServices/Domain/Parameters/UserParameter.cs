using System;
using Domain.Interfaces;

namespace Domain.Parameters {
    public class UserParameter : ICommandParameter {
        public Guid UserId { get; set; }
    }
}