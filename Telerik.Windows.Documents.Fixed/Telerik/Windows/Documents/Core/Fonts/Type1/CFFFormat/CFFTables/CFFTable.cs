using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	abstract class CFFTable
	{
		public CFFTable(ICFFFontFile file, long offset)
		{
			this.file = file;
			this.offset = offset;
		}

		protected CFFFontReader Reader
		{
			get
			{
				return this.file.Reader;
			}
		}

		internal ICFFFontFile File
		{
			get
			{
				return this.file;
			}
		}

		public long Offset
		{
			get
			{
				return this.offset;
			}
		}

		public abstract void Read(CFFFontReader reader);

		readonly ICFFFontFile file;

		readonly long offset;
	}
}
