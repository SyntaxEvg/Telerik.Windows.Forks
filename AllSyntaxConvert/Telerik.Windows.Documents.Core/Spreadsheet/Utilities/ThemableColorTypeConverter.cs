using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	public class ThemableColorTypeConverter : TypeConverter
	{
		static ThemableColorTypeConverter()
		{
			ThemableColorTypeConverter.colorNameToColor.Add("Black", Colors.Black);
			ThemableColorTypeConverter.colorNameToColor.Add("Blue", Colors.Blue);
			ThemableColorTypeConverter.colorNameToColor.Add("Brown", Colors.Brown);
			ThemableColorTypeConverter.colorNameToColor.Add("Cyan", Colors.Cyan);
			ThemableColorTypeConverter.colorNameToColor.Add("DarkGray", Colors.DarkGray);
			ThemableColorTypeConverter.colorNameToColor.Add("Gray", Colors.Gray);
			ThemableColorTypeConverter.colorNameToColor.Add("Green", Colors.Green);
			ThemableColorTypeConverter.colorNameToColor.Add("LightGray", Colors.LightGray);
			ThemableColorTypeConverter.colorNameToColor.Add("Magenta", Colors.Magenta);
			ThemableColorTypeConverter.colorNameToColor.Add("Orange", Colors.Orange);
			ThemableColorTypeConverter.colorNameToColor.Add("Purple", Colors.Purple);
			ThemableColorTypeConverter.colorNameToColor.Add("Red", Colors.Red);
			ThemableColorTypeConverter.colorNameToColor.Add("Transparent", Colors.Transparent);
			ThemableColorTypeConverter.colorNameToColor.Add("White", Colors.White);
			ThemableColorTypeConverter.colorNameToColor.Add("Yellow", Colors.Yellow);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = (string)value;
			if (ThemableColorTypeConverter.colorNameToColor.ContainsKey(text))
			{
				return new ThemableColor(ThemableColorTypeConverter.colorNameToColor[text]);
			}
			text = text.Substring(1, text.Length - 1);
			uint num = uint.Parse(text, NumberStyles.HexNumber);
			byte a = (byte)(num >> 24);
			byte r = (byte)(num >> 16);
			byte g = (byte)(num >> 8);
			byte b = (byte)num;
			return new ThemableColor(Color.FromArgb(a, r, g, b));
		}

		static readonly Dictionary<string, Color> colorNameToColor = new Dictionary<string, Color>();
	}
}
