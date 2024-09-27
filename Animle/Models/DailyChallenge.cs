using System.ComponentModel.DataAnnotations.Schema;

namespace Animle.Models
{
    public class DailyChallenge
    {
        public int Id { get; set; }

        public DateTime TimeCreated { get; set; } = DateTime.Now.Date;

        public string Type { get; set; }

        public virtual ICollection<GameContest> GameContests { get; set; } = new List<GameContest>();

        public virtual ICollection<AnimeWithEmoji> Animes { get; set; } = new List<AnimeWithEmoji>();


    }
}
