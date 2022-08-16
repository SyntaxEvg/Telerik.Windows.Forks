using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class SchemeColorElement : ThemeElementBase
	{
		public SchemeColorElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<string>("val", false);
			this.luminanceModulation = base.RegisterChildElement<LuminanceModulationElement>("lumMod");
			this.luminanceOffset = base.RegisterChildElement<LuminanceOffsetElement>("lumOff");
			this.tint = base.RegisterChildElement<TintElement>("tint");
		}

		public override string ElementName
		{
			get
			{
				return "schemeClr";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public string Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				this.value.Value = value;
			}
		}

		public LuminanceModulationElement LuminanceModulationElement
		{
			get
			{
				return this.luminanceModulation.Element;
			}
			set
			{
				this.luminanceModulation.Element = value;
			}
		}

		public LuminanceOffsetElement LuminanceOffsetElement
		{
			get
			{
				return this.luminanceOffset.Element;
			}
			set
			{
				this.luminanceOffset.Element = value;
			}
		}

		public TintElement TintElement
		{
			get
			{
				return this.tint.Element;
			}
			set
			{
				this.tint.Element = value;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ThemableColor themableColor)
		{
			this.Value = Converters.ThemeColorTypeToStringConverter.ConvertToString(themableColor.ThemeColorType);
			double? num = null;
			if (themableColor.TintAndShade != null)
			{
				num = new double?(themableColor.TintAndShade.Value);
			}
			else if (themableColor.ColorShadeType != null)
			{
				num = new double?(context.Theme.ColorScheme.GetTintAndShade(themableColor.ThemeColorType, themableColor.ColorShadeType.Value));
			}
			if (num != null)
			{
				double num2;
				double num3;
				ColorsHelper.GetLuminanceModulationAndOffsetFromTint(num.Value, out num2, out num3);
				base.CreateElement(this.luminanceModulation);
				this.LuminanceModulationElement.Value = (int)(num2 * 100000.0);
				if (num3 != 0.0)
				{
					base.CreateElement(this.luminanceOffset);
					this.LuminanceOffsetElement.Value = (int)(num3 * 100000.0);
				}
			}
		}

		public ThemableColor CreateColor()
		{
			double? num = null;
			if (this.TintElement != null)
			{
				num = new double?(this.TintElement.Value);
			}
			else if (this.LuminanceModulationElement != null)
			{
				int num2 = this.LuminanceModulationElement.Value;
				int num3 = 0;
				if (this.LuminanceOffsetElement != null)
				{
					num3 = this.LuminanceOffsetElement.Value;
				}
				num = new double?(ColorsHelper.GetTintFromLuminanceModulationAndOffset((double)num2 / 100000.0, (double)num3 / 100000.0));
			}
			ThemeColorType themeColorType;
			try
			{
				themeColorType = Converters.ThemeColorTypeToStringConverter.ConvertFromString(this.Value);
			}
			catch (NotImplementedException)
			{
				return null;
			}
			if (num != null)
			{
				return new ThemableColor(themeColorType, num.Value);
			}
			return new ThemableColor(themeColorType, null);
		}

		readonly OpenXmlAttribute<string> value;

		readonly OpenXmlChildElement<LuminanceModulationElement> luminanceModulation;

		readonly OpenXmlChildElement<LuminanceOffsetElement> luminanceOffset;

		readonly OpenXmlChildElement<TintElement> tint;
	}
}
