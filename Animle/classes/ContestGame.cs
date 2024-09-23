using Animle.Models;

namespace Animle.interfaces
{
    public class ContestGame
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Type { get; set; } 
      
        public List<AnimeWithEmoji> Anime { get; set; } = new List<AnimeWithEmoji>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
