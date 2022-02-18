using System;

namespace Exam.Models
{
    [Serializable]
    public abstract class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte Age { get; set; }
    }
}
