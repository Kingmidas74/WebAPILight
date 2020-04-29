using System.Collections.Generic;
using BusinessServices.Models;
using MediatR;

namespace WebAPIService.MediatR
{
    public class GetAllParentsQuery<T>:IRequest<IEnumerable<Parent<T>>>
    {
    }
}