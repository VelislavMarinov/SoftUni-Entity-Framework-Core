﻿
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting.Data.Models
{
    public class Position
    {

        public int PositionId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }


        public ICollection<Player> Players { get; set; }
    }
}