using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSharpWeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace CSharpWeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext db;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid) 
            {
                if (db.Users.Any(u=> u.Email == newUser.Email)) 
                {
                    ModelState.AddModelError("Email", "Email is already in use");
                    return View("Index");
                }
                PasswordHasher<User> Hasher =  new PasswordHasher<User>();
                //Hash the password only after verifying that everything else is good to go
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                db.Add(newUser);
                db.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Dashboard");
            }

            return View("Index");
        }

        [HttpPost("loginUser")]
        public IActionResult LoginUser(LogUser logUser)
        {
            if (ModelState.IsValid)
            {
                User userindb = db.Users.FirstOrDefault(u => u.Email == logUser.LoginEmail);
                if (userindb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid login attempt");
                    return View("Index");
                }
                //check if password is correct
                PasswordHasher<LogUser> Hasher = new PasswordHasher<LogUser>();
                PasswordVerificationResult result = Hasher.VerifyHashedPassword(logUser, userindb.Password, logUser.LoginPassword); 
                //When the vertifcation runs, it will passed 1(successfully) or 0(password is incorrect)
                if (result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid login attempt");
                    return View("Index");
                }

                HttpContext.Session.SetInt32("UserId", userindb.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }


        [HttpGet("/Dashboard")]
        public IActionResult Dashboard()
        {
                if (HttpContext.Session.GetInt32("UserId") == null)
                {
                    return RedirectToAction("Index");
                }
                int ? userId = HttpContext.Session.GetInt32("UserId");
                List<Wedding> AllWeddings = db.Weddings.Include(w => w.GuestReponses).OrderBy(u => u.Date).ToList();
                ViewBag.UserId = (int)userId;
                return View("Dashboard", AllWeddings);
        }

        // [HttpGet("/NewWedding")]
        // public IActionResult NewWedding()
        // {
        //     if (HttpContext.Session.GetInt32("UserId") == null)
        //     {
        //         return RedirectToAction("Index");
        //     }

        //     return View("Planner");
        // }

        // [HttpPost("/RVSP/{WeddingID}")]
        // public IActionResult RSVP(GuestResponse newGuestResponse, int WeddingID, int UserId, bool existingRSVP)
        // {
        //     int ? userId = HttpContext.Session.GetInt32("UserId");
        //     if (userId != null)
        //     {
        //         if(existingRSVP){
        //             newGuestResponse.UserId = (int)HttpContext.Session.GetInt32("UserId");
        //             newGuestResponse = db.GuestResponses.FirstOrDefault(g => g.WeddingId == WeddingID && g.UserId == newGuestResponse.UserId);
        //             db.GuestResponses.Remove(newGuestResponse);
        //             db.SaveChanges();
        //             return RedirectToAction("Dashboard");

        //         } else {
        //             Wedding currentWedding = db.Weddings.Include(w => w.GuestReponses).ThenInclude(u => u.Guest).FirstOrDefault(cw => cw.WeddingId == WeddingID);

        //             newGuestResponse.WeddingId = currentWedding.WeddingId; 
        //             newGuestResponse.UserId =(int) HttpContext.Session.GetInt32("UserId");
        //             db.Add(newGuestResponse);
        //             db.SaveChanges();
        //             return RedirectToAction("Dashboard");
        //         }
        //     }
        //     return View("Index");
        // }

        // [HttpPost("/Delete/{WeddingID}")]
        // public IActionResult Delete(int WeddingID)
        // {
        //     int? userId = HttpContext.Session.GetInt32("UserId");
        //     if (userId != null)
        //     {
        //         Wedding getWedding = db.Weddings.FirstOrDefault(w => w.WeddingId == WeddingID);
        //         db.Weddings.Remove(getWedding);
        //         db.SaveChanges();
        //         return RedirectToAction("Dashboard");
        //     }
        //     return View("Index");
        // }

        // [HttpGet("/View/{WeddingId}")]
        // public IActionResult View(int WeddingId)
        // {
        //     int? userId = HttpContext.Session.GetInt32("UserId");
        //     if (userId != null)
        //     {
        //         ViewBag.WeddingDetail = db.Weddings
        //             .Include(gr => gr.GuestReponses)
        //                 .ThenInclude(g => g.Guest)
        //                 .FirstOrDefault(p => p.WeddingId == WeddingId);
        //         return View("Display");
        //     }
        //     return View("Index");
        // }

        // [HttpPost("logout")]
        // public IActionResult LogOut()
        // {
        //     HttpContext.Session.Clear();
        //     return View("Index");
        // }

        // [HttpPost("Create")]
        // public IActionResult Create(Wedding newWedding)
        // {
        //     if(ModelState.IsValid)
        //     {
        //         newWedding.PlannerId = (int)HttpContext.Session.GetInt32("UserId");
        //         db.Add(newWedding);
        //         db.SaveChanges();
        //         return RedirectToAction("Dashboard");
        //     }
        //     return View("Planner");
        // }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
