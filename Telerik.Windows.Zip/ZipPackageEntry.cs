using System;
using System.ComponentModel;
using System.IO;

namespace Telerik.Windows.Zip
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This class has been deprecated. Use ZipArchiveEntry instead.")]
	public class ZipPackageEntry
	{
		internal ZipPackageEntry(ZipArchiveEntry entry)
		{
			this.entry = entry;
		}

		public FileAttributes Attributes
		{
			get
			{
				return (FileAttributes)this.entry.ExternalAttributes;
			}
		}

		public int CompressedSize
		{
			get
			{
				return (int)this.entry.CompressedLength;
			}
		}

		public string FileNameInZip
		{
			get
			{
				return this.entry.FullName;
			}
		}

		public int UncompressedSize
		{
			get
			{
				return (int)this.entry.Length;
			}
		}

		public Stream OpenInputStream()
		{
			return this.entry.Open();
		}

		internal void Delete()
		{
			this.entry.Delete();
		}

		ZipArchiveEntry entry;
	}
}
