using System.Collections.ObjectModel;

namespace FIT5032_W4.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public virtual Collection<Unit> Units { get; set; }

        public Student(int id)
        {
            Id = id;
        }

        public Student() { }

        public Student(int id, string firstName, string lastName) 
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public Student(int id, string firstName, string lastName, Collection<Unit> units) : this(id, firstName, lastName)
        {
            Units = units;
        }
    }
}
