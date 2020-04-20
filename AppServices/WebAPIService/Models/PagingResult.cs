using Contracts.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace WebAPIService.Models
{
    public class PagingResult<T> where T: IBusinessEntity<Guid>
    {
        public IEnumerable<T> Entities { get; set; } = default;
        public int TotalCount { get; set; }
    }
}
