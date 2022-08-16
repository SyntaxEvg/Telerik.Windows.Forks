using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	interface IEncoding
	{
		IEnumerable<Tuple<char, CharCodeOld>> Encode(byte[] bytes);
	}
}
