using System;

namespace Exam.Models
{
    [Serializable]
    public class Statistics
    {
        public int Day { get; set; }
        public int PurchasedProductCount { get; set; }
        public int CustomersCount { get; set; }
        public int GarbageCount { get; set; }
        public decimal Earnings { get; set; }

        public Statistics() { }

        public void Add(Statistics statistics)
        {
            this.CustomersCount += statistics.CustomersCount;
            this.Earnings += statistics.Earnings;
            this.GarbageCount += statistics.GarbageCount;
            this.PurchasedProductCount += statistics.PurchasedProductCount;
        }

        public void Reset()
        {
            Day = default;
            CustomersCount = default;
            Earnings = default;
            GarbageCount = default;
            PurchasedProductCount = default;
        }
    }
}
