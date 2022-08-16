using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	abstract class TableBase
	{
		public TableBase(OpenTypeFontSourceBase fontSource)
		{
			this.fontSource = fontSource;
		}

		internal long Offset { get; set; }

		protected OpenTypeFontReader Reader
		{
			get
			{
				return this.fontSource.Reader;
			}
		}

		protected OpenTypeFontSourceBase FontSource
		{
			get
			{
				return this.fontSource;
			}
		}

		public abstract void Read(OpenTypeFontReader reader);

		internal virtual void Write(FontWriter writer)
		{
		}

		internal virtual void Import(OpenTypeFontReader reader)
		{
		}

		readonly OpenTypeFontSourceBase fontSource;
	}
}
