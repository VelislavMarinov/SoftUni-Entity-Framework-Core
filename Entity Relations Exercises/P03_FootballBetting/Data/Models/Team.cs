using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Team
    {
        public int TeamId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        
        public string LogoUrl { get; set; }

        public string Initials { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [ForeignKey("PrimaryKitTeams")]
        public int PrimaryKitColorId { get; set; }
        public Color PrimaryKitColor { get; set; }
        [ForeignKey("SecondaryKitTeams")]
        public int SecondaryKitColorId { get; set; }
        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        public ICollection<Game> HomeGames { get; set; }

        public ICollection<Game> AwayGames { get; set; }


        public ICollection<Player> Players { get; set; }

    }
}
