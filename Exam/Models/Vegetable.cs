using System;
using System.Timers;
using Exam.Services;

namespace Exam.Models
{
    [Serializable]
    public class Vegetable
    {
        [NonSerialized]
        private System.Timers.Timer StatusChanger = new(RandomService.RandomInteger(72000, 144000));

        public VegetableStatus Status { get; set; }

        public Vegetable(VegetableStatus status)
        {
            Status = status;

            if (status == VegetableStatus.Good) StatusChanger.Elapsed += ChangeFromGoodStatus;
            else if (status == VegetableStatus.Normal) StatusChanger.Elapsed += ChangeFromNormalStatus;
            else if (status == VegetableStatus.Bad) StatusChanger.Elapsed += ChangeFromBadStatus;
            else
            {
                StatusChanger.Dispose();
                StatusChanger = null;
                return;
            }

            StatusChanger.AutoReset = true;
            StatusChanger.Start();
        }

        public void ChangeFromGoodStatus(object source, ElapsedEventArgs e)
        {
            Status = VegetableStatus.Normal;
            StatusChanger.Elapsed -= ChangeFromGoodStatus;
            StatusChanger.Elapsed += ChangeFromNormalStatus;
        }
        public void ChangeFromNormalStatus(object source, ElapsedEventArgs e)
        {
            Status = VegetableStatus.Bad;
            StatusChanger.Elapsed -= ChangeFromNormalStatus;
            StatusChanger.Elapsed += ChangeFromBadStatus;

        }
        public void ChangeFromBadStatus(object source, ElapsedEventArgs e)
        {
            Status = VegetableStatus.Toxic;
            StatusChanger.Dispose();
        }
    }
}
