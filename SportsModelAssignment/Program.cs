using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using SportsModel;

namespace SportsModelAssignment
{
    public class Program
    {
        public static string MyNameIs => "Jonathan Fairhurst";
        static List<Team> teams = new List<Team>();
        static List<Roster> rosters = new List<Roster>();
        static List<Player> players = new List<Player>();
        static void Main(string[] args)
        {
            // My name
            Console.WriteLine($"My name is: {MyNameIs}");
            // Load data from JSON file
            LoadData();
            QueryTeamRosters();
        }

        // Load data from JSON file into the teams list
        static void LoadData()
        {
            // Determine the solution root directory by navigating up from the current base directory
            string solutionRoot = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..")
            );
            // Construct the full path to the JSON file
            string jsonPath = Path.Combine(solutionRoot, "SportsModel", "SportsModel.json");

            // Check if the JSON file exists before attempting to read it
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"JSON not found: {jsonPath}");
                return;
            }// if

            // Read the JSON file content into a string
            string jsonText = File.ReadAllText(jsonPath);
            
            // Configure JSON deserialization options, including a custom converter for boolean values
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new BoolIntJsonConverter());

            // Deserialize the JSON into the SportsModelRoot object
            SportsModelRoot? root = JsonSerializer.Deserialize<SportsModelRoot>(jsonText, options);
            
            // Check if deserialization was successful and if the Teams property is not null
            if (root?.SportsModel?.Teams == null)
            {
                Console.WriteLine("JSON loaded, but Teams is null.");
                return;
            }// if

            // Extract teams from the deserialized root object
            teams = root.SportsModel.Teams.ToList();

            // Extract all rosters from the teams
            rosters = teams.SelectMany(t => t.Rosters).ToList();

            // Extract unique players from the rosters
            players = rosters
                .Where(r => r.Player != null)
                .Select(r => r.Player!)
                .GroupBy(p => p.PlayerId)
                .Select(g => g.First())
                .ToList();

            // Rebuild navigation properties
            Dictionary<int, Player> playerById = players.ToDictionary(p => p.PlayerId);
            
            // Ensure each roster has its Team and Player references set correctly
            foreach (Team team in teams)
            {
                // Set the Team reference and TeamId for each roster, and link the Player reference
                foreach (Roster roster in team.Rosters)
                {
                    roster.Team = team;
                    roster.TeamId = team.TeamId;

                    Player player = playerById[roster.PlayerId];
                    roster.Player = player;
                    player.Rosters.Add(roster);
                }// foreach roster
            }// foreach team
        }// LoadData

        // Execute the sports query and display results
        static void QueryTeamRosters()
        {
            // Query to get each team and its players, ordered by team name and then by player name
            var teamWithPlayers = from t in teams
                                orderby t.Name
                                select new
                                {
                                    TeamName = t.Name,
                                    Players = (from r in t.Rosters
                                                let p = r.Player!
                                                orderby p.LastName, p.FirstName
                                                select p).ToList()
                                };// from team orderby team name select team and players

            // Display the team names and their players in the console
            foreach (var team in teamWithPlayers)
            {
                Console.WriteLine($"\nTeam: {team.TeamName}");

                // Display each player's full name under the team
                foreach (var p in team.Players)
                    Console.WriteLine($"  {p.FirstName} {p.LastName}");
            }// foreach team
        }// QueryTeamRosters

        // There was an issue with the JSON data where some boolean values were represented as 0/1 instead of true/false.
        // This custom converter handles both formats for boolean values during deserialization.
        private sealed class BoolIntJsonConverter : JsonConverter<bool>
        {
            // Override the Read method to handle different representations of boolean values in JSON
            public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Handle standard boolean values (true/false)
                if (reader.TokenType == JsonTokenType.True) return true;
                if (reader.TokenType == JsonTokenType.False) return false;

                // Handle numeric representations of boolean values (0 and 1)
                if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int number))
                    return number != 0;

                // Handle string representations of boolean values (e.g., "1", "0", "true", "false")
                if (reader.TokenType == JsonTokenType.String)
                {
                    string? value = reader.GetString();
                    if (value == "1" || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)) return true;
                    if (value == "0" || string.Equals(value, "false", StringComparison.OrdinalIgnoreCase)) return false;
                }// if

                // If the value cannot be interpreted as a boolean, throw an exception
                throw new JsonException("Expected boolean, 0/1, or true/false.");
            }// Read

            // Override the Write method to serialize boolean values as true/false in JSON
            public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
                => writer.WriteBooleanValue(value);
        }// BoolIntJsonConverter
    }// Program
}// namespace SportsModelAssignment