using System;
using System.Collections.Generic;

namespace SportsModel
{
    /* Use SportsModelRoot for JSON data */
    public class SportsModelRoot
    {
        public SportsModel SportsModel { get; set; }
    }
    public class SportsModel
    {
        public List<Team> Teams { get; set; }
    }
    public partial class Team
    {
        public static string MyNameIs => "Jonathan Fairhurst";     //--< Put Your Name Here <<<
        public Team()
        {
            this.Rosters = new HashSet<Roster>();
        }

        public int TeamId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual ICollection<Roster> Rosters { get; set; }
    }
    public partial class Roster
    {
        public int RosterId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }
        public virtual Team Team { get; set; }
    }
    public partial class Player
    {
        public Player()
        {
            this.Rosters = new HashSet<Roster>();
        }

        public int PlayerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsActivePlayer { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NickName { get; set; }
        public bool IsOptInEmail { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public virtual ICollection<Roster> Rosters { get; set; }
    }
}