using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Telerik.Windows.Zip.Extensions
{
	public static class ZipFile
	{
		public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName)
		{
			return destination.CreateEntryFromFile(sourceFileName, entryName, null);
		}

		public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName, CompressionLevel compressionLevel)
		{
			DeflateSettings compressionSettings = new DeflateSettings
			{
				CompressionLevel = compressionLevel,
				HeaderType = CompressedStreamHeader.None
			};
			return destination.CreateEntryFromFile(sourceFileName, entryName, compressionSettings);
		}

		public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName, CompressionSettings compressionSettings)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName");
			}
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			ZipArchiveEntry zipArchiveEntry;
			using (Stream stream = File.Open(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				zipArchiveEntry = ((compressionSettings == null) ? destination.CreateEntry(entryName) : destination.CreateEntry(entryName, compressionSettings));
				zipArchiveEntry.ExternalAttributes = (int)File.GetAttributes(sourceFileName);
				DateTime lastWriteTime = File.GetLastWriteTime(sourceFileName);
				if (lastWriteTime.Year < 1980 || lastWriteTime.Year > 2107)
				{
					lastWriteTime = new DateTime(1980, 1, 1, 0, 0, 0);
				}
				zipArchiveEntry.LastWriteTime = lastWriteTime;
				Stream stream2 = zipArchiveEntry.Open();
				stream.CopyTo(stream2);
				if (destination.Mode == ZipArchiveMode.Create)
				{
					stream2.Dispose();
					zipArchiveEntry.Dispose();
				}
			}
			return zipArchiveEntry;
		}

		public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName)
		{
			ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, null, false, null);
		}

		public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory)
		{
			DeflateSettings compressionSettings = new DeflateSettings
			{
				CompressionLevel = compressionLevel,
				HeaderType = CompressedStreamHeader.None
			};
			ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionSettings, includeBaseDirectory, null);
		}

		public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
		{
			DeflateSettings compressionSettings = new DeflateSettings
			{
				CompressionLevel = compressionLevel,
				HeaderType = CompressedStreamHeader.None
			};
			ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionSettings, includeBaseDirectory, entryNameEncoding);
		}

		public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionSettings compressionSettings, bool includeBaseDirectory, Encoding entryNameEncoding)
		{
			char[] trimChars = new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			};
			sourceDirectoryName = Path.GetFullPath(sourceDirectoryName);
			destinationArchiveFileName = Path.GetFullPath(destinationArchiveFileName);
			using (ZipArchive zipArchive = ZipFile.Open(destinationArchiveFileName, ZipArchiveMode.Create, entryNameEncoding))
			{
				bool flag = true;
				DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectoryName);
				string fullName = directoryInfo.FullName;
				if (includeBaseDirectory && directoryInfo.Parent != null)
				{
					fullName = directoryInfo.Parent.FullName;
				}
				foreach (FileSystemInfo fileSystemInfo in directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
				{
					flag = false;
					int length = fileSystemInfo.FullName.Length - fullName.Length;
					string text = fileSystemInfo.FullName.Substring(fullName.Length, length);
					text = text.TrimStart(trimChars);
					DirectoryInfo directoryInfo2 = fileSystemInfo as DirectoryInfo;
					if (directoryInfo2 != null)
					{
						if (ZipFile.IsDirectoryEmpty(directoryInfo2))
						{
							zipArchive.CreateEntry(text + Path.DirectorySeparatorChar);
						}
					}
					else
					{
						zipArchive.CreateEntryFromFile(fileSystemInfo.FullName, text, compressionSettings);
					}
				}
				if (includeBaseDirectory && flag)
				{
					zipArchive.CreateEntry(directoryInfo.Name + Path.DirectorySeparatorChar);
				}
			}
		}

		public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
		{
			ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName, null);
		}

		public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, Encoding entryNameEncoding)
		{
			if (sourceArchiveFileName == null)
			{
				throw new ArgumentNullException("sourceArchiveFileName");
			}
			using (ZipArchive zipArchive = ZipFile.Open(sourceArchiveFileName, ZipArchiveMode.Read, entryNameEncoding))
			{
				zipArchive.ExtractToDirectory(destinationDirectoryName);
			}
		}

		public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (destinationDirectoryName == null)
			{
				throw new ArgumentNullException("destinationDirectoryName");
			}
			string fullName = Directory.CreateDirectory(destinationDirectoryName).FullName;
			foreach (ZipArchiveEntry zipArchiveEntry in source.Entries)
			{
				string fullPath = Path.GetFullPath(Path.Combine(fullName, zipArchiveEntry.FullName));
				if (!fullPath.StartsWith(fullName, StringComparison.OrdinalIgnoreCase))
				{
					throw new IOException("Extracting results in outside");
				}
				if (Path.GetFileName(fullPath).Length != 0)
				{
					Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
					zipArchiveEntry.ExtractToFile(fullPath, false);
				}
				else
				{
					if (zipArchiveEntry.Length != 0L)
					{
						throw new IOException("Directory name with data");
					}
					Directory.CreateDirectory(fullPath);
				}
			}
		}

		public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName)
		{
			source.ExtractToFile(destinationFileName, false);
		}

		public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName, bool overwrite)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (destinationFileName == null)
			{
				throw new ArgumentNullException("destinationFileName");
			}
			using (Stream stream = File.Open(destinationFileName, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{
				using (Stream stream2 = source.Open())
				{
					stream2.CopyTo(stream);
				}
			}
			File.SetLastWriteTime(destinationFileName, source.LastWriteTime.DateTime);
		}

		public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode)
		{
			return ZipFile.Open(archiveFileName, mode, null);
		}

		public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode, Encoding entryNameEncoding)
		{
			FileMode mode2;
			FileAccess access;
			FileShare share;
			switch (mode)
			{
			case ZipArchiveMode.Create:
				mode2 = FileMode.CreateNew;
				access = FileAccess.Write;
				share = FileShare.None;
				break;
			case ZipArchiveMode.Read:
				mode2 = FileMode.Open;
				access = FileAccess.Read;
				share = FileShare.Read;
				break;
			case ZipArchiveMode.Update:
				mode2 = FileMode.OpenOrCreate;
				access = FileAccess.ReadWrite;
				share = FileShare.None;
				break;
			default:
				throw new ArgumentOutOfRangeException("mode");
			}
			FileStream fileStream = null;
			ZipArchive result;
			try
			{
				fileStream = File.Open(archiveFileName, mode2, access, share);
				result = new ZipArchive(fileStream, mode, false, entryNameEncoding);
			}
			catch
			{
				if (fileStream != null)
				{
					fileStream.Dispose();
				}
				throw;
			}
			return result;
		}

		public static ZipArchive OpenRead(string archiveFileName)
		{
			return ZipFile.Open(archiveFileName, ZipArchiveMode.Read);
		}

		static bool IsDirectoryEmpty(DirectoryInfo directoryInfo)
		{
			bool result = true;
			using (IEnumerator<FileSystemInfo> enumerator = directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					result = false;
				}
			}
			return result;
		}
	}
}
