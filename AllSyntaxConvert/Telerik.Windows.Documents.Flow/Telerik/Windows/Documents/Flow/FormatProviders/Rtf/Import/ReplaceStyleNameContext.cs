using System;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	class ReplaceStyleNameContext
	{
		public string InputStyleName { get; set; }

		public Style StyleDefinition { get; set; }
	}
}
