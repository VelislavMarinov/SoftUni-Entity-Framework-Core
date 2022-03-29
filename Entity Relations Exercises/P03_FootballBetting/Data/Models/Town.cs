using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting.Data.Models
{
    public class Town
    {
        public int TownId { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public int CountryId { get; set; }

        public ICollection<Team> Teams { get; set; }

        public Country Country { get; set; }
    }
}