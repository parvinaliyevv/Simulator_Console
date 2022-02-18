namespace Exam.Models
{
    [Serializable]
    public class Onion : Vegetable
    {
        [NonSerialized]
        public const string name = "Onion";

        private static List<decimal> rating { get; }
        public static decimal AverageRating
        {
            get { return (rating.Sum() / rating.Count); }
            set
            {
                if (value > 0 & value < 11) rating.Add(value);
                else throw new ArgumentOutOfRangeException(nameof(value), "The rating of vegetables cannot be less than zero and more than ten!");
            }
        }

        static Onion() => rating = new();

        public Onion(VegetableStatus status) : base(status) { }
    }
}
