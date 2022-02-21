using System;
using System.Collections.Generic;
using Exam.Services;

namespace Exam.Models
{
    [Serializable]
    public sealed class Customer : Person
    {
        public Dictionary<string, int> basket = new();


        private byte _review = 5;
        public byte Review
        {
            get { return _review; }
            set
            {
                if (value < 2) _review = 2;
                else if (value > 5) return;
                else _review = value;   
            }
        }


        public Customer(ref List<Type> products) => RandomBasket(ref products);

        private void RandomBasket(ref List<Type> products)
        {
            if (products is null) throw new ArgumentNullException(nameof(products));

            var productCount = RandomService.RandomInteger(1, products.Count);

            for (int i = 0; i < productCount; i++)
            {
                var product = products[RandomService.RandomInteger(0, products.Count)];

                if (basket.ContainsKey(product.Name)) { i--; continue; }
                else basket.Add(product.Name, RandomService.RandomInteger(1, 7));
            }
        }
    }
}
