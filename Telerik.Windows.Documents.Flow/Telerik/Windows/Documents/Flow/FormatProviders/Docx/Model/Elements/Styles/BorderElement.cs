using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class BorderElement : DocxElementBase
	{
		public BorderElement(DocxPartsManager partsManager, string elementName)
			: base(partsManager)
		{
			this.elementName = elementName;
			this.colorAttributes = new ThemableColorAttributes(base.RegisterAttribute<ConvertedOpenXmlAttribute<AutoColor>>(new ConvertedOpenXmlAttribute<AutoColor>("color", OpenXmlNamespaces.WordprocessingMLNamespace, DocxConverters.AutoColorConverter, false)), base.RegisterAttribute<MappedOpenXmlAttribute<ThemeColorType>>(new MappedOpenXmlAttribute<ThemeColorType>("themeColor", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.ThemeColorTypeMapper, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeTint", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeShade", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), Colors.Black);
			this.frameAttribute = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("frame", OpenXmlNamespaces.WordprocessingMLNamespace, false, false));
			this.shadowAttribute = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("shadow", OpenXmlNamespaces.WordprocessingMLNamespace, false, false));
			this.thicnessAttribute = base.RegisterAttribute<OpenXmlAttribute<double>>(new OpenXmlAttribute<double>("sz", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.spacingAttribute = base.RegisterAttribute<OpenXmlAttribute<double>>(new OpenXmlAttribute<double>("space", OpenXmlNamespaces.WordprocessingMLNamespace, true));
			this.styleAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<BorderStyle>>(new MappedOpenXmlAttribute<BorderStyle>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.BorderStyleMapper, true));
		}

		public override string ElementName
		{
			get
			{
				return this.elementName;
			}
		}

		public Border Value
		{
			get
			{
				return new Border(Unit.PointToDip(this.thicnessAttribute.Value / 8.0), this.styleAttribute.Value, this.colorAttributes.GetThemableColor(), this.shadowAttribute.Value, this.frameAttribute.Value, this.spacingAttribute.Value);
			}
			set
			{
				this.thicnessAttribute.Value = Unit.DipToPoint(value.Thickness * 8.0);
				this.styleAttribute.Value = value.Style;
				if (value.Color != null)
				{
					this.colorAttributes.FillAttributes(value.Color, this.Theme);
				}
				this.shadowAttribute.Value = value.Shadow;
				this.frameAttribute.Value = value.Frame;
				this.spacingAttribute.Value = value.Spacing;
			}
		}

		public DocumentTheme Theme
		{
			get
			{
				if (this.properties.OwnerDocumentElement != null)
				{
					return this.properties.OwnerDocumentElement.Document.Theme;
				}
				return this.properties.OwnerStyle.Document.Theme;
			}
		}

		public void SetAssociatedFlowModelElement(DocumentElementPropertiesBase properties)
		{
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			this.properties = properties;
		}

		readonly ThemableColorAttributes colorAttributes;

		readonly BoolOpenXmlAttribute frameAttribute;

		readonly BoolOpenXmlAttribute shadowAttribute;

		readonly OpenXmlAttribute<double> thicnessAttribute;

		readonly OpenXmlAttribute<double> spacingAttribute;

		readonly MappedOpenXmlAttribute<BorderStyle> styleAttribute;

		readonly string elementName;

		DocumentElementPropertiesBase properties;
	}
}
