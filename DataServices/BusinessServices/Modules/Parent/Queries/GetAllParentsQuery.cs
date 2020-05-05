using System.Collections.Generic;
using Domain;
using MediatR;

namespace BusinessServices.Modules.ParentModule
{
    public class GetAllParentsQuery:IRequest<IEnumerable<Parent>>
    {
    }
}