namespace Library
{
  public struct Book
  {
    public string Title;
    public string Author;
    public int Available;
  }

  public struct LendingEvent
  {
    public string BookTitle;
    public string UserName;
    public DateTime LendingDate;
    public int Returned;
  }

  public class Library
  {
    private const string BooksFilename = "books.txt";
    private const string LendingFilename = "lending_events.txt";

    public void AddBook()
    {
      using (StreamWriter file = new StreamWriter(BooksFilename, true))
      {
        Book book;
        Console.Write("Book title: ");
        book.Title = Console.ReadLine();

        Console.Write("Author: ");
        book.Author = Console.ReadLine();

        book.Available = 1;

        file.WriteLine($"{book.Title}|{book.Author}|{book.Available}");
      }
      Console.WriteLine("Book added successfully.");
    }

    public void ListBooks()
    {
      if (!File.Exists(BooksFilename))
      {
        Console.WriteLine("No books entered so far");
        return;
      }

      Console.WriteLine("Books available in the library:");
      using (StreamReader file = new StreamReader(BooksFilename))
      {
        string line;
        while ((line = file.ReadLine()) != null)
        {
          string[] parts = line.Split('|');
          Console.WriteLine($"Title: {parts[0]}");
          Console.WriteLine($"Author: {parts[1]}");
          Console.WriteLine($"Available: {(parts[2] == "1" ? "True" : "False")}");
          Console.WriteLine("------------------------------");
        }
      }
    }

    public void LendBook()
    {
      if (!File.Exists(BooksFilename))
      {
        Console.WriteLine("No books entered so far");
        return;
      }

      string bookTitle, userName;
      Console.Write("Enter the title of the book to lend: ");
      bookTitle = Console.ReadLine();

      string[] lines = File.ReadAllLines(BooksFilename);
      bool bookFound = false;

      for (int i = 0; i < lines.Length; i++)
      {
        string[] parts = lines[i].Split('|');
        if (parts[0] == bookTitle && parts[2] == "1")
        {
          lines[i] = $"{parts[0]}|{parts[1]}|0";
          bookFound = true;

          Console.Write("Enter your name: ");
          userName = Console.ReadLine();

          using (StreamWriter lendingFile = new StreamWriter(LendingFilename, true))
          {
            lendingFile.WriteLine($"{bookTitle}|{userName}|{DateTime.Now}|0");
          }

          Console.WriteLine($"Book '{bookTitle}' has been lent to {userName}.");
          break;
        }
      }

      if (!bookFound)
      {
        Console.WriteLine($"Book '{bookTitle}' not found or not available.");
      }

      File.WriteAllLines(BooksFilename, lines);
    }

    public void ReturnBook()
    {
      if (!File.Exists(LendingFilename))
      {
        Console.WriteLine("No lending events entered so far");
        return;
      }

      string bookTitle;
      Console.Write("Enter the title of the book to return: ");
      bookTitle = Console.ReadLine();

      string[] booksLines = File.ReadAllLines(BooksFilename);
      bool bookFound = false;

      for (int i = 0; i < booksLines.Length; i++)
      {
        string[] parts = booksLines[i].Split('|');
        if (parts[0] == bookTitle && parts[2] == "0")
        {
          booksLines[i] = $"{parts[0]}|{parts[1]}|1";
          bookFound = true;

          string[] lendingLines = File.ReadAllLines(LendingFilename);

          for (int j = 0; j < lendingLines.Length; j++)
          {
            parts = lendingLines[j].Split('|');
            if (parts[0] == bookTitle && parts[3] == "0")
            {
              lendingLines[j] = $"{parts[0]}|{parts[1]}|{parts[2]}|1";
              Console.WriteLine($"Book '{bookTitle}' has been returned.");
              break;
            }
          }

          File.WriteAllLines(LendingFilename, lendingLines);
          break;
        }
      }

      if (!bookFound)
      {
        Console.WriteLine($"Book '{bookTitle}' not found or already returned.");
      }

      File.WriteAllLines(BooksFilename, booksLines);
    }

    public void ListLendingEvents()
    {
      if (!File.Exists(LendingFilename))
      {
        Console.WriteLine("No lending events entered so far");
        return;
      }

      Console.WriteLine("Lending events:");
      using (StreamReader file = new StreamReader(LendingFilename))
      {
        string line;
        while ((line = file.ReadLine()) != null)
        {
          string[] parts = line.Split('|');
          Console.WriteLine($"Book Title: {parts[0]}");
          Console.WriteLine($"User Name: {parts[1]}");
          Console.WriteLine($"Lending Date: {parts[2]}");
          Console.WriteLine($"Returned: {(parts[3] == "1" ? "True" : "False")}");
          Console.WriteLine("------------------------------");
        }
      }
    }

    static void Main(string[] args)
    {
      Library lib = new Library();
      int choice;
      do
      {
        Console.WriteLine("\n1. Add a book\n2. List all books\n" +
          "3. Lend a book\n4. Return a book\n5. List lending events\n0. Exit");
        Console.Write("Enter your choice: ");
        choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
          case 1:
            lib.AddBook();
            break;
          case 2:
            lib.ListBooks();
            break;
          case 3:
            lib.LendBook();
            break;
          case 4:
            lib.ReturnBook();
            break;
          case 5:
            lib.ListLendingEvents();
            break;
          case 0:
            Console.WriteLine("Exiting.");
            break;
          default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
        }

      } while (choice != 0);
    }
  }
}
