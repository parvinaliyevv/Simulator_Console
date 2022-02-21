using System;
using System.Collections.Generic;
using Exam.Services;

namespace Exam.Models
{
    [Serializable]
    public sealed class Onion : Vegetable
    {
        [NonSerialized]
        public const string name = nameof(Onion);


        [NonSerialized]
        public const double price = 0.20;
        public override double Price { get { return price; } }


        private static List<byte> _ratings = new();
        public static int AverageRating
        {
            get { return (_ratings.SumElementsByte() / ((_ratings.Count == 0) ? 1 : _ratings.Count)); }
            set { if (value > 2 & value <= 5) _ratings.Add(Convert.ToByte(value)); }
        }


        public Onion() { }
        public Onion(VegetableStatus status) : base(status) { }
    }
}
