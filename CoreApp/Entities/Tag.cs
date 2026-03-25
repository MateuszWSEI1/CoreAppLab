namespace CoreApp.Entities
{
    public class Tag : EntityBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}