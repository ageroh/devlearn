using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Domain
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("eventid")]
        public int EventId { get; set; }

        [JsonProperty("hometeam")]
        public string HomeTeam { get; set; }

        [JsonProperty("awayteam")]
        public string AwayTeam { get; set; }

        [JsonProperty("eventname")]
        public string EventName => $"{HomeTeam} vs {AwayTeam}";

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonProperty("created")]
        public DateTime DateCreated { get; set; }

        [JsonIgnore]
        public ICollection<Score> Scores { get; set; }
    }


}