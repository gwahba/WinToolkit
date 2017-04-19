using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


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
		public static Process RunningInstance()
		{
			Process current = Process.GetCurrentProcess();
			Process[] processes = Process.GetProcessesByName(current.ProcessName);
			foreach (Process process in processes)
			{
				//Ignore the current process 
				if (process.Id == current.Id) continue;

				//MessageBox.Show("Application Already running you will be switched to the running application","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				SetForegroundWindow(process.MainWindowHandle);
				ShowWindow(process.MainWindowHandle, SW_SHOWMAXIMIZED);

				Environment.Exit(0);
			}
			//No other instance was found, return null.  
			return null;
		}

		public static string GetRunningApplicationName()
		{
			return (Process.GetCurrentProcess() == null) ? "" : Process.GetCurrentProcess().ProcessName;
		}
		public static int GetCurrentProcessId()
		{
			Process current = Process.GetCurrentProcess();
			return current.Id;
		}
	}
}
