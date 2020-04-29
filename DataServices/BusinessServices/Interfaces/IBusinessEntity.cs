namespace BusinessServices.Interfaces
{
    public interface IBusinessEntity<TKey>
    {
         TKey Id { get; set; }
    }
}