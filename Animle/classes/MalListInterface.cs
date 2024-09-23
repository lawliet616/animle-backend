namespace Animle.interfaces
{
    using System.Collections.Generic;

    public interface IAnimeData
    {
        IPagingData Paging { get; set; }
        List<IAnimeNode> Data { get; set; }
    }

    public interface IPagingData
    {
        string Next { get; set; }
    }

    public interface IAnimeNode
    {
        IAnimeDetails Node { get; set; }
    }

    public interface IAnimeDetails
    {
        int Id { get; set; }
        string Title { get; set; }
        IMainPicture MainPicture { get; set; }
    }

    public interface IMainPicture
    {
        string Medium { get; set; }
        string Large { get; set; }
    }
}
