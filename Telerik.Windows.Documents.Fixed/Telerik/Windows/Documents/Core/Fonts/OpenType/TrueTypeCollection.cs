using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType
{
	class TrueTypeCollection
	{
		public TrueTypeCollection(OpenTypeFontReader reader)
		{
			this.reader = reader;
		}

		internal OpenTypeFontReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public IEnumerable<OpenTypeFontSourceBase> Fonts
		{
			get
			{
				return this.header.Fonts;
			}
		}

		public void Initialize()
		{
			this.header = new TCCHeader(this);
			this.header.Read(this.Reader);
		}

		readonly OpenTypeFontReader reader;

		TCCHeader header;
	}
}
