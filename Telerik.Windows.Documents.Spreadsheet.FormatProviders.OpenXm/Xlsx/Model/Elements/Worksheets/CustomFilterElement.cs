using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class CustomFilterElement : WorksheetElementBase
	{
		public CustomFilterElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.filterOperator = base.RegisterAttribute<ConvertedOpenXmlAttribute<ComparisonOperator>>(new ConvertedOpenXmlAttribute<ComparisonOperator>("operator", null, XlsxConverters.ComparisonOperatorConverter, ComparisonOperator.EqualsTo, false));
			this.value = base.RegisterAttribute<string>("val", true);
		}

		public override string ElementName
		{
			get
			{
				return "customFilter";
			}
		}

		public ComparisonOperator FilterOperator
		{
			get
			{
				return this.filterOperator.Value;
			}
			set
			{
				this.filterOperator.Value = value;
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

		internal void CopyPropertiesFrom(CustomFilterCriteriaInfo customFilterInfo)
		{
			this.FilterOperator = customFilterInfo.FilterOperator;
			this.Value = customFilterInfo.FilterValue;
		}

		readonly ConvertedOpenXmlAttribute<ComparisonOperator> filterOperator;

		readonly OpenXmlAttribute<string> value;
	}
}
