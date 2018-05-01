using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class EventViewModel
    {
        [Key]
        public int EventId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
    }
}
