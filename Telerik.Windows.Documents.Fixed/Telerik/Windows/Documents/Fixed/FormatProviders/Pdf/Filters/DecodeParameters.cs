using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	public class DecodeParameters : Dictionary<string, object>
	{
		internal DecodeParameters()
		{
		}

		internal DecodeParameters(Dictionary<string, object> dictionary)
			: base(dictionary)
		{
		}
	}
}
