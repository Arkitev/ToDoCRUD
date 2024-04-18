using ToDoCRUD.Models.Interfaces;

namespace ToDoCRUD.Models.Entities
{
    public class ToDoItem : IEntity
    {
        public Guid Id { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
