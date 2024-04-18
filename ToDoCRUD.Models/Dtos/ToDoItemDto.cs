namespace ToDoCRUD.Models.Dtos
{
    public class ToDoItemDto
    {
        public Guid Id { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
