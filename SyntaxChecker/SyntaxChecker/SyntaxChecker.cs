namespace SyntaxChecker
{
  class Stack
  {
    private char[] items;
    private int top;

    public Stack()
    {
      items = new char[100];
      top = -1;
    }

    public void Push(char c)
    {
      if (top == items.Length - 1)
      {
        Console.WriteLine("Stack is full");
        Environment.Exit(1);
      }
      items[++top] = c;
    }

    public char Pop()
    {
      if (top == -1)
      {
        Console.WriteLine("Stack is empty");
        Environment.Exit(1);
      }
      return items[top--];
    }

    public bool CheckEmpty()
    {
      return (top == -1);
    }
  }

  class SyntaxChecker
  {
    static int CheckBalanced(string filename)
    {
      using (StreamReader file = new StreamReader(filename))
      {
        char c;
        Stack stack = new Stack();

        while ((c = (char)file.Read()) != '\0')
        {
          if (c == '(' || c == '[' || c == '{')
          {
            stack.Push(c);
          }
          else if (c == ')' || c == ']' || c == '}')
          {
            if (stack.CheckEmpty())
            {
              file.Close();
              return 0;
            }

            char openingChar = stack.Pop();

            if ((c == ')' && openingChar != '(') ||
                (c == ']' && openingChar != '[') ||
                (c == '}' && openingChar != '{'))
            {
              file.Close();
              return 0;
            }
          }
        }

        int result = stack.CheckEmpty() ? 1 : 0;
        file.Close();
        return result;
      }
    }

    static void Main(string[] args)
    {
      Console.Write("Path to the source file: ");
      string filename = Console.ReadLine();

      if (CheckBalanced(filename) == 1)
      {
        Console.WriteLine("The input file is balanced.");
      }
      else
      {
        Console.WriteLine("The input file is not balanced.");
      }
    }
  }
}
