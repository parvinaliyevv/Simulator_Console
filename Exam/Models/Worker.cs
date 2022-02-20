using System;
using System.Collections.Generic;

namespace Exam.Models
{
    [Serializable]
    public sealed class Worker : Employee
    {
        public Worker(string position, ushort salary) : base(position, salary) { }

        public Worker(string name, string surname, byte age, string position, ushort salary) : base(name, surname, age, position, salary) { }

        public void FilterStands(ref Dictionary<string, Stack<Vegetable>> stands)
        {
            List<Vegetable> goodProducts = new(), normalProducts = new();

            foreach (KeyValuePair<string, Stack<Vegetable>> firstItem in stands)
            {
                foreach (var secondItem in firstItem.Value)
                {
                    if (secondItem.Status == VegetableStatus.Good)
                        goodProducts.Add(secondItem);
                    else if (secondItem.Status == VegetableStatus.Normal)
                        normalProducts.Add(secondItem);
                    else continue;
                }

                firstItem.Value.Clear();

                foreach (var secondItem in goodProducts) firstItem.Value.Push(secondItem);
                foreach (var secondItem in normalProducts) firstItem.Value.Push(secondItem);
            }
        }
    }
}
