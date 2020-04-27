using System;

namespace Contracts.Shared.Parameters {
    public class IrreversibleDeleteInputParameter : UserInputParameter {
        public Guid Id { get; set; }
    }
}