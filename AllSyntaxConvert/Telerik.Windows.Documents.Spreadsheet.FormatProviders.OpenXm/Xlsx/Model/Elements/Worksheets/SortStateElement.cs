using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class SortStateElement : WorksheetElementBase
	{
		public SortStateElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.caseSensitive = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("caseSensitive"));
			this.columnSort = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("columnSort"));
			this.sortRange = base.RegisterAttribute<ConvertedOpenXmlAttribute<Ref>>(new ConvertedOpenXmlAttribute<Ref>("ref", XlsxConverters.RefConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "sortState";
			}
		}

		public bool CaseSensitive
		{
			get
			{
				return this.caseSensitive.Value;
			}
			set
			{
				this.caseSensitive.Value = value;
			}
		}

		public bool ColumnSort
		{
			get
			{
				return this.columnSort.Value;
			}
			set
			{
				this.columnSort.Value = value;
			}
		}

		public Ref SortRange
		{
			get
			{
				return this.sortRange.Value;
			}
			set
			{
				this.sortRange.Value = value;
			}
		}

		List<SortConditionInfo> Conditions
		{
			get
			{
				if (this.conditions == null)
				{
					this.conditions = new List<SortConditionInfo>();
				}
				return this.conditions;
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			return context.GetSortStateInfo().Range != null;
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			foreach (SortConditionInfo sortCondition in context.GetSortConditionInfos())
			{
				SortConditionElement sortConditionElement = base.CreateElement<SortConditionElement>("sortCondition");
				sortConditionElement.CopyPropertiesFrom(sortCondition);
				yield return sortConditionElement;
			}
			yield break;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			SortStateInfo sortStateInfo = context.GetSortStateInfo();
			if (sortStateInfo.Range != null)
			{
				this.SortRange = new Ref(sortStateInfo.Range);
			}
		}

		protected override void OnAfterReadChildElement(IXlsxWorksheetImportContext context, OpenXmlElementBase childElement)
		{
			SortConditionElement sortConditionElement = childElement as SortConditionElement;
			if (sortConditionElement != null)
			{
				this.Conditions.Add(sortConditionElement.GetInfo());
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			context.ApplySortState(new SortStateInfo
			{
				Range = this.SortRange.ToCellRange(),
				Conditions = this.Conditions
			});
		}

		protected override void ClearOverride()
		{
			this.conditions = null;
		}

		readonly BoolOpenXmlAttribute caseSensitive;

		readonly BoolOpenXmlAttribute columnSort;

		readonly ConvertedOpenXmlAttribute<Ref> sortRange;

		List<SortConditionInfo> conditions;
	}
}
