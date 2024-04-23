namespace UniversityCourses
{
  public class Student
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int[] Courses { get; set; }
    public int CourseCount { get; set; }
  }

  public class Course
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int PrereqCount { get; set; }
    public int[] PrereqIDs { get; set; }

    public bool CanEnroll(Student student)
    {
      foreach (int prereqId in PrereqIDs)
      {
        bool hasPrereq = false;
        foreach (int courseId in student.Courses)
        {
          if (courseId == prereqId)
          {
            hasPrereq = true;
            break;
          }
        }
        if (!hasPrereq)
        {
          return false;
        }
      }
      return true;
    }
  }

  class UniversityCourses
  {
    static void Main(string[] args)
    {
      Course[] courses = {
            new Course { Id = 0, Name = "Intro to Programming", 
              PrereqCount = 0, PrereqIDs = new int[] { -1 } },
            new Course { Id = 1, Name = "Data Structures", 
              PrereqCount = 1, PrereqIDs = new int[] { 0 } },
            new Course { Id = 2, Name = "Algorithms", 
              PrereqCount = 1, PrereqIDs = new int[] { 1 } },
            new Course { Id = 3, Name = "Database Management", 
              PrereqCount = 1, PrereqIDs = new int[] { 0 } },
            new Course { Id = 4, Name = "Web Development", 
              PrereqCount = 1, PrereqIDs = new int[] { 0 } },
            new Course { Id = 5, Name = "Operating Systems", 
              PrereqCount = 2, PrereqIDs = new int[] { 1, 2 } },
            new Course { Id = 6, Name = "Computer Networks", 
              PrereqCount = 2, PrereqIDs = new int[] { 1, 5 } },
            new Course { Id = 7, Name = "Software Engineering", 
              PrereqCount = 2, PrereqIDs = new int[] { 1, 2 } },
            new Course { Id = 8, Name = "Machine Learning", 
              PrereqCount = 2, PrereqIDs = new int[] { 1, 2 } },
            new Course { Id = 9, Name = "Distributed Systems", 
              PrereqCount = 1, PrereqIDs = new int[] { 5 } },
            new Course { Id = 10, Name = "Cybersecurity", 
              PrereqCount = 2, PrereqIDs = new int[] { 2, 3 } },
            new Course { Id = 11, Name = "Cloud Computing", 
              PrereqCount = 2, PrereqIDs = new int[] { 2, 3 } },
            new Course { Id = 12, Name = "Mobile App Development", 
              PrereqCount = 1, PrereqIDs = new int[] { 4 } },
            new Course { Id = 13, Name = "Game Development", 
              PrereqCount = 1, PrereqIDs = new int[] { 0 } },
            new Course { Id = 14, Name = "Artificial Intelligence", 
              PrereqCount = 2, PrereqIDs = new int[] { 2, 8 } },
            new Course { Id = 15, Name = "Big Data Analytics", 
              PrereqCount = 2, PrereqIDs = new int[] { 2, 3 } },
            new Course { Id = 16, Name = "Blockchain Technology", 
              PrereqCount = 2, PrereqIDs = new int[] { 2, 3 } },
            new Course { Id = 17, Name = "UI/UX Design", 
              PrereqCount = 1, PrereqIDs = new int[] { 14 } },
            new Course { Id = 18, Name = "Embedded Systems", 
              PrereqCount = 2, PrereqIDs = new int[] { 1, 5 } },
            new Course { Id = 19, Name = "Computer Graphics", 
              PrereqCount = 1, PrereqIDs = new int[] { 0 } }
        };

      Student student = new Student
      {
        Id = 1,
        Name = "John Doe",
        CourseCount = 5,
        Courses = new int[] { 0, 1, 2, 3, 4 }
      };

      Course[] targetCourses = {
            courses[13], // Game Development
            courses[16], // Blockchain Technology
            courses[17], // UI/UX Design (student cannot enroll)
            courses[18]  // Embedded Systems
        };

      Console.WriteLine($"Enrollment status for {student.Name}: ");
      for (int i = 0; i < 4; ++i)
      {
        if (targetCourses[i].CanEnroll(student))
        {
          Console.WriteLine($"- Can enroll in {targetCourses[i].Name}");
        }
        else
        {
          Console.WriteLine($"- Cannot enroll in {targetCourses[i].Name} " +
            $"due to missing prerequisites.");
        }
      }
    }
  }
}
