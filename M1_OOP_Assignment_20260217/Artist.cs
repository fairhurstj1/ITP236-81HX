using System;
using System.Collections.Generic;
namespace MusicCatalog
{
    public class Artist
    {
        public string Id { get; }
        public string Name { get; set; }
        public string? Country { get; set; }
        public List<Album> Albums { get; } = new();

        public Artist(string id, string name, string? country = null)
        {
            Id = id;
            Name = name;
            Country = country;
        }

        public void AddAlbum(Album album)
        {
            if (!Albums.Contains(album))
            {
                Albums.Add(album);
            }
        }

        public int GetAlbumCount()
        {
            return Albums.Count;
        }

        public string GetDisplayName()
        {
            return string.IsNullOrWhiteSpace(Country) ? Name : $"{Name} ({Country})";
        }

        public override string ToString()
        {
            return GetDisplayName();
        }
    }
}
