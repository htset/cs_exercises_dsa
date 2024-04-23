using fs = System.IO;

namespace FileIndexer
{
  class FileIndexer
  {
    private class Node
    {
      public string fileName;
      public string filePath;
      public Node? left;
      public Node? right;

      public Node(string name, string path)
      {
        fileName = name;
        filePath = path;
        left = null;
        right = null;
      }
    }

    private Node? root;

    // Insert node to tree
    private void InsertNode(string fileName, string filePath)
    {
      // If the tree is empty, insert node here
      if (root == null)
      {
        root = new Node(fileName, filePath);
        return;
      }

      // If not empty, then go down the tree
      Node current = root;
      while (true)
      {
        if (fileName.CompareTo(current.fileName) < 0)
        {
          if (current.left == null)
          {
            current.left = new Node(fileName, filePath);
            return;
          }
          current = current.left;
        }
        else
        {
          if (current.right == null)
          {
            current.right = new Node(fileName, filePath);
            return;
          }
          current = current.right;
        }
      }
    }

    // Index the specified directory
    private void IndexDirectoryHelper(string dirPath)
    {
      //if it's not a directory, return
      if (!fs.Directory.Exists(dirPath))
        return;

      //loop over files within directory
      foreach (string filePath in Directory.GetFiles(dirPath))
      {
        string fileName = Path.GetFileName(filePath);
        InsertNode(fileName, filePath);
      }

      //loop over directories within directory
      foreach (string subDirPath in Directory.GetDirectories(dirPath))
      {
        //recursive indexing
        IndexDirectoryHelper(subDirPath);
      }
    }

    // Deallocate memory recursively
    private void DeleteSubtree(Node? root)
    {
      if (root != null)
      {
        DeleteSubtree(root.left);
        DeleteSubtree(root.right);
        root = null;
      }
    }

    private void Traverse(Node? root)
    {
      if (root != null)
      {
        Traverse(root.left);
        Console.WriteLine(root.fileName + ": " + root.filePath);
        Traverse(root.right);
      }
    }

    public void IndexDirectory(string? directoryPath)
    {
      root = null;
      IndexDirectoryHelper(directoryPath);
    }

    public void PrintFiles()
    {
      Console.WriteLine("Indexed files:");
      Traverse(root);
    }

    // Search for a file in the BST
    public string SearchFileLocation(string? filename)
    {
      // Traverse the tree until a match is found or the tree is exhausted
      Node? current = root;
      while (current != null)
      {
        if (filename == current.fileName)
        {
          return current.filePath; // File found
        }
        else if (filename.CompareTo(current.fileName) < 0)
        {
          current = current.left; // Search in the left subtree
        }
        else
        {
          current = current.right; // Search in the right subtree
        }
      }
      return ""; // File not found
    }

  }

  class Program
  {
    static void Main(string[] args)
    {
      Console.Write("Path to index recursively: ");
      string path = Console.ReadLine();

      FileIndexer indexer = new FileIndexer();
      indexer.IndexDirectory(path);
      indexer.PrintFiles();

      Console.Write("Let's search for a file's location. Give the file name: ");
      string filenameToSearch = Console.ReadLine();

      string location = indexer.SearchFileLocation(filenameToSearch);
      if (!string.IsNullOrEmpty(location))
      {
        Console.WriteLine($"File {filenameToSearch} found. Location: {location}");
      }
      else
      {
        Console.WriteLine($"File {filenameToSearch} not found.");
      }
    }
  }
}
