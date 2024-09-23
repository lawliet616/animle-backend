using System.ComponentModel.DataAnnotations;

namespace Animle.Models
{

    public class Player
    {
    [Key]
    public int Id { get; set; } 
    public string ConnectionId { get; set; }
  
    public string Username { get; set; }

    public int Rating { get; set; }

     public int? Result { get; set; } = null;

    public bool PlayerWaitingForMatch { get; set; } = false;
    public Player opponent { get; set; } = null;
    public bool IsPlaying { get; set; } = false;

    public int? QuizId { get; set; } = null;
    }
}
