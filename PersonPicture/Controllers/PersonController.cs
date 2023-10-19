using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonPicture.Models;
using PersonPicture.Service;

namespace PersonPicture.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService; 

        public PersonController(IPersonService personService)
        {
            _personService = personService;            
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost("AddFriend")]

        public async Task<IActionResult> AddFriendAsync(string id)
        {
          await _personService.AddFriends(id);
            return Ok();
        }
        [HttpPost("AddPicture")]
        public async Task<IActionResult> AddPicture([FromForm] FileUpload file)
        {
          await _personService.AddPicture( file);
            return Ok();
        }
        [HttpGet("GetAllPicture")]
        public async Task<IActionResult> GetAllPicture()
        {
            var list = new List<Picture>();
            list =await _personService.AllMyPictureAsync();
            return View(list);
        }
        [HttpGet("GetPicturesFriends")]
        public async Task<IActionResult> GetAllFriendsPicturesAsync(string id)
        {            
            var list = new List<Picture>();
            list =await _personService.GetAllFriendsPicturesAsync(id);
            if (list==null)
            {
                return NoContent();
            }
            else
            {
                return View(list);
            }
           
        }
    }
}
