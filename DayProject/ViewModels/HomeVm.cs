using DayProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayProject.ViewModels
{
    public class HomeVm
    {
        public List<Portfolio> Portfolios { get; set; }
        public List<Category> Categories { get; set; }
    }
}
