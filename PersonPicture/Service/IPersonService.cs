using PersonPicture.Models;

namespace PersonPicture.Service
{
    public interface IPersonService
    {
        Task AddFriends(string id);
        void AddPicture(FileUpload file);       
        Task<List<Picture>> AllMyPictureAsync();       
        Task<List<Picture>> GetAllFriendsPicturesAsync(string id);
    }
}
