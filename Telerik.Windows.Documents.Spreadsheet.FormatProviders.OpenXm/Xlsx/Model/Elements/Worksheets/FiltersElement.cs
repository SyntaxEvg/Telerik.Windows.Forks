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
	class FiltersElement : FilterElementBase
	{
		public FiltersElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.blank = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("blank"));
		}

		public override string ElementName
		{
			get
			{
				return "filters";
			}
		}

		public bool Blank
		{
			get
			{
				return this.blank.Value;
			}
			set
			{
				this.blank.Value = value;
			}
		}

		FiltersInfo FiltersInfo
		{
			get
			{
				if (this.filtersInfo == null)
				{
					this.filtersInfo = new FiltersInfo();
				}
				return this.filtersInfo;
			}
		}

		internal override IFilterInfo GetInfo()
		{
			return this.filtersInfo;
		}

		internal override void CopyPropertiesFrom(IXlsxWorksheetExportContext context, int columnId)
		{
			this.columnId = new int?(columnId);
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			return true;
		}

		protected override void ClearOverride()
		{
			this.filtersInfo = null;
		}

		protected override void OnAfterReadChildElement(IXlsxWorksheetImportContext context, OpenXmlElementBase childElement)
		{
			FilterElement filterElement = childElement as FilterElement;
			if (filterElement != null)
			{
				this.FiltersInfo.StringFilters.Add(filterElement.Value);
			}
			DateGroupItemElement dateGroupItemElement = childElement as DateGroupItemElement;
			if (dateGroupItemElement != null)
			{
				DateGroupItemInfo item = new DateGroupItemInfo(dateGroupItemElement.DateTimeGrouping, dateGroupItemElement.Year, dateGroupItemElement.Month, dateGroupItemElement.Day, dateGroupItemElement.Hour, dateGroupItemElement.Minute, dateGroupItemElement.Second);
				this.FiltersInfo.DateFilters.Add(item);
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			this.FiltersInfo.Blank = this.Blank;
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			FiltersInfo filterInfo = context.GetFiltersInfo(this.columnId.Value);
			for (int i = 0; i < filterInfo.StringFilters.Count; i++)
			{
				FilterElement filterElement = base.CreateElement<FilterElement>("filter");
				filterElement.CopyPropertiesFrom(filterInfo.StringFilters[i]);
				yield return filterElement;
			}
			for (int j = 0; j < filterInfo.DateFilters.Count; j++)
			{
				DateGroupItemElement dateGroupItemElement = base.CreateElement<DateGroupItemElement>("dateGroupItem");
				dateGroupItemElement.CopyPropertiesFrom(filterInfo.DateFilters[j]);
				yield return dateGroupItemElement;
			}
			yield break;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			FiltersInfo filtersInfo = context.GetFiltersInfo(this.columnId.Value);
			if (filtersInfo.Blank != this.blank.DefaultValue)
			{
				this.Blank = filtersInfo.Blank;
			}
		}

		readonly BoolOpenXmlAttribute blank;

		int? columnId;

		FiltersInfo filtersInfo;
	}
}
