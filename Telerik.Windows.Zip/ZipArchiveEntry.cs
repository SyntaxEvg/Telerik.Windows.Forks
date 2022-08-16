using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Telerik.Windows.Zip
{
	public class ZipArchiveEntry : IDisposable, INotifyPropertyChanged
	{
		internal ZipArchiveEntry(ZipArchive archive, CentralDirectoryHeader header)
		{
			this.Archive = archive;
			this.header = header;
			this.settings = ZipHelper.GetCompressionSettings((CompressionMethod)this.header.CompressionMethod, this.Archive.CompressionSettings);
			this.settings.PrepareForZip(null);
			this.existedInArchive = true;
		}

		internal ZipArchiveEntry(ZipArchive archive, string entryName)
		{
			this.Archive = archive;
			this.settings = this.Archive.CompressionSettings;
			this.header = new CentralDirectoryHeader();
			this.header.VersionNeededToExtract = 10;
			this.header.GeneralPurposeBitFlag = 8;
			if (this.Archive.EncryptionSettings is DefaultEncryptionSettings)
			{
				CentralDirectoryHeader centralDirectoryHeader = this.header;
				centralDirectoryHeader.GeneralPurposeBitFlag |= 1;
				this.ValidateVersionNeeded(VersionNeededToExtract.Deflate);
			}
			this.FullName = entryName;
			this.LastWriteTime = DateTimeOffset.Now;
		}

		internal ZipArchiveEntry(ZipArchive archive, string entryName, CompressionSettings settings)
			: this(archive, entryName)
		{
			this.settings = settings;
			this.settings.PrepareForZip(null);
			this.CompressionMethod = settings.Method;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ZipArchive Archive
		{
			get
			{
				return this.archiveRef;
			}
			set
			{
				this.archiveRef = value;
			}
		}

		public long CompressedLength
		{
			get
			{
				this.EnsureCentralDirectoryHeader();
				Zip64ExtraField zip64ExtraField = this.SelectZip64ExtraField();
				if (zip64ExtraField != null && zip64ExtraField.CompressedSize != null)
				{
					return (long)zip64ExtraField.CompressedSize.Value;
				}
				return (long)((ulong)this.header.CompressedSize);
			}
			set
			{
				this.EnsureCentralDirectoryHeader();
				this.ValidateArchiveModeForWriting();
				if (value >= (long)-1)
				{
					Zip64ExtraField zip64ExtraField = this.EnsureZip64ExtraField();
					zip64ExtraField.CompressedSize = new ulong?((ulong)value);
					this.header.CompressedSize = uint.MaxValue;
					return;
				}
				this.header.CompressedSize = (uint)value;
			}
		}

		public int ExternalAttributes
		{
			get
			{
				this.EnsureCentralDirectoryHeader();
				return (int)this.header.ExternalAttributes;
			}
			set
			{
				this.EnsureCentralDirectoryHeader();
				this.header.ExternalAttributes = (uint)value;
				this.OnPropertyChanged("ExternalAttributes");
			}
		}

		public string FullName
		{
			get
			{
				this.EnsureCentralDirectoryHeader();
				return this.DecodeEntryName(this.header.FileName);
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.EnsureCentralDirectoryHeader();
				string text = value.Replace(PlatformSettings.Manager.DirectorySeparatorChar, "/");
				text = text.Replace(PlatformSettings.Manager.AltDirectorySeparatorChar, "/");
				this.header.FileName = this.EncodeEntryName(text);
				if (ZipHelper.EndsWithDirChar(value))
				{
					this.ValidateVersionNeeded(VersionNeededToExtract.Deflate);
				}
				this.OnPropertyChanged("FullName");
			}
		}

		public DateTimeOffset LastWriteTime
		{
			get
			{
				this.EnsureCentralDirectoryHeader();
				DateTime dateTime = ZipHelper.PackedToDateTime(this.header.FileTime);
				DateTimeOffset result = new DateTimeOffset(dateTime);
				return result;
			}
			set
			{
				this.EnsureCentralDirectoryHeader();
				this.ValidateArchiveModeForWriting();
				if (value.DateTime.Year < 1980 || value.DateTime.Year > 2107)
				{
					throw new ArgumentOutOfRangeException("value", "Date-Time out of range.");
				}
				DateTime dateTime = value.DateTime;
				uint fileTime = ZipHelper.DateTimeToPacked(dateTime);
				this.header.FileTime = fileTime;
			}
		}

		public long Length
		{
			get
			{
				this.EnsureCentralDirectoryHeader();
				Zip64ExtraField zip64ExtraField = this.SelectZip64ExtraField();
				if (zip64ExtraField != null && zip64ExtraField.OriginalSize != null)
				{
					return (long)zip64ExtraField.OriginalSize.Value;
				}
				return (long)((ulong)this.header.UncompressedSize);
			}
			set
			{
				this.EnsureCentralDirectoryHeader();
				this.ValidateArchiveModeForWriting();
				if (value >= (long)-1)
				{
					Zip64ExtraField zip64ExtraField = this.EnsureZip64ExtraField();
					zip64ExtraField.OriginalSize = new ulong?((ulong)value);
					this.header.UncompressedSize = uint.MaxValue;
					return;
				}
				this.header.UncompressedSize = (uint)value;
			}
		}

		public string Name
		{
			get
			{
				string[] array = this.FullName.Split(new string[]
				{
					PlatformSettings.Manager.DirectorySeparatorChar,
					PlatformSettings.Manager.AltDirectorySeparatorChar
				}, StringSplitOptions.None);
				return array[array.Length - 1];
			}
		}

		internal CompressionMethod CompressionMethod
		{
			get
			{
				this.EnsureCentralDirectoryHeader();
				return (CompressionMethod)this.header.CompressionMethod;
			}
			set
			{
				this.EnsureCentralDirectoryHeader();
				if (value == CompressionMethod.Deflate)
				{
					this.ValidateVersionNeeded(VersionNeededToExtract.Deflate);
				}
				this.header.CompressionMethod = (ushort)value;
			}
		}

		long CompressedDataOffset
		{
			get
			{
				if (this.offsetOfCompressedData == null)
				{
					this.Archive.Reader.BaseStream.Seek(this.LocalHeaderOffset, SeekOrigin.Begin);
					this.localFileHeader = new LocalFileHeader();
					if (!this.localFileHeader.TryReadBlock(this.Archive.Reader))
					{
						throw new InvalidDataException("Local file header is corrupted.");
					}
					this.offsetOfCompressedData = new long?(this.Archive.Reader.BaseStream.Position);
				}
				return this.offsetOfCompressedData.Value;
			}
			set
			{
				this.offsetOfCompressedData = new long?(value);
			}
		}

		uint DiskStartNumber
		{
			get
			{
				this.EnsureCentralDirectoryHeader();
				Zip64ExtraField zip64ExtraField = this.SelectZip64ExtraField();
				if (zip64ExtraField != null && zip64ExtraField.DiskStartNumber != null)
				{
					return zip64ExtraField.DiskStartNumber.Value;
				}
				return (uint)this.header.DiskNumberStart;
			}
		}

		long LocalHeaderOffset
		{
			get
			{
				this.EnsureCentralDirectoryHeader();
				Zip64ExtraField zip64ExtraField = this.SelectZip64ExtraField();
				if (zip64ExtraField != null && zip64ExtraField.RelativeHeaderOffset != null)
				{
					return (long)zip64ExtraField.RelativeHeaderOffset.Value;
				}
				return (long)((ulong)this.header.LocalHeaderOffset);
			}
			set
			{
				this.EnsureCentralDirectoryHeader();
				this.ValidateArchiveModeForWriting();
				if (value >= (long)-1)
				{
					Zip64ExtraField zip64ExtraField = this.EnsureZip64ExtraField();
					zip64ExtraField.RelativeHeaderOffset = new ulong?((ulong)value);
					this.header.LocalHeaderOffset = uint.MaxValue;
					return;
				}
				this.header.LocalHeaderOffset = (uint)value;
			}
		}

		Stream UpdatableData
		{
			get
			{
				if (this.updatableData == null)
				{
					this.updatableData = PlatformSettings.Manager.CreateTemporaryStream();
					if (this.existedInArchive)
					{
						using (Stream stream = this.OpenForReading())
						{
							try
							{
								stream.CopyTo(this.updatableData);
							}
							catch (InvalidDataException)
							{
								this.updatableData.Dispose();
								PlatformSettings.Manager.DeleteTemporaryStream(this.updatableData);
								this.updatableData = null;
								this.written = false;
								throw;
							}
						}
					}
				}
				return this.updatableData;
			}
		}

		public void Delete()
		{
			if (this.Archive == null)
			{
				return;
			}
			if (this.Archive.Mode != ZipArchiveMode.Update)
			{
				throw new NotSupportedException("Entry can be deleted in Update mode only.");
			}
			this.Archive.ThrowIfDisposed();
			this.Archive.RemoveEntry(this);
			this.Archive = null;
			this.deleted = true;
			this.DisposeStreams();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public Stream Open()
		{
			switch (this.Archive.Mode)
			{
			case ZipArchiveMode.Create:
				return this.OpenForWriting();
			case ZipArchiveMode.Read:
				return this.OpenForReading();
			case ZipArchiveMode.Update:
				return this.OpenForUpdate();
			default:
				return null;
			}
		}

		internal bool CheckIntegrity(out string message)
		{
			message = null;
			if (this.existedInArchive)
			{
				if (!ZipHelper.IsCompressionMethodSupported(this.CompressionMethod))
				{
					message = string.Format("Unsupported compression method: {0}.", this.CompressionMethod);
					return false;
				}
				if (this.DiskStartNumber != this.Archive.NumberOfThisDisk)
				{
					message = "Splitted archive is not supported.";
					return false;
				}
				if (this.LocalHeaderOffset > this.Archive.Reader.BaseStream.Length)
				{
					message = "Local file header is corrupted.";
					return false;
				}
				this.Archive.Reader.BaseStream.Seek(this.LocalHeaderOffset, SeekOrigin.Begin);
				this.localFileHeader = new LocalFileHeader();
				if (!this.localFileHeader.TryReadBlock(this.Archive.Reader))
				{
					message = "Local file header is corrupted.";
					return false;
				}
				if (!this.ValidateLocalFileHeader())
				{
					message = "Local file header is corrupted.";
					return false;
				}
				if (this.CompressedDataOffset + this.CompressedLength > this.Archive.Reader.BaseStream.Length)
				{
					message = "Local file header is corrupted.";
					return false;
				}
			}
			return true;
		}

		internal void WriteCentralDirectoryHeader()
		{
			this.header.WriteBlock(this.Archive.Writer);
		}

		void CompressedData_ChecksumReady(object sender, EventArgs e)
		{
			if (this.Archive.Mode == ZipArchiveMode.Create || this.Archive.Mode == ZipArchiveMode.Update)
			{
				this.header.Crc = (uint)this.compressedData.Checksum;
			}
		}

		string DecodeEntryName(byte[] entryNameBytes)
		{
			Encoding entryNameEncoding = this.GetEntryNameEncoding();
			return entryNameEncoding.GetString(entryNameBytes, 0, entryNameBytes.Length);
		}

		void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.FlushEntryData();
					this.DisposeStreams();
				}
				this.disposed = true;
			}
		}

		void DisposeStreams()
		{
			if (this.compressedData != null)
			{
				if (!this.deleted)
				{
					this.compressedData.Dispose();
				}
				this.compressedData.ChecksumReady -= this.CompressedData_ChecksumReady;
				this.compressedData = null;
			}
			if (this.updatableData != null)
			{
				this.updatableData.Dispose();
				PlatformSettings.Manager.DeleteTemporaryStream(this.updatableData);
				this.updatableData = null;
			}
		}

		byte[] EncodeEntryName(string entryName)
		{
			if (!string.IsNullOrEmpty(entryName))
			{
				Encoding encoding = ((this.Archive == null) ? PlatformSettings.Manager.DefaultEncoding : (this.Archive.EntryNameEncoding ?? PlatformSettings.Manager.DefaultEncoding));
				bool flag = encoding is UTF8Encoding && encoding.Equals(Encoding.UTF8);
				if (flag)
				{
					this.header.GeneralPurposeBitFlag = (ushort)(this.header.GeneralPurposeBitFlag | 2048);
				}
				else
				{
					this.header.GeneralPurposeBitFlag = (ushort)(this.header.GeneralPurposeBitFlag & 63487);
				}
				return encoding.GetBytes(entryName);
			}
			return new byte[0];
		}

		void EnsureCentralDirectoryHeader()
		{
			if (this.header == null)
			{
				this.header = new CentralDirectoryHeader();
			}
		}

		Zip64ExtraField EnsureZip64ExtraField()
		{
			Zip64ExtraField zip64ExtraField = null;
			foreach (ExtraFieldBase extraFieldBase in this.header.ExtraFields)
			{
				zip64ExtraField = extraFieldBase as Zip64ExtraField;
				if (zip64ExtraField != null)
				{
					break;
				}
			}
			if (zip64ExtraField == null)
			{
				zip64ExtraField = new Zip64ExtraField(new CentralDirectoryHeaderInfo(this.header));
				this.header.ExtraFields.Add(zip64ExtraField);
			}
			return zip64ExtraField;
		}

		void FlushEntryData()
		{
			if (this.Archive.Mode == ZipArchiveMode.Create)
			{
				if (this.compressedData != null)
				{
					this.FlushFinalBlockAndDataDescriptor();
					return;
				}
			}
			else if (this.Archive.Mode == ZipArchiveMode.Update)
			{
				if (this.updatableData != null)
				{
					this.OpenForWriting();
					this.updatableData.Seek(0L, SeekOrigin.Begin);
					this.updatableData.CopyTo(this.compressedData);
					this.FlushFinalBlockAndDataDescriptor();
					return;
				}
				long compressedDataOffset = this.CompressedDataOffset;
				this.WriteLocalFileHeader();
				this.Archive.Reader.BaseStream.Seek(compressedDataOffset, SeekOrigin.Begin);
				ZipHelper.CopyStream(this.Archive.Reader.BaseStream, this.Archive.Writer.BaseStream, this.CompressedLength);
				this.WriteDataDescriptor();
			}
		}

		void FlushFinalBlockAndDataDescriptor()
		{
			if (!this.compressedData.HasFlushedFinalBlock)
			{
				this.compressedData.Flush();
			}
			this.CompressedLength = this.compressedData.CompressedSize;
			this.Length = this.compressedData.TotalPlainCount;
			this.WriteDataDescriptor();
		}

		Encoding GetEntryNameEncoding()
		{
			Encoding result;
			if ((this.header.GeneralPurposeBitFlag & 2048) != 0)
			{
				result = Encoding.UTF8;
			}
			else
			{
				result = ((this.Archive == null) ? PlatformSettings.Manager.DefaultEncoding : (this.Archive.EntryNameEncoding ?? PlatformSettings.Manager.DefaultEncoding));
			}
			return result;
		}

		void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		Stream OpenForReading()
		{
			string message = null;
			if (!this.CheckIntegrity(out message))
			{
				throw new InvalidDataException(message);
			}
			this.Archive.Reader.BaseStream.Position = this.CompressedDataOffset;
			this.compressedData = new CompressedStream(this.Archive.Reader.BaseStream, StreamOperationMode.Read, this.settings, true, this.Archive.EncryptionSettings);
			this.compressedData.ChecksumReady += this.CompressedData_ChecksumReady;
			this.compressedData.SetLength(this.CompressedLength);
			return this.compressedData;
		}

		Stream OpenForUpdate()
		{
			if (this.written)
			{
				throw new IOException("Create mode writes entry once and one entry at a time");
			}
			this.UpdatableData.Seek(0L, SeekOrigin.Begin);
			return this.UpdatableData;
		}

		Stream OpenForWriting()
		{
			if (this.written)
			{
				throw new IOException("Create mode writes entry once and one entry at a time");
			}
			this.WriteLocalFileHeader();
			if (this.Archive.EncryptionSettings != null)
			{
				DefaultEncryptionSettings defaultEncryptionSettings = this.Archive.EncryptionSettings as DefaultEncryptionSettings;
				if (defaultEncryptionSettings != null)
				{
					defaultEncryptionSettings.FileTime = this.header.FileTime;
				}
			}
			this.compressedData = new CompressedStream(this.Archive.Writer.BaseStream, StreamOperationMode.Write, this.settings, true, this.Archive.EncryptionSettings);
			this.compressedData.ChecksumReady += this.CompressedData_ChecksumReady;
			return this.compressedData;
		}

		Zip64ExtraField SelectZip64ExtraField()
		{
			Zip64ExtraField zip64ExtraField = null;
			if (this.header.ExtraFields.Count > 0)
			{
				foreach (ExtraFieldBase extraFieldBase in this.header.ExtraFields)
				{
					zip64ExtraField = extraFieldBase as Zip64ExtraField;
					if (zip64ExtraField != null)
					{
						break;
					}
				}
			}
			return zip64ExtraField;
		}

		bool ValidateLocalFileHeader()
		{
			return this.localFileHeader != null;
		}

		void ValidateVersionNeeded(VersionNeededToExtract value)
		{
			if (this.header.VersionNeededToExtract < (ushort)value)
			{
				this.header.VersionNeededToExtract = (ushort)value;
			}
		}

		void ValidateArchiveModeForWriting()
		{
			if (this.Archive.Mode == ZipArchiveMode.Read)
			{
				throw new NotSupportedException("Read only archive.");
			}
			if (this.Archive.Mode == ZipArchiveMode.Create && this.written)
			{
				throw new IOException("Entry is frozen after write.");
			}
		}

		void WriteDataDescriptor()
		{
			if ((this.CompressedLength >= (long)-1) || (this.Length >= (long)-1))
			{
				this.header.VersionNeededToExtract = 45;
			}
			DataDescriptor dataDescriptor = DataDescriptor.FromFileHeader(this.header);
			dataDescriptor.WriteBlock(this.Archive.Writer);
		}

		void WriteLocalFileHeader()
		{
			this.LocalHeaderOffset = this.Archive.Writer.BaseStream.Position;
			LocalFileHeader localFileHeader = new LocalFileHeader(this.header);
			localFileHeader.WriteBlock(this.Archive.Writer);
			this.CompressedDataOffset = this.Archive.Writer.BaseStream.Position;
		}

		ZipArchive archiveRef;

		CentralDirectoryHeader header;

		CompressionSettings settings;

		CompressedStream compressedData;

		bool written;

		bool existedInArchive;

		LocalFileHeader localFileHeader;

		long? offsetOfCompressedData = null;

		Stream updatableData;

		bool deleted;

		bool disposed;
	}
}
