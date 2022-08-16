using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class Supplement
	{
		public void Read(CFFFontReader reader)
		{
			reader.ReadCard8();
			reader.ReadSID();
		}
	}
}
