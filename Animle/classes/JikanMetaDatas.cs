using Animle.Models;

namespace Animle.Models
{
    using System;
    using System.Collections.Generic;

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);


    public class Broadcast
    {
        public string day { get; set; }
        public string time { get; set; }
        public string timezone { get; set; }
        public string @string { get; set; }
    }

    public class Datum
    {
        public int mal_id { get; set; }
        public string url { get; set; }
        public Images images { get; set; }
        public Trailer trailer { get; set; }
        public bool approved { get; set; }
        public List<Title> titles { get; set; }
        public string title { get; set; }
        public string title_english { get; set; }
        public string title_japanese { get; set; }
        public List<string> title_synonyms { get; set; }
        public string type { get; set; }
        public string source { get; set; }
        public int episodes { get; set; }
        public string status { get; set; }
        public bool airing { get; set; }
        public string duration { get; set; }
        public string rating { get; set; }
        public double score { get; set; }
        public int scored_by { get; set; }
        public int rank { get; set; }
        public int popularity { get; set; }
        public int members { get; set; }
        public int favorites { get; set; }
        public string synopsis { get; set; }
        public string background { get; set; }
        public string season { get; set; }
        public int year { get; set; }
        public Broadcast broadcast { get; set; }
        public List<Genre> producers { get; set; }
        public List<Genre> licensors { get; set; }
        public List<Genre> studios { get; set; }
        public List<Genre> genres { get; set; }
        public List<Genre> explicit_genres { get; set; }
        public List<Genre> themes { get; set; }
        public List<Demographic> demographics { get; set; }
    }

    public class Demographic
    {
        public int mal_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class From
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class Genre
    {
        public int mal_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Images
    {
        public Jpg jpg { get; set; }
        public Webp webp { get; set; }
        public string image_url { get; set; }
        public string small_image_url { get; set; }
        public string medium_image_url { get; set; }
        public string large_image_url { get; set; }
        public string maximum_image_url { get; set; }
    }

    public class Items
    {
        public int count { get; set; }
        public int total { get; set; }
        public int per_page { get; set; }
    }

    public class Jpg
    {
        public string image_url { get; set; }
        public string small_image_url { get; set; }
        public string large_image_url { get; set; }
    }


    public class Pagination
    {
        public int last_visible_page { get; set; }
        public bool has_next_page { get; set; }
        public int current_page { get; set; }
        public Items items { get; set; }
    }



    public class Prop
    {
        public From from { get; set; }
        public To to { get; set; }
    }

    public class JikanAnimeData
    {
        public Pagination pagination { get; set; }
        public List<Datum> data { get; set; }
    }




    public class Title
    {
        public string type { get; set; }
        public string title { get; set; }
    }

    public class To
    {
        public int? day { get; set; }
        public int? month { get; set; }
        public int? year { get; set; }
    }

    public class Trailer
    {
        public string youtube_id { get; set; }
        public string url { get; set; }
        public string embed_url { get; set; }
        public Images images { get; set; }
    }

    public class Webp
    {
        public string image_url { get; set; }
        public string small_image_url { get; set; }
        public string large_image_url { get; set; }
    }


}



