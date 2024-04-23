using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AuctionServer
{
  class Client
  {
    public TcpClient Socket { get; }
    public StreamWriter Writer { get; }
    public int Id { get; }

    public Client(TcpClient socket, int id)
    {
      Socket = socket;
      Writer = new StreamWriter(socket.GetStream(), Encoding.ASCII) 
      { 
        AutoFlush = true 
      };
      Id = id;
    }
  }

  class Program
  {
    const int PORT = 8080;
    const int MAX_CLIENTS = 5;
    static TcpListener server;
    static Client[] clients = new Client[MAX_CLIENTS];
    static int bestBid = 0;
    static int winningClient = 0;
    static Timer timer;

    static void Main()
    {
      try
      {
        server = new TcpListener(IPAddress.Any, PORT);
        server.Start();
        Console.WriteLine($"Server listening on port {PORT}");

        // Start the timer
        timer = new Timer(TimerCompletionRoutine, null,
          TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20));

        while (true)
        {
          TcpClient clientSocket = server.AcceptTcpClient();
          Console.WriteLine($"Client connected: " +
            $"{clientSocket.Client.RemoteEndPoint}");

          // Find an available slot for the client
          int clientId = -1;
          for (int i = 0; i < MAX_CLIENTS; i++)
          {
            if (clients[i] == null)
            {
              clientId = i;
              clients[i] = new Client(clientSocket, clientId + 1);
              Console.WriteLine($"Client no. {clientId + 1} connected.");
              break;
            }
          }

          // Handle client in a separate thread
          Thread clientThread = new Thread(() => HandleClient(clientId));
          clientThread.Start();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Server error: {ex.Message}");
      }
      finally
      {
        server?.Stop();
      }
    }

    static void HandleClient(int clientId)
    {
      try
      {
        Client client = clients[clientId];
        TcpClient clientSocket = client.Socket;
        NetworkStream stream = clientSocket.GetStream();
        StreamReader reader = new StreamReader(stream, Encoding.ASCII);
        StreamWriter writer = new StreamWriter(stream, Encoding.ASCII) 
        { 
          AutoFlush = true 
        };

        string? bid;
        while (true)
        {
          bid = reader.ReadLine();
          if (bid == null)
          {
            Console.WriteLine($"Client disconnected: " +
              $"{clientSocket.Client.RemoteEndPoint}");
            clients[clientId] = null;
            break;
          }

          int bidAmount = int.Parse(bid);
          Console.WriteLine($"Received bid {bidAmount} from client {client.Id}");

          if (bidAmount > bestBid)
          {
            bestBid = bidAmount;
            winningClient = client.Id;

            string msg = $"New best bid: {bestBid} (Client: {winningClient})";
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
              if (clients[i] != null)
              {
                writer.WriteLine(msg);
              }
            }
            Console.WriteLine($"New best bid: {bestBid} (Client: {winningClient})");

            // Reset the timer
            timer.Change(TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20));
          }
          else
          {
            string msg = $"Received lower bid. Best bid remains at: {bestBid}";
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
              if (clients[i] != null)
              {
                writer.WriteLine(msg);
              }
            }
            Console.WriteLine($"Received lower bid. " +
              $"Best bid remains at: {bestBid}");
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Client error: {ex.Message}");
      }
    }

    static void TimerCompletionRoutine(object state)
    {
      Console.WriteLine($"Auction finished. " +
        $"Winning bid: {bestBid}, winner: client no. {winningClient}");

      string msg = $"Auction finished. " +
        $"Winning bid: {bestBid}, winner: client no. {winningClient}";
      for (int i = 0; i < MAX_CLIENTS; i++)
      {
        if (clients[i] != null)
        {
          clients[i].Writer.WriteLine(msg);
        }
      }

      Environment.Exit(0);
    }
  }

}