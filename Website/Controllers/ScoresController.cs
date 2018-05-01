using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class ScoresController : Controller
    {
        public IActionResult Index(int lastKnownScoreId = 0)
        {
            return null;
        }

    }
}
