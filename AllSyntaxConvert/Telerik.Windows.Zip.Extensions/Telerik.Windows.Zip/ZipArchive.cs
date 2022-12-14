using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Telerik.Windows.Zip
{
	public class ZipArchive : IDisposable, INotifyPropertyChanged
	{
		public ZipArchive(Stream stream)
			: this(stream, ZipArchiveMode.Read, true, null, null, null)
		{
		}

		public ZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen, Encoding entryNameEncoding)
			: this(stream, mode, leaveOpen, entryNameEncoding, null, null)
		{
		}

		public ZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen, Encoding entryNameEncoding, CompressionSettings compressionSettings, EncryptionSettings encryptionSettings)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.EntryNameEncoding = entryNameEncoding;
			if (compressionSettings == null)
			{
				compressionSettings = new DeflateSettings
				{
					HeaderType = CompressedStreamHeader.None
				};
			}
			this.CompressionSettings = compressionSettings;
			this.CompressionSettings.PrepareForZip(null);
			this.EncryptionSettings = encryptionSettings;
			this.Init(stream, mode, leaveOpen);
		}

		~ZipArchive()
		{
			this.Dispose(false);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public IEnumerable<ZipArchiveEntry> Entries
		{
			get
			{
				this.EnsureCentralDirectoryRead();
				foreach (ZipArchiveEntry entry in this.entries.Values)
				{
					yield return entry;
				}
				yield break;
			}
		}

		public Encoding EntryNameEncoding
		{
			get
			{
				return this.entryNameEncoding;
			}
			set
			{
				if (!PlatformSettings.Manager.IsEncodingSupported(value))
				{
					throw new ArgumentException("Entry name encoding is not supported", "value");
				}
				this.entryNameEncoding = value;
			}
		}

		public ZipArchiveMode Mode
		{
			get
			{
				return this.archiveMode;
			}
		}

		internal CompressionSettings CompressionSettings { get; set; }

		internal EncryptionSettings EncryptionSettings { get; set; }

		internal uint NumberOfThisDisk
		{
			get
			{
				if (this.zip64EndOfCentralDirectoryRecord != null)
				{
					return this.zip64EndOfCentralDirectoryRecord.NumberOfThisDisk;
				}
				return (uint)this.endOfCentralDirectoryRecord.NumberOfThisDisk;
			}
		}

		internal BinaryReader Reader
		{
			get
			{
				return this.archiveReader;
			}
		}

		internal BinaryWriter Writer
		{
			get
			{
				return this.archiveWriter;
			}
		}

		long CentralDirectoryStart
		{
			get
			{
				if (this.zip64EndOfCentralDirectoryRecord != null)
				{
					return (long)this.zip64EndOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber;
				}
				return (long)((ulong)this.endOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber);
			}
		}

		ulong NumberOfEntriesInTheCentralDirectory
		{
			get
			{
				if (this.zip64EndOfCentralDirectoryRecord != null)
				{
					return this.zip64EndOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectory;
				}
				return (ulong)this.endOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectory;
			}
		}

		public ZipArchiveEntry CreateEntry(string entryName)
		{
			ZipArchiveEntry zipArchiveEntry = new ZipArchiveEntry(this, entryName, this.CompressionSettings);
			this.entries.Add(zipArchiveEntry.FullName, zipArchiveEntry);
			return zipArchiveEntry;
		}

		public ZipArchiveEntry CreateEntry(string entryName, CompressionSettings settings)
		{
			ZipArchiveEntry zipArchiveEntry = new ZipArchiveEntry(this, entryName, settings);
			this.entries.Add(zipArchiveEntry.FullName, zipArchiveEntry);
			return zipArchiveEntry;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public ZipArchiveEntry GetEntry(string entryName)
		{
			if (entryName == null)
			{
				throw new ArgumentNullException("entryName");
			}
			if (this.Mode == ZipArchiveMode.Create)
			{
				throw new NotSupportedException("Can't get entry in the create mode.");
			}
			this.EnsureCentralDirectoryRead();
			ZipArchiveEntry result;
			this.entries.TryGetValue(entryName, out result);
			return result;
		}

		internal void ThrowIfDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		internal void RemoveEntry(ZipArchiveEntry entry)
		{
			this.entries.Remove(entry.FullName);
		}

		internal void DisposeStreams(bool closeStream)
		{
			this.leaveStreamOpen = !closeStream;
			this.Dispose();
		}

		void ClearEntries()
		{
			foreach (ZipArchiveEntry zipArchiveEntry in this.entries.Values)
			{
				zipArchiveEntry.Dispose();
				zipArchiveEntry.Archive = null;
			}
			this.entries.Clear();
		}

		void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					ZipArchiveMode mode = this.Mode;
					if (mode == ZipArchiveMode.Read)
					{
						this.DisposeStreams();
					}
					else
					{
						try
						{
							this.WriteArchive();
							this.DisposeStreams();
						}
						catch (InvalidDataException)
						{
							this.DisposeStreams();
							this.disposed = true;
							throw;
						}
					}
				}
				this.ClearEntries();
			}
			this.disposed = true;
		}

		void DisposeStreams()
		{
			if (!this.leaveStreamOpen)
			{
				this.workingStream.Dispose();
				if (this.originalStream != null)
				{
					this.originalStream.Dispose();
					PlatformSettings.Manager.DeleteTemporaryStream(this.workingStream);
				}
				if (this.archiveReader != null)
				{
					this.archiveReader.Close();
				}
				if (this.archiveWriter != null)
				{
					this.archiveWriter.Close();
					return;
				}
			}
			else if (this.originalStream != null)
			{
				this.originalStream.Flush();
				this.workingStream.Dispose();
				PlatformSettings.Manager.DeleteTemporaryStream(this.workingStream);
			}
		}

		void EnsureCentralDirectoryRead()
		{
			if (!this.centralDirectoryRead)
			{
				this.ReadCentralDirectory();
				this.centralDirectoryRead = true;
			}
		}

		void Init(Stream stream, ZipArchiveMode mode, bool leaveOpen)
		{
			try
			{
				this.originalStream = null;
				switch (mode)
				{
				case ZipArchiveMode.Create:
					if (!stream.CanWrite || !stream.CanSeek)
					{
						throw new ArgumentException("Stream must support writing and seeking.");
					}
					stream.SetLength(0L);
					break;
				case ZipArchiveMode.Read:
					if (!stream.CanRead)
					{
						throw new ArgumentException("Stream must support reading.");
					}
					if (!stream.CanSeek)
					{
						this.originalStream = stream;
						stream = PlatformSettings.Manager.CreateTemporaryStream();
						this.originalStream.CopyTo(stream);
						stream.Seek(0L, SeekOrigin.Begin);
					}
					break;
				case ZipArchiveMode.Update:
					if (stream.CanRead && stream.CanSeek)
					{
						this.originalStream = stream;
						stream = PlatformSettings.Manager.CreateTemporaryStream();
						stream.Seek(0L, SeekOrigin.Begin);
						if (stream.CanWrite)
						{
							break;
						}
					}
					throw new ArgumentException("Stream must support reading, writing and seeking.");
				default:
					throw new ArgumentOutOfRangeException("mode");
				}
				this.archiveMode = mode;
				this.workingStream = stream;
				if (mode == ZipArchiveMode.Read)
				{
					this.archiveWriter = null;
					this.archiveReader = new BinaryReader(this.workingStream);
				}
				else if (mode == ZipArchiveMode.Update)
				{
					this.archiveReader = new BinaryReader(this.originalStream);
					this.archiveWriter = new BinaryWriter(this.workingStream);
				}
				else
				{
					this.archiveReader = null;
					this.archiveWriter = new BinaryWriter(this.workingStream);
				}
				this.centralDirectoryRead = false;
				this.leaveStreamOpen = leaveOpen;
				switch (mode)
				{
				case ZipArchiveMode.Create:
					this.centralDirectoryRead = true;
					break;
				case ZipArchiveMode.Read:
					this.ReadEndOfCentralDirectory();
					break;
				case ZipArchiveMode.Update:
					if (this.Reader.BaseStream.Length != 0L)
					{
						this.ReadEndOfCentralDirectory();
						this.EnsureCentralDirectoryRead();
						using (IEnumerator<ZipArchiveEntry> enumerator = this.Entries.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ZipArchiveEntry zipArchiveEntry = enumerator.Current;
								string message = null;
								if (!zipArchiveEntry.CheckIntegrity(out message))
								{
									throw new InvalidDataException(message);
								}
							}
							break;
						}
					}
					this.centralDirectoryRead = true;
					break;
				}
			}
			catch
			{
				if (this.originalStream != null)
				{
					PlatformSettings.Manager.DeleteTemporaryStream(this.workingStream);
				}
				throw;
			}
		}

		void ReadCentralDirectory()
		{
			this.Reader.BaseStream.Seek(this.CentralDirectoryStart, SeekOrigin.Begin);
			for (ulong num = 0UL; num < this.NumberOfEntriesInTheCentralDirectory; num += 1UL)
			{
				CentralDirectoryHeader centralDirectoryHeader = new CentralDirectoryHeader();
				if (!centralDirectoryHeader.TryReadBlock(this.archiveReader))
				{
					throw new InvalidDataException("Central directory header is broken.");
				}
				ZipArchiveEntry zipArchiveEntry = new ZipArchiveEntry(this, centralDirectoryHeader);
				this.entries.Add(zipArchiveEntry.FullName, zipArchiveEntry);
			}
		}

		void ReadEndOfCentralDirectory()
		{
			try
			{
				this.Reader.BaseStream.Seek(-18L, SeekOrigin.End);
				if (!ZipHelper.SeekBackwardsToSignature(this.Reader.BaseStream, 101010256U))
				{
					throw new InvalidDataException("End of central directory not found.");
				}
				long position = this.Reader.BaseStream.Position;
				this.endOfCentralDirectoryRecord = new EndOfCentralDirectoryRecord();
				if (!this.endOfCentralDirectoryRecord.TryReadBlock(this.archiveReader))
				{
					throw new InvalidDataException("End of central directory not found.");
				}
				if (this.endOfCentralDirectoryRecord.NumberOfThisDisk != this.endOfCentralDirectoryRecord.NumberOfTheDiskWithTheStartOfTheCentralDirectory)
				{
					throw new InvalidDataException("Splitted archive is not supported.");
				}
				if (this.endOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectory != this.endOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectoryOnThisDisk)
				{
					throw new InvalidDataException("Splitted archive is not supported.");
				}
				if (this.endOfCentralDirectoryRecord.NumberOfThisDisk == 65535 || this.endOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber == 4294967295U || this.endOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectory == 65535)
				{
					this.Reader.BaseStream.Seek(position - 16L, SeekOrigin.Begin);
					if (ZipHelper.SeekBackwardsToSignature(this.Reader.BaseStream, 117853008U))
					{
						this.zip64EndOfCentralDirectoryLocator = new Zip64EndOfCentralDirectoryLocator();
						if (!this.zip64EndOfCentralDirectoryLocator.TryReadBlock(this.archiveReader))
						{
							throw new InvalidDataException("ZIP64 End of central directory locator not found.");
						}
						if (this.zip64EndOfCentralDirectoryLocator.RelativeOffsetOfTheZip64EndOfCentralDirectoryRecord > 9223372036854775807UL)
						{
							throw new InvalidDataException("Relative offset of the Zip64 End Of Central Directory Record is too big.");
						}
						long relativeOffsetOfTheZip64EndOfCentralDirectoryRecord = (long)this.zip64EndOfCentralDirectoryLocator.RelativeOffsetOfTheZip64EndOfCentralDirectoryRecord;
						this.Reader.BaseStream.Seek(relativeOffsetOfTheZip64EndOfCentralDirectoryRecord, SeekOrigin.Begin);
						this.zip64EndOfCentralDirectoryRecord = new Zip64EndOfCentralDirectoryRecord();
						if (!this.zip64EndOfCentralDirectoryRecord.TryReadBlock(this.archiveReader))
						{
							throw new InvalidDataException("Zip64 End Of Central Directory Record not found.");
						}
						if (this.zip64EndOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectory > 9223372036854775807UL)
						{
							throw new InvalidDataException("Number of entries is too big.");
						}
						if (this.zip64EndOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber > 9223372036854775807UL)
						{
							throw new InvalidDataException("Relative offset of the Central Directory Start is too big.");
						}
						if (this.zip64EndOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectory != this.zip64EndOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectoryOnThisDisk)
						{
							throw new InvalidDataException("Splitted archive is not supported.");
						}
					}
				}
				if (this.CentralDirectoryStart > this.Reader.BaseStream.Length)
				{
					throw new InvalidDataException("Relative offset of the Central Directory Start is too big.");
				}
			}
			catch (EndOfStreamException innerException)
			{
				throw new InvalidDataException("Archive corrupted", innerException);
			}
			catch (IOException innerException2)
			{
				throw new InvalidDataException("Archive corrupted", innerException2);
			}
		}

		void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		void WriteArchive()
		{
			if (this.Mode == ZipArchiveMode.Update)
			{
				foreach (ZipArchiveEntry zipArchiveEntry in this.entries.Values)
				{
					zipArchiveEntry.Dispose();
				}
			}
			long position = this.Writer.BaseStream.Position;
			foreach (ZipArchiveEntry zipArchiveEntry2 in this.entries.Values)
			{
				zipArchiveEntry2.WriteCentralDirectoryHeader();
			}
			long num = this.Writer.BaseStream.Position - position;
			bool flag = false;
			if ((position >= (long)-1) || (num >= (long)-1) || (this.entries.Count >= 65535))
			{
				flag = true;
			}
			if (flag)
			{
				long position2 = this.Writer.BaseStream.Position;
				if (this.zip64EndOfCentralDirectoryRecord == null)
				{
					this.zip64EndOfCentralDirectoryRecord = new Zip64EndOfCentralDirectoryRecord();
				}
				this.zip64EndOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectory = (ulong)((long)this.entries.Count);
				this.zip64EndOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectoryOnThisDisk = (ulong)((long)this.entries.Count);
				this.zip64EndOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber = (ulong)position;
				this.zip64EndOfCentralDirectoryRecord.SizeOfCentralDirectory = (ulong)num;
				this.zip64EndOfCentralDirectoryRecord.WriteBlock(this.Writer);
				if (this.zip64EndOfCentralDirectoryLocator == null)
				{
					this.zip64EndOfCentralDirectoryLocator = new Zip64EndOfCentralDirectoryLocator();
				}
				this.zip64EndOfCentralDirectoryLocator.NumberOfTheDiskWithTheStartOfTheZip64EndOfCentralDirectory = 0U;
				this.zip64EndOfCentralDirectoryLocator.NumberOfDisks = 1U;
				this.zip64EndOfCentralDirectoryLocator.RelativeOffsetOfTheZip64EndOfCentralDirectoryRecord = (ulong)position2;
				this.zip64EndOfCentralDirectoryLocator.WriteBlock(this.Writer);
			}
			if (this.endOfCentralDirectoryRecord == null)
			{
				this.endOfCentralDirectoryRecord = new EndOfCentralDirectoryRecord();
			}
			this.endOfCentralDirectoryRecord.NumberOfThisDisk = 0;
			this.endOfCentralDirectoryRecord.NumberOfTheDiskWithTheStartOfTheCentralDirectory = 0;
			this.endOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectoryOnThisDisk = ((this.entries.Count < 65535) ? ((ushort)this.entries.Count) : ushort.MaxValue);
			this.endOfCentralDirectoryRecord.NumberOfEntriesInTheCentralDirectory = ((this.entries.Count < 65535) ? ((ushort)this.entries.Count) : ushort.MaxValue);
			this.endOfCentralDirectoryRecord.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber = (uint)((position > (long)-1) ? -1 : position);
			this.endOfCentralDirectoryRecord.SizeOfCentralDirectory = (uint)((num > (long)-1) ? -1 : num);
			this.endOfCentralDirectoryRecord.WriteBlock(this.Writer);
			if (this.Mode == ZipArchiveMode.Update)
			{
				this.workingStream.Seek(0L, SeekOrigin.Begin);
				this.originalStream.Seek(0L, SeekOrigin.Begin);
				this.workingStream.CopyTo(this.originalStream);
				this.originalStream.SetLength(this.workingStream.Length);
			}
		}

		ZipArchiveMode archiveMode;

		BinaryReader archiveReader;

		BinaryWriter archiveWriter;

		bool disposed;

		Encoding entryNameEncoding;

		Stream originalStream;

		Stream workingStream;

		bool leaveStreamOpen;

		bool centralDirectoryRead;

		EndOfCentralDirectoryRecord endOfCentralDirectoryRecord;

		Zip64EndOfCentralDirectoryLocator zip64EndOfCentralDirectoryLocator;

		Zip64EndOfCentralDirectoryRecord zip64EndOfCentralDirectoryRecord;

		Dictionary<string, ZipArchiveEntry> entries = new Dictionary<string, ZipArchiveEntry>();
	}
}
