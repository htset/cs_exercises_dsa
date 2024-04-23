using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebServerCache
{
  class LRUCache
  {
    const int CACHE_SIZE = 3;
    Node? head;
    Node? tail;
    int size;

    class Node
    {
      public string? url;
      public string? content;
      public Node? prev;
      public Node? next;
    }

    public LRUCache()
    {
      size = 0;
      head = null;
      tail = null;
    }

    // Get the content associated with a URL from the cache
    public string GetContent(string url)
    {
      Node? current = head;
      while (current != null)
      {
        if (current.url == url)
        {
          MoveToHead(current);
          Console.WriteLine("Got content from cache: " + current.content);
          return current.content;
        }
        current = current.next;
      }
      return "";
    }

    // Put a URL-content pair into the cache
    public void PutContent(string url, string content)
    {
      if (size == CACHE_SIZE)
      {
        DeleteNode(tail);
        size--;
      }
      Node newNode = CreateNode(url, content);
      InsertAtHead(newNode);
      size++;
    }

    // Create a new node
    Node CreateNode(string url, string content)
    {
      Node newNode = new Node();
      newNode.url = url;
      newNode.content = content;
      newNode.prev = null;
      newNode.next = null;
      Console.WriteLine("New node created: " + content);
      return newNode;
    }

    // Insert a new node at the head of the cache
    void InsertAtHead(Node node)
    {
      node.next = head;
      node.prev = null;
      if (head != null)
        head.prev = node;
      head = node;
      if (tail == null)
        tail = node;
      Console.WriteLine("Node inserted at head: " + node.content);
    }

    // Move a node to the head of the cache
    void MoveToHead(Node node)
    {
      if (node == head)
        return;
      if (node.prev != null)
        node.prev.next = node.next;
      if (node.next != null)
        node.next.prev = node.prev;
      node.prev = null;
      node.next = head;
      if (head != null)
        head.prev = node;
      head = node;
      if (tail == null)
        tail = node;
      Console.WriteLine("Node moved to head: " + node.content);
    }

    // Delete a node from the cache
    void DeleteNode(Node? node)
    {
      if (node == null)
        return;
      if (node == head)
        head = node.next;
      if (node == tail)
        tail = node.prev;
      if (node.prev != null)
        node.prev.next = node.next;
      if (node.next != null)
        node.next.prev = node.prev;
      Console.WriteLine("Node deleted: " + node.content);
    }
  }

  class HttpServer
  {
    const int PORT = 8080;
    const int MAX_REQUEST_SIZE = 1024;
    LRUCache cache;

    public HttpServer()
    {
      cache = new LRUCache();
    }

    public void Start()
    {
      TcpListener server = new TcpListener(IPAddress.Any, PORT);
      server.Start();
      Console.WriteLine("Server started on port " + PORT);

      while (true)
      {
        TcpClient client = server.AcceptTcpClient();
        HandleRequest(client);
        client.Close();
      }
    }

    void HandleRequest(TcpClient client)
    {
      NetworkStream stream = client.GetStream();
      byte[] buffer = new byte[MAX_REQUEST_SIZE];
      int bytesRead = stream.Read(buffer, 0, buffer.Length);
      string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
      Console.WriteLine("Received request: " + request);

      string[] parts = request.Split(' ');
      if (parts.Length < 2 || parts[0] != "GET")
      {
        Console.WriteLine("Invalid request format.");
        return;
      }

      string url = parts[1];
      string content = cache.GetContent(url);

      if (string.IsNullOrEmpty(content))
      {
        try
        {
          // Assuming files are in the same directory as the executable
          content = File.ReadAllText(url.Substring(1)); 
          Console.WriteLine("Got content from file: " + content);
          cache.PutContent(url, content);
        }
        catch (FileNotFoundException)
        {
          Console.WriteLine("File not found: " + url.Substring(1));
          content = "HTTP/1.1 404 Not Found\r\n\r\n";
        }
      }
      else
      {
        Console.WriteLine("Serving content from cache.");
      }

      // Check if the content is HTML or plain text
      string contentType = "text/plain"; // Default content type is plain text
      if (url.EndsWith(".html") || url.EndsWith(".htm"))
      {
        // If the URL ends with .html or .htm, it's HTML content
        contentType = "text/html"; 
      }

      // Build the response
      string response = "HTTP/1.1 200 OK\r\nContent-Type: " 
        + contentType + "\r\n\r\n" + content;

      byte[] responseBytes = Encoding.ASCII.GetBytes(response);
      stream.Write(responseBytes, 0, responseBytes.Length);
    }
  }

  class Program
  {
    static void Main()
    {
      HttpServer server = new HttpServer();
      server.Start();
    }
  }
}
