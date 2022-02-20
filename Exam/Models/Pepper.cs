using System;
using System.Linq;
using System.Collections.Generic;

namespace Exam.Models
{
    [Serializable]
    public sealed class Pepper : Vegetable
    {
        [NonSerialized]
        public const string name = nameof(Pepper);

        [NonSerialized]
        public const double price = 0.60;

        public override double Price { get { return Pepper.price; } }

        private static List<int> _ratings = new();
        public static int AverageRating
        {
            get { return (_ratings.Sum() / _ratings.Count); }
            set
            {
                if (value > 2 & value < 11) _ratings.Add(value);
                else throw new ArgumentOutOfRangeException(nameof(value), "The rating of vegetables cannot be less than two and more than ten!");
            }
        }

        public Pepper() { }
        public Pepper(VegetableStatus status) : base(status) { }
    }
}
