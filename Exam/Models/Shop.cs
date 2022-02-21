using System;
using System.Timers;
using System.Collections.Generic;
using Exam.Services;

namespace Exam.Models
{
    [Serializable]
    public sealed class Shop
    {
        [NonSerialized] private Timer _statusTimer = new(Program.hour * 24 * 7);


        [NonSerialized] public List<Type> products = new();
        [NonSerialized] public Queue<Customer> customers = new();


        private List<byte> _ratings = new();
        public int AverageRating
        {
            get { return (_ratings.SumElementsByte() / ((_ratings.Count == 0) ? 1 : _ratings.Count)); }
            set { if (value > 2 & value <= 5) _ratings.Add(Convert.ToByte(value)); }
        }


        public List<Worker> workers = new();
        public Dictionary<string, Stack<Vegetable>> stands = new();


        public DateTime GameTime { get; set; }
        public Statistics HourStatistics { get; } = new();
        public Statistics WeekStatistics { get; set; } = new();


        private decimal _budget;
        public decimal Budget { get { return _budget; } set { if (value > 0) _budget = value; } }


        private ShopStatus? _status;
        public ShopStatus? Status
        {
            get { return _status; }
            set
            {
                _status = value;

                if (_statusTimer is null)
                {
                    _statusTimer = new(Program.hour * 24 * 7);

                    _statusTimer.AutoReset = true;
                    _statusTimer.Elapsed += ChangeShopStatus;
                    _statusTimer.Start();
                }
            }
        }


        public Shop()
        {
            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            var baseTypeName = typeof(Vegetable).Name;

            foreach (var item in types) if (item.BaseType?.Name == baseTypeName) products.Add(item);

            _statusTimer.AutoReset = true;
            _statusTimer.Elapsed += ChangeShopStatus;
            _statusTimer.Start();
        }

        public void EnqueueCustomers()
        {
            if (Status == ShopStatus.Epidemic) return;

            var random = RandomService.RandomInteger(0, (AverageRating == 0) ? 2 : AverageRating);

            for (int i = 0; i < random; i++)
            {
                try { customers.Enqueue(new Customer(ref products)); }
                catch (ArgumentNullException ex) { LegacyService.MessageBox(IntPtr.Zero, ex.Message, "Parameter is null!", default); }
            }

            HourStatistics.CustomersCount += customers.Count;
        }
        public void DequeueCustomers()
        {
            if (Status == ShopStatus.Epidemic | customers.Count == 0) return;

            Stack<Vegetable> stand = null;
            Customer customer = null;
            Vegetable product = null;
            decimal money = default;
            byte review = default;
            bool flag = false;

            while (customers.Count != 0)
            {
                customer = customers.Dequeue();
                money = default;
                flag = false;

                foreach (KeyValuePair<string, int> item in customer.basket)
                {
                    review = default;
                    stand = stands.GetValueOrDefault(item.Key);

                    if (stand is null) review = 3;
                    else if (stand.Count < item.Value)
                    {
                        customer.Review = 4;
                        review = 4;

                        while (stand.Count != 0)
                        {
                            product = stand.Pop();

                            if (product.Status == VegetableStatus.Toxic)
                            {
                                flag = true;
                                HourStatistics.WasteProductCount++;
                                customer.Review = 3;
                                break;
                            }
                            else if (product.Status == VegetableStatus.Bad) { HourStatistics.WasteProductCount++; continue; }

                            money += Convert.ToDecimal(product.Price * 1.7);
                        }
                    }
                    else
                    {
                        customer.Review = 5;
                        review = 5;

                        for (int i = 0; i < item.Value; i++)
                        {
                            product = stand.Pop();

                            if (product.Status == VegetableStatus.Toxic)
                            {
                                flag = true;
                                HourStatistics.WasteProductCount++;
                                customer.Review = 3;
                                break;
                            }
                            else if (product.Status == VegetableStatus.Bad) { HourStatistics.WasteProductCount++; continue; }

                            money += Convert.ToDecimal(product.Price * 1.7);
                        }
                    }

                    foreach (var types in products)
                    {
                        if (types.Name.Equals(item.Key))
                        {
                            types.GetProperty("AverageRating").SetValue(item, review);
                            break;
                        }

                    }

                    if (flag) break;

                    Budget += money;
                    HourStatistics.Earnings += money;
                }

                this.AverageRating = customer.Review;
            }
        }

        public void AddProducts(Type type, int count)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            else if (count == 0) return;

            if (!stands.ContainsKey(type.Name)) stands.Add(type.Name, new Stack<Vegetable>());

            var stand = stands.GetValueOrDefault(type.Name);
            Vegetable product = null;

            HourStatistics.PurchasedProductCount += count;

            for (int i = 0; i < count; i++)
            {
                product = null;

                product = Activator.CreateInstance(type) as Vegetable;
                product.Status = RandomService.RandomEnumElement(typeof(VegetableStatus)) as VegetableStatus?;

                Budget -= Convert.ToDecimal(product.Price);
                HourStatistics.Earnings -= Convert.ToDecimal(product.Price);

                stand.Push(product);
            }
        }
        public void BuyProducts()
        {
            decimal money = Budget / 2;
            decimal part = money / products.Count;
            Stack<Vegetable> stand = null;

            foreach (var item in products)
            {
                stand = stands.GetValueOrDefault(item.Name);
                if (stand != null)
                {
                    if (stand.Count > 5000) continue;
                }

                try { AddProducts(item, Convert.ToInt32(part / Convert.ToDecimal(item.GetField("price").GetValue(null)))); }
                catch (ArgumentNullException ex) { LegacyService.MessageBox(IntPtr.Zero, ex.Message, "Parameter is null!", default); }
            }

            CheckProducts();
        }

        public void CheckProducts()
        {
            try { HourStatistics.WasteProductCount += workers[RandomService.RandomInteger(0, workers.Count)].FilterStands(ref stands); }
            catch (ArgumentNullException ex) { LegacyService.MessageBox(IntPtr.Zero, ex.Message, "Parameter is null!", default); }
            catch (ArgumentOutOfRangeException ex) { LegacyService.MessageBox(IntPtr.Zero, ex.Message, "Parameter is out of range!", default); }
        }
        public void ChangeShopStatus(object source, EventArgs e) => Status = RandomService.RandomEnumElement(typeof(ShopStatus)) as ShopStatus?;
    }
}
