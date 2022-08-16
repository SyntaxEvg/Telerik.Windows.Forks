using System;
using System.Windows.Media;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	abstract class ColorSchemeElementBase : ThemeElementBase
	{
		public ColorSchemeElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.rgbHexColorModel = base.RegisterChildElement<RgbHexColorModelElement>("srgbClr");
			this.systemColor = base.RegisterChildElement<SystemColorElement>("sysClr");
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public abstract ThemeColorType ThemeColorType { get; }

		public SystemColorElement SystemColorElement
		{
			get
			{
				return this.systemColor.Element;
			}
			set
			{
				this.systemColor.Element = value;
			}
		}

		public RgbHexColorModelElement RgbHexColorModelElement
		{
			get
			{
				return this.rgbHexColorModel.Element;
			}
			set
			{
				this.rgbHexColorModel.Element = value;
			}
		}

		public Color ToColor(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			if (this.SystemColorElement != null)
			{
				Color color = this.SystemColorElement.GetColor(context);
				base.ReleaseElement(this.systemColor);
				return color;
			}
			if (this.RgbHexColorModelElement != null)
			{
				Color color2 = this.RgbHexColorModelElement.GetColor();
				base.ReleaseElement(this.rgbHexColorModel);
				return color2;
			}
			throw new ArgumentException("Color type not supported.");
		}

		protected override void OnBeforeWrite(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			base.CreateElement(this.rgbHexColorModel);
			this.RgbHexColorModelElement.SetColor(context.Theme.ColorScheme[this.ThemeColorType].Color);
		}

		readonly OpenXmlChildElement<RgbHexColorModelElement> rgbHexColorModel;

		readonly OpenXmlChildElement<SystemColorElement> systemColor;
	}
}
