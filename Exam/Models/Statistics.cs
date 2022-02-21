using System;

namespace Exam.Models
{
    [Serializable]
    public class Statistics
    {
        [Newtonsoft.Json.JsonIgnore] public int day;

        public int PurchasedProductCount { get; set; }
        public int WasteProductCount { get; set; }
        public int CustomersCount { get; set; }
        public decimal Earnings { get; set; }

        public Statistics() => Reset();

        public void Add(Statistics statistics)
        {
            this.PurchasedProductCount += statistics.PurchasedProductCount;
            this.WasteProductCount += statistics.WasteProductCount;
            this.CustomersCount += statistics.CustomersCount;
            this.Earnings += statistics.Earnings;
        }

        public void Reset()
        {
            day = default;
            PurchasedProductCount = default;
            WasteProductCount = default;
            CustomersCount = default;
            Earnings = default;
        }

        public override string ToString()
        {
            return String.Format("1) Purchased Product Count: {0}\n2) Waste Products Count: {1}\n3) Customer Count: {2}\n4) Earnings: {3} $"
                , PurchasedProductCount, WasteProductCount, CustomersCount, Earnings);
        }
    }
}
