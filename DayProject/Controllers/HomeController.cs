using DayProject.DAL;
using DayProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DayProject.Controllers
{
    public class HomeController : Controller
    {
        readonly Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVm homeVm = new HomeVm
            {
                Portfolios = await _context.Portfolios.Include(x => x.Category).ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };
            return View(homeVm);
        }
    }
}
