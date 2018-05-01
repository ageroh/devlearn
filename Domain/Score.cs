using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Domain
{
    public class Score
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("home")]
        public int Home { get; set; }

        [JsonProperty("away")]
        public int Away { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonProperty("created")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("eventid")]
        public int EventId { get; set; }

        [JsonProperty("event")]
        public Event Event { get; set; }
    }

}
