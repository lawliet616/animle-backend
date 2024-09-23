namespace Animle.interfaces
{
    public class AnimeFilter
    {
        public int Id { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }

        public string Image { get; set; }
        public  string? JapaneseTitle { get; set; }
        public  object? Description { get; set; }
        public  string? EmojiDescription { get; set; }
        public  int MyanimeListId { get; set; }

        public string Type { get; set; }

        public string? properties { get; set; }


    }
}
