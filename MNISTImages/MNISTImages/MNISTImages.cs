namespace MNISTImages
{
  class Image
  {
    public byte[] Data { get; }
    public int Id { get; }

    public Image(byte[] data, int id)
    {
      Data = new byte[data.Length];
      Array.Copy(data, Data, data.Length);
      Id = id;
    }

    public void Print()
    {
      for (int i = 0; i < Data.Length; i++)
      {
        if (Data[i] == 0)
          Console.Write(" ");
        else
          Console.Write("*");
        if ((i + 1) % 28 == 0)
          Console.WriteLine();
      }
    }

    public double EuclideanDistance(Image img)
    {
      double distance = 0.0;
      for (int i = 0; i < Data.Length; i++)
      {
        distance += Math.Sqrt(Math.Pow((Data[i] - img.Data[i]), 2));
      }
      return Math.Sqrt(distance);
    }
  }

  class Program
  {
    const int ImageSize = 784;
    const int MetaDataSize = 15;

    static void Main()
    {
      List<Image> images = new List<Image>();

      using (FileStream ifs = new("input.dat", FileMode.Open, FileAccess.Read))
      {
        // Skip the meta data at the beginning of the file
        ifs.Seek(MetaDataSize, SeekOrigin.Begin);

        byte[] pixels = new byte[ImageSize];
        int count = 0;
        // Read data from the file and insert images into the list
        while (ifs.Read(pixels, 0, ImageSize) > 0)
        {
          images.Add(new Image(pixels, count++));
        }
      }

      Console.WriteLine("Total images: " + (images.Count-1));

      // Example: Find the closest image to a randomly selected image
      // Seed the random number generator
      Random rand = new Random();

      // Generate a random index within the range of the list length
      int randomIndex = rand.Next(0, images.Count);
      Console.WriteLine("Random index: " + randomIndex);

      Image randomImage = images[randomIndex];
      randomImage.Print();

      Image? closestImage = null;
      double minDistance = double.PositiveInfinity;
      int minIndex = 0;

      for (int i = 0; i < images.Count; i++)
      {
        double distance = randomImage.EuclideanDistance(images[i]);
        if (distance != 0 && distance < minDistance)
        {
          minDistance = distance;
          minIndex = i;
          closestImage = images[i];
        }
      }

      // Output the label of the closest image
      Console.WriteLine("\nClosest image (distance=" + minDistance +
          ", index = " + minIndex + ")\n");

      // Print closest image
      closestImage.Print();
    }
  }

}
