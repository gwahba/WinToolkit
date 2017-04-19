using System;
using System.Windows.Threading;

namespace WinToolkit
{
	public static class MainThreadActionManager
	{
		private static Dispatcher _mainThreadDispatcher;

		public static void RegisterMainThreadDispatcher()
		{
			_mainThreadDispatcher = Dispatcher.CurrentDispatcher;
		}

		public static void ExecuteInMainContext(Action action)
		{
			Dispatcher dispatcher = _mainThreadDispatcher ?? Dispatcher.CurrentDispatcher;
			dispatcher.Invoke(action);
		}
	}
}
