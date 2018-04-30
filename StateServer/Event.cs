using System;

namespace StateServer
{
    public class Event
    {
        public int EventId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public Score Score { get; set; }
    }


}