namespace ToDoCRUD.Models.Interfaces
{
    public interface IEntity
    {
        public Guid Id { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
