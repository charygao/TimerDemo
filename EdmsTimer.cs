using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Edms.Helpers
{
    public class EdmsTimer : INotifyPropertyChanged
    {

        private Timer _timer;
        private int _eventCounter;
        private Stopwatch _stopwatch;
        public int SecondsBetweenEvents { get; set; }

        private int _secondsToCountDownPerSecond;

        public int SecondsToCountDownPerSecond
        {
            get { return _secondsToCountDownPerSecond; }
            set
            {
                _secondsToCountDownPerSecond = value;
                OnTimespanChanged(TaskTimeUsed);
            }
        }

        #region INotifyPropertyChanged RaisePropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IsPaused (INotifyPropertyChanged Property)

        private bool _IsPaused;

        public bool IsPaused
        {
            get { return _IsPaused; }
            set
            {
                if (_IsPaused == value)
                    return;
                _IsPaused = value;
                RaisePropertyChanged("IsPaused");
                OnHasBeenPaused(value);
            }
        }

        #endregion

        #region IsNegative (INotifyPropertyChanged Property)

        private bool _IsNegative;
        //是否可以超时
        //true：超过预设的时间可以继续
        //false：超过预设的时间则该任务结束
        public bool IsNegative
        {
            get { return _IsNegative; }
            set
            {
                if (_IsNegative == value)
                    return;
                _IsNegative = value;
                RaisePropertyChanged("IsNegative");

            }
        }

        #endregion

        #region IsEnable (INotifyPropertyChanged Property)

        private bool _isEnable;

        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (_isEnable.Equals(value)) return;
                _isEnable = value;
                RaisePropertyChanged("IsEnable");
            }
        }

        #endregion

        #region TaskTime (INotifyPropertyChanged Property)

        private TimeSpan _TaskTime;

        public TimeSpan TaskTime
        {
            get { return _TaskTime; }
            set
            {
                if (_TaskTime != null && _TaskTime.Equals(value)) return;
                _TaskTime = value;
                RaisePropertyChanged("TaskTime");

            }
        }

        #endregion TaskTime (INotifyPropertyChanged Property)

        #region TaskTimeUsed (INotifyPropertyChanged Property)

        private TimeSpan _TaskTimeUsed;

        public TimeSpan TaskTimeUsed
        {
            get { return _TaskTimeUsed; }
            set
            {
                if (_TaskTimeUsed != null && _TaskTimeUsed.Equals(value)) return;
                _TaskTimeUsed = value;
                RaisePropertyChanged("TaskTimeUsed");

            }
        }

        #endregion TaskTimeUsed (INotifyPropertyChanged Property)

        #region WarningTime (INotifyPropertyChanged Property)

        private TimeSpan _WarningTime;

        public TimeSpan WarningTime
        {
            get { return _WarningTime; }
            set
            {
                if (_WarningTime != null && _WarningTime.Equals(value)) return;
                _WarningTime = value;
                RaisePropertyChanged("WarningTime");

            }
        }

        #endregion

        #region TotalTaskTimeUsed (INotifyPropertyChanged Property)

        private TimeSpan _TotalTaskTimeUsed;

        public TimeSpan TotalTaskTimeUsed
        {
            get { return _TotalTaskTimeUsed; }
            set
            {
                if (_TotalTaskTimeUsed != null && _TotalTaskTimeUsed.Equals(value)) return;
                _TotalTaskTimeUsed = value;
                RaisePropertyChanged("TotalTaskTimeUsed");

            }
        }

        #endregion TotalTaskTimeUsed (INotifyPropertyChanged Property)

        #region CountDown (INotifyPropertyChanged Property)

        private bool _CountDown;

        public bool CountDown
        {
            get { return _CountDown; }
            set
            {
                if (_CountDown == value)
                    return;
                _CountDown = value;
                RaisePropertyChanged("CountDown");

            }
        }

        #endregion

        #region Context (INotifyPropertyChanged Property)

        private object _Context;

        public object Context
        {
            get { return _Context; }
            set
            {
                if (_Context != null && _Context.Equals(value)) return;
                _Context = value;
                RaisePropertyChanged("Context");

            }
        }

        #endregion

        #region TimespanChanged

        public delegate void TimespanChangedHandler(EdmsTimer sender, TimeSpan timeLeft);

        public event TimespanChangedHandler TimespanChanged;

        protected void OnTimespanChanged(TimeSpan timeLeft)
        {
            TimespanChanged?.Invoke(this, timeLeft);
        }

        #endregion

        #region HasBeenPaused

        public delegate void HasBeenPausedHandler(EdmsTimer sender, bool paused);

        public event HasBeenPausedHandler HasBeenPaused;

        protected void OnHasBeenPaused(bool paused)
        {
            if (paused)
            {
                if (_stopwatch.IsRunning)
                {
                    _stopwatch.Reset();
                }
            }
            else
            {
                _stopwatch.Restart();
            }

            HasBeenPaused?.Invoke(this, paused);
        }

        #endregion


        public EdmsTimer()
        {
            SecondsToCountDownPerSecond = 1;
            SecondsBetweenEvents = 1;
            _stopwatch = new Stopwatch();
        }

        public EdmsTimer(bool isPaused, object context, int secsondsBetweenEvents = 10) : this()
        {
            SecondsBetweenEvents = secsondsBetweenEvents;
            Context = context;
            IsPaused = isPaused;
        }

        public void Start()
        {
            //Timer is already running
            if (_timer != null) return;

            _timer = new Timer(TimerTick);
           
            _timer.Change(1000, 500);
            if (_stopwatch == null)
            {
                _stopwatch = new Stopwatch();
            }
            _stopwatch.Start();
        }

        public void Pause()
        {
            try
            {
                if (_timer != null && !IsPaused)
                {
                    IsPaused = true;
                }
            }
            catch (Exception exc)
            {
                //todo
            }
        }

        public void Resume()
        {
            try
            {
                if (_timer != null)
                {
                    if (IsPaused)
                    {

                        //_timer.Change(1000, 1000);
                        IsPaused = false;
                    }
                }
            }
            catch (Exception exc)
            {
                //todo
            }
        }

        public void Restart()
        {
            try
            {
                TaskTimeUsed += TaskTimeUsed;
                TaskTimeUsed = new TimeSpan(0);
                OnTimespanChanged(TaskTimeUsed);
            }
            catch (Exception exc)
            {
                //todo
            }
        }

        public void CleanUp()
        {
            try
            {
                if (_timer != null)
                {
                    IsPaused = true;
                    TaskTimeUsed = new TimeSpan(0);
                    _timer.Dispose();
                    _timer = null;
                    _stopwatch.Stop();
                    _stopwatch = null;
                }
            }
            catch (Exception exc)
            {
                //todo
            }
        }

        private void TimerTick(object state)
        {
            try
            {
                if (IsPaused) return;
                if (!_stopwatch.IsRunning) _stopwatch.Start();
                TimeSpan ticks = _stopwatch.Elapsed;
                _stopwatch.Restart();
                TaskTimeUsed =
                    TaskTimeUsed.Add(new TimeSpan(0, 0, 0, 0, (int)ticks.TotalMilliseconds * SecondsToCountDownPerSecond));

                if ((SecondsToCountDownPerSecond > 0 && _eventCounter >= SecondsBetweenEvents) ||
                    (TaskTime - TaskTimeUsed).TotalSeconds <= 0 && (TaskTime - TaskTimeUsed).TotalSeconds > -2)
                {
                    _eventCounter = 0;
                    OnTimespanChanged(TaskTime - TaskTimeUsed);
                }
                else
                {
                    _eventCounter++;
                }
            }
            catch (Exception exc)
            {
                //todo
            }
        }
    }
}
