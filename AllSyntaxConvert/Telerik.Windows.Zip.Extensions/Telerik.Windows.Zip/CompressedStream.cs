using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	public class CompressedStream : OperationStream
	{
		public CompressedStream(Stream baseStream, StreamOperationMode mode, CompressionSettings settings)
			: this(baseStream, mode, settings, true, null)
		{
		}

		public CompressedStream(Stream baseStream, StreamOperationMode mode, CompressionSettings settings, bool useCrc32, EncryptionSettings encryptionSettings)
			: base(baseStream, mode)
		{
			baseStream = ((encryptionSettings != null) ? new CryptoStream(baseStream, mode, PlatformSettings.Manager.GetCryptoProvider(encryptionSettings)) : baseStream);
			ICompressionAlgorithm compressionAlgorithm = ZipHelper.GetCompressionAlgorithm(settings);
			IChecksumAlgorithm checksumAlgorithm2;
			if (!useCrc32)
			{
				IChecksumAlgorithm checksumAlgorithm = new Adler32();
				checksumAlgorithm2 = checksumAlgorithm;
			}
			else
			{
				checksumAlgorithm2 = new Crc32();
			}
			IChecksumAlgorithm checksumAlgorithm3 = checksumAlgorithm2;
			this.Initialize(baseStream, compressionAlgorithm, checksumAlgorithm3);
		}

		internal CompressedStream(Stream baseStream, StreamOperationMode mode, ICompressionAlgorithm compressionAlgorithm, IChecksumAlgorithm checksumAlgorithm)
			: base(baseStream, mode)
		{
			if (compressionAlgorithm == null)
			{
				throw new ArgumentNullException("compressionAlgorithm");
			}
			this.Initialize(baseStream, compressionAlgorithm, checksumAlgorithm);
		}

		public event EventHandler ChecksumReady;

		public long Checksum { get; set; }

		public long CompressedSize
		{
			get
			{
				if (base.IsDisposed)
				{
					return this.compressedSize;
				}
				CryptoStream cryptoStream = base.BaseStream as CryptoStream;
				if (cryptoStream != null)
				{
					return cryptoStream.TotalTransformedCount + ((cryptoStream.Transform != null && cryptoStream.Transform.Header.Buffer != null) ? ((long)cryptoStream.Transform.Header.Buffer.Length) : 0L);
				}
				return base.TotalTransformedCount + ((base.Transform != null && base.Transform.Header.Buffer != null && base.Transform.Header.CountHeaderInCompressedSize) ? ((long)base.Transform.Header.Buffer.Length) : 0L);
			}
		}

		internal IChecksumAlgorithm ChecksumAlgorithm { get; set; }

		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = base.Read(buffer, offset, count);
			if (num != 0)
			{
				if (this.ChecksumAlgorithm != null)
				{
					this.Checksum = (long)((ulong)this.ChecksumAlgorithm.UpdateChecksum((uint)this.Checksum, buffer, offset, num));
				}
			}
			else if (this.ChecksumReady != null)
			{
				this.ChecksumReady(this, EventArgs.Empty);
			}
			return num;
		}

		public override void SetLength(long value)
		{
			if (base.Mode == StreamOperationMode.Read)
			{
				CryptoStream cryptoStream = base.BaseStream as CryptoStream;
				if (cryptoStream != null)
				{
					cryptoStream.SetLength(value);
				}
				base.SetLength(value);
				return;
			}
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			base.Write(buffer, offset, count);
			if (this.ChecksumAlgorithm != null)
			{
				this.Checksum = (long)((ulong)this.ChecksumAlgorithm.UpdateChecksum((uint)this.Checksum, buffer, offset, count));
				if (this.ChecksumReady != null)
				{
					this.ChecksumReady(this, EventArgs.Empty);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!base.IsDisposed)
			{
				try
				{
					if (!base.HasFlushedFinalBlock)
					{
						this.FlushFinalBlock();
					}
					this.compressedSize = this.CompressedSize;
				}
				finally
				{
					this.algorithm = null;
					base.Dispose(disposing);
				}
			}
		}

		void Initialize(Stream baseStream, ICompressionAlgorithm compressionAlgorithm, IChecksumAlgorithm checksumAlgorithm)
		{
			base.BaseStream = baseStream;
			this.algorithm = compressionAlgorithm;
			this.ChecksumAlgorithm = checksumAlgorithm;
			switch (base.Mode)
			{
			case StreamOperationMode.Read:
				base.Transform = this.algorithm.CreateDecompressor();
				return;
			case StreamOperationMode.Write:
				base.Transform = this.algorithm.CreateCompressor();
				return;
			default:
				return;
			}
		}

		ICompressionAlgorithm algorithm;

		long compressedSize;
	}
}
