using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace WinToolkit
{
	public static class DirectoryContentsIterator
	{
		public static readonly string[] ImageExtensions = { "*.png", "*.jpg", "*.jpeg", "*.exif", "*.tiff", "*.gif", "*.bmp" };
		public static List<FileDataElement> GetAllFiles(String directoryFullPath, bool enableSubDirectories = false)
		{
			if (directoryFullPath == null || !Directory.Exists(directoryFullPath)) return null;
			string[] subDirectories = Directory.GetDirectories(directoryFullPath);
			List<FileDataElement> scannedFiles=new List<FileDataElement>();
			if (subDirectories != null && subDirectories.Length > 0 && enableSubDirectories)
			{
				foreach (string subDirectory in subDirectories)
				{
					List<FileDataElement> scannedList=GetAllFiles(subDirectory, enableSubDirectories);
					scannedFiles.AddRange(scannedList);
				}
			}
			List<FileDataElement> scanned = GetFilesDataElementsWithExt(directoryFullPath, ImageExtensions);
			scannedFiles.AddRange(scanned);
			return scannedFiles;
		}

		private static readonly string[] _allFilesFilter = { "*.*" };

		public static List<FileDataElement> GetFilesDataElementsWithExt(String directoryFullPath, String[] filter = null)
		{
			if (String.IsNullOrEmpty(directoryFullPath) ||
				!Directory.Exists(directoryFullPath)) return null;

			List<FileDataElement> scannedFiles = new List<FileDataElement>();
			String[] internalFilter = filter ?? _allFilesFilter;
			foreach (string extension in internalFilter)
			{
				List<String> foundFiles = GetAllFilesWithExt(directoryFullPath, extension);
				if (foundFiles != null && foundFiles.Count > 0)
					scannedFiles.AddRange(foundFiles.Select(filePath => new FileDataElement { FullPath = filePath }));
			}

			return scannedFiles;
		}

		public static List<String> GetAllFilesWithExt(String directoryFullPath, String filter)
		{
			IEnumerator<String> patientFiles = Directory.EnumerateFiles(directoryFullPath, filter).GetEnumerator();
			List<String> foundFiles = new List<String>();
			while (patientFiles.MoveNext())
				foundFiles.Add(patientFiles.Current);

			return foundFiles;
		}

		public static bool CheckHasFiles(String pathToCheck)
		{
			List<FileDataElement> items = GetAllFiles(pathToCheck);
			return items != null && items.Count > 0;
		}

		public static string GetImageFilter()
		{
			StringBuilder allImageExtensions = new StringBuilder();
			string separator = "";
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
			Dictionary<string, string> images = new Dictionary<string, string>();
			foreach (ImageCodecInfo codec in codecs)
			{
				allImageExtensions.Append(separator);
				allImageExtensions.Append(codec.FilenameExtension);
				separator = ";";
				images.Add(string.Format("{0} Files: ({1})", codec.FormatDescription, codec.FilenameExtension),
						   codec.FilenameExtension);
			}
			StringBuilder sb = new StringBuilder();
			if (allImageExtensions.Length > 0)
			{
				sb.AppendFormat("{0}|{1}", "All Images", allImageExtensions.ToString());
			}

			foreach (KeyValuePair<string, string> image in images)
			{
				sb.AppendFormat("|{0}|{1}", image.Key, image.Value);
			}
			return sb.ToString();
		}
	}

	public class FileDataElement
	{
		public String ID { get { return FullPath; } }
		public String Name { get { return Path.GetFileName(FullPath); } }

		public String FullPath { private get; set; }
		public override string ToString()
		{
			return FullPath;
		}
	}
}
