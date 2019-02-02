using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wall.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace wall.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;
        public HomeController(UserContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Create(ValidateUser user)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<ValidateUser> Hasher = new PasswordHasher<ValidateUser>();
                user.Password = Hasher.HashPassword(user, user.Password);
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password
                };
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("user_id",newUser.Id);
                return RedirectToAction("Success", newUser.Id);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult LoginIn(string Email, string Password)
        {
            var user = _context.user.Where(u=> u.Email == Email).FirstOrDefault();
            if(user != null && Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, Password))
                {
                    HttpContext.Session.SetInt32("user_id", user.Id);
                    return RedirectToAction("Success", user.Id);
                }
            }
            ViewBag.error="Email and/or Password dont match";
            return View("Index");
        }

        [Route("success")]
        public IActionResult Success(int Id)
        {
            User currUser = _context.user.Include(u => u.Messages).SingleOrDefault(c=>c.Id == HttpContext.Session.GetInt32("user_id"));
            List<Messages> mess = _context.messages.OrderByDescending(u => u.CreatedAt).Include(u => u.user).Include(c => c.Comments).ThenInclude(cu => cu.user).ToList();
            ViewBag.mess = mess;
            return View("Success", currUser);
        }

        [HttpPost("create/message")]
        public IActionResult Message(string Message)
        {
            int Id = (int)HttpContext.Session.GetInt32("user_id");
            User user = _context.user.Include(u => u.Messages).SingleOrDefault(x=>x.Id == Id);
            Messages newMessage = new Messages();
            newMessage.Message = Message;
            newMessage.User_Id= (int)HttpContext.Session.GetInt32("user_id");
            _context.messages.Add(newMessage);
            _context.SaveChanges();
            return RedirectToAction("Success");
        }

        [HttpPost("create/comment")]
        public IActionResult Comment(string Comment, int msgId)
        {
            int Id = (int)HttpContext.Session.GetInt32("user_id");
            User user = _context.user.Include(u => u.Comments).SingleOrDefault(x=>x.Id == Id);
            Comments newComment = new Comments();
            newComment.Comment = Comment;
            newComment.User_Id= Id;
            newComment.Messages_Id= msgId;
            _context.comments.Add(newComment);
            _context.SaveChanges();
            return RedirectToAction("Success");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
