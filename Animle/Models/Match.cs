namespace Animle.Models
{
    public class Match
    {
        public virtual int MatchId { get; set; }
        public virtual string Player1Id { get; set; }
        public virtual string Player2Id { get; set; }
        public virtual string MatchResult { get; set; }
        public virtual DateTime MatchTime { get; set; }
    }
}
