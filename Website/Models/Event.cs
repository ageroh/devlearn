using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
    }
}
