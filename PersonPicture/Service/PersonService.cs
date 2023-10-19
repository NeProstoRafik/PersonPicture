using Microsoft.EntityFrameworkCore;
using PersonPicture.DAL;
using PersonPicture.Models;
using System.Security.Claims;

namespace PersonPicture.Service
{
    public class PersonService : IPersonService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
       
        public PersonService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
          
        }
        private Person GetUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = "";
            if (httpContext != null)
            {
                userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            var user =  _context.DbPeoples.FirstOrDefault(u => u.Id == userId);
            return user;
        }
        private async Task<Person> GetFriend(string id)
        {            
            var user = await _context.DbPeoples.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task AddFriends(string id)
        {
            var friend = await GetFriend(id);
            var per = GetUser();
           
          
            var listFriends = _context.Entry(per)
                  .Collection(p => p.Friends)
                  .Query()
                  .ToList();
            if (!listFriends.Contains(friend))
            {
                per.Friends.Add(friend);
               await _context.SaveChangesAsync();
            }
       
        }
        public async void AddPicture( FileUpload picture)
        {
           // var files = httpContext.Request.Form.Files;
           
            string webRootPath = _webHostEnvironment.WebRootPath;

            string upad = _configuration["ImageDirectory"];
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
           await _context.SaveChangesAsync();
        }       
        public async Task<List<Picture>> AllMyPictureAsync()
        {
            var per = GetUser();
            var listPictures = new List<Picture>();        
            var pictures = _context.Entry(per)
                   .Collection(p => p.Pictures)
                   .Query()
                   .ToList();            

            string upload = _configuration["ImageDirectory"];
           
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
        public async Task<List<Picture>> GetAllFriendsPicturesAsync(string id)
        {
            var user = GetUser();
            var target =await GetFriend(id);
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
            var listFriends = _context.Entry(user)
                       .Collection(p => p.Friends)
                       .Query()
                       .ToList();
            if (listFriends.Contains(target))
            {
                userbool = true;
            }
            listFriends = _context.Entry(target)
                      .Collection(p => p.Friends)
                      .Query()
                      .ToList();
            if (listFriends.Contains(user))
            {
                targetbool = true;
            }
            return userbool == true && targetbool == true;
          
        }
    }
}
