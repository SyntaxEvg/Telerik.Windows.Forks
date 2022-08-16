using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	class TextRenderingContext : ContentRenderingContext
	{
		internal TextRenderingContext(TextFragment textFragment, Rect currentContainerBounds)
			: base(textFragment, currentContainerBounds)
		{
			string familyName;
			FontWeight fontWeight;
			FontStyle fontStyle;
			FontsHelper.GetFontFamily(textFragment.Font.FontName, out familyName, out fontWeight, out fontStyle);
			this.fontProperties = new FontProperties(new FontFamily(familyName), fontStyle, fontWeight);
			this.text = textFragment.Text;
			this.fontSize = textFragment.FontSize;
			this.geometryProperties = textFragment.TextProperties.GeometryProperties;
		}

		public string Text
		{
			get
			{
				return this.text;
			}
		}

		public double FontSize
		{
			get
			{
				return this.fontSize;
			}
		}

		public FontProperties FontProperties
		{
			get
			{
				return this.fontProperties;
			}
		}

		public GeometryPropertiesOwner GeometryProperties
		{
			get
			{
				return this.geometryProperties;
			}
		}

		readonly string text;

		readonly double fontSize;

		readonly FontProperties fontProperties;

		readonly GeometryPropertiesOwner geometryProperties;
	}
}
