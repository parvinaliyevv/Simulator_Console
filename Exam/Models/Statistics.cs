using System;

namespace Exam.Models
{
    [Serializable]
    public abstract class Statistics
    {
        public byte Day { get; set; }
        public int SelledProductsCount { get; set; }
        public int WasteCount { get; set; }
        public int CustomerCount { get; set; }
        public int Earnings { get; set; }
    }
}
