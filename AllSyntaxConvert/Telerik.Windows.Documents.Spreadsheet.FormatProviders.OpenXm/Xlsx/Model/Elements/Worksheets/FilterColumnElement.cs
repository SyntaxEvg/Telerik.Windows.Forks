using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class FilterColumnElement : WorksheetElementBase
	{
		public FilterColumnElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.columnId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("colId", true));
		}

		public override string ElementName
		{
			get
			{
				return "filterColumn";
			}
		}

		public int ColumnId
		{
			get
			{
				return this.columnId.Value;
			}
			set
			{
				this.columnId.Value = value;
			}
		}

		FilterColumnInfo FilterColumnInfo
		{
			get
			{
				if (this.filterColumnInfo == null)
				{
					this.filterColumnInfo = new FilterColumnInfo(this.ColumnId);
				}
				return this.filterColumnInfo;
			}
		}

		internal FilterColumnInfo GetInfo()
		{
			return this.filterColumnInfo;
		}

		internal void CopyPropertiesFrom(IXlsxWorksheetExportContext context, int columnId)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			this.ColumnId = columnId;
		}

		protected override void OnAfterReadChildElement(IXlsxWorksheetImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			FilterElementBase filterElementBase = childElement as FilterElementBase;
			if (filterElementBase != null)
			{
				this.FilterColumnInfo.FilterInfo = filterElementBase.GetInfo();
			}
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			FilterElementBase element = null;
			IFilterInfo filterInfo = context.GetFilterInfo(this.ColumnId);
			if (filterInfo is FiltersInfo)
			{
				element = base.CreateElement<FiltersElement>("filters");
			}
			else if (filterInfo is CustomFiltersInfo)
			{
				element = base.CreateElement<CustomFiltersElement>("customFilters");
			}
			else if (filterInfo is DynamicFilterInfo)
			{
				element = base.CreateElement<DynamicFilterElement>("dynamicFilter");
			}
			else if (filterInfo is Top10FilterInfo)
			{
				element = base.CreateElement<Top10FilterElement>("top10");
			}
			else if (filterInfo is ColorFilterInfo)
			{
				element = base.CreateElement<ColorFilterElement>("colorFilter");
			}
			if (element != null)
			{
				element.CopyPropertiesFrom(context, this.ColumnId);
				yield return element;
			}
			yield break;
		}

		protected override void ClearOverride()
		{
			this.filterColumnInfo = null;
		}

		readonly IntOpenXmlAttribute columnId;

		FilterColumnInfo filterColumnInfo;
	}
}
