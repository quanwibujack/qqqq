using System.ComponentModel.DataAnnotations;

namespace ServerGame106.Models
{
    public class GameLevel
    {
        [Key]
        public int LevelId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
    }
}