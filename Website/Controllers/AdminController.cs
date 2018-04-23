using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddScore(Score score)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            return Created("", new object());
        }

        [HttpPost]
        public IActionResult AddEvent(Event eventDetail)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            return Created("", new object());
        }
    }
}
