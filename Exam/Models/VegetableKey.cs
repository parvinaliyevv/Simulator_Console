using System;

namespace Exam.Models
{
    internal class VegetableKey : IRating
    {
        public string Name { get; set; }
        private List<int> Ratings { get; } = new();

        public VegetableKey(string name) => Name = name;

        public int GetAverageRating() => Ratings.Sum() / Ratings.Count;

        public void SetRating(int rate)
        {
            if (rate > 0 & rate < 11) Ratings.Add(rate);
            else throw new ArgumentOutOfRangeException(nameof(rate), "The rating of vegetables cannot be less than zero and more than ten!");
        }

        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object? obj) => this.Name.Equals((obj as VegetableKey)?.Name);
    }
}
