using PersonPicture.Models;

namespace PersonPicture.Service
{
    public interface IPersonService
    {
        void AddFriends(string id);
        void AddPicture(FileUpload file);
        List<Picture> AllMyPicture();
        List<Picture> GetAllFriendsPictures(string id);
   
    }
}
