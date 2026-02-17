using System;
using System.Collections.Generic;
namespace MusicCatalog
{
    public abstract class CatalogItem
    {
        private string id = string.Empty;
        private string title = string.Empty;
        private int releaseYear;
        public string Id { get=>id; set=>id = value; }
        public string Title { get=>title; set=>title = value; }
        public int ReleaseYear { get=>releaseYear; set=>releaseYear = value; }
        protected CatalogItem(string id, string title, int releaseYear)
        {
            Id = id;
            Title = title;
            ReleaseYear = releaseYear;
        }
    }
}