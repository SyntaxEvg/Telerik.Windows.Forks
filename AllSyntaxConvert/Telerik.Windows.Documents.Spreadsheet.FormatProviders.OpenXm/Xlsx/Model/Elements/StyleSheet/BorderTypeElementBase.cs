using System;
using System.Windows.Media;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	abstract class BorderTypeElementBase : StyleSheetElementBase
	{
		public BorderTypeElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.style = base.RegisterAttribute<string>("style", BorderStyles.None, false);
			this.color = base.RegisterChildElement<ColorElement>("color");
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public string Style
		{
			get
			{
				return this.style.Value;
			}
			set
			{
				this.style.Value = value;
			}
		}

		public ColorElement ColorElement
		{
			get
			{
				return this.color.Element;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, CellBorder border)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<CellBorder>(border, "border");
			this.Style = BorderStyles.GetBorderStyleName(border.Style);
			if (border.Style != CellBorderStyle.None && border.Color != null)
			{
				base.CreateElement(this.color);
				this.ColorElement.CopyPropertiesFrom(context, border.Color);
			}
		}

		public CellBorder CreateCellBorder(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			CellBorder result = null;
			if (this.Style != BorderStyles.None)
			{
				ThemableColor themableColor = ((this.ColorElement != null) ? this.ColorElement.CreateThemableColor(context) : new ThemableColor(Colors.Black));
				result = new CellBorder(BorderStyles.GetBorderStyle(this.Style), themableColor);
			}
			base.ReleaseElement(this.color);
			return result;
		}

		readonly OpenXmlAttribute<string> style;

		readonly OpenXmlChildElement<ColorElement> color;
	}
}
