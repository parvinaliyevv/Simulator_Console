using System;
using System.Collections.Generic;

namespace Exam.Models
{
    [Serializable]
    public sealed class Worker : Employee
    {
        public Worker(string position, ushort salary) : base(position, salary) { }

        public Worker(string name, string surname, byte age, string position, ushort salary) : base(name, surname, age, position, salary) { }

        /// <returns> Waste Product Count </returns>
        public int FilterStands(ref Dictionary<string, Stack<Vegetable>> stands)
        {
            if (stands is null) throw new ArgumentNullException(nameof(stands));

            List<Vegetable> goodProducts = new(), normalProducts = new();
            int WasteProductCount = default;

            foreach (KeyValuePair<string, Stack<Vegetable>> firstItem in stands)
            {
                foreach (var secondItem in firstItem.Value)
                {
                    if (secondItem.Status == VegetableStatus.Good)
                        goodProducts.Add(secondItem);
                    else if (secondItem.Status == VegetableStatus.Normal)
                        normalProducts.Add(secondItem);
                    else WasteProductCount++;
                }

                firstItem.Value.Clear();

                foreach (var secondItem in goodProducts) firstItem.Value.Push(secondItem);
                foreach (var secondItem in normalProducts) firstItem.Value.Push(secondItem);

                goodProducts.Clear();
                normalProducts.Clear();
            }

            return WasteProductCount;
        }
    }
}
