using System;

namespace Telerik.Windows.Zip
{
	public class DeflateSettings : CompressionSettings
	{
		public DeflateSettings()
		{
			base.Method = CompressionMethod.Deflate;
			this.CompressionLevel = CompressionLevel.Optimal;
			this.HeaderType = CompressedStreamHeader.ZLib;
		}

		public CompressionLevel CompressionLevel
		{
			get
			{
				return this.compressionLevel;
			}
			set
			{
				this.compressionLevel = value;
				base.OnPropertyChanged("CompressionLevel");
			}
		}

		public CompressedStreamHeader HeaderType
		{
			get
			{
				return this.headerType;
			}
			set
			{
				this.headerType = value;
				base.OnPropertyChanged("HeaderType");
			}
		}

		internal override void CopyFrom(CompressionSettings baseSettings)
		{
			DeflateSettings deflateSettings = baseSettings as DeflateSettings;
			if (deflateSettings != null)
			{
				this.HeaderType = deflateSettings.HeaderType;
				this.CompressionLevel = deflateSettings.CompressionLevel;
			}
		}

		internal override void PrepareForZip(CentralDirectoryHeader header = null)
		{
			base.PrepareForZip(header);
			this.HeaderType = CompressedStreamHeader.None;
		}

		CompressionLevel compressionLevel;

		CompressedStreamHeader headerType;
	}
}
