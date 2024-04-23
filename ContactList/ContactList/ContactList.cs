namespace ContactList
{
  class Contact
  {
    public string? Name;
    public string? Phone;
    public Contact? Next;
  }

  class ContactList
  {
    private const int HASH_SIZE = 100;
    private Contact?[] bucketTable;

    public ContactList()
    {
      bucketTable = new Contact[HASH_SIZE];
      for (int i = 0; i < HASH_SIZE; i++)
      {
        bucketTable[i] = null;
      }
    }

    private uint Hash(string name)
    {
      uint hash = 0;
      foreach (char c in name)
      {
        hash = ((hash << 5) + hash) + c;
      }
      return hash % HASH_SIZE;
    }

    public void ContactAdd(string name, string phone)
    {
      uint hashIndex = Hash(name);
      Contact newContact = new Contact();
      newContact.Name = name;
      newContact.Phone = phone;
      newContact.Next = bucketTable[hashIndex];
      bucketTable[hashIndex] = newContact;
    }

    public void ContactRemove(string name)
    {
      uint index = Hash(name);
      Contact? contact = bucketTable[index];
      Contact? previous = null;

      while (contact != null)
      {
        if (contact.Name == name)
        {
          if (previous == null)
          {
            // Contact to remove is the head of the list
            bucketTable[index] = contact.Next;
          }
          else
          {
            // Contact to remove is not the head of the list
            previous.Next = contact.Next;
          }
          Console.WriteLine("Contact '" + name + "' removed successfully.");
          return;
        }
        previous = contact;
        contact = contact.Next;
      }
      Console.WriteLine("Contact '" + name + "' not found.");
    }

    public void ContactSearch(string name)
    {
      uint hashIndex = Hash(name);
      Contact? contact = bucketTable[hashIndex];
      while (contact != null)
      {
        if (contact.Name == name)
        {
          Console.WriteLine("Name: " + contact.Name 
            + "\nPhone Number: " + contact.Phone);
          return;
        }
        contact = contact.Next;
      }
      Console.WriteLine("Contact '" + name + "' not found.");
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      ContactList phonebook = new ContactList();
      phonebook.ContactAdd("John", "235454545");
      phonebook.ContactAdd("Jane", "775755454");
      phonebook.ContactAdd("George", "4344343477");

      phonebook.ContactSearch("John");
      phonebook.ContactSearch("Alex");
      phonebook.ContactSearch("George");

      phonebook.ContactRemove("Jake");
      phonebook.ContactRemove("Jane");
      phonebook.ContactSearch("Jane");
    }
  }
}
