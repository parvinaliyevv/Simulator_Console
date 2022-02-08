using System;

namespace Exam.Models
{
    internal interface IRating
    {
        int GetAverageRating();

        void SetRating(int rate);
    }
}
