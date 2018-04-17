using System;

namespace StateServer
{
    public class Score
    {
        public int Id { get; set; }
        public string Event { get; set; }
        public int Home { get; set; }
        public int Away { get; set; }
        public DateTime Timestamp { get; set; }
    }
}