using System;
using System.Collections.Generic;

class Album
{
	public string Id { get; }
	public string Title { get; set; }
	public int ReleaseYear { get; set; }
	public Artist Artist { get; }
	public List<Song> Songs { get; } = new();

	public Album(string id, string title, int releaseYear, Artist artist)
	{
		Id = id;
		Title = title;
		ReleaseYear = releaseYear;
		Artist = artist;
		artist.AddAlbum(this);
	}

	public void AddSong(Song song)
	{
		if (!Songs.Contains(song))
		{
			Songs.Add(song);
		}
	}

	public int GetTrackCount()
	{
		return Songs.Count;
	}

	public TimeSpan GetTotalDuration()
	{
		TimeSpan total = TimeSpan.Zero;
		foreach (var song in Songs)
		{
			total = total.Add(song.Duration);
		}
		return total;
	}

	public string GetSummary()
	{
		return $"{Title} ({ReleaseYear}) - {Artist.Name}";
	}

	public override string ToString()
	{
		return GetSummary();
	}
}
