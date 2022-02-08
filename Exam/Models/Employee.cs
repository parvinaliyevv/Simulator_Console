namespace Exam.Models
{
    internal abstract class Employee: Person
    {
        public string Position { get; set; }
        public ushort Salary { get; set; }

    }
}
