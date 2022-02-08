namespace Exam.Services
{
    public static class RandomService
    {
        private static Random RandomProp { get; } = new Random();

        public static int RandomInteger(int min = 1, int max = 1000000) => RandomProp.Next(min, max);

        public static double RandomDouble() => RandomProp.NextDouble();

        public static int RandomPercentIndex(int[] arr)
        {
            int total = arr.Sum();
            int temp = RandomProp.Next(total);

            for (int i = 0, sum = 0; i < arr.Length; i++)
            {
                sum += arr[i];
                if (temp < sum) return i;
            }

            return arr.Length - 1;
        }
    }
}
