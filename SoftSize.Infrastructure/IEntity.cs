namespace SoftSize.Infrastructure
{
    public interface IEntity<T>
    {
        T Id { get; }

        bool IsValid();
    }
}
