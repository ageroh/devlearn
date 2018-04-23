using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Website.Models
{
    public class Score
    {
        public List<SelectListItem> Events { get; set; }
        public int EventId { get; set; }
        public int Home { get; set; }
        public int Away { get; set; }

        public Score()
        {
            Events = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Paok - Aek", Value = "1"
                },
                new SelectListItem
                {
                    Text = "P.S.G. - Barcelona", Value = "2"
                },
                new SelectListItem
                {
                    Text = "Seville - Olympiacos", Value = "3"
                },
            };
        }
    }
}
