using System;
using System.Timers;
using Exam.Services;

namespace Exam.Models
{
    [Serializable]
    public abstract class Vegetable
    {
        [NonSerialized] public Timer _statusTimer;


        private VegetableStatus? _status;
        public VegetableStatus? Status
        {
            get { return _status; }
            set
            {
                _status = value;

                if (_statusTimer is null)
                {
                    _statusTimer = new(RandomService.RandomInteger(Program.hour * 24 * 2, Program.hour * 24 * 4));

                    if (value == VegetableStatus.Good) _statusTimer.Elapsed += ChangeFromGoodStatus;
                    else if (value == VegetableStatus.Normal) _statusTimer.Elapsed += ChangeFromNormalStatus;
                    else if (value == VegetableStatus.Bad) _statusTimer.Elapsed += ChangeFromBadStatus;
                    else
                    {
                        _statusTimer.Dispose();
                        _statusTimer = null;
                        return;
                    }

                    _statusTimer.AutoReset = true;
                    _statusTimer.Start();
                }
            }
        }


        public virtual double Price { get; }


        public Vegetable() { }
        public Vegetable(VegetableStatus status) => Status = status;

        private void ChangeFromGoodStatus(object source, ElapsedEventArgs e)
        {
            _status = VegetableStatus.Normal;
            _statusTimer.Elapsed -= ChangeFromGoodStatus;
            _statusTimer.Elapsed += ChangeFromNormalStatus;
        }
        private void ChangeFromNormalStatus(object source, ElapsedEventArgs e)
        {
            _status = VegetableStatus.Bad;
            _statusTimer.Elapsed -= ChangeFromNormalStatus;
            _statusTimer.Elapsed += ChangeFromBadStatus;

        }
        private void ChangeFromBadStatus(object source, ElapsedEventArgs e)
        {
            _status = VegetableStatus.Toxic;
            _statusTimer.Stop();
            _statusTimer.Dispose();
            _statusTimer = null;
        }
    }
}
