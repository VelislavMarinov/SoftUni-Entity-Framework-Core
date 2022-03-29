﻿using P03_FootballBetting.Data;
using System;

namespace P03_FootballBetting
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new FootballBettingContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
