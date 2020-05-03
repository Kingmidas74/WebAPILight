using BusinessServices.Models;

namespace BusinessServices.Interfaces
{
    public interface IParentService<T>:IBusinessEntityService<Parent<T>,T>    
    {
         
    }
}