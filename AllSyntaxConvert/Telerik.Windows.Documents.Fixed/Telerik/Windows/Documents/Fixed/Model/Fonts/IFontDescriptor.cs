using System;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	interface IFontDescriptor
	{
		bool IsSymbolic { get; }

		bool IsNonSymbolic { get; }
	}
}
