using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class AutoFilterElement : WorksheetElementBase
	{
		public AutoFilterElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.reference = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRefRange>>(new ConvertedOpenXmlAttribute<CellRefRange>("ref", XlsxConverters.CellRefRangeConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "autoFilter";
			}
		}

		public CellRefRange Reference
		{
			get
			{
				return this.reference.Value;
			}
			set
			{
				this.reference.Value = value;
			}
		}

		List<FilterColumnInfo> FilterColumnInfos
		{
			get
			{
				if (this.filterColumnInfos == null)
				{
					this.filterColumnInfos = new List<FilterColumnInfo>();
				}
				return this.filterColumnInfos;
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			return context.GetAutoFilterRange() != null;
		}

		protected override void OnAfterReadChildElement(IXlsxWorksheetImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			FilterColumnElement filterColumnElement = childElement as FilterColumnElement;
			if (filterColumnElement != null)
			{
				this.FilterColumnInfos.Add(filterColumnElement.GetInfo());
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			context.AutoFilterInfo = new AutoFilterInfo(this.Reference.ToCellRange(), this.filterColumnInfos);
		}

		protected override void ClearOverride()
		{
			this.filterColumnInfos = null;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			CellRefRange autoFilterRange = context.GetAutoFilterRange();
			if (autoFilterRange != null)
			{
				this.Reference = autoFilterRange;
			}
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			foreach (FilterColumnInfo filterColumnInfo in context.GetFilterColumnInfos())
			{
				FilterColumnElement filterColumnElement = base.CreateElement<FilterColumnElement>("filterColumn");
				filterColumnElement.CopyPropertiesFrom(context, filterColumnInfo.ColumnId);
				yield return filterColumnElement;
			}
			yield break;
		}

		readonly ConvertedOpenXmlAttribute<CellRefRange> reference;

		List<FilterColumnInfo> filterColumnInfos;
	}
}
