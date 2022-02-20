using System;
using System.Timers;

namespace Exam.Models
{
    [Serializable]
    public abstract class Vegetable
    {
        [NonSerialized]
        private System.Timers.Timer _statusChanger = new(Exam.Services.RandomService.RandomInteger(72000, 144000));

        private VegetableStatus? _status;
        public VegetableStatus? Status
        {
            get { return _status; }
            set
            {
                _status = value;

                if (value == VegetableStatus.Good) _statusChanger.Elapsed += ChangeFromGoodStatus;
                else if (value == VegetableStatus.Normal) _statusChanger.Elapsed += ChangeFromNormalStatus;
                else if (value == VegetableStatus.Bad) _statusChanger.Elapsed += ChangeFromBadStatus;
                else
                {
                    _statusChanger.Dispose();
                    _statusChanger = null;
                    return;
                }

                _statusChanger.AutoReset = true;
                _statusChanger.Start();
            }
        }

        public virtual double Price { get; }

        public Vegetable() { }
        public Vegetable(VegetableStatus status) => Status = status;

        public void ChangeFromGoodStatus(object source, ElapsedEventArgs e)
        {
            _status = VegetableStatus.Normal;
            _statusChanger.Elapsed -= ChangeFromGoodStatus;
            _statusChanger.Elapsed += ChangeFromNormalStatus;
        }
        public void ChangeFromNormalStatus(object source, ElapsedEventArgs e)
        {
            _status = VegetableStatus.Bad;
            _statusChanger.Elapsed -= ChangeFromNormalStatus;
            _statusChanger.Elapsed += ChangeFromBadStatus;

        }
        public void ChangeFromBadStatus(object source, ElapsedEventArgs e)
        {
            _status = VegetableStatus.Toxic;
            _statusChanger.Dispose();
        }
    }
}
