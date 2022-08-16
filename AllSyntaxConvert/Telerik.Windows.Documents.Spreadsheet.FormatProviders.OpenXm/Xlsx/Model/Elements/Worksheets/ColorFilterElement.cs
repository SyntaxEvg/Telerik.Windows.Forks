using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class ColorFilterElement : FilterElementBase
	{
		public ColorFilterElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.dxfId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("dxfId", false));
			this.cellColor = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("cellColor", true, false));
		}

		public override string ElementName
		{
			get
			{
				return "colorFilter";
			}
		}

		public bool CellColor
		{
			get
			{
				return this.cellColor.Value;
			}
			set
			{
				this.cellColor.Value = value;
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

		internal override IFilterInfo GetInfo()
		{
			return new ColorFilterInfo
			{
				DxfId = this.DxfId,
				CellColor = this.CellColor
			};
		}

		internal override void CopyPropertiesFrom(IXlsxWorksheetExportContext context, int columnId)
		{
			ColorFilterInfo colorFilterInfo = context.GetColorFilterInfo(columnId);
			this.DxfId = colorFilterInfo.DxfId;
			if (colorFilterInfo.CellColor != this.cellColor.DefaultValue)
			{
				this.CellColor = colorFilterInfo.CellColor;
			}
		}

		readonly BoolOpenXmlAttribute cellColor;

		readonly IntOpenXmlAttribute dxfId;
	}
}
