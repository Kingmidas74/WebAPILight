using System;

namespace Domain.Parameters {
    public class GetOneBaseParameter<TKey> : GetAllParameter where TKey : struct, IComparable<TKey>, IEquatable<TKey> {
        public TKey? Id { get; set; }
    }
}