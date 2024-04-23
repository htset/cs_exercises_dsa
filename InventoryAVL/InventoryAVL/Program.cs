namespace InventoryAVL
{
  class Product
  {
    public int id;
    public string? name;
    public float price;
    public int quantity;
  }

  class InventoryNode
  {
    public Product product;
    public InventoryNode? left;
    public InventoryNode? right;
    public int height;
  }

  class Inventory
  {
    private InventoryNode? root;

    private int GetHeight(InventoryNode? node)
    {
      return node == null ? 0 : node.height;
    }

    private int GetBalance(InventoryNode? node)
    {
      return node == null ? 0 : GetHeight(node.left) - GetHeight(node.right);
    }

    private InventoryNode NewNode(Product product)
    {
      InventoryNode node = new InventoryNode
      {
        product = product,
        left = null,
        right = null,
        height = 1
      };
      return node;
    }

    private InventoryNode RotateRight(InventoryNode y)
    {
      InventoryNode? x = y.left;
      InventoryNode? T2 = x.right;

      x.right = y;
      y.left = T2;

      y.height = Math.Max(GetHeight(y.left), GetHeight(y.right)) + 1;
      x.height = Math.Max(GetHeight(x.left), GetHeight(x.right)) + 1;

      return x;
    }

    private InventoryNode RotateLeft(InventoryNode x)
    {
      InventoryNode? y = x.right;
      InventoryNode? T2 = y.left;

      y.left = x;
      x.right = T2;

      x.height = Math.Max(GetHeight(x.left), GetHeight(x.right)) + 1;
      y.height = Math.Max(GetHeight(y.left), GetHeight(y.right)) + 1;

      return y;
    }

    private InventoryNode InsertProduct(InventoryNode? node, Product product)
    {
      if (node == null)
        return NewNode(product);

      if (product.id < node.product.id)
        node.left = InsertProduct(node.left, product);
      else if (product.id > node.product.id)
        node.right = InsertProduct(node.right, product);
      else
        return node;

      node.height = 1 + Math.Max(GetHeight(node.left), GetHeight(node.right));

      int balance = GetBalance(node);

      if (balance > 1 && product.id < node.left.product.id)
        return RotateRight(node);

      if (balance < -1 && product.id > node.right.product.id)
        return RotateLeft(node);

      if (balance > 1 && product.id > node.left.product.id)
      {
        node.left = RotateLeft(node.left);
        return RotateRight(node);
      }

      if (balance < -1 && product.id < node.right.product.id)
      {
        node.right = RotateRight(node.right);
        return RotateLeft(node);
      }

      return node;
    }

    private void TraverseTree(InventoryNode? node)
    {
      if (node != null)
      {
        TraverseTree(node.left);
        Console.WriteLine($"ID: {node.product.id}, " +
          $"Name: {node.product.name}, " +
          $"Price: {node.product.price}," +
          $" Quantity: {node.product.quantity}");
        TraverseTree(node.right);
      }
    }

    private InventoryNode SearchProduct(InventoryNode? node, int id)
    {
      if (node == null || node.product.id == id)
      {
        if (node == null)
          Console.WriteLine("Product not found.");
        else
          Console.WriteLine($"Found product: ID: {node.product.id}, " +
            $"Name: {node.product.name}, " +
            $"Price: {node.product.price}, " +
            $"Quantity: {node.product.quantity}");

        return node;
      }

      Console.WriteLine($"Visited product ID: {node.product.id}");

      if (id < node.product.id)
        return SearchProduct(node.left, id);
      else
        return SearchProduct(node.right, id);
    }

    public void InsertProduct(Product product)
    {
      root = InsertProduct(root, product);
    }

    public void TraverseTree()
    {
      TraverseTree(root);
    }

    public InventoryNode SearchProduct(int id)
    {
      return SearchProduct(root, id);
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      Inventory inv = new Inventory();
      Product[] products = new Product[100];

      Random random = new Random();
      for (int i = 0; i < 100; i++)
      {
        products[i] = new Product
        {
          id = i + 1,
          name = $"Product {i + 1}",
          price = (float)random.Next(1000) / 10.0f,
          quantity = random.Next(100) + 1
        };
      }

      for (int i = 99; i >= 0; i--)
      {
        int j = random.Next(i + 1);
        Product temp = products[i];
        products[i] = products[j];
        products[j] = temp;
      }

      foreach (Product product in products)
      {
        inv.InsertProduct(product);
      }

      Console.WriteLine("Inventory:");
      inv.TraverseTree();

      int productIdToSearch = 35;
      InventoryNode foundProduct = inv.SearchProduct(productIdToSearch);
      if (foundProduct != null)
      {
        Console.WriteLine($"Product found: ID: {foundProduct.product.id}, " +
          $"Name: {foundProduct.product.name}, Price: {foundProduct.product.price}, " +
          $"Quantity: {foundProduct.product.quantity}");
      }
      else
      {
        Console.WriteLine($"Product with ID {productIdToSearch} not found.");
      }
    }
  }

}
