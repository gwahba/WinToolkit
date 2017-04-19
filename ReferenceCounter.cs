namespace WinToolkit
{
	public class ReferenceCounter
	{
		private int _currentValue = 0;
		public int Reset()
		{
			_currentValue = 0;
			return _currentValue;
		}
		public int Decrement()
		{
			return --_currentValue;
		}
		public int Increment()
		{
			return ++_currentValue;
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
