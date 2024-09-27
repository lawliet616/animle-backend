using System.ComponentModel.DataAnnotations.Schema;

namespace Animle.Models
{
    public class GameContest
    {
        public int Id { get; set; }

        public DateTime TimePlayed { get; set; } = DateTime.Now;


        public int UserId { get; set; }

        public int Result { get; set; }

        public int DailyChallengeId { get; set; }
        
        [ForeignKey("DailyChallengeId")]
        public virtual DailyChallenge Challenge { get; set; }


        public virtual User User { get; set; }
    }
}
