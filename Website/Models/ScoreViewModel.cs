﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Models
{
    public class ScoreViewModel
    {
        [NotMapped]
        public List<SelectListItem> Events { get; set; }

        [Key]
        public int EventId { get; set; }

        public int Home { get; set; }
        public int Away { get; set; }

        public ScoreViewModel()
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
