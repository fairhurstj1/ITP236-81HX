//create three related classes for a Library system: LibraryItem (abstract), Book, CD, and DVD. 
//Each item should have a unique ID, title, and publication year.
using System;
using System.Collections.Generic;
namespace LibrarySystem
{
    /// <summary>
    /// Represents a generic library item with shared metadata.
    /// </summary>
    public abstract class LibraryItem
    {
        private string id = string.Empty;
        private string title = string.Empty;
        private int publicationYear;
        /// <summary>
        /// Gets or sets the unique identifier for the item.
        /// </summary>
        public string Id { get=>id; set=>id = value; }
        /// <summary>
        /// Gets or sets the title of the item.
        /// </summary>
        public string Title { get=>title; set=>title = value; }
        /// <summary>
        /// Gets or sets the year the item was published.
        /// </summary>
        public int PublicationYear { get=>publicationYear; set=>publicationYear = value; }
        /// <summary>
        /// Initializes a new library item.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="title">Title of the item.</param>
        /// <param name="publicationYear">Publication year.</param>
        protected LibraryItem(string id, string title, int publicationYear)
        {
            Id = id;
            Title = title;
            PublicationYear = publicationYear;
        }//LibraryItem constructor
    }//LibraryItem

    /// <summary>
    /// Represents a book in the library.
    /// </summary>
    public class Book : LibraryItem
    {
        private string author = string.Empty;
        private int pages = 0;
        /// <summary>
        /// Gets or sets the author name.
        /// </summary>
        public string Author { get=>author; set=>author = value; }
        /// <summary>
        /// Gets or sets the number of pages.
        /// </summary>
        public int Pages { get=>pages; set=>pages = value; }
        /// <summary>
        /// Initializes a new book.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="title">Book title.</param>
        /// <param name="publicationYear">Publication year.</param>
        /// <param name="author">Author name.</param>
        /// <param name="pages">Page count.</param>
        public Book(string id, string title, int publicationYear, string author, int pages) : base(id, title, publicationYear)
        {
            Author = author;
            Pages = pages;
        }//Book constructor
    }//Book

    /// <summary>
    /// Represents a music CD in the library.
    /// </summary>
    public class CD : LibraryItem
    {
        private string artist = string.Empty;
        private int tracks = 0;
        /// <summary>
        /// Gets or sets the artist name.
        /// </summary>
        public string Artist { get=>artist; set=>artist = value; }
        /// <summary>
        /// Gets or sets the number of tracks.
        /// </summary>
        public int Tracks { get=>tracks; set=>tracks = value; }
        /// <summary>
        /// Initializes a new CD.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="title">CD title.</param>
        /// <param name="publicationYear">Publication year.</param>
        /// <param name="artist">Artist name.</param>
        /// <param name="tracks">Track count.</param>
        public CD(string id, string title, int publicationYear, string artist, int tracks) : base(id, title, publicationYear)
        {
            Artist = artist;
            Tracks = tracks;
        }//CD constructor
    }//CD

    /// <summary>
    /// Represents a DVD in the library.
    /// </summary>
    public class DVD : LibraryItem
    {
        private string director = string.Empty;
        private int duration = 0;
        /// <summary>
        /// Gets or sets the director name.
        /// </summary>
        public string Director { get=>director; set=>director = value; }
        /// <summary>
        /// Gets or sets the duration in minutes.
        /// </summary>
        public int Duration { get=>duration; set=>duration = value; }
        /// <summary>
        /// Initializes a new DVD.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="title">DVD title.</param>
        /// <param name="publicationYear">Publication year.</param>
        /// <param name="director">Director name.</param>
        /// <param name="duration">Duration in minutes.</param>
        public DVD(string id, string title, int publicationYear, string director, int duration) : base(id, title, publicationYear)
        {
            Director = director;
            Duration = duration;
        }//DVD constructor
    }//DVD

    /// <summary>
    /// Application entry point and menu workflow.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            Library library = new Library();
            App(library);
            DisplayLibraryItems(library);
            Console.WriteLine("\nThank you for using the Library System! Press any key to exit.");
            Console.ReadKey();
        }//Main

        //create methods that demonstrate the created classes and their relationships
        /// <summary>
        /// Displays the main menu and routes user actions.
        /// </summary>
        /// <param name="library">Library instance to modify.</param>
        public static void App(Library library)
        {
            Console.WriteLine("Welcome to the Library System!");
            Console.WriteLine("1. Add new item");
            Console.WriteLine("2. Exit");
            string input = Console.ReadLine()?.Trim() ?? "2";
            switch (input)
            {
                case "1":
                    AddNewItems(library);
                    break;
                case "2":
                    Console.WriteLine("Exiting the system. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Exiting.");
                    break;
            }//switch
        }//App
        //moved all variable declarations to the top of the method to avoid scope issues and ensure they are accessible throughout the method
        /// <summary>
        /// Prompts the user for item details and adds them to the library.
        /// </summary>
        /// <param name="library">Library instance to modify.</param>
        public static void AddNewItems(Library library)
        {
            //function control, menu selection
            Boolean loop = true;
            int itemType = 0;
            //common variables
            string title = string.Empty;
            int publicationYear = 0;
            string id = string.Empty;
            //book-specific
            string author = string.Empty;
            int pages = 0;
            //CD-specific
            string artist = string.Empty;
            int tracks = 0;
            //DVD-specific
            string director = string.Empty;
            int duration = 0;

            //main loop to allow multiple item entries until user chooses to exit
            while (loop)
            {
                //prompt for item type and common details
                Console.WriteLine("\nEnter item type:\n1. Book\n2. CD\n3. DVD\n\nEnter the number corresponding to the item type:");
                itemType = int.TryParse(Console.ReadLine(), out int type) ? type : 0;
                //prompt for common details
                Console.WriteLine("\nEnter title:");
                title = Console.ReadLine()?.Trim() ?? "Unknown Title";
                //prompt for publication year and validate input
                Console.WriteLine("\nEnter publication year:");
                publicationYear = int.TryParse(Console.ReadLine(), out int year) ? year : 0;
                //generate unique ID for the item
                id = Guid.NewGuid().ToString();

                //prompt for specific details based on item type and create the appropriate object, then add it to the library
                switch (itemType)
                {
                    case 1:
                        //prompt for book-specific details
                        Console.WriteLine("\nEnter author:");
                        author = Console.ReadLine()?.Trim() ?? "Unknown Author";

                        Console.WriteLine("\nEnter number of pages:");
                        pages = int.TryParse(Console.ReadLine(), out int numPages) ? numPages : 0;

                        //create book object and add to library
                        Book book = new Book(id, title, publicationYear, author, pages);
                        Library.AddItem(book);

                        //confirm addition to user
                        Console.WriteLine($"\nAdded book: {book.Title} by {book.Author}");
                        break;

                    case 2:
                        //prompt for CD-specific details
                        Console.WriteLine("\nEnter artist:");
                        artist = Console.ReadLine()?.Trim() ?? "Unknown Artist";

                        Console.WriteLine("\nEnter number of tracks:");
                        tracks = int.TryParse(Console.ReadLine(), out int numTracks) ? numTracks : 0;

                        //create CD object and add to library
                        CD cd = new CD(id, title, publicationYear, artist, tracks);
                        Library.AddItem(cd);

                        //confirm addition to user
                        Console.WriteLine($"\nAdded CD: {cd.Title} by {cd.Artist}");
                        break;

                    case 3:
                        //prompt for DVD-specific details
                        Console.WriteLine("\nEnter director:");
                        director = Console.ReadLine()?.Trim() ?? "Unknown Director";
                        
                        Console.WriteLine("\nEnter duration in minutes:");
                        duration = int.TryParse(Console.ReadLine(), out int numDuration) ? numDuration : 0;
                        
                        //create DVD object and add to library
                        DVD dvd = new DVD(id, title, publicationYear, director, duration);
                        Library.AddItem(dvd);

                        //confirm addition to user
                        Console.WriteLine($"\nAdded DVD: {dvd.Title} directed by {dvd.Director}");
                        break;

                    default:
                        //handle invalid item type input
                        Console.WriteLine("Invalid item type. No item added.");
                        break;
                }//switch

                //prompt user to add another item or exit the loop
                Console.WriteLine("\nWould you like to add another item? (y/n)");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "n";

                if (input != "y")
                {
                    loop = false;
                }//if
            }//while
        }//AddNewItems

        //self-created method to display all items in the library in a formatted table structure
        /// <summary>
        /// Displays all items currently stored in the library.
        /// </summary>
        /// <param name="library">Library instance to read.</param>
        public static void DisplayLibraryItems(Library library)
        {
            //header for the display table
            Console.WriteLine("\nLibrary Items:\n");
            Console.WriteLine("{0,-40} {1,-30} {2,-20} {3,-10} {4,-30} {5}", "Title", "Author/Artist/Director", "Publication Year", "Type", "Pages/Tracks/Duration", "ID");

            //iterate through library items and display details based on their type
            foreach (var item in library.Items)
            {
                if (item is Book book)
                {
                    Console.WriteLine("{0,-40} {1,-30} {2,-20} {3,-10} {4,-30} {5}", book.Title, book.Author, book.PublicationYear, "Book", $"{book.Pages} pages", book.Id);
                }//if
                else if (item is CD cd)
                {
                    Console.WriteLine("{0,-40} {1,-30} {2,-20} {3,-10} {4,-30} {5}", cd.Title, cd.Artist, cd.PublicationYear, "CD", $"{cd.Tracks} tracks", cd.Id);
                }//else if
                else if (item is DVD dvd)
                {
                    Console.WriteLine("{0,-40} {1,-30} {2,-20} {3,-10} {4,-30} {5,-10}", dvd.Title, dvd.Director, dvd.PublicationYear, "DVD", $"{dvd.Duration} minutes", dvd.Id);
                }//else if
            }//foreach
        }//DisplayLibraryItems
    }//Program

    //include a basic method for each class

    /// <summary>
    /// Stores and manages library items.
    /// </summary>
    public class Library
    {
        private static List<LibraryItem> items = new List<LibraryItem>();
        /// <summary>
        /// Gets or sets the collection of library items.
        /// </summary>
        public List<LibraryItem> Items { get=>items; set=>items = value; }
        /// <summary>
        /// Adds an item to the library collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public static void AddItem(LibraryItem item)
        {
            items.Add(item);
        }//AddItem
    }//Library
    


}//LibrarySystem