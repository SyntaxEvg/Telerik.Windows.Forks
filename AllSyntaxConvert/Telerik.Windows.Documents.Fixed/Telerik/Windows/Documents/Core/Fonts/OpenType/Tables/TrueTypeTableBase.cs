using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	abstract class TrueTypeTableBase : TableBase
	{
		public TrueTypeTableBase(OpenTypeFontSourceBase fontSource)
			: base(fontSource)
		{
		}

		internal abstract uint Tag { get; }
	}
}
