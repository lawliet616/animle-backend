using Animle.Models;

namespace Animle.SignalR
{
    public class SignalrAnimeService
    {
        private List<AnimeWithEmoji> itemList = new List<AnimeWithEmoji>();

        public List<AnimeWithEmoji> GetList()
        {
            return itemList;
        }

        public void SetList(List<AnimeWithEmoji> itemsToUse)
        {
            itemList = new List<AnimeWithEmoji>(itemsToUse);
        }
    }
}
