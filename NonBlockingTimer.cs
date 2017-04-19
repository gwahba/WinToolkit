using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinToolkit
{
	/// <summary>
	/// Not Accurate Timer
	/// </summary>
	public class NonBlockingTimer : IDisposable
	{
		private bool _isStarted = false;
		private Thread _thread;

		public bool Start(TimerTickDelegate timerTickFunction, int period)
		{
			TimerTick += timerTickFunction;
			Period = period;
			return Start();
		}
		public bool Start()
		{
			if (TimerTick == null || Period <= 0) return false;
			if (_isStarted) return true;
			_isStarted = true;

			//_thread = new Thread(new ThreadStart(TimerLoap));
			_thread = new Thread(TimerLoap);
			_thread.IsBackground = true;
			//_thread = new Thread(() =>this.TimerLoap());
			_thread.Start();
			return true;
		}
		public void End()
		{
			_isStarted = false;
		}

		private void TimerLoap()
		{
			while (_isStarted)
			{
				if (_isDisposed) return;
				if (Enabled) OnTimerTick();
				if (!_isStarted) return;
				Thread.Sleep(Period);
			}
		}

		private Mutex _resetMutex;
		protected virtual void OnTimerTick()
		{
			if (_resetMutex != null)
			{
				using (new MutexLocker(_resetMutex))
				{
					if (_reset)
					{
						_reset = false;
						return;
					}
				}
			}

			TimerTickDelegate handler = TimerTick;
			if (handler != null) handler();
		}

		/// <summary>
		/// Define Period in Milli second
		/// </summary>
		public int Period { get; set; }

		private bool _enabled = true;
		public bool Enabled { get { return _enabled; } set { _enabled = value; } }

		public delegate void TimerTickDelegate();
		public event TimerTickDelegate TimerTick;

		public void Reset()
		{
			if (_resetMutex == null)
				_resetMutex = new Mutex();
			using (new MutexLocker(_resetMutex))
			{
				_reset = true;
			}
		}

		private bool _reset;
		private bool _isDisposed = false;
		public void Dispose()
		{
			_isDisposed = true;
		}
	}
}
