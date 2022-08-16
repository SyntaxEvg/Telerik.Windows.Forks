using System;
using System.ComponentModel;
using System.Globalization;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utils
{
	public class FormatProviderSettingsTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(FormatProviderSettings);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return false;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				string a;
				if ((a = text) != null)
				{
					if (a == "ReadAllAtOnce")
					{
						return FormatProviderSettings.ReadAllAtOnce;
					}
					if (a == "ReadOnDemand")
					{
						return FormatProviderSettings.ReadOnDemand;
					}
				}
				return null;
			}
			if (value is FormatProviderSettings)
			{
				return value;
			}
			return null;
		}
	}
}
