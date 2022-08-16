using System;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class DynamicFilterInfo : IFilterInfo
	{
		public DynamicFilterType DynamicFilterType { get; set; }
	}
}
