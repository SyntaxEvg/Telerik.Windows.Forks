using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class SortConditionElement : WorksheetElementBase
	{
		public SortConditionElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.range = base.RegisterAttribute<ConvertedOpenXmlAttribute<Ref>>(new ConvertedOpenXmlAttribute<Ref>("ref", XlsxConverters.RefConverter, true));
			this.customList = base.RegisterAttribute<string>("customList", false);
			this.descending = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("descending"));
			this.sortBy = base.RegisterAttribute<string>("sortBy", Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types.SortBy.Value, false);
			this.dxfId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("dxfId", false));
		}

		public override string ElementName
		{
			get
			{
				return "sortCondition";
			}
		}

		public Ref Range
		{
			get
			{
				return this.range.Value;
			}
			set
			{
				this.range.Value = value;
			}
		}

		public string CustomList
		{
			get
			{
				return this.customList.Value;
			}
			set
			{
				this.customList.Value = value;
			}
		}

		public bool Descending
		{
			get
			{
				return this.descending.Value;
			}
			set
			{
				this.descending.Value = value;
			}
		}

		public string SortBy
		{
			get
			{
				return this.sortBy.Value;
			}
			set
			{
				this.sortBy.Value = value;
			}
		}

		public int DxfId
		{
			get
			{
				return this.dxfId.Value;
			}
			set
			{
				this.dxfId.Value = value;
			}
		}

		internal void CopyPropertiesFrom(SortConditionInfo sortConditionInfo)
		{
			Guard.ThrowExceptionIfNull<SortConditionInfo>(sortConditionInfo, "sortConditionInfo");
			this.Range = new Ref(sortConditionInfo.Range);
			if (sortConditionInfo.Descending != this.descending.DefaultValue)
			{
				this.Descending = sortConditionInfo.Descending;
			}
			if (sortConditionInfo.SortBy != this.sortBy.DefaultValue)
			{
				this.SortBy = sortConditionInfo.SortBy;
			}
			if (sortConditionInfo.CustomList != this.customList.DefaultValue)
			{
				this.CustomList = sortConditionInfo.CustomList;
			}
			if (sortConditionInfo.DxfId != null)
			{
				this.DxfId = sortConditionInfo.DxfId.Value;
			}
		}

		internal SortConditionInfo GetInfo()
		{
			return new SortConditionInfo
			{
				CustomList = this.CustomList,
				Descending = this.Descending,
				Range = this.Range.ToCellRange(),
				SortBy = this.SortBy,
				DxfId = new int?(this.DxfId)
			};
		}

		readonly ConvertedOpenXmlAttribute<Ref> range;

		readonly OpenXmlAttribute<string> customList;

		readonly BoolOpenXmlAttribute descending;

		readonly OpenXmlAttribute<string> sortBy;

		readonly IntOpenXmlAttribute dxfId;
	}
}
