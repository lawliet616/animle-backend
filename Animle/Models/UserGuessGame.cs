namespace Animle.Models
{
    public class UserGuessGame
    {
        public int Id { get; set; }

        public GuessGame GuessGame { get; set; }

        public int GuessGameId { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public int Result { get; set; } 

        public int Attempts { get; set; }

        public User user { get; set; }  


        public int UserId { get; set; }
    }
}
