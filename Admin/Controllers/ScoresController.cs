using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Admin.Models;
using Domain;
using Admin.Services;

namespace Admin.Controllers
{
    public class ScoresController : Controller
    {
        private readonly DatabaseContext _context;

        public ScoresController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Scores
        public async Task<IActionResult> Index()
        {
            var adminContext = _context.Score.Include(s => s.Event);
            return View(await adminContext.ToListAsync());
        }

        // GET: Scores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _context.Score
                .Include(s => s.Event)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (score == null)
            {
                return NotFound();
            }

            return View(score);
        }

        // GET: Scores/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName");
            return View();
        }

        // POST: Scores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Home,Away,DateCreated,EventId")] Score score)
        {
            if (ModelState.IsValid)
            {
                score.DateCreated = DateTime.UtcNow;
                _context.Add(score);
                score.Event = _context.Event.FirstOrDefault(e=> e.EventId == score.EventId);
                var result = await _context.SaveChangesAsync();
                if (result > 0 && await RabbitmqProvider.Publish(score, Constants.ScoresQueue))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", score.EventId);
            return View(score);
        }

        //// GET: Scores/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var score = await _context.Score.SingleOrDefaultAsync(m => m.Id == id);
        //    if (score == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", score.EventId);
        //    return View(score);
        //}

        //// POST: Scores/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Home,Away,DateCreated,EventId")] Score score)
        //{
        //    if (id != score.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(score);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ScoreExists(score.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["EventId"] = new SelectList(_context.Event, "EventId", "EventName", score.EventId);
        //    return View(score);
        //}

        //// GET: Scores/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var score = await _context.Score
        //        .Include(s => s.Event)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (score == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(score);
        //}

        //// POST: Scores/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var score = await _context.Score.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Score.Remove(score);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ScoreExists(int id)
        {
            return _context.Score.Any(e => e.Id == id);
        }
    }
}
