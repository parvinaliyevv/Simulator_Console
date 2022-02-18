using System;

namespace Exam.Models
{
    [Serializable]
    public abstract class Employee : Person
    {
        public string Position { get; set; }
        public ushort Salary { get; set; }

    }
}
