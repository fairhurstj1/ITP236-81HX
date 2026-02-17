using System;
using System.Collections.Generic;

namespace MusicCatalog
{
    public class App
    {
        // engine for main method to run the music catalog application
        public static void Menu()
        {
            string input;
            
            string albumTitle = string.Empty;
            int releaseYear = 0;
            
            string albumId = string.Empty;
            Artist? artist = null;
            Album? album = null;


            //introduce application
            Console.WriteLine("Welcome to the Music Catalog!");
            
            //prompt for artist info
            Console.WriteLine("Would you like to add an artist? (y/n)");
            input = Console.ReadLine()?.Trim().ToLower() ?? "n";
            
            //if user wants to add artist, gather info and create artist object
            if (input == "y")
            {
                artist = CreateNewArtist();
            }
            else
            {
                Console.WriteLine("No artist added. Exiting.");
            }

            //prompt for album info if artist was added
            if (artist != null)
            {
                Console.WriteLine("Would you like to add an album for this artist? (y/n)");
                input = Console.ReadLine()?.Trim().ToLower() ?? "n";
                if (input == "y")
                {
                    Console.WriteLine("Enter album title:");
                    albumTitle = Console.ReadLine()?.Trim() ?? "Unknown Album";
                    Console.WriteLine("Enter album release year:");
                    releaseYear = int.TryParse(Console.ReadLine(), out int year) ? year : 0;
                    albumId = Guid.NewGuid().ToString();
                    album = new Album(albumId, albumTitle, releaseYear, artist);
                    Console.WriteLine($"Album '{album.GetSummary()}' added with ID: {album.Id}");
                }
                else
                {
                    Console.WriteLine("No album added. Exiting.");
                }
            }

            //prompt for song info if album was added
            if (album != null)
            {
                Console.WriteLine("Would you like to add a song for this album? (y/n)");
                input = Console.ReadLine()?.Trim().ToLower() ?? "n";
                if (input == "y")
                {
                    Console.WriteLine("Enter song title:");
                    string songTitle = Console.ReadLine()?.Trim() ?? "Unknown Song";
                    Console.WriteLine("Enter track number:");
                    int trackNumber = int.TryParse(Console.ReadLine(), out int track) ? track : 0;
                    Console.WriteLine("Enter song duration in seconds:");
                    int durationSeconds = int.TryParse(Console.ReadLine(), out int seconds) ? seconds : 0;
                    TimeSpan duration = TimeSpan.FromSeconds(durationSeconds);
                    Song song = new(Guid.NewGuid().ToString(), songTitle, trackNumber, duration, album);
                    
                    Console.WriteLine($"Song '{song.GetLabel()}' added with ID: {song.Id}");
                }
                else
                {
                    Console.WriteLine("No song added. Exiting.");
                }
            }
        }

        public static Artist CreateNewArtist()
        {
            string artistName = string.Empty;
            string artistCountry = string.Empty;
            string artistId = string.Empty;
            Console.WriteLine("Enter artist name:");
                artistName = Console.ReadLine()?.Trim() ?? "Unknown Artist";
                Console.WriteLine("Enter artist country (optional):");
                artistCountry = Console.ReadLine()?.Trim() ?? string.Empty;
                artistId = Guid.NewGuid().ToString();
                Artist artist = new(artistId, artistName, string.IsNullOrWhiteSpace(artistCountry) ? null : artistCountry);
                Console.WriteLine($"Artist '{artist.GetDisplayName()}' added with ID: {artist.Id}");
            return artist;
        }


    }
}