using System;

namespace Telerik.Windows.Zip
{
	public class LzmaSettings : CompressionSettings
	{
		public LzmaSettings()
		{
			base.Method = CompressionMethod.Lzma;
			this.UseZipHeader = false;
			this.dictionarySize = 23;
			this.positionStateBits = 2;
			this.literalContextBits = 3;
			this.literalPositionBits = 0;
			this.fastBytes = 32;
			this.matchFinderType = LzmaMatchFinderType.BT4;
			this.StreamLength = -1L;
		}

		public int DictionarySize
		{
			get
			{
				return this.dictionarySize;
			}
			set
			{
				this.dictionarySize = value;
				base.OnPropertyChanged("DictionarySize");
			}
		}

		public int PositionStateBits
		{
			get
			{
				return this.positionStateBits;
			}
			set
			{
				this.positionStateBits = value;
				base.OnPropertyChanged("PositionStateBits");
			}
		}

		public int LiteralContextBits
		{
			get
			{
				return this.literalContextBits;
			}
			set
			{
				this.literalContextBits = value;
				base.OnPropertyChanged("LiteralContextBits");
			}
		}

		public int LiteralPositionBits
		{
			get
			{
				return this.literalPositionBits;
			}
			set
			{
				this.literalPositionBits = value;
				base.OnPropertyChanged("LiteralPositionBits");
			}
		}

		public int FastBytes
		{
			get
			{
				return this.fastBytes;
			}
			set
			{
				this.fastBytes = value;
				base.OnPropertyChanged("FastBytes");
			}
		}

		public LzmaMatchFinderType MatchFinderType
		{
			get
			{
				return this.matchFinderType;
			}
			set
			{
				this.matchFinderType = value;
				base.OnPropertyChanged("MatchFinderType");
			}
		}

		public long StreamLength { get; set; }

		internal long InternalStreamLength { get; set; }

		internal bool UseZipHeader { get; set; }

		internal override void CopyFrom(CompressionSettings baseSettings)
		{
			LzmaSettings lzmaSettings = baseSettings as LzmaSettings;
			if (lzmaSettings != null)
			{
				this.UseZipHeader = lzmaSettings.UseZipHeader;
				this.DictionarySize = lzmaSettings.DictionarySize;
				this.PositionStateBits = lzmaSettings.PositionStateBits;
				this.LiteralContextBits = lzmaSettings.LiteralContextBits;
				this.LiteralPositionBits = lzmaSettings.LiteralPositionBits;
				this.FastBytes = lzmaSettings.FastBytes;
				this.MatchFinderType = lzmaSettings.MatchFinderType;
			}
		}

		internal override void PrepareForZip(CentralDirectoryHeader header = null)
		{
			base.PrepareForZip(header);
			this.UseZipHeader = true;
			if (header != null)
			{
				this.InternalStreamLength = (long)((ulong)header.UncompressedSize);
			}
		}

		int dictionarySize;

		int positionStateBits;

		int literalContextBits;

		int literalPositionBits;

		int fastBytes;

		LzmaMatchFinderType matchFinderType;
	}
}
