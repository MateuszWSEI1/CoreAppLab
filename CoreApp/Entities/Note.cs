namespace CoreApp.Entities
{
    public class Note : EntityBase
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}