namespace Exam.Models
{
    [Serializable]
    public sealed class Customer: Person
    {
        private List<Vegetable> Basket { get; } = new();

        public byte Review { get; set; }


        public void RandomBasket() { }

        public void RandomReview() { }
    }
}
