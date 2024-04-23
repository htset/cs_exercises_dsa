namespace MazeSolver
{
  public class Point
  {
    public int row, col;

    public Point()
    {
      row = 0;
      col = 0;
    }

    public Point(int x, int y)
    {
      row = x;
      col = y;
    }
  }

  public class Stack<T>
  {
    private T[] items;
    private int top;

    public Stack(int capacity)
    {
      items = new T[capacity];
      top = -1;
    }

    public bool IsEmpty()
    {
      return top == -1;
    }

    public void Push(T t)
    {
      top++;
      items[top] = t;
    }

    public T Pop()
    {
      return items[top--];
    }
  }

  public class Maze
  {
    private Stack<Point> stack = new Stack<Point>(ROWS * COLS);

    private const int ROWS = 15;
    private const int COLS = 15;
    private int[,] matrix = {
        {0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0},
        {0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0},
        {0, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0},
        {0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0},
        {0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0},
        {0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0},
        {0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0},
        {0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 0},
        {0, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0},
        {0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0},
        {0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0},
        {0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
        {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0}
    };

    public Maze() { }

    // Check if we can move to this cell
    private bool CanMove(int row, int col)
    {
      return (row >= 0
          && row < ROWS
          && col >= 0
          && col < COLS
          && matrix[row, col] == 0);
    }

    public void Print()
    {
      for (int i = 0; i < ROWS; i++)
      {
        for (int j = 0; j < COLS; j++)
        {
          Console.Write(matrix[i, j] + " ");
        }
        Console.WriteLine();
      }
    }

    // Solve the maze using backtracking
    public int Solve(int row, int col)
    {
      if (row == ROWS - 1 && col == COLS - 1)
      {
        // destination reached
        stack.Push(new Point(row, col));
        return 1;
      }

      if (CanMove(row, col))
      {
        stack.Push(new Point(row, col));
        matrix[row, col] = 2; // Marking visited

        // Move right
        if (Solve(row, col + 1) == 1)
          return 1;

        // Move down
        if (Solve(row + 1, col) == 1)
          return 1;

        // Move left
        if (Solve(row, col - 1) == 1)
          return 1;

        // Move up
        if (Solve(row - 1, col) == 1)
          return 1;

        // If none of the above movements work, backtrack
        stack.Pop();
        return 0;
      }

      return 0;
    }

    public void PrintPath()
    {
      while (!stack.IsEmpty())
      {
        Point p = stack.Pop();
        Console.Write($"({p.row}, {p.col}), ");
      }
    }
  }

  class MazeSolver
  {
    static void Main(string[] args)
    {
      Maze maze = new Maze();

      Console.WriteLine("This is the maze:");
      maze.Print();

      if (maze.Solve(0, 0) == 1)
      {
        Console.WriteLine("\n\nThis is the path found:");
        maze.PrintPath();

        Console.WriteLine("\n\nThis is the maze with all the points crossed:");
        maze.Print();
      }
      else
      {
        Console.WriteLine("No path found");
      }
    }
  }
}
