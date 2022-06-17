using DayProject.DAL;
using DayProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayProject.Areas.Manage.Controllers
{
    [Area("Manage"),Authorize]
    public class CategoryController : Controller
    {

        readonly Context _context;

        public CategoryController(Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            if (_context.Categories.Any(x => x.Name.Trim().ToLower() == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("", "This category already have");
                return View(category);
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            return View(category);
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public IActionResult Edit(Category category)
        {
            var existbrand = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (existbrand == null) ModelState.AddModelError("Error", "Category can't empty");
            if (_context.Categories.Any(x => x.Name.Trim().ToLower() == category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("", "This category already have");
                return View(category);
            }
            existbrand.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var brand = _context.Categories.FirstOrDefault(x => x.Id == id);
            _context.Categories.Remove(brand);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
