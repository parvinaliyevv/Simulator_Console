using System;
using System.Linq;
using System.Collections.Generic;
using Exam.Services;

namespace Exam.Models
{
    [Serializable]
    public sealed class Shop
    {
        private static List<int> _ratings = new();
        public int AverageRating
        {
            get { return (_ratings.Sum() / _ratings.Count); }
            set
            {
                if (value > 2 & value < 11) _ratings.Add(value);
                else throw new ArgumentOutOfRangeException(nameof(value), "The rating of vegetables cannot be less than two and more than ten!");
            }
        }

        // private Statistics Week = new();
        // private Statistics Day = new();

        private Dictionary<string, Stack<Vegetable>> _stands = new();
        private Queue<Customer> _customers = new();
        private List<Worker> _workers = new();

        [NonSerialized] private List<Type> _products = new();

        private decimal _budget = 5000;
        public decimal Budget { get { return _budget; } set { if (value > 0) _budget = value; } }

        public ShopStatus Status { get; set; }

        public void AddWorker(string name, string surname, byte age, string position, ushort salary) => _workers.Add(new Worker(name, surname, age, position, salary));

        public void EnqueueCustomers()
        {
            if (Status == ShopStatus.Normal)
            {
                for (int i = 0; i < RandomService.RandomInteger(1, (int)AverageRating); i++)
                    _customers.Enqueue(new Customer(ref _products));
            }
        }
        public void DequeueCustomers()
        {
            Customer customer = null;
            Stack<Vegetable> stand = null;
            Vegetable product = null;
            decimal money = default;

            while (_customers.Count != 0)
            {
                customer = _customers.Dequeue();
                money = default;

                foreach (KeyValuePair<string, int> item in customer._basket)
                {
                    stand = _stands.GetValueOrDefault(item.Key);

                    if (stand is null)
                    {
                        // foreach (var item in _products)
                        // {
                        //     item.GetProperty()
                        // }
                        { customer.Review -= 2; continue; }
                    }
                    else if (stand.Count < item.Value)
                    {
                        customer.Review -= 1;

                        while (stand.Count != 0)
                        {
                            product = stand.Pop();

                            if (product.Status == VegetableStatus.Toxic)
                            {
                                customer.Review -= 5;
                                goto skip;
                            }
                            else if (product.Status == VegetableStatus.Bad) continue;

                            money += Convert.ToDecimal(product.Price);
                        }

                        _budget += money;
                        continue;
                    }

                    for (int i = 0; i < item.Value; i++)
                    {
                        product = stand.Pop();

                        if (product.Status == VegetableStatus.Toxic)
                        {
                            customer.Review -= 5;
                            goto skip;
                        }
                        else if (product.Status == VegetableStatus.Bad) continue;

                        money += Convert.ToDecimal(product.Price);
                    }

                    _budget += money;
                }

            skip:
                AverageRating = customer.Review;
            }
        }

        public void AddProducts(string fullname, int count)
        {
            var type = Type.GetType(fullname);

            if (!_stands.ContainsKey(type.Name)) _stands.Add(type.Name, new Stack<Vegetable>());

            var stand = _stands.GetValueOrDefault(type.Name);
            Vegetable product = null;

            for (int i = 0; i < count; i++)
            {
                product = Activator.CreateInstance(type) as Vegetable;
                product.Status = RandomService.RandomEnumElement(typeof(VegetableStatus)) as VegetableStatus?;
                _budget -= Convert.ToDecimal(product.Price);
                stand.Push(product);
            }
        }

        public void CalcProducts()
        {
            decimal money = _budget / 2;
            decimal part = money / _products.Count;

            _budget -= money;

            foreach (var item in _products) 
                AddProducts(item.Name, Convert.ToInt32(part / Convert.ToDecimal(item.GetField("price").GetValue(item))));
        }

        public void CheckProducts() => _workers[RandomService.RandomInteger(0, _workers.Count)].FilterStands(ref _stands);

        public Shop()
        {
            var vegetableType = typeof(Vegetable);
            foreach (var item in vegetableType.Assembly.GetTypes())
                if (item.BaseType?.Name == typeof(Vegetable).Name) _products.Add(item);

        }
    }
}
