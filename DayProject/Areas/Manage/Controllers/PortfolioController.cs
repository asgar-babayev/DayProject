using DayProject.DAL;
using DayProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DayProject.Areas.Manage.Controllers
{
    [Area("Manage"), Authorize]
    public class PortfolioController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Portfolios.Include(x => x.Category).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public IActionResult Create(Portfolio portfolio)
        {
            if (!ModelState.IsValid) return View(portfolio);
            if (portfolio.ImageFile != null)
            {
                if (portfolio.ImageFile.ContentType != "image/jpeg" && portfolio.ImageFile.ContentType != "image/png" && portfolio.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Image can be only .jpeg or .png");
                    return View(portfolio);
                }
                if (portfolio.ImageFile.Length / 1024 > 2000)
                {
                    ModelState.AddModelError("ImageFile", "Image size must be lower than 2mb");
                    return View(portfolio);
                }

                string filename = portfolio.ImageFile.FileName;
                if (filename.Length > 64)
                {
                    filename.Substring(filename.Length - 64, 64);
                }
                string newFileName = Guid.NewGuid().ToString() + filename;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", newFileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    portfolio.ImageFile.CopyTo(stream);
                }
                portfolio.Image = newFileName;
                _context.Portfolios.Add(portfolio);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Portfolio portfolio = _context.Portfolios.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            if (portfolio == null) return NotFound();
            ViewBag.Categories = _context.Categories.ToList();
            return View(portfolio);
        }

        [HttpPost]
        public IActionResult Edit(Portfolio portfolio)
        {
            var existPortfolio = _context.Portfolios.Include(x => x.Category).FirstOrDefault(x => x.Id == portfolio.Id);
            if (existPortfolio == null) return NotFound();
            string newFileName = null;

            if (portfolio.ImageFile != null)
            {
                if (portfolio.ImageFile.ContentType != "image/jpeg" && portfolio.ImageFile.ContentType != "image/png" && portfolio.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Image can be only .jpeg or .png");
                    return View();
                }
                if (portfolio.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Image size must be lower than 2mb");
                    return View();
                }
                string fileName = portfolio.ImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                newFileName = Guid.NewGuid().ToString() + fileName;

                string path = Path.Combine(_env.WebRootPath, "assets", "images", newFileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    portfolio.ImageFile.CopyTo(stream);
                }
            }
            if (newFileName != null)
            {
                string deletePath = Path.Combine(_env.WebRootPath, "assets", "images", existPortfolio.Image);

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existPortfolio.Image = newFileName;
            }

            existPortfolio.CategoryId = portfolio.CategoryId;
            existPortfolio.Title = portfolio.Title;
            existPortfolio.Description = portfolio.Description;
            existPortfolio.Url = portfolio.Url;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            var portfolio = _context.Portfolios.FirstOrDefault(x => x.Id == id);
            _context.Portfolios.Remove(portfolio);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
