using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class StopElement : StyleSheetElementBase
	{
		public StopElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.position = base.RegisterAttribute<double>("position", true);
			this.color = base.RegisterChildElement<ColorElement>("color");
		}

		public double Position
		{
			get
			{
				return this.position.Value;
			}
			set
			{
				this.position.Value = value;
			}
		}

		public ColorElement ColorElement
		{
			get
			{
				return this.color.Element;
			}
			set
			{
				this.color.Element = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "stop";
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, GradientStop stop)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<GradientStop>(stop, "stop");
			this.Position = stop.Position;
			base.CreateElement(this.color);
			this.ColorElement.CopyPropertiesFrom(context, stop.ThemableColor);
		}

		public GradientStop ToGradientStop(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			GradientStop result = null;
			if (this.ColorElement != null)
			{
				result = new GradientStop(this.Position, this.ColorElement.CreateThemableColor(context));
				base.ReleaseElement(this.color);
			}
			return result;
		}

		readonly OpenXmlAttribute<double> position;

		readonly OpenXmlChildElement<ColorElement> color;
	}
}
