namespace Flights
{
  // Structure to represent each city
  public class City
  {
    public string Name { get; }
    public Dictionary<string, int> Flights { get; } // Map of connected cities and their costs

    public City(string name)
    {
      Name = name;
      Flights = new Dictionary<string, int>();
    }
  }

  // Graph class to represent all cities and flights
  public class FlightGraph
  {
    // Map of city names and their objects
    private Dictionary<string, City> cities = new Dictionary<string, City>(); 

    // Add a city to the graph
    public void AddCity(string name)
    {
      cities[name] = new City(name);
    }

    // Add a flight between two cities and its cost
    public void AddFlight(string src, string dest, int cost)
    {
      // Assuming flights are bidirectional
      cities[src].Flights[dest] = cost;
      cities[dest].Flights[src] = cost;
    }

    // Function to find the cheapest route between two cities using Dijkstra's algorithm
    public List<string> FindCheapestRoute(string src, string dest, out int totalPrice)
    {
      Dictionary<string, int> dist = new Dictionary<string, int>();
      Dictionary<string, string> prev = new Dictionary<string, string>();
      PriorityQueue<(int, string), int> pq = new PriorityQueue<(int, string), int>();

      foreach (var city in cities.Keys)
      {
        dist[city] = int.MaxValue;
        prev[city] = "";
      }

      dist[src] = 0;
      pq.Enqueue((0, src), 0);

      while (pq.Count > 0)
      {
        var (uDist, u) = pq.Dequeue();

        foreach (var flight in cities[u].Flights)
        {
          var (v, cost) = flight;

          if (dist[u] != int.MaxValue && dist[u] + cost < dist[v])
          {
            dist[v] = dist[u] + cost;
            prev[v] = u;
            pq.Enqueue((dist[v], v), dist[v]);
          }
        }
      }

      // Reconstructing the path
      List<string> path = new List<string>();
      string current = dest;
      while (!string.IsNullOrEmpty(prev[current]))
      {
        path.Add(current);
        current = prev[current];
      }
      path.Add(src);
      path.Reverse();

      // Calculate total price
      totalPrice = dist[dest];

      return path;
    }

    // Display all possible flights between two cities using DFS
    public void DisplayAllFlights(string src, string dest)
    {
      if (!cities.ContainsKey(src) || !cities.ContainsKey(dest))
      {
        Console.WriteLine("Invalid cities entered.");
        return;
      }

      HashSet<string> visited = new HashSet<string>();
      Stack<string> path = new Stack<string>();
      path.Push(src);
      DFS(src, dest, visited, path);
    }

    // Recursive DFS function to find all flights between source and destination
    private void DFS(string src, string dest, HashSet<string> visited, Stack<string> path)
    {
      visited.Add(src);

      if (src == dest)
      {
        PrintPath(path);
      }
      else
      {
        foreach (var flight in cities[src].Flights)
        {
          if (!visited.Contains(flight.Key))
          {
            path.Push(flight.Key);
            DFS(flight.Key, dest, visited, path);
            path.Pop();
          }
        }
      }

      visited.Remove(src);
    }

    // Helper function to print a path (stack content)
    private void PrintPath(Stack<string> path)
    {
      var temp = new Stack<string>(path.Reverse());
      Console.WriteLine(string.Join(" -> ", temp));
    }
  }


  class Program
  {
    static void Main(string[] args)
    {
      FlightGraph graph = new FlightGraph();

      graph.AddCity("London");
      graph.AddCity("Paris");
      graph.AddCity("Berlin");
      graph.AddCity("Rome");
      graph.AddCity("Madrid");
      graph.AddCity("Amsterdam");

      graph.AddFlight("London", "Paris", 100);
      graph.AddFlight("London", "Berlin", 150);
      graph.AddFlight("London", "Madrid", 200);
      graph.AddFlight("Paris", "Berlin", 120);
      graph.AddFlight("Paris", "Rome", 180);
      graph.AddFlight("Berlin", "Rome", 220);
      graph.AddFlight("Madrid", "Rome", 250);
      graph.AddFlight("Madrid", "Amsterdam", 170);
      graph.AddFlight("Amsterdam", "Berlin", 130);

      Console.Write("Enter departure city: ");
      string departure = Console.ReadLine();
      Console.Write("Enter destination city: ");
      string destination = Console.ReadLine();

      // Display all possible flights
      Console.WriteLine($"All possible flights between {departure} and {destination}:");
      graph.DisplayAllFlights(departure, destination);

      // Find the cheapest route and total price
      List<string> route = graph.FindCheapestRoute(departure, destination, out int totalPrice);

      // Display the cheapest route and total price
      Console.Write("Cheapest Route: ");
      Console.WriteLine(string.Join(" -> ", route));
      Console.WriteLine($"Total Price: {totalPrice}");
    }
  }
}
