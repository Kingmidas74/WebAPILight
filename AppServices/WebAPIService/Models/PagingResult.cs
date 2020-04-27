using System;
using System.Collections.Generic;
using Contracts.Shared.Interfaces;

namespace WebAPIService.Models {
    public class PagingResult<T> where T : IBusinessEntity<Guid> {
        public IEnumerable<T> Entities { get; set; } = default;
        public int TotalCount { get; set; }
    }
}