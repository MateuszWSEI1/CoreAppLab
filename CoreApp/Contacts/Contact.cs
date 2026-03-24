using CoreApp.Enums;

namespace CoreApp.Entities
{
    public abstract class Contact
    {
        public Guid Id { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ContactStatus Status { get; set; }

        public List<Tag> Tags { get; set; } = new();
        public List<Note> Notes { get; set; } = new();

        public abstract string GetDisplayName();
    }
}