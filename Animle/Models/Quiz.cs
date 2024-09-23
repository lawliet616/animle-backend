using Animle.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animle
{
    public class Quiz
    {
        public int Id { get; set; }

        public ICollection<AnimeWithEmoji> Animes { get; set; } = null;

        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Title { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Thumbnail { get; set; }
        public int UserId { get; set; }
        public virtual User user { get; set; }


        public virtual IList<QuizLikes> Likes { get; set; } = new List<QuizLikes>();   
    }
}