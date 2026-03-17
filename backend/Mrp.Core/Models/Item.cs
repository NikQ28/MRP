using System.Security.Cryptography.X509Certificates;

namespace Mrp.Core.Models
{
    public class Item
    {
        private Item(int id, string name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public int Id { get; }

        public string Name { get; } = string.Empty;

        public string? Description { get; } = string.Empty;

        public static (Item item, string error) Create(int id, string name, string? description)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(name))
                error = "Name can't be empty!";
            var item = new Item(id, name, description);
            return (item, error);
        }
    }
}
