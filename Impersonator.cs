using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace WinToolkit
{
	public class Impersonator : IDisposable
	{
		private readonly WindowsImpersonationContext _impersonatedUser = null;
		private readonly IntPtr _userHandle;
		public Impersonator(string user, string userDomain, string password)
		{
			_userHandle = new IntPtr(0);

			var returnValue = LogonUser(user, userDomain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref _userHandle);
			if (returnValue == 0)
			{
				StringBuilder msg = new StringBuilder(1000);
				FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, (uint)Marshal.GetLastWin32Error(), 0, msg, 1000, IntPtr.Zero);
				throw new ApplicationException("Could not impersonate user. Reported System Error: " + msg);
			}

			IntPtr dupeTokenHandle = IntPtr.Zero;
			bool retVal = DuplicateToken(_userHandle, SecurityImpersonation, ref dupeTokenHandle);
			if (false == retVal)
			{
				CloseHandle(_userHandle);
				throw new ApplicationException("Creating duplicate token failed. Reported System Error Code: " + Marshal.GetLastWin32Error());
			}

			WindowsIdentity newId = new WindowsIdentity(dupeTokenHandle);
			_impersonatedUser = newId.Impersonate();
		}
		#region IDisposable Members
		public void Dispose()
		{
			if (_impersonatedUser != null)
			{
				_impersonatedUser.Undo();
				CloseHandle(_userHandle);
			}
		}
		#endregion
		#region Interop imports/constants

		private const int LOGON32_LOGON_INTERACTIVE = 2;
		public const int LOGON32_LOGON_SERVICE = 3;
		private const int LOGON32_PROVIDER_DEFAULT = 0;
		private const int SecurityImpersonation = 2;
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int LogonUser(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool DuplicateToken(IntPtr existingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr duplicateTokenHandle);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize, IntPtr arguments);
		const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
		#endregion
	}

}
