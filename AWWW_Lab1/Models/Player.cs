﻿using AWWW_Lab1.Interfaces;

namespace AWWW_Lab1.Models
{
    public class Player: IModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTime BirthDate { get; set; }
        public Team? Team { get; set; }
        public List<Position> Positions { get; set; }  
        public List<MatchPlayer> MatchPlayers { get; set; }

        public Player() { }
        public Player(PlayerVM playervm) {
            Id = playervm.Id;
            FirstName = playervm.FirstName;
            LastName = playervm.LastName;
            Country = playervm.Country;
            BirthDate = playervm.BirthDate;
        }
    }
}
