using System;

namespace Exam.Models
{
    public interface IWorker
    {
        public bool IsWorking { get; set; }

        public void Work();
    }
}
