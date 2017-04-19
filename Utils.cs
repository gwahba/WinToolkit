using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace WinToolkit
{
	public class Utils
	{
		public static Stream GenerateStreamFromString(String stringToStream)
		{
			MemoryStream streamToReturn = new MemoryStream();
			StreamWriter writer = new StreamWriter(streamToReturn);
			writer.Write(stringToStream);
			writer.Flush();
			streamToReturn.Position = 0;
			return streamToReturn;
		}
		public static PropertyInfo GetProperyFromObject(object objToTest, string propertyToCheck)
		{
			PropertyInfo requestedProperty = objToTest.GetType().GetProperty(propertyToCheck,
				BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
			return requestedProperty;
		}
		public static void RunApplication(string filePath)
		{
			System.Diagnostics.Process.Start(filePath);
		}
		public static bool CheckContainsArabicLetters(string inputstring)
		{
			Regex regex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
			MatchCollection matches = regex.Matches(inputstring);
			return matches.Count > 0;
		}
	}

	public class MutexLocker : IDisposable
	{
		public Mutex _mutex;
		public MutexLocker(Mutex mutex)
		{
			_mutex = mutex;
			_mutex.WaitOne();
		}
		public void Dispose()
		{
			_mutex.ReleaseMutex();
		}
	}
}
