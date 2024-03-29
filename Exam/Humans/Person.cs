﻿using System;

namespace Exam.Models
{
    [Serializable]
    public abstract class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte Age { get; set; }


        protected Person() { }

        /// <summary> Properties just for fun </summary>
        protected Person(string name, string surname, byte age)
        {
            Name = name;
            Surname = surname;
            Age = age;
        }
        
        
    }
}
