namespace Wallet.Application.DTOs
{
    public class NameAndId<T>
    {
        public NameAndId(T id, string name)
        {
            Id = id;
            Name = name;
        }

        public T Id { get; set; }
        public string Name { get; set; }
    }
}
