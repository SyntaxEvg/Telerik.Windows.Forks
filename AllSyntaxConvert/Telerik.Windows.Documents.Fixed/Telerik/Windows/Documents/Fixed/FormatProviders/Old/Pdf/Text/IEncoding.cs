using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	interface IEncoding
	{
		global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Core.Data.Tuple<char, global::Telerik.Windows.Documents.Fixed.Model.Internal.Classes.CharCodeOld>> Encode(byte[] bytes);
	}
}
