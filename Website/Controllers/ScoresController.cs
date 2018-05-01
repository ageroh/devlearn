using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Website.Controllers
{
    public class ScoresController : Controller
    {
        private readonly DatabaseContext _context;

        public ScoresController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int lastKnownScoreId = 0)
        {
            var scores = await _context.Score
                .Include(s => s.Event)
                .Where(s => s.Id > lastKnownScoreId)
                .GroupBy(s => s.EventId)
                .Select(se => se.OrderByDescending(s => s.Id).FirstOrDefault())
                .ToListAsync();

            return Json(scores);
        }

    }
}
