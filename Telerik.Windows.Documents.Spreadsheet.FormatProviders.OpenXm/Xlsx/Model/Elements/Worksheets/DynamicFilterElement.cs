using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class DynamicFilterElement : FilterElementBase
	{
		public DynamicFilterElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.filterType = base.RegisterAttribute<ConvertedOpenXmlAttribute<DynamicFilterType>>(new ConvertedOpenXmlAttribute<DynamicFilterType>("type", null, XlsxConverters.DynamicFilterTypeConverter, DynamicFilterType.None, false));
			this.value = base.RegisterAttribute<double>("val", false);
			this.valueIso = base.RegisterAttribute<double>("valIso", false);
			this.maxValueIso = base.RegisterAttribute<double>("maxValIso", false);
		}

		public override string ElementName
		{
			get
			{
				return "dynamicFilter";
			}
		}

		public DynamicFilterType Type
		{
			get
			{
				return this.filterType.Value;
			}
			set
			{
				this.filterType.Value = value;
			}
		}

		public double Value
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

		public double ValueIso
		{
			get
			{
				return this.valueIso.Value;
			}
			set
			{
				this.valueIso.Value = value;
			}
		}

		public double MaxValueIso
		{
			get
			{
				return this.maxValueIso.Value;
			}
			set
			{
				this.maxValueIso.Value = value;
			}
		}

		internal override IFilterInfo GetInfo()
		{
			return new DynamicFilterInfo
			{
				DynamicFilterType = this.Type
			};
		}

		internal override void CopyPropertiesFrom(IXlsxWorksheetExportContext context, int columnId)
		{
			DynamicFilterInfo dynamicFilterInfo = context.GetDynamicFilterInfo(columnId);
			this.Type = dynamicFilterInfo.DynamicFilterType;
		}

		readonly ConvertedOpenXmlAttribute<DynamicFilterType> filterType;

		readonly OpenXmlAttribute<double> value;

		readonly OpenXmlAttribute<double> valueIso;

		readonly OpenXmlAttribute<double> maxValueIso;
	}
}
