using IdentitySample.Models;
using IdentitySample.Models.DTOs;
using IdentitySample.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentitySample.Controllers
{
    public class BlogController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly UserManager<User> _userManager;

        public BlogController(DataBaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var blogs = _context.Blogs.Include(p => p.User).Select(p => new BlogDTO
            {
                Id = p.Id,
                Body = p.Body,
                Title = p.Title,
                UserName = p.User.UserName,
            });
            return View(blogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BlogDTO dto)
        {
            var user = _userManager.GetUserAsync(User).Result;

            Blog newBlog = new Blog
            {
                Body = dto.Body,
                Title = dto.Title,
                User = user
            };

            _context.Add(newBlog);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(long Id)
        {
            var blog = _context.Blogs.Include(p => p.User).Where(p => p.Id == Id).Select(p => new BlogDTO
                        {
                            Body = p.Body,
                            Id = p.Id,
                            Title = p.Title,
                            UserId = p.UserId,
                            UserName = p.User.UserName
                        }).FirstOrDefault();

            return View(blog);
        }

        [HttpPost]
        public IActionResult Edit(BlogDTO dto)
        {
            var user = _userManager.GetUserAsync(User).Result;

            Blog newBlog = new Blog
            {
                Body = dto.Body,
                Title = dto.Title,
                User = user
            };

            _context.Add(newBlog);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
