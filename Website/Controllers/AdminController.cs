using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Models;
using Website.Services;

namespace Website.Controllers
{
    public class AdminController : Controller
    {
        private SbContext _context;
        public AdminController(SbContext context)
        {
            _context = context;
        }

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
        public async Task<IActionResult> AddEvent(Event eventDetail)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            try
            {
                _context.Add(eventDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventDetail.EventId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
    }
}
