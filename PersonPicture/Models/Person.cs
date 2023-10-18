using Microsoft.AspNetCore.Identity;

namespace PersonPicture.Models
{
    public class Person : IdentityUser
    {
        private List<Picture> _pictures = new List<Picture>();
        public IReadOnlyCollection<Picture> Pictures => _pictures.AsReadOnly();
        public ICollection<Person> Friends { get; set; } = new List<Person>();
        public void AddPicture(Picture picture)
        {
            _pictures.Add(picture);
        }
    }
}
