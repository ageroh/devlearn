using System;
using System.Collections.Generic;
using System.Text;

namespace StateServer
{
    public class Score
    {
        public int EventId { get; set; }
        public int Home { get; set; }
        public int Away { get; set; }
        public Event Event { get; set; }
    }
}
