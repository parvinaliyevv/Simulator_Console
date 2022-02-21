using System;
using System.Collections.Generic;

namespace Exam.Services
{
    public static class ExtensionService
    {
        public static int SumElementsByte(this List<byte> ratings)
        {
            int summ = default;
            
            foreach (byte item in ratings) summ += item;

            return summ;
        }
    }
}
