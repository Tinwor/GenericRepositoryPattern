namespace Repository.Infrastructure
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }

        int TotalCount { get; set; }
    }


    public interface IEntity : IEntity<int>
    {

    }
}