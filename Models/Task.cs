
namespace TasksApi.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public bool Completed { get; set; }
    }
}