namespace WordFrequencies
{
  class WordFrequencies
  {
    static string CleanWord(string word)
    {
      return new string(word
                        .Where(char.IsLetter)
                        .Select(char.ToLower)
                        .ToArray());
    }

    static void Main(string[] args)
    {
      Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

      // Read text from file
      using (StreamReader inputFile = new StreamReader("input.txt"))
      {
        string line;
        while ((line = inputFile.ReadLine()) != null)
        {
          foreach (string word in 
            line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
          {
            string cleanedWord = CleanWord(word);
            if (!string.IsNullOrEmpty(cleanedWord))
            {
              if (wordFrequency.ContainsKey(cleanedWord))
              {
                wordFrequency[cleanedWord]++;
              }
              else
              {
                wordFrequency[cleanedWord] = 1;
              }
            }
          }
        }
      }

      // Display word frequencies
      Console.WriteLine("Word Frequencies:");
      foreach (var pair in wordFrequency)
      {
        Console.WriteLine($"{pair.Key}: {pair.Value}");
      }
    }
  }
}
