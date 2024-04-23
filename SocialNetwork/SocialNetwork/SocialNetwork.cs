namespace SocialNetwork
{
  public class FriendNode
  {
    public string name;
    public FriendNode? next;
  }

  public class User
  {
    public string name;
    public FriendNode? friends;
  }

  public class Queue
  {
    private class QueueNode
    {
      public int userIndex;
      public QueueNode? next;
    }

    private QueueNode? front;
    private QueueNode? rear;

    public Queue()
    {
      front = rear = null;
    }

    public bool IsEmpty()
    {
      return (front == null);
    }

    public void Enqueue(int userIndex)
    {
      QueueNode newNode = new QueueNode
      {
        userIndex = userIndex,
        next = null
      };

      if (IsEmpty())
      {
        front = rear = newNode;
      }
      else
      {
        rear.next = newNode;
        rear = newNode;
      }
    }

    public int Dequeue()
    {
      if (IsEmpty())
      {
        Console.WriteLine("Queue is empty!");
        return -1;
      }

      QueueNode? temp = front;
      int userIndex = temp.userIndex;
      front = front.next;

      if (front == null)
      {
        rear = null;
      }

      return userIndex;
    }
  }

  public class Graph
  {
    private const int MAX_USERS = 100;
    private User[] users;
    private int numUsers;

    public Graph()
    {
      users = new User[MAX_USERS];
      numUsers = 0;
    }

    public void AddUser(string name)
    {
      if (numUsers >= MAX_USERS)
      {
        Console.WriteLine("Max user limit reached!");
        return;
      }

      users[numUsers] = new User
      {
        name = name,
        friends = null
      };

      numUsers++;
    }

    public void AddConnection(int src, int dest)
    {
      if (src < 0 || src >= numUsers || dest < 0 || dest >= numUsers)
      {
        Console.WriteLine("Invalid user index!");
        return;
      }

      FriendNode newFriendSrc = new FriendNode
      {
        name = users[dest].name,
        next = users[src].friends
      };

      users[src].friends = newFriendSrc;

      FriendNode newFriendDest = new FriendNode
      {
        name = users[src].name,
        next = users[dest].friends
      };

      users[dest].friends = newFriendDest;
    }

    public void RecommendFriends(int userIndex)
    {
      Console.WriteLine($"Recommended friends for {users[userIndex].name}:");

      Queue queue = new Queue();
      int[] visited = new int[MAX_USERS];

      visited[userIndex] = 1;
      queue.Enqueue(userIndex);

      while (!queue.IsEmpty())
      {
        int currentUserIndex = queue.Dequeue();
        FriendNode current = users[currentUserIndex].friends;

        while (current != null)
        {
          int friendIndex = -1;
          for (int i = 0; i < numUsers; i++)
          {
            if (current.name == users[i].name)
            {
              friendIndex = i;
              break;
            }
          }

          if (friendIndex != -1 && visited[friendIndex] == 0)
          {
            Console.WriteLine($"- {current.name}");
            visited[friendIndex] = 1;
            queue.Enqueue(friendIndex);
          }

          current = current.next;
        }
      }
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      Graph graph = new Graph();
      graph.AddUser("User A");
      graph.AddUser("User B");
      graph.AddUser("User C");
      graph.AddUser("User D");
      graph.AddUser("User E");
      graph.AddUser("User F");
      graph.AddUser("User G");
      graph.AddUser("User H");

      graph.AddConnection(0, 1);
      graph.AddConnection(1, 2);
      graph.AddConnection(2, 3);
      graph.AddConnection(4, 5);
      graph.AddConnection(5, 7);
      graph.AddConnection(3, 6);

      graph.RecommendFriends(0);
      graph.RecommendFriends(1);
      graph.RecommendFriends(7);
    }
  }
}
