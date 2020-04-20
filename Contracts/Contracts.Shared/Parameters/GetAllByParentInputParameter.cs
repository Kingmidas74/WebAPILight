using System;

namespace Contracts.Shared.Parameters
{
    public class GetAllByParentInputParameter: UserInputParameter
    {
        public Guid ParentId {get;set;}
    }
}