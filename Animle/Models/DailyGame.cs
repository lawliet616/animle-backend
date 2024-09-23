namespace Animle.Models
{
    public class GameContest
    {
        public int Id { get; set; }

        public DateTime TimePlayed { get; set; }

        public int Points { get; set; }

        public string Type { get; set; }

        public Guid gameGuid { get; set; }

        public virtual User User { get; set; }  

        public int UserId { get; set; }
    }
}
