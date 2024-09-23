using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animle.Models
{
    public class Threebythree
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int UserId { get; set; } 
        public virtual ICollection<AnimeWithEmoji> Animes { get; set; } = new List<AnimeWithEmoji>();
        public virtual ICollection<ThreebythreeLike> ThreebythreeLikes { get; set; }

    }
}
