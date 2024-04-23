namespace Songs
{
  class Song
  {
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public int ReleaseYear { get; set; }
  }

  class Songs
  {
    // Compare songs based on artist
    static int CompareByArtist(Song a, Song b)
    {
      return string.Compare(a.Artist, b.Artist);
    }

    // Compare songs based on album
    static int CompareByAlbum(Song a, Song b)
    {
      return string.Compare(a.Album, b.Album);
    }

    // Compare songs based on release date
    static int CompareByReleaseDate(Song a, Song b)
    {
      return a.ReleaseYear - b.ReleaseYear;
    }

    static void InsertionSort(Song[] arr, int n, Func<Song, Song, int> compare)
    {
      for (int i = 1; i < n; i++)
      {
        Song key = arr[i];
        int j = i - 1;

        // Move elements of arr[0..i-1], that are greater than key, 
        // to one position ahead of their current position
        while (j >= 0 && compare(arr[j], key) > 0)
        {
          arr[j + 1] = arr[j];
          j = j - 1;
        }
        arr[j + 1] = key;
      }
    }

    static void Main(string[] args)
    {
      Song[] songs = {
            new Song { Title = "Song1", Artist = "Artist2", 
              Album = "Album1", ReleaseYear = 2010 },
            new Song { Title = "Song2", Artist = "Artist1", 
              Album = "Album2", ReleaseYear = 2005 },
            new Song { Title = "Song3", Artist = "Artist3", 
              Album = "Album1", ReleaseYear = 2015 },
            new Song { Title = "Song4", Artist = "Artist4", 
              Album = "Album3", ReleaseYear = 2008 },
            new Song { Title = "Song5", Artist = "Artist1", 
              Album = "Album2", ReleaseYear = 2003 },
            new Song { Title = "Song6", Artist = "Artist3", 
              Album = "Album4", ReleaseYear = 2019 },
            new Song { Title = "Song7", Artist = "Artist2", 
              Album = "Album3", ReleaseYear = 2012 },
            new Song { Title = "Song8", Artist = "Artist4", 
              Album = "Album4", ReleaseYear = 2017 },
            new Song { Title = "Song9", Artist = "Artist5", 
              Album = "Album5", ReleaseYear = 2014 },
            new Song { Title = "Song10", Artist = "Artist5", 
              Album = "Album5", ReleaseYear = 2011 }
        };

      int numSongs = songs.Length;

      // Sort by artist
      InsertionSort(songs, numSongs, CompareByArtist);
      Console.WriteLine("Sorted by Artist:");
      foreach (Song song in songs)
      {
        Console.WriteLine(song.Title + " from " + song.Artist);
      }
      Console.WriteLine();

      // Sort by album
      InsertionSort(songs, numSongs, CompareByAlbum);
      Console.WriteLine("Sorted by Album:");
      foreach (Song song in songs)
      {
        Console.WriteLine(song.Title + " from " + song.Album);
      }
      Console.WriteLine();

      // Sort by release date
      InsertionSort(songs, numSongs, CompareByReleaseDate);
      Console.WriteLine("Sorted by Release Date:");
      foreach (Song song in songs)
      {
        Console.WriteLine(song.Title + " released in " + song.ReleaseYear);
      }
    }
  }
}
