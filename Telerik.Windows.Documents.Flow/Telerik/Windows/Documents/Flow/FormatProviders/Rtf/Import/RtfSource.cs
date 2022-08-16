using System;
using System.IO;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Parser;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	sealed class RtfSource
	{
		public RtfSource(RtfReader rtf)
		{
			if (rtf == null)
			{
				throw new ArgumentNullException("rtf");
			}
			this.Reader = rtf;
		}

		public RtfSource(Stream rtf)
		{
			if (rtf == null)
			{
				throw new ArgumentNullException("rtf");
			}
			this.Reader = new RtfReader(rtf);
		}

		public RtfReader Reader { get; set; }
	}
}
