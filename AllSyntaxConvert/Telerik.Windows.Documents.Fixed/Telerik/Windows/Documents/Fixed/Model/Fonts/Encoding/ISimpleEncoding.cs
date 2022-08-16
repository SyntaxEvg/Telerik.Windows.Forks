using System;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding
{
	interface ISimpleEncoding
	{
		bool IsNamedEncoding { get; }

		string BaseEncodingName { get; }

		string GetName(byte b);
	}
}
