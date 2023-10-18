using System.ComponentModel.DataAnnotations.Schema;

namespace PersonPicture.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public string PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }
    }
}
