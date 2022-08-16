using System;
using System.Windows.Media;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Theming;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class SolidFillElement : ThemeElementBase
	{
		public SolidFillElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.schemeColor = base.RegisterChildElement<SchemeColorElement>("schemeClr");
			this.rgbHexColor = base.RegisterChildElement<RgbHexColorModelElement>("srgbClr");
		}

		public override string ElementName
		{
			get
			{
				return "solidFill";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public SchemeColorElement SchemeColorElement
		{
			get
			{
				return this.schemeColor.Element;
			}
			set
			{
				this.schemeColor.Element = value;
			}
		}

		public RgbHexColorModelElement RgbHexColorModelElement
		{
			get
			{
				return this.rgbHexColor.Element;
			}
			set
			{
				this.rgbHexColor.Element = value;
			}
		}

		public void SetDefaultProperties()
		{
			base.CreateElement(this.schemeColor);
			this.SchemeColorElement.Value = "phClr";
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, SolidFill solidFill)
		{
			if (solidFill.Color.IsFromTheme)
			{
				base.CreateElement(this.schemeColor);
				this.SchemeColorElement.CopyPropertiesFrom(context, solidFill.Color);
				return;
			}
			base.CreateElement(this.rgbHexColor);
			this.RgbHexColorModelElement.SetColor(solidFill.Color.LocalValue);
		}

		public SolidFill CreateSolidFill()
		{
			if (this.SchemeColorElement != null)
			{
				ThemableColor color = this.SchemeColorElement.CreateColor();
				base.ReleaseElement(this.schemeColor);
				return new SolidFill(color);
			}
			if (this.RgbHexColorModelElement != null)
			{
				Color color2 = this.RgbHexColorModelElement.GetColor();
				base.ReleaseElement(this.schemeColor);
				return new SolidFill(color2);
			}
			return null;
		}

		readonly OpenXmlChildElement<SchemeColorElement> schemeColor;

		readonly OpenXmlChildElement<RgbHexColorModelElement> rgbHexColor;
	}
}
