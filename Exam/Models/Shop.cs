using System;

namespace Exam.Models
{
    [Serializable]
    public sealed class Shop
    {
        private Dictionary<string, Stack<Vegetable>> Stands { get; } = new();

        public void add()
        {
            Stands.Add(Tomato.name, new Stack<Vegetable>());
            Stands.GetValueOrDefault(Tomato.name).Push(new Tomato(VegetableStatus.Toxic));
        }

        public void print()
        {
            Console.WriteLine(Stands.Count);
            foreach (KeyValuePair<string, Stack<Vegetable>> item1 in Stands)
            {
                foreach (var item2 in item1.Value)
                {
                    Console.WriteLine(item2);
                }
            }
        }

        private Queue<Customer> Customers { get; } = new();

        private List<Employee> Employers { get; } = new();

    }
}
