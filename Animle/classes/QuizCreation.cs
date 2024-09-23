using Animle.Models;

namespace Animle.interfaces
{
    public class QuizCreation
    {
        public int? Id { get; set; }
        public string Title { get; set; }

        public int SelectedImageId { get; set; }
        public IList<int> AnimeIds { get; set; }  = new List<int>();

    }
}
