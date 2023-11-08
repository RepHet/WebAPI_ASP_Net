namespace WebAPI_ASP_Net.Repositories.Stack
{
    public interface IStackRepository<T> : ICollectionRepository<T>
    {
        T Peek();
    }
}
