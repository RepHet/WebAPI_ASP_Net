namespace WebAPI_ASP_Net.Repositories.List
{
    public interface IListRepository<T> : ICollectionRepository<T>
    {
        int GetLength();
    }
}
