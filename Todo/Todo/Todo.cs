namespace Todo
{
  class Task
  {
    public string? Description { get; set; }
    public int Priority { get; set; }
    public Task? Next { get; set; }
  }

  class TodoList
  {
    private Task? Head;
    private int Size;

    public TodoList()
    {
      Head = null;
      Size = 0;
    }

    public void AddTask(string description, int priority)
    {
      Task task = new Task();
      task.Description = description;
      task.Priority = priority;
      task.Next = null;

      if (Head == null)
      {
        // List is empty
        Head = task;
      }
      else
      {
        Task temp = Head;
        // Find the last node
        while (temp.Next != null)
        {
          temp = temp.Next;
        }
        // Insert the new task after the last node
        temp.Next = task;
      }
      Size++;
    }

    public void RemoveTask(int index)
    {
      if (Head == null)
      {
        Console.WriteLine("List is empty.");
        return;
      }

      if (index == 0)
      {
        // If we remove the first item in the list
        Task? temp = Head;
        Head = Head.Next;
        temp = null;
        Size--;
        return;
      }

      Task? previous = null;
      Task? current = Head;
      int i = 0;
      // Go to the selected index
      while (current != null && i < index)
      {
        previous = current;
        current = current.Next;
        i++;
      }

      if (current == null)
      {
        Console.WriteLine("Index out of bounds.");
        return;
      }

      previous.Next = current.Next;
      current = null;
      Size--;
    }

    public void DisplayTasks()
    {
      Task? temp = Head;
      int i = 1;
      while (temp != null)
      {
        Console.WriteLine(i++ + ") Description: " + temp.Description
            + ", Priority: " + temp.Priority);
        temp = temp.Next;
      }
    }

    public void SortTasks()
    {
      int swapped;
      Task? ptr1;
      Task? ptr2 = null;

      if (Head == null)
        return;

      do
      {
        swapped = 0; // will change if swapping happens
        ptr1 = Head;

        while (ptr1.Next != ptr2)
        {
          if (ptr1.Priority > ptr1.Next.Priority)
          {
            // Swap data of adjacent nodes
            int tempPriority = ptr1.Priority;
            ptr1.Priority = ptr1.Next.Priority;
            ptr1.Next.Priority = tempPriority;

            string? tempDescription = ptr1.Description;
            ptr1.Description = ptr1.Next.Description;
            ptr1.Next.Description = tempDescription;

            swapped = 1; // swap happened in this loop pass; don't stop yet
          }
          ptr1 = ptr1.Next;
        }
        ptr2 = ptr1;
      } while (swapped != 0); // quit loop when no swap happened
    }
  }

  class Todo
  {
    static void Main(string[] args)
    {
      TodoList list = new TodoList();

      int choice = 0;
      string description;
      int priority;
      int index;

      do
      {
        try
        {
          Console.WriteLine("\nTo-Do List Manager");
          Console.WriteLine("1. Add Task");
          Console.WriteLine("2. Remove Task");
          Console.WriteLine("3. Display Tasks");
          Console.WriteLine("4. Sort Tasks by Priority");
          Console.WriteLine("0. Exit");
          Console.Write("Enter your choice: ");
          choice = int.Parse(Console.ReadLine());

          switch (choice)
          {
            case 1:
              Console.Write("Enter task description: ");
              description = Console.ReadLine();
              Console.Write("Enter priority: ");
              priority = int.Parse(Console.ReadLine());
              list.AddTask(description, priority);
              Console.WriteLine("Task added successfully.");
              break;
            case 2:
              Console.Write("Enter number of task to remove: ");
              index = int.Parse(Console.ReadLine());
              list.RemoveTask(index - 1);
              Console.WriteLine("Task removed successfully.");
              break;
            case 3:
              Console.WriteLine("List of tasks:");
              list.DisplayTasks();
              break;
            case 4:
              list.SortTasks();
              Console.WriteLine("Tasks sorted by priority.");
              break;
            case 0:
              Console.WriteLine("Exiting...");
              break;
            default:
              Console.WriteLine("Invalid choice. Please try again.");
              break;
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e.ToString());
        }
      } while (choice != 0);
    }
  }
}
