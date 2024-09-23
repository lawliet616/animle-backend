
namespace Animle.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AlternativeTitles
    {
        public List<string> synonyms { get; set; }
        public string en { get; set; }
        public string ja { get; set; }
    }

    public class MalBroadCast
    {
        public string day_of_the_week { get; set; }
        public string start_time { get; set; }
    }

    public class AnimeData
    {
        public Node node { get; set; }
        public Ranking ranking { get; set; }
    }

    public class MalGenre
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class MainPicture
    {
        public string medium { get; set; }
        public string large { get; set; }
    }

    public class Node
    {
        public int id { get; set; }
        public string title { get; set; }
        public MainPicture main_picture { get; set; }
        public AlternativeTitles alternative_titles { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string synopsis { get; set; }
        public double mean { get; set; }
        public int rank { get; set; }
        public int popularity { get; set; }
        public int num_list_users { get; set; }
        public int num_scoring_users { get; set; }
        public string nsfw { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string media_type { get; set; }
        public string status { get; set; }
        public List<MalGenre> genres { get; set; }
        public int num_episodes { get; set; }
        public StartSeason start_season { get; set; }
        public MalBroadCast broadcast { get; set; }
        public string source { get; set; }
        public int average_episode_duration { get; set; }
        public string rating { get; set; }
        public List<Studio> studios { get; set; }
    }

    public class Paging
    {
        public string next { get; set; }
    }

    public class Ranking
    {
        public int rank { get; set; }
    }

    public class MalApiObject
    {
        public List<AnimeData> data { get; set; }
        public Paging paging { get; set; }
    }

    public class StartSeason
    {
        public int year { get; set; }
        public string season { get; set; }
    }

    public class Studio
    {
        public int id { get; set; }
        public string name { get; set; }
    }




}