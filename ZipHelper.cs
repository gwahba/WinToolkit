using System;
using System.IO;
using System.IO.Compression;

namespace WinToolkit
{
	public class ZipHelper
	{
		public static void ZipDirectory(string dirctoryPath, string zipFilePath)
		{
			ZipFile.CreateFromDirectory(dirctoryPath, zipFilePath);
			if (!FileHelper.IsFileExists(zipFilePath)) return;
		}
		public static bool UnZipDirectory(string zipFilePath, string toDirctoryPath)
		{
			if (!FileHelper.IsFileExists(zipFilePath))
			{
				return false;
			}
			try
			{
				ZipFile.ExtractToDirectory(zipFilePath, toDirctoryPath);
			}
			catch (InvalidDataException exception)
			{
				throw;	// TODO: must change the return type to something significant (Success, FileNotFound, CorruptFile ... etc)
			}
			catch (Exception exception)
			{
				return false;
			}
			return true;
		}
	}
}
