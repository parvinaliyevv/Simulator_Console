using System;

namespace Exam.Services
{
    public static class RandomService
    {
        private static Random RandomProp { get; } = new Random();

        public static int RandomInteger(int min = 1, int max = 1000000) => RandomProp.Next(min, max);

        public static double RandomDouble() => RandomProp.NextDouble();

        public static object RandomEnumElement(Type data)
        {
            var arr = data.GetEnumValues();

            int total = default;
            foreach (var item in arr) total += ((int)item);

            int temp = RandomProp.Next(total);

            for (int i = arr.Length - 1, sum = 0; i >= 0; i--)
            {
                sum += Convert.ToInt32(arr.GetValue(i));
                if (temp < sum) return arr.GetValue(i);
            }

            return arr.GetValue(arr.Length - 1);
        }
    }
}
