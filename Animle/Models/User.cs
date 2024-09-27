using Animle.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Animle
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public int Rating { get; set; }

        public Threebythree? Threebythree { get; set; } = null;
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public virtual ICollection<GameContest> GameContests { get; set; } = new List<GameContest>();
        public virtual ICollection<QuizLikes> QuizLikes { get; set; } = new List<QuizLikes>();

        public virtual ICollection<ThreebythreeLike> ThreebythreeLikes { get; set; } = new List<ThreebythreeLike>();

        public virtual ICollection<Versus> VersusRecords { get; set; } = new List<Versus>();

        public virtual ICollection<UserGuessGame> UserGuessGames { get; set; } = new List<UserGuessGame>();



    }
}
