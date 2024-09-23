namespace Animle.Models
{

    public class Versus
    {
        public int Id { get; set; }

        public int Points { get; set; } 
        public bool gameWon { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }    
    

}
