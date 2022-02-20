using System;
using System.Linq;
using System.Collections.Generic;
using Exam.Services;

namespace Exam.Models
{
    [Serializable]
    public sealed class Shop
    {
        private List<int> _ratings = new();
        public int AverageRating
        {
            get { return (_ratings.Sum() / ((_ratings.Count != 0) ? _ratings.Count : 1)); }
            set { if (value > 2 & value < 11) _ratings.Add(value); }
        }

        [NonSerialized] public List<Type> products = new();
        public List<Worker> workers = new();
        public Queue<Customer> customers = new();
        public Dictionary<string, Stack<Vegetable>> stands = new();

        [NonSerialized] private System.Timers.Timer _statusTimer = new System.Timers.Timer(Program._refresh * 24 * 7); 
        private decimal _budget = default;
        private ShopStatus? _status = default;

        public ShopStatus? Status
        {
            get { return _status; }
            set
            {
                if (_statusTimer is null) _statusTimer = new();
                _status = value;
                _statusTimer.AutoReset = true;
                _statusTimer.Elapsed += ChangeStatus;
                _statusTimer.Start();
            }
        }
        public decimal Budget { get { return _budget; } set { if (value > 0) _budget = value; } }
        public DateTime GameTime { get; set; } = default;
        public Statistics HourStatistics { get; } = new();
        public Statistics WeekStatistics { get; set; } = new();

        public void EnqueueCustomers()
        {
            if (Status == ShopStatus.Epidemic) return;

            for (int i = 0; i < RandomService.RandomInteger(1, AverageRating); i++)
                customers.Enqueue(new Customer(ref products));
        }
        public void DequeueCustomers()
        {
            if (Status == ShopStatus.Epidemic) return;
            HourStatistics.CustomersCount += customers.Count;

            Customer customer = null;
            Stack<Vegetable> stand = null;
            Vegetable product = null;
            decimal money = default;

            while (customers.Count != 0)
            {
                customer = customers.Dequeue();
                money = default;

                foreach (KeyValuePair<string, int> item in customer._basket)
                {
                    stand = stands.GetValueOrDefault(item.Key);

                    if (stand is null)
                    {
                        foreach (var types in products)
                        {
                            if (types.Name.Equals(item.Key))
                            {
                                types.GetProperty("AverageRating").SetValue(item, 3);
                                break;
                            }
                                
                        }
                        continue;
                    }
                    else if (stand.Count < item.Value)
                    {
                        foreach (var types in products)
                        {
                            if (types.Name.Equals(item.Key))
                                types.GetProperty("AverageRating").SetValue(item, 7);
                        }

                        customer.Review -= 1;

                        while (stand.Count != 0)
                        {
                            product = stand.Pop();

                            if (product.Status == VegetableStatus.Toxic)
                            {
                                HourStatistics.GarbageCount++;
                                customer.Review -= 5;
                                goto skip;
                            }
                            else if (product.Status == VegetableStatus.Bad) { HourStatistics.GarbageCount++; continue; }

                            money += Convert.ToDecimal(product.Price);
                        }

                        _budget += money;
                        continue;
                    }

                    foreach (var types in products)
                    {
                        if (types.Name.Equals(item.Key))
                            types.GetProperty("AverageRating").SetValue(item, 10);
                    }

                    for (int i = 0; i < item.Value; i++)
                    {
                        product = stand.Pop();

                        if (product.Status == VegetableStatus.Toxic)
                        {
                            HourStatistics.GarbageCount++;
                            customer.Review -= 5;
                            goto skip;
                        }
                        else if (product.Status == VegetableStatus.Bad) { HourStatistics.GarbageCount++; continue; }

                        money += Convert.ToDecimal(product.Price);
                    }

                    _budget += money;
                    HourStatistics.Earnings += money;
                }

            skip:
                AverageRating = customer.Review;
            }
        }

        public void AddProducts(string name, int count)
        {
            Type type = null;

            foreach (var item in products) if (item.Name == name) type = item;

            if (!stands.ContainsKey(type.Name)) stands.Add(type.Name, new Stack<Vegetable>());

            var stand = stands.GetValueOrDefault(type.Name);
            Vegetable product = null;

            HourStatistics.PurchasedProductCount += count;

            for (int i = 0; i < count; i++)
            {
                product = Activator.CreateInstance(type) as Vegetable;
                product.Status = RandomService.RandomEnumElement(typeof(VegetableStatus)) as VegetableStatus?;
                _budget -= Convert.ToDecimal(product.Price);
                HourStatistics.Earnings -= Convert.ToDecimal(product.Price);
                stand.Push(product);
            }
        }
        public void BuyProducts()
        {
            decimal money = _budget / 2;
            decimal part = money / products.Count;

            foreach (var item in products) 
                AddProducts(item.Name, Convert.ToInt32(part / Convert.ToDecimal(item.GetField("price").GetValue(item))));

            CheckProducts();
        }

        public void CheckProducts() => HourStatistics.GarbageCount += workers[RandomService.RandomInteger(0, workers.Count)].FilterStands(ref stands);
        public void ChangeStatus(object source, EventArgs e) => Status = RandomService.RandomEnumElement(typeof(ShopStatus)) as ShopStatus?;

        public Shop()
        {
            var vegetableType = typeof(Vegetable);
            foreach (var item in vegetableType.Assembly.GetTypes())
                if (item.BaseType?.Name == typeof(Vegetable).Name) products.Add(item);
        }
    }
}
