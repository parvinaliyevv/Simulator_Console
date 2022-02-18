namespace Exam.Models
{
    [Serializable]
    public class Potato : Vegetable
    {
        [NonSerialized]
        public const string name = "Potato";
        
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

        static Potato() => rating = new();

        public Potato(VegetableStatus status) : base(status) { }
    }
}
