using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Converters;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Dictionaries
{
	class FontInfo : PostScriptObject
	{
		public FontInfo()
		{
			this.familyName = base.CreateProperty<string>(new PropertyDescriptor
			{
				Name = "FamilyName"
			});
			this.weight = base.CreateProperty<string>(new PropertyDescriptor
			{
				Name = "Weight"
			});
			this.italicAngle = base.CreateProperty<double>(new PropertyDescriptor
			{
				Name = "ItalicAngle"
			}, 0.0);
			this.underlinePosition = base.CreateProperty<double>(new PropertyDescriptor
			{
				Name = "UnderlinePosition"
			}, Type1Converters.DoubleConverter);
			this.underlineThickness = base.CreateProperty<double>(new PropertyDescriptor
			{
				Name = "UnderlineThickness"
			}, Type1Converters.DoubleConverter);
		}

		public string FamilyName
		{
			get
			{
				return this.familyName.GetValue();
			}
		}

		public string Weight
		{
			get
			{
				return this.weight.GetValue();
			}
		}

		public double ItalicAngle
		{
			get
			{
				return this.italicAngle.GetValue();
			}
		}

		public double UnderlinePosition
		{
			get
			{
				return this.underlinePosition.GetValue();
			}
		}

		public double UnderlineThickness
		{
			get
			{
				return this.underlineThickness.GetValue();
			}
		}

		readonly Property<string> familyName;

		readonly Property<string> weight;

		readonly Property<double> italicAngle;

		readonly Property<double> underlinePosition;

		readonly Property<double> underlineThickness;
	}
}
