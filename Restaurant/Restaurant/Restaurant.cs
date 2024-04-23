namespace Restaurant
{
  public class Customer
  {
    public Customer(string name)
    {
      Name = name;
    }

    public string Name { get; }
  }

  public class Table
  {
    public Table(int id, int capacity)
    {
      ID = id;
      Capacity = capacity;
    }

    public int ID { get; }
    public int Capacity { get; }
  }

  public class Reservation
  {
    public Reservation(Customer customer, Table table, int startTimeSlot, 
      int endTimeSlot)
    {
      Customer = customer;
      Table = table;
      StartTimeSlot = startTimeSlot;
      EndTimeSlot = endTimeSlot;
    }

    public Customer Customer { get; }
    public Table Table { get; }
    public int StartTimeSlot { get; }
    public int EndTimeSlot { get; }
  }

  public class Restaurant
  {
    private readonly List<Table> tables = new List<Table>();
    private readonly List<Reservation> reservations = new List<Reservation>();

    public void AddTable(Table table)
    {
      tables.Add(table);
    }

    public bool IsTableAvailable(Table table, int startTimeSlot, int endTimeSlot)
    {
      return !reservations.Any(reservation =>
          reservation.Table.ID == table.ID &&
          ((startTimeSlot >= reservation.StartTimeSlot 
            && startTimeSlot < reservation.EndTimeSlot) 
          ||
          (endTimeSlot > reservation.StartTimeSlot 
            && endTimeSlot <= reservation.EndTimeSlot) 
          ||
          (startTimeSlot <= reservation.StartTimeSlot 
            && endTimeSlot >= reservation.EndTimeSlot)));
    }

    public List<Table> FindAvailableTables(int capacity, int startTimeSlot, 
      int endTimeSlot)
    {
      var availableTables = 
        tables.Where(table => 
          table.Capacity >= capacity 
          && IsTableAvailable(table, startTimeSlot, endTimeSlot)
        ).ToList();
      availableTables.Sort((a, b) => a.Capacity.CompareTo(b.Capacity));
      return availableTables;
    }

    public void AddReservation(string name, int capacity, int startSlot, int endSlot)
    {
      var availableTables = FindAvailableTables(capacity, startSlot, endSlot);
      if (availableTables.Any())
      {
        reservations.Add(
          new Reservation(
            new Customer(name), 
            availableTables.First(), 
            startSlot, 
            endSlot)
        );
        Console.WriteLine("Reservation successfully added.");
      }
      else
      {
        Console.WriteLine("No available tables for the requested time slot.");
      }
    }

    public void PrintReservations()
    {
      Console.WriteLine("All reservations:");
      foreach (var reservation in reservations)
      {
        Console.WriteLine($"Customer: {reservation.Customer.Name}" +
          $", Table Capacity: {reservation.Table.Capacity}, " +
          $"Start Time Slot: {reservation.StartTimeSlot}, " +
          $"End Time Slot: {reservation.EndTimeSlot}");
      }
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      var restaurant = new Restaurant();

      // Add tables
      restaurant.AddTable(new Table(1, 6));
      restaurant.AddTable(new Table(2, 4));
      restaurant.AddTable(new Table(3, 2));

      // Find available tables for a new reservation
      restaurant.AddReservation("Customer 1", 4, 1, 3);
      restaurant.AddReservation("Customer 2", 6, 2, 4);
      restaurant.AddReservation("Customer 3", 4, 3, 5);
      restaurant.AddReservation("Customer 4", 4, 1, 3);

      restaurant.PrintReservations();
    }
  }
}
