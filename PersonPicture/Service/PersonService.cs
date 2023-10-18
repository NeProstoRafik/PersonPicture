using PersonPicture.DAL;
using PersonPicture.Models;
using System.Security.Claims;

namespace PersonPicture.Service
{
    public class PersonService : IPersonService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
       
        public PersonService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
          
        }
        private Person GetUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = "";
            if (httpContext != null)
            {
                userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            var user = _context.DbPeoples.FirstOrDefault(u => u.Id == userId);
            return user;
        }
        private Person GetFriend(string id)
        {            
            var user = _context.DbPeoples.FirstOrDefault(u => u.Id == id);
            return user;
        }
        public void AddFriends(string id)
        {
            var per = GetUser();
            var friend=GetFriend(id);
          
            var ListFriends = _context.Entry(per)
                  .Collection(p => p.Friends)
                  .Query()
                  .ToList();
            if (!ListFriends.Contains(friend))
            {
                per.Friends.Add(friend);
                _context.SaveChanges();
            }
       
        }
        public void AddPicture( FileUpload picture)
        {
           // var files = httpContext.Request.Form.Files;
           
            string webRootPath = webHostEnvironment.WebRootPath;

            string upad = configuration["ImageDirectory"];
            string upload = webRootPath + upad;
            string fileName = Guid.NewGuid().ToString();
           // string extention = Path.GetExtension(files[0].FileName);
            string extention = Path.GetExtension(picture.files.FileName);
            using (var filestrim = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
            {
               // files[0].CopyTo(filestrim);
                picture.files.CopyTo(filestrim);
            }
            var per = GetUser();
            var pic = new Picture
            {
                DateCreate = DateTime.Now,
                Image = fileName + extention,
                Name = "fileName",
                Person = per,
                PersonId = per.Id,
            };
          
            per.AddPicture(pic);
            _context.SaveChanges();
        }       
        public List<Picture> AllMyPicture()
        {
            var per = GetUser();
            var listPictures = new List<Picture>();        
            var pictures = _context.Entry(per)
                   .Collection(p => p.Pictures)
                   .Query()
                   .ToList();            

            string upload = configuration["ImageDirectory"];
           
            foreach (var item in pictures)
            {
                string imagePath = null;
                if (item.Image != null)
                {
                    imagePath = Path.Combine(upload, item.Image);
                }               
                item.Image = imagePath;
                listPictures.Add(item);
            }
            return listPictures;
        }
        public List<Picture> GetAllFriendsPictures(string id)
        {
            var user = GetUser();
            var target = GetFriend(id);
            var look = LookingPikture(user, target);
        
            if (look==true)
            {
                var list = _context.Entry(target)
                     .Collection(p => p.Pictures)
                     .Query()
                     .ToList();
           
                return list;
            }
            else
            {
                return null;
            }
         
        }
        private bool LookingPikture(Person user, Person target)
        {
            var userbool = false;
            var targetbool = false;
            var ListFriends = _context.Entry(user)
                       .Collection(p => p.Friends)
                       .Query()
                       .ToList();
            if (ListFriends.Contains(target))
            {
                userbool = true;
            }
            ListFriends = _context.Entry(target)
                      .Collection(p => p.Friends)
                      .Query()
                      .ToList();
            if (ListFriends.Contains(user))
            {
                targetbool = true;
            }

            if (userbool == true && targetbool==true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
