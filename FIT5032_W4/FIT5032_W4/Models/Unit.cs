namespace FIT5032_W4.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }
    }
}
