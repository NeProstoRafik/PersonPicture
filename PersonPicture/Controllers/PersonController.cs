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

        public IActionResult AddFriend(string id)
        {
            _personService.AddFriends(id);
            return Ok();
        }
        [HttpPost("AddPicture")]
        public IActionResult AddPicture([FromForm] FileUpload file)
        {
           _personService.AddPicture( file);
            return Ok();
        }
        [HttpGet("GetAllPicture")]
        public IActionResult GetAllPicture()
        {
            var list = new List<Picture>();
            list = _personService.AllMyPicture();
            return View(list);
        }
        [HttpGet("GetPicturesFriends")]
        public IActionResult GetAllFriendsPictures(string id)
        {            
            var list = new List<Picture>();
            list = _personService.GetAllFriendsPictures(id);
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
