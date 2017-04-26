using System.Threading;

namespace WinToolkit
{
	public class ReferenceCounter
	{
		private readonly Mutex _lock = new Mutex();
		private int _currentValue = 0;
		public int Reset()
		{
			using (new MutexLocker(_lock))
			{
				_currentValue = 0;
				return _currentValue;
			}
		}
		public int Decrement()
		{
			using (new MutexLocker(_lock))
			{
				return --_currentValue;
			}
		}
		public int Increment()
		{
			using (new MutexLocker(_lock))
			{
				return ++_currentValue;
			}
		}
		public bool IsNoReferences
		{
			get
			{
				return _currentValue == 0;
			}
		}
	}
}
