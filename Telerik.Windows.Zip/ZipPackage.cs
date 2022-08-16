using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace Telerik.Windows.Zip
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This class has been deprecated. Use ZipArchive instead.")]
	public class ZipPackage : ZipArchive
	{
		ZipPackage(Stream stream, ZipArchiveMode mode)
			: base(stream, mode, false, null)
		{
		}

		public string FileName
		{
			get
			{
				return this.zipFileName;
			}
		}

		public IList<ZipPackageEntry> ZipPackageEntries
		{
			get
			{
				List<ZipPackageEntry> list = new List<ZipPackageEntry>();
				foreach (ZipArchiveEntry entry in base.Entries)
				{
					list.Add(new ZipPackageEntry(entry));
				}
				this.zipPackageEntries = list;
				return list;
			}
		}

		public static ZipPackage Create(Stream stream)
		{
			return new ZipPackage(stream, ZipArchiveMode.Create);
		}

		public static ZipPackage CreateFile(string fileName)
		{
			Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			ZipPackage zipPackage = ZipPackage.Create(stream);
			zipPackage.zipFileName = fileName;
			return zipPackage;
		}

		public static bool IsZipFile(Stream stream)
		{
			if (stream == null || stream.Length < 22L)
			{
				return false;
			}
			long position = stream.Position;
			bool result;
			try
			{
				using (ZipPackage.Open(stream))
				{
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				stream.Position = position;
			}
			return result;
		}

		public static bool IsZipFile(string fileName)
		{
			if (!File.Exists(fileName))
			{
				return false;
			}
			bool result;
			try
			{
				using (ZipPackage.OpenFile(fileName, FileAccess.Read))
				{
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static ZipPackage Open(Stream stream)
		{
			ZipArchiveMode mode = (stream.CanWrite ? ZipArchiveMode.Update : ZipArchiveMode.Read);
			return new ZipPackage(stream, mode);
		}

		public static ZipPackage OpenFile(string fileName, FileAccess access)
		{
			if (access != FileAccess.Read)
			{
				throw new InvalidOperationException("File should be opened with read access.");
			}
			Stream stream = new FileStream(fileName, FileMode.Open, access, FileShare.Read);
			ZipPackage zipPackage = ZipPackage.Open(stream);
			zipPackage.zipFileName = fileName;
			return zipPackage;
		}

		public void Add(string fileName)
		{
			string fileName2 = ZipPackage.GetFileName(fileName);
			this.Add(fileName, fileName2, CompressionType.Default);
		}

		public void Add(string fileName, CompressionType compressionType)
		{
			string fileName2 = ZipPackage.GetFileName(fileName);
			this.Add(fileName, fileName2, compressionType);
		}

		public void Add(IEnumerable<string> fileNames)
		{
			foreach (string fileName in fileNames)
			{
				string fileName2 = ZipPackage.GetFileName(fileName);
				this.Add(fileName, fileName2, CompressionType.Default);
			}
		}

		public void Add(IEnumerable<string> fileNames, CompressionType compressionType)
		{
			foreach (string fileName in fileNames)
			{
				string fileName2 = ZipPackage.GetFileName(fileName);
				this.Add(fileName, fileName2, compressionType);
			}
		}

		public void Add(string fileName, string fileNameInZip)
		{
			this.Add(fileName, fileNameInZip, DateTime.Now, CompressionType.Default);
		}

		public void Add(string fileName, string fileNameInZip, CompressionType compressionType)
		{
			this.Add(fileName, fileNameInZip, DateTime.Now, compressionType);
		}

		public void Add(string fileName, string fileNameInZip, DateTime dateTime)
		{
			if (string.Compare(fileName, this.FileName, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0)
			{
				throw new ArgumentException("Can't add a zip file to itself.");
			}
			bool flag = File.Exists(fileName);
			bool flag2 = !flag && Directory.Exists(fileName);
			if (!flag && !flag2)
			{
				throw new FileNotFoundException(string.Format("File not found: '{0}'.", fileName));
			}
			this.AddEntry(fileName, fileNameInZip, dateTime, CompressionType.Default);
		}

		public void Add(string fileName, string fileNameInZip, DateTime dateTime, CompressionType compressionType)
		{
			if (string.Compare(fileName, this.FileName, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0)
			{
				throw new ArgumentException("Can't add a zip file to itself.");
			}
			bool flag = File.Exists(fileName);
			bool flag2 = !flag && Directory.Exists(fileName);
			if (!flag && !flag2)
			{
				throw new FileNotFoundException(string.Format("File not found: '{0}'.", fileName));
			}
			this.AddEntry(fileName, fileNameInZip, dateTime, compressionType);
		}

		public void AddStream(Stream stream, string fileNameInZip)
		{
			this.AddStream(stream, fileNameInZip, ZipCompression.Default, DateTime.Now, CompressionType.Default);
		}

		public void AddStream(Stream stream, string fileNameInZip, CompressionType compressionType)
		{
			this.AddStream(stream, fileNameInZip, ZipCompression.Default, DateTime.Now, compressionType);
		}

		public void AddStream(Stream stream, string fileNameInZip, ZipCompression method, DateTime dateTime)
		{
			this.AddStream(stream, fileNameInZip, method, dateTime, CompressionType.Default);
		}

		public void AddStream(Stream stream, string fileNameInZip, ZipCompression method, DateTime dateTime, CompressionType compressionType)
		{
			if (compressionType == CompressionType.Lzma)
			{
				throw new NotSupportedException();
			}
			DeflateSettings deflateSettings = ZipOutputStream.CreateDeflateSettings(method);
			deflateSettings.HeaderType = CompressedStreamHeader.None;
			this.DeleteAvailableEntry(fileNameInZip);
			using (ZipArchiveEntry zipArchiveEntry = base.CreateEntry(fileNameInZip, deflateSettings))
			{
				zipArchiveEntry.LastWriteTime = dateTime;
				zipArchiveEntry.ExternalAttributes = 32;
				Stream destination = zipArchiveEntry.Open();
				stream.CopyTo(destination);
			}
		}

		public void Close(bool closeStream)
		{
			base.DisposeStreams(closeStream);
		}

		public int IndexOf(string fileNameInZip)
		{
			int num = 0;
			IList<ZipPackageEntry> list = this.ZipPackageEntries;
			foreach (ZipPackageEntry zipPackageEntry in list)
			{
				if (string.Compare(zipPackageEntry.FileNameInZip, fileNameInZip, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) == 0)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public void RemoveEntry(ZipPackageEntry zipPackageEntry)
		{
			zipPackageEntry.Delete();
		}

		static string GetFileName(string fileName)
		{
			fileName = Path.GetFileName(fileName);
			return fileName;
		}

		void AddEntry(string fileName, string fileNameInZip, DateTime dateTime, CompressionType compressionType)
		{
			if (compressionType == CompressionType.Lzma)
			{
				throw new NotSupportedException();
			}
			if (File.Exists(fileName))
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					this.SaveEntry(fileStream, fileName, fileNameInZip, dateTime);
					return;
				}
			}
			if (Directory.Exists(fileName))
			{
				this.SaveEntry(null, fileName, fileNameInZip, dateTime);
				return;
			}
			throw new FileNotFoundException(string.Format("File not found: '{0}'.", fileName));
		}

		void DeleteAvailableEntry(string fileNameInZip)
		{
			int num = this.IndexOf(fileNameInZip);
			if (num != -1)
			{
				this.zipPackageEntries[num].Delete();
			}
		}

		void SaveEntry(FileStream stream, string fileName, string fileNameInZip, DateTime dateTime)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			int attributes = (int)fileInfo.Attributes;
			this.DeleteAvailableEntry(fileNameInZip);
			using (ZipArchiveEntry zipArchiveEntry = base.CreateEntry(fileNameInZip, new DeflateSettings
			{
				CompressionLevel = CompressionLevel.Optimal,
				HeaderType = CompressedStreamHeader.None
			}))
			{
				Stream destination = zipArchiveEntry.Open();
				if (stream != null)
				{
					stream.CopyTo(destination);
				}
				zipArchiveEntry.LastWriteTime = fileInfo.LastWriteTime;
				zipArchiveEntry.ExternalAttributes = attributes;
			}
		}

		const uint ArchiveAttribute = 32U;

		string zipFileName;

		List<ZipPackageEntry> zipPackageEntries;
	}
}
