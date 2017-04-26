using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinToolkit
{
	public class AppSingleInstanceChecker
	{
		[DllImport("User32.DLL")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean SetForegroundWindow(IntPtr hWnd);
		private const int SW_SHOWMAXIMIZED = 3;
		public static Process CheckRunningInstance(bool showWindowIfRunning, int exitCodeIfExists = 0)
		{
			Process current = Process.GetCurrentProcess();
			Process[] processes = Process.GetProcessesByName(current.ProcessName);
			foreach (Process process in processes)
			{
				//Ignore the current process 
				if (process.Id == current.Id) continue;

				if (showWindowIfRunning)
				{
					SetForegroundWindow(process.MainWindowHandle);
					ShowWindow(process.MainWindowHandle, SW_SHOWMAXIMIZED);
				}

				Environment.Exit(exitCodeIfExists);
			}
			//No other instance was found, return null
			return null;
		}

		public static string GetRunningApplicationName()
		{
			return Process.GetCurrentProcess().ProcessName;
		}
		public static int GetCurrentProcessId()
		{
			Process current = Process.GetCurrentProcess();
			return current.Id;
		}
	}
}
