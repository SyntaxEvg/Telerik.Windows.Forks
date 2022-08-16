using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class CustomFiltersElement : FilterElementBase
	{
		public CustomFiltersElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.isAnd = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("and"));
		}

		public override string ElementName
		{
			get
			{
				return "customFilters";
			}
		}

		public bool IsAnd
		{
			get
			{
				return this.isAnd.Value;
			}
			set
			{
				this.isAnd.Value = value;
			}
		}

		CustomFiltersInfo CustomFiltersInfo
		{
			get
			{
				if (this.customFiltersInfo == null)
				{
					this.customFiltersInfo = new CustomFiltersInfo();
				}
				return this.customFiltersInfo;
			}
		}

		internal override IFilterInfo GetInfo()
		{
			return this.customFiltersInfo;
		}

		internal override void CopyPropertiesFrom(IXlsxWorksheetExportContext context, int columnId)
		{
			this.columnId = new int?(columnId);
		}

		protected override void ClearOverride()
		{
			this.customFiltersInfo = null;
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			return base.ShouldExport(context) || this.columnId != null;
		}

		protected override void OnAfterReadChildElement(IXlsxWorksheetImportContext context, OpenXmlElementBase childElement)
		{
			CustomFilterElement customFilterElement = childElement as CustomFilterElement;
			if (customFilterElement != null)
			{
				CustomFilterCriteriaInfo item = new CustomFilterCriteriaInfo(new CustomFilterCriteria(customFilterElement.FilterOperator, customFilterElement.Value));
				this.CustomFiltersInfo.CustomFilters.Add(item);
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			this.CustomFiltersInfo.IsAnd = this.IsAnd;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			CustomFiltersInfo customFilterInfo = context.GetCustomFilterInfo(this.columnId.Value);
			if (customFilterInfo.IsAnd)
			{
				this.IsAnd = customFilterInfo.IsAnd;
			}
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			CustomFiltersInfo customFilters = context.GetCustomFilterInfo(this.columnId.Value);
			CustomFilterElement customFilterElement = base.CreateElement<CustomFilterElement>("customFilter");
			customFilterElement.CopyPropertiesFrom(customFilters.CustomFilters[0]);
			yield return customFilterElement;
			if (customFilters.CustomFilters.Count > 1)
			{
				CustomFilterElement customFilterElement2 = base.CreateElement<CustomFilterElement>("customFilter");
				customFilterElement2.CopyPropertiesFrom(customFilters.CustomFilters[1]);
				yield return customFilterElement2;
			}
			yield break;
		}

		readonly BoolOpenXmlAttribute isAnd;

		CustomFiltersInfo customFiltersInfo;

		int? columnId;
	}
}
