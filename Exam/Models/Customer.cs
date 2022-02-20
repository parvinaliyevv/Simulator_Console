using System;
using System.Collections.Generic;
using Exam.Services;

namespace Exam.Models
{
    [Serializable]
    public sealed class Customer : Person
    {
        private byte _review = Convert.ToByte(RandomService.RandomInteger(8, 10));

        public byte Review
        {
            get { return _review; }
            set
            {
                if (value < 2) _review = 2;
                else if (value > 10) return;
                else _review = value;   
            }
        }

        public Dictionary<string, int> _basket = new();

        public Customer(ref List<Type> products) => RandomBasket(ref products);

        public void RandomBasket(ref List<Type> products)
        {
            for (int i = 0; i < RandomService.RandomInteger(1, products.Count); i++)
            {
                var product = products[RandomService.RandomInteger(0, products.Count)];

                if (_basket.ContainsKey(product.Name)) { i--; continue; }
                else _basket.Add(product.Name, RandomService.RandomInteger(50, 250));
            }
        }
    }
}
