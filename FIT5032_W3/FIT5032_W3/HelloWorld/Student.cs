namespace FIT5032_W3.HelloWorld
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Student(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
