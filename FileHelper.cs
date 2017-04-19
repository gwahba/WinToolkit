using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WinToolkit
{
	public enum FileAction
	{
		Copy = 0,
		Move = 1, Cut = 1,
	}
	public class FileHelper
	{
		private static bool Save(string sourceFilePath, string destinationFilePath, bool overWrite)
		{
			try
			{
				File.Copy(sourceFilePath, destinationFilePath, overWrite);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static string GetParentDirectory(string sourceFilePath, int levels = 1)
		{
			DirectoryInfo d = new DirectoryInfo(sourceFilePath);
			DirectoryInfo upDirectory;

			for (int i = 0; i < levels; i++)
			{
				d = d.Parent;
			}
			return d.FullName;
		}
		private static bool SaveInDirectory(string sourceFilePath, string destinationDirectory, bool overWrite)
		{
			string fileName = Path.GetFileName(sourceFilePath);
			string destinationFilePath = Path.Combine(destinationDirectory, fileName);
			return Save(sourceFilePath, destinationFilePath, overWrite);
		}

		private static bool SaveInDirectory(string[] sourceFilePaths, string destinationDirectory, bool overWrite)
		{
			List<string> failedFiles = new List<string>();
			foreach (string sourceFilePath in sourceFilePaths)
			{
				if (!SaveInDirectory(sourceFilePath, destinationDirectory, overWrite))
					failedFiles.Add(sourceFilePath);
			}
			return failedFiles.Count == 0;
		}

		private static bool SaveInDirectory(List<string> sourceFilePaths, string destinationDirectory, bool overWrite)
		{
			List<string> failedFiles =
				sourceFilePaths.Where(sourceFilePath => !SaveInDirectory(sourceFilePath, destinationDirectory, overWrite)).ToList();
			return failedFiles.Count == 0;
		}

		private static bool SaveInDirectory(Dictionary<string, string> sourceFilePaths, string destinationDirectory, bool overWrite)
		{
			List<string> failedFiles = new List<string>();
			foreach (var sourceFilePath in sourceFilePaths)
			{
				if (!SaveInDirectory(sourceFilePath.Value, destinationDirectory, overWrite))
					failedFiles.Add(sourceFilePath.Value);
			}
			return failedFiles.Count == 0;
		}

		public static bool SaveFiles(Dictionary<string, string> sourceFilePaths, bool overWrite)
		{
			if (sourceFilePaths == null || sourceFilePaths.Count == 0) return false;
			string destinationDirectory = GetDirectoryToSaveIn();
			return destinationDirectory != null && SaveInDirectory(sourceFilePaths, destinationDirectory, overWrite);
		}

		public static bool SaveFile(string sourceFilePath, bool overWrite)
		{
			if (sourceFilePath == null) return false;
			string destinationDirectory = GetDirectoryToSaveIn();
			return destinationDirectory != null && SaveInDirectory(sourceFilePath, destinationDirectory, overWrite);
		}

		public static string GetDirectoryToSaveIn()
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

			if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
				return null;
			return folderBrowserDialog.SelectedPath;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dirPath"></param>
		/// <returns>error Message</returns>
		public static string CreateDirectoryIfNotExists(string dirPath)
		{
			if (string.IsNullOrEmpty(dirPath) || string.IsNullOrWhiteSpace(dirPath)) return "Need to create Directory of Null Value";
			if (Directory.Exists(dirPath)) return null;
			try
			{
				Directory.CreateDirectory(dirPath);
			}
			catch (System.Exception exp)
			{
				return exp.Message;
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dirPath"></param>
		/// <returns>error Message</returns>
		public static string CopyFiles(string sourceDirectory, string destinationDirectory, bool isDeleteIfExist = false)
		{
			return ActFiles(sourceDirectory, destinationDirectory, FileAction.Copy, isDeleteIfExist);
		}
		public static bool CopyFile(string sourceFileName, string destinationFileName, bool isDeleteIfExist = false)
		{
			return ActOnFile(FileAction.Copy, true, destinationFileName, sourceFileName);
		}

		public static string MoveFiles(string sourceDirectory, string destinationDirectory)
		{
			return ActFiles(sourceDirectory, destinationDirectory, FileAction.Move);
		}

		public static string ActFiles(string sourceDirectory, string destinationDirectory, FileAction fileAction, bool isDeleteIfExist = false)
		{
			if (System.IO.Directory.Exists(sourceDirectory))
			{
				string[] files = System.IO.Directory.GetFiles(sourceDirectory);

				// Copy the files and overwrite destination files if they already exist.
				foreach (string sourcefile in files)
				{

					// Use static Path methods to extract only the file name from the path.
					string fileName = System.IO.Path.GetFileName(sourcefile);
					string destFile = System.IO.Path.Combine(destinationDirectory, fileName);
					ActOnFile(fileAction, isDeleteIfExist, destFile, sourcefile);
				}
			}
			else
			{
				return "Source path '" + sourceDirectory + "' does not exist!";
			}
			return null;
		}

		private static bool ActOnFile(FileAction fileAction, bool isDeleteIfExist, string destFile, string sourcefile)
		{
			try
			{
				if (IsFileExists(destFile) && isDeleteIfExist) FileHelper.DeleteFile(destFile);
				switch (fileAction)
				{
					case FileAction.Copy:
						System.IO.File.Copy(sourcefile, destFile, true);
						break;
					case FileAction.Move:
						File.Move(sourcefile, destFile);
						break;
				}
				return true;
			}
			catch (Exception e)
			{
				return false;
			}

		}

		public static string[] GetAllFiles(string directory)
		{
			return String.IsNullOrEmpty(directory) ? null : System.IO.Directory.GetFiles(directory);
		}

		public static bool IsFileLocked(string filePath)
		{
			//check that problem is not in destination file
			if (!IsFileExists(filePath)) return false;
			FileStream stream = null;
			try
			{
				stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}
			catch (Exception ex2)
			{
				return true;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}
			return false;
		}

		public static bool IsFileExists(string filePath)
		{
			return !string.IsNullOrEmpty(filePath) && !string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath);
		}
		private static void EmptyDirectory(DirectoryInfo directoryInfo, bool recursiveDelete)
		{
			foreach (FileInfo file in directoryInfo.GetFiles())
				try
				{
					file.Delete();
				}
				catch (Exception exception)
				{
				}

			if (!recursiveDelete) return;
			foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
				try
				{
					subfolder.Delete(true);
				}
				catch (Exception exception)
				{
				}
		}
		public static bool DeleteDirectory(string directoryPath, bool isEmptyDirecotryOnly = false)
		{
			if (!Directory.Exists(directoryPath)) return false;
			try
			{
				EmptyDirectory(new DirectoryInfo(directoryPath), true);
				if (!isEmptyDirecotryOnly)
					Directory.Delete(directoryPath);
			}
			catch (Exception e)
			{
				return false;
			}
			return true;
		}

		public static bool DeleteFile(string filePath)
		{
			if (IsFileExists(filePath))
			{
				File.Delete(filePath);
				return true;
			}
			return false;
		}

		public static bool ClearDirectoryFiles(String directoryPathStr)
		{
			if (!Directory.Exists(directoryPathStr)) return false;
			DirectoryInfo di = new DirectoryInfo(directoryPathStr);

			EmptyDirectory(di, false);

			return true;
		}

		public static string GetFileName(string fullPath, bool withoutExtenstion = false)
		{
			if (!withoutExtenstion)
				return Path.GetFileName(fullPath);
			return Path.GetFileNameWithoutExtension(fullPath);
		}

		public static string GetDirectoryPath(string fileFullPath)
		{
			string filename = fileFullPath;
			FileInfo fileInfo = new FileInfo(filename);
			string directoryFullPath = fileInfo.DirectoryName; // contains "C:\MyDirectory"
			return directoryFullPath;
		}
		public static bool CopyDirectory(string sourceDirectory, string targetDirectory)
		{
			DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
			DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);
			try
			{
				return CopyDirectoryInfo(diSource, diTarget);
			}
			catch (Exception e)
			{
				return false;
			}

		}
		public static long GetDirectorySize(string path)
		{
			if (!Directory.Exists(path)) return 0;
			// 1.
			// Get array of all file names.
			string[] a = Directory.GetFiles(path, "*.*");

			// 2.
			// Calculate total bytes of all files in a loop.
			long b = 0;
			foreach (string name in a)
			{
				// 3.
				// Use FileInfo to get length of each file.
				FileInfo info = new FileInfo(name);
				b += info.Length;
			}
			// 4.
			// Return total size
			return b;
		}

		public static bool CopyDirectoryInfo(DirectoryInfo source, DirectoryInfo target)
		{
			// Check if the target directory exists; if not, create it.
			if (Directory.Exists(target.FullName) == false)
			{
				Directory.CreateDirectory(target.FullName);
			}

			// Copy each file into the new directory.

			foreach (FileInfo fi in source.GetFiles())
			{
				//Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
				fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
			{
				DirectoryInfo nextTargetSubDir =
					target.CreateSubdirectory(diSourceSubDir.Name);
				CopyDirectoryInfo(diSourceSubDir, nextTargetSubDir);
			}
			return true;
		}

		public static bool WaitTillFileUnlocks(string filePath, int maxTimeoutInMillSec, bool waitTillCreated)
		{
			const int iterationSleepDurationInMilliSec = 50;
			int maxIterationsCount = maxTimeoutInMillSec / iterationSleepDurationInMilliSec;
			int sleepCount = 0;
			if (waitTillCreated)
			{
				// Use half time for the any potential copy to finish
				maxIterationsCount /= 2;
				while (!IsFileExists(filePath))
				{
					if (sleepCount++ > maxIterationsCount)
						return false;

					Thread.Sleep(iterationSleepDurationInMilliSec);
				}

				sleepCount = 0;
			}
			while (IsFileLocked(filePath))
			{
				if (sleepCount++ > maxIterationsCount)
					return false;

				Thread.Sleep(iterationSleepDurationInMilliSec);
			}

			return true;
		}

		public static bool WaitTillCopyIsFinished(string fileToCheck, long expectedSizeInBytes, int maxWaitTimeInMilliSec)
		{
			const int iterationSleepDurationInMilliSec = 50;
			int maxIterationsCount = maxWaitTimeInMilliSec / iterationSleepDurationInMilliSec;
			int sleepCount = 0;
			while (!IsFileExists(fileToCheck))
			{
				if (sleepCount++ > maxIterationsCount) return false;
				Thread.Sleep(iterationSleepDurationInMilliSec);
			}

			sleepCount = 0;
			long lastCheckedSize = new FileInfo(fileToCheck).Length;
			while (lastCheckedSize < expectedSizeInBytes)
			{
				Thread.Sleep(iterationSleepDurationInMilliSec);

				if (lastCheckedSize == new FileInfo(fileToCheck).Length)
				{
					if (sleepCount++ > maxIterationsCount) return false;
					Thread.Sleep(iterationSleepDurationInMilliSec);
				}
				else sleepCount = 0;

				lastCheckedSize = new FileInfo(fileToCheck).Length;
			}

			return true;
		}
	}
}
