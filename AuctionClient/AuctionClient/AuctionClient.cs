using System.Net.Sockets;
using System.Text;

namespace AuctionClient
{
  class AuctionClient
  {
    const int PORT = 8080;
    const string SERVER_IP = "127.0.0.1";

    static TcpClient client;
    static bool isRunning = true;
    static Thread receiveThread;

    static void Main()
    {
      // Connect to the server
      try
      {
        client = new TcpClient(SERVER_IP, PORT);
        Console.WriteLine("Connected to server.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Connect failed: {ex.Message}");
        return;
      }

      // Start receive handler thread
      receiveThread = new Thread(ReceiveHandler);
      receiveThread.Start();

      // Send bids until the user quits
      string bid;
      while (isRunning)
      {
        Console.Write("\nEnter your bid (or 'q' to quit): ");
        bid = Console.ReadLine();

        if (bid.ToLower() == "q")
        {
          break;
        }

        StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII);
        try
        {
          writer.WriteLine(bid);
          writer.Flush();
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Send failed: {ex.Message}");
          return;
        }
      }

      // Close client and cleanup
      client.Close();
    }

    static void ReceiveHandler()
    {
      StreamReader reader = new StreamReader(client.GetStream(), Encoding.ASCII);
      while (true)
      {
        try
        {
          string message = reader.ReadLine();
          if (string.IsNullOrEmpty(message))
          {
            Console.WriteLine("\nServer disconnected.");
            break;
          }

          Console.WriteLine($"\nServer: {message}");

          if (message.StartsWith("Auction"))
          {
            Console.WriteLine("Auction ended. Exiting program.");
            isRunning = false;
            client.Close();
            break;
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Receive failed: {ex.Message}");
          break;
        }
      }
    }
  }
}
