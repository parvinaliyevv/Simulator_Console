using System;
using System.Timers;

namespace Exam.Models
{

    internal class Vegetable
    {
        public string Name { get; set; }
        public string Status { get; set; }

        private System.Timers.Timer StatusChanger { get; set; } = new(Exam.Services.RandomService.RandomInteger(72000, 144000));

        public Vegetable(string name, string status)
        {
            Name = name;
            Status = status;

            if (status == VegetableStatus.Good.ToString()) StatusChanger.Elapsed += ChangeFromGoodStatus;
            else if (status == VegetableStatus.Normal.ToString()) StatusChanger.Elapsed += ChangeFromNormalStatus;
            else
            {
                StatusChanger.Close();
                StatusChanger.Dispose();
                StatusChanger = null;
                return;
            }

            StatusChanger.AutoReset = true;
            StatusChanger.Start();
        }

        public void ChangeFromGoodStatus(Object source, ElapsedEventArgs e)
        {
            Status = VegetableStatus.Normal.ToString();
            StatusChanger.Elapsed -= ChangeFromGoodStatus;
            StatusChanger.Elapsed += ChangeFromNormalStatus;
        }

        public void ChangeFromNormalStatus(Object source, ElapsedEventArgs e)
        {
            Status = VegetableStatus.Bad.ToString();
            StatusChanger.Stop();
            StatusChanger.Close();
            StatusChanger.Dispose();
        }
    }
}
