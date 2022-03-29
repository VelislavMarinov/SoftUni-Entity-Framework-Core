using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class Color
    {

        public int ColorId { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        
        public ICollection<Team> PrimaryKitTeams { get; set; }
        
        public ICollection<Team> SecondaryKitTeams { get; set; }
    }
}