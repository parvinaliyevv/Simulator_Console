namespace Exam.Models
{
    [Serializable]
    public class Mushroom : Vegetable
    {
        [NonSerialized]
        public const string name = "Mushroom";

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

        static Mushroom() => rating = new();

        public Mushroom(VegetableStatus status) : base(status) { }
    }
}
