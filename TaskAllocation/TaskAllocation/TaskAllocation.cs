namespace TaskAllocation
{
  // Task structure
  class Task
  {
    public string Description;
    public int Duration;

    public Task(string description, int duration)
    {
      Description = description;
      Duration = duration;
    }
  }

  // Worker structure
  class Worker
  {
    public int Id;
    public int Workload;

    public Worker(int id, int workload)
    {
      Id = id;
      Workload = workload;
    }
  }

  class TaskAllocation
  {
    static void Main(string[] args)
    {
      List<Task> tasks = new List<Task>();
      PriorityQueue<Worker, int> workerQueue = new PriorityQueue<Worker, int>();

      Console.Write("Enter the number of workers: ");
      int numWorkers = int.Parse(Console.ReadLine());

      // Initialize workers with ID and 0 workload
      for (int i = 0; i < numWorkers; i++)
      {
        workerQueue.Enqueue(new Worker(i, 0), 0);
      }

      int choice;
      do
      {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. Display Tasks");
        Console.WriteLine("3. Print Workers Queue");
        Console.WriteLine("4. Exit");
        Console.Write("Enter your choice: ");
        choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
          case 1:
            AddTask(workerQueue, tasks);
            break;
          case 2:
            DisplayTasks(tasks);
            break;
          case 3:
            PrintWorkersQueue(workerQueue);
            break;
          case 4:
            Console.WriteLine("Exiting program...");
            break;
          default:
            Console.WriteLine("Invalid choice! Please try again.");
            break;
        }
      } while (choice != 4);
    }

    // Function to add a task and allocate it to a worker
    static void AddTask(PriorityQueue<Worker, int> workerQueue, List<Task> tasks)
    {
      Console.Write("Enter task description: ");
      string description = Console.ReadLine();
      Console.Write("Enter task duration (in minutes): ");
      int duration = int.Parse(Console.ReadLine());

      if (workerQueue.Count == 0)
      {
        Console.WriteLine("No workers available! Task cannot be assigned.");
        return;
      }

      // Dequeue the worker with the shortest workload
      var worker = workerQueue.Dequeue();

      // Assign the task to the worker and update workload
      tasks.Add(new Task(description, duration));
      Console.WriteLine($"Task added successfully " +
        $"and allocated to Worker {worker.Id}!");

      // Update workload
      worker.Workload += duration;
      workerQueue.Enqueue(worker, worker.Workload);
    }

    // Function to display all tasks
    static void DisplayTasks(List<Task> tasks)
    {
      Console.WriteLine("Task List:");
      foreach (Task task in tasks)
      {
        Console.WriteLine($"Task description: {task.Description}, " +
          $"Duration: {task.Duration} minutes");
      }
    }

    // Function to print the workers queue
    static void PrintWorkersQueue(PriorityQueue<Worker, int> workerQueue)
    {
      Console.WriteLine("Workers Queue:");
      foreach (var item in workerQueue.UnorderedItems)
      {
        Console.WriteLine($"Worker ID: {item.Element.Id}, " +
          $"Workload: {item.Priority} minutes");
      }
    }
  }
}
