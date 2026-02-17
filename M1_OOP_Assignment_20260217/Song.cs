using System;

class Song
{
	public string Id { get; }
	public string Title { get; set; }
	public int TrackNumber { get; set; }
	public TimeSpan Duration { get; set; }
	public Album Album { get; }

	public Song(string id, string title, int trackNumber, TimeSpan duration, Album album)
	{
		Id = id;
		Title = title;
		TrackNumber = trackNumber;
		Duration = duration;
		Album = album;
		album.AddSong(this);
	}

	public string GetLabel()
	{
		return $"{TrackNumber}. {Title}";
	}

	public string GetFullMetadata()
	{
		return $"{Title} - {Album.Artist.Name} ({Album.Title}, {Album.ReleaseYear})";
	}

	public override string ToString()
	{
		return GetLabel();
	}
}
