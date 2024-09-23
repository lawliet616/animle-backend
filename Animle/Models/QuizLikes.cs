using System.ComponentModel.DataAnnotations;

namespace Animle.Models
{
    public class QuizLikes
    {
        [Key]

        public int Id { get; set; }
   
     public int quizId { get; set; }
     public Quiz LikedQuiz { get; set; }
     public int UserId { get; set; }
     public User User { get; set; }
    }

    public class ThreebythreeLike
    {
        [Key]
        public int Id { get; set; }

        public int ThreebythreeId { get; set; }
        public Threebythree LikedThreebyThree { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
