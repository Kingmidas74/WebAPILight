using System.Collections.Generic;
using Domain;
using MediatR;

namespace BusinessServices.MediatR
{
    public class GetAllParentsQuery:IRequest<IEnumerable<Parent>>
    {
    }
}