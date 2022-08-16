using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables;

namespace Telerik.Windows.Documents.Core.Fonts.Type1
{
	interface ICFFFontFile
	{
		SubrsIndex GlobalSubrs { get; }

		CFFFontReader Reader { get; }

		void ReadTable(CFFTable table);

		string ReadString(ushort sid);
	}
}
