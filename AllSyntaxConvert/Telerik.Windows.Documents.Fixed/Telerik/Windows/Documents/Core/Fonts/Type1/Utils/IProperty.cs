using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Utils
{
	interface IProperty
	{
		PropertyDescriptor Descriptor { get; }

		bool SetValue(object value);
	}
}
