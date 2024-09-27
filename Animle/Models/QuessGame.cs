using System.ComponentModel.DataAnnotations.Schema;

namespace Animle.Models
{
    public class GuessGame
    {
        public int Id { get; set; }

        public AnimeWithEmoji Anime { get; set; }

        public int AnimeWithEmojiId { get; set; }

        public DateTime TimeCreated { get; set; } = DateTime.Now.Date;

        public virtual ICollection<UserGuessGame> UserGuessGames { get; set; } = new List<UserGuessGame>();
    }
}
