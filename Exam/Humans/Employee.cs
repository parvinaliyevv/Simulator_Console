using System;

namespace Exam.Models
{
    [Serializable]
    public abstract class Employee : Person
    {
        public string Position { get; set; }
        public ushort Salary { get; set; }


        protected Employee(string position, ushort salary)
        {
            Position = position;
            Salary = salary;
        }

        protected Employee(string name, string surname, byte age, string position, ushort salary): base(name, surname, age)
        {
            Position = position;
            Salary = salary;
        }
    }
}
