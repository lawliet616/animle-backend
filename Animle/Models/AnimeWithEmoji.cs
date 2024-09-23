using System.ComponentModel.DataAnnotations.Schema;

namespace Animle.Models
{
    public class AnimeWithEmoji
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string JapaneseTitle { get; set; }
        public string Description { get; set; }
        public string? EmojiDescription { get; set; }
        public string Thumbnail { get; set; }
        public string? Image { get; set; }
        public int MyanimeListId { get; set; }

        public string properties { get; set; }

        [NotMapped]
        public string Type { get; set; }

        public virtual ICollection<Threebythree> Threebythree { get; set; } = new List<Threebythree>();
        public virtual ICollection<Quiz>? Quizzes { get; set; } = null;
        
   
    }
}
