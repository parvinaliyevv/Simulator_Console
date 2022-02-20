using System;
using System.Linq;
using System.Collections.Generic;

namespace Exam.Models
{
    [Serializable]
    public sealed class Tomato : Vegetable
    {
        [NonSerialized]
        public const string name = nameof(Tomato);

        [NonSerialized]
        public const double price = 0.70;

        public override double Price { get { return Tomato.price; } }

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

        public Tomato() { }
        public Tomato(VegetableStatus status) : base(status) { }
    }
}
