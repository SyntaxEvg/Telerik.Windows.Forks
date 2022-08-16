using System;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	class FontFileInfo
	{
		public FontFileInfo()
		{
			this.fileType = new PdfProperty<FontFileType>();
			this.fileSubType = new PdfProperty<FontFileSubType>();
			this.length1 = new PdfProperty<int>();
			this.length2 = new PdfProperty<int>();
			this.length3 = new PdfProperty<int>();
			this.metadata = new PdfProperty<byte[]>();
		}

		internal PdfProperty<FontFileType> FileType
		{
			get
			{
				return this.fileType;
			}
		}

		internal PdfProperty<FontFileSubType> FileSubType
		{
			get
			{
				return this.fileSubType;
			}
		}

		internal PdfProperty<int> Length1
		{
			get
			{
				return this.length1;
			}
		}

		internal PdfProperty<int> Length2
		{
			get
			{
				return this.length2;
			}
		}

		internal PdfProperty<int> Length3
		{
			get
			{
				return this.length3;
			}
		}

		internal PdfProperty<byte[]> Metadata
		{
			get
			{
				return this.metadata;
			}
		}

		readonly PdfProperty<FontFileType> fileType;

		readonly PdfProperty<FontFileSubType> fileSubType;

		readonly PdfProperty<int> length1;

		readonly PdfProperty<int> length2;

		readonly PdfProperty<int> length3;

		readonly PdfProperty<byte[]> metadata;
	}
}
