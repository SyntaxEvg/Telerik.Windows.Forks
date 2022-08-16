using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class FilterElement : WorksheetElementBase
	{
		public FilterElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<string>("val", true);
		}

		public override string ElementName
		{
			get
			{
				return "filter";
			}
		}

		public string Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				this.value.Value = value;
			}
		}

		internal void CopyPropertiesFrom(string value)
		{
			this.Value = value;
		}

		readonly OpenXmlAttribute<string> value;
	}
}
