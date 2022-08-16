using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.TextMeasurer
{
	class RadTextMeasurer : ITextMeasurer
	{
		public TextMeasurementInfo MeasureText(TextProperties textProperties, FontProperties fontProperties)
		{
			double num = textProperties.Size / 100.0;
			double width;
			if (textProperties.FlowDirection == FlowDirection.RightToLeft || RadTextMeasurer.LigatureLanguageRegex.IsMatch(textProperties.Text))
			{
				width = RadTextMeasurer.MeasureWholeTextWidth(textProperties, fontProperties);
			}
			else
			{
				width = RadTextMeasurer.MeasureTextWidth(textProperties, fontProperties) * num;
			}
			TextMeasurementInfo textMeasurementInfo = new TextMeasurementInfo();
			textMeasurementInfo.Size = new Size(width, RadTextMeasurer.GetLineHeight(fontProperties) * num);
			textMeasurementInfo.BaselineOffset = RadTextMeasurer.GetBaseLineOffset(fontProperties, textProperties.FlowDirection) * textMeasurementInfo.Size.Height;
			return textMeasurementInfo;
		}

		internal static double GetPairWidthKerningError(string pair, FontProperties fontProperties)
		{
			string source = fontProperties.FontFamily.Source;
			if (!SystemFontsManager.HasKerning(source))
			{
				return 0.0;
			}
			double num = 0.0;
			Dictionary<string, double> dictionary;
			lock (RadTextMeasurer.kerningPairToFontKerningValue)
			{
				if (!RadTextMeasurer.kerningPairToFontKerningValue.TryGetValue(pair, out dictionary) && dictionary == null)
				{
					dictionary = new Dictionary<string, double>();
					RadTextMeasurer.kerningPairToFontKerningValue[pair] = dictionary;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(fontProperties.FontFamily.Source);
			stringBuilder.Append(fontProperties.FontWeight);
			stringBuilder.Append(fontProperties.FontStyle);
			string key = stringBuilder.ToString();
			lock (dictionary)
			{
				if (!dictionary.TryGetValue(key, out num))
				{
					num = RadTextMeasurer.CalculateKerning(pair, fontProperties);
					dictionary.Add(key, num);
				}
			}
			return num;
		}

		static double MeasureTextWidth(TextProperties textProperties, FontProperties fontProperties)
		{
			string text = textProperties.Text;
			double num = 0.0;
			Size symbolSize = new Size(0.0, 0.0);
			for (int i = 0; i < text.Length; i++)
			{
				if (i + 1 < text.Length)
				{
					string pair = text[i].ToString() + text[i + 1];
					double pairWidthKerningError = RadTextMeasurer.GetPairWidthKerningError(pair, fontProperties);
					num += pairWidthKerningError;
				}
				symbolSize = RadTextMeasurer.GetSymbolSize(text[i], fontProperties);
				num += symbolSize.Width;
			}
			return num;
		}

		static double GetLineHeight(FontProperties fontProperties)
		{
			return RadTextMeasurer.GetSymbolSize('X', fontProperties).Height;
		}

		static double CalculateKerning(string pair, FontProperties fontProperties)
		{
			double width = RadTextMeasurer.GetSymbolSize(pair[0], fontProperties).Width;
			double width2 = RadTextMeasurer.GetSymbolSize(pair[1], fontProperties).Width;
			string text = DocumentsHelper.EscapeWhiteSpaces(pair);
			double width3 = RadTextMeasurer.MeasureTextSize(text, fontProperties).Width;
			return width3 - (width + width2);
		}

		static Size GetSymbolSize(char symbol, FontProperties fontProperties)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(symbol);
			stringBuilder.Append(fontProperties.FontFamily.Source);
			stringBuilder.Append(fontProperties.FontWeight);
			stringBuilder.Append(fontProperties.FontStyle);
			string key = stringBuilder.ToString();
			Size size;
			lock (RadTextMeasurer.letterSizeCache)
			{
				if (!RadTextMeasurer.letterSizeCache.TryGetValue(key, out size))
				{
					string text = DocumentsHelper.EscapeWhiteSpaces(symbol.ToString());
					size = RadTextMeasurer.MeasureTextSize(text, fontProperties);
					RadTextMeasurer.letterSizeCache[key] = size;
				}
			}
			return size;
		}

		public static TextMeasurementInfo MeasureTextWithWrapping(TextProperties textProperties, FontProperties fontProperties, double wrappingWidth)
		{
			FormattedText formattedText = RadTextMeasurer.CreateFormattedText(textProperties.Text.TrimEnd(new char[0]), textProperties.Size, fontProperties, textProperties.FlowDirection);
			formattedText.MaxTextWidth = wrappingWidth;
			formattedText.Trimming = TextTrimming.None;
			return new TextMeasurementInfo
			{
				Size = new Size(formattedText.Width, formattedText.Height)
			};
		}

		static double MeasureWholeTextWidth(TextProperties textProperties, FontProperties fontProperties)
		{
			FormattedText formattedText = RadTextMeasurer.CreateFormattedText(textProperties.Text, textProperties.Size, fontProperties, textProperties.FlowDirection);
			return formattedText.Width;
		}

		static FormattedText CreateFormattedText(string text, double fontSize, FontProperties fontProperties, FlowDirection flowDirection)
		{
			return new FormattedText(text, CultureInfo.CurrentCulture, flowDirection, new Typeface(fontProperties.FontFamily, fontProperties.FontStyle, fontProperties.FontWeight, FontStretches.Normal), fontSize, Brushes.Black);
		}

		static Size MeasureTextSize(string text, FontProperties fontProperties)
		{
			FormattedText formattedText = RadTextMeasurer.CreateFormattedText(text, 100.0, fontProperties, FlowDirection.LeftToRight);
			return new Size(formattedText.Width, formattedText.Height);
		}

		static double GetBaseLineOffset(FontProperties fontProperties, FlowDirection flowDirection)
		{
			FontProperties fontProperties2 = new FontProperties(fontProperties.FontFamily);
			double num;
			lock (RadTextMeasurer.fontBaseLineOffsetCache)
			{
				if (!RadTextMeasurer.fontBaseLineOffsetCache.TryGetValue(fontProperties2.FontFamilyName, out num))
				{
					FormattedText formattedText = RadTextMeasurer.CreateFormattedText("’'`\"\\@#$%^&*()+=-_/|:].,!~abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", 100.0, fontProperties2, flowDirection);
					num = formattedText.Baseline / formattedText.Height;
					RadTextMeasurer.fontBaseLineOffsetCache[fontProperties2.FontFamilyName] = num;
				}
			}
			return num;
		}

		internal const int BaseFontSize = 100;

		const string BaselineString = "’'`\"\\@#$%^&*()+=-_/|:].,!~abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

		const string IsLigatureLanguagePattern = "[\\p{IsDevanagari}]";

		internal static readonly Regex LigatureLanguageRegex = new Regex("[\\p{IsDevanagari}]");

		static readonly Dictionary<string, Size> letterSizeCache = new Dictionary<string, Size>();

		static readonly Dictionary<string, Dictionary<string, double>> kerningPairToFontKerningValue = new Dictionary<string, Dictionary<string, double>>();

		static readonly Dictionary<string, double> fontBaseLineOffsetCache = new Dictionary<string, double>();
	}
}
