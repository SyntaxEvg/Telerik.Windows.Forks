using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Collections;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;
using Telerik.Windows.Documents.Fixed.Model.Extensions;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils
{
	static class Extensions
	{
		public static void CopyPropertiesFrom(this RadFixedDocumentEditor editor, Telerik.Windows.Documents.Flow.Model.Styles.SectionProperties properties)
		{
			Guard.ThrowExceptionIfNull<RadFixedDocumentEditor>(editor, "editor");
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Flow.Model.Styles.SectionProperties>(properties, "properties");
			editor.SectionProperties.PageMargins = properties.PageMargins.GetActualValue();
			editor.SectionProperties.PageSize = Extensions.GetPageSize(properties);
		}

		public static void CopyPropertiesFrom(this Block block, PdfExportContext context, Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties properties)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			Guard.ThrowExceptionIfNull<PdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties>(properties, "properties");
			block.BackgroundColor = properties.BackgroundColor.ToColor(context);
			block.HorizontalAlignment = Extensions.GetHorizontalAlignment(properties);
			block.FirstLineIndent = Extensions.GetFirstLineIndent(properties);
			block.LeftIndent = Extensions.CalculateActualLeftIndent(properties.LeftIndent.GetActualValue(), block.FirstLineIndent);
			block.RightIndent = Extensions.GetValue(properties.RightIndent.GetActualValue());
			block.LineSpacing = Extensions.GetValue(properties.LineSpacing.GetActualValue());
			block.LineSpacingType = Extensions.GetHeightType(properties.LineSpacingType.GetActualValue());
			block.TabStops = Extensions.GetTabStops(properties.TabStops.GetActualValue(), block);
			block.DefaultTabStopWidth = context.Document.DefaultTabStopWidth;
		}

		public static double CalculateActualLeftIndent(double? wordsProcessingLeftIndent, double firstLineIndent)
		{
			double value = Extensions.GetValue(wordsProcessingLeftIndent);
			if (firstLineIndent < 0.0)
			{
				return value + firstLineIndent;
			}
			return value;
		}

		public static double GetFirstLineIndent(Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties properties)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties>(properties, "properties");
			double? actualValue = properties.HangingIndent.GetActualValue();
			if (actualValue != null && actualValue.Value != 0.0)
			{
				return -actualValue.Value;
			}
			actualValue = new double?(Extensions.GetValue(properties.FirstLineIndent.GetActualValue()));
			return actualValue.Value;
		}

		public static void CopyPropertiesFrom(this Table fixedProperties, PdfExportContext context, TableProperties properties, double availableContentWidth)
		{
			Guard.ThrowExceptionIfNull<Table>(fixedProperties, "fixedProperties");
			Guard.ThrowExceptionIfNull<PdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<TableProperties>(properties, "properties");
			fixedProperties.LayoutType = properties.GetLayoutType();
			TableWidthUnit actualValue = properties.PreferredWidth.GetActualValue();
			fixedProperties.PreferredWidth = actualValue.ToPreferedWidth(availableContentWidth);
		}

		public static void CopyPropertiesFrom(this Block block, PdfExportContext context, Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties properties)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties>(properties, "properties");
			FontFamily actualValue = properties.FontFamily.GetActualValue().GetActualValue(context.Document.Theme);
			FontStyle? actualValue2 = properties.FontStyle.GetActualValue();
			FontWeight? actualValue3 = properties.FontWeight.GetActualValue();
			FontStyle fontStyle = ((actualValue2 != null) ? actualValue2.Value : FontStyles.Normal);
			FontWeight fontWeight = ((actualValue3 != null) ? actualValue3.Value : FontWeights.Normal);
			block.TextProperties.TrySetFont(actualValue, fontStyle, fontWeight);
			block.TextProperties.FontSize = properties.FontSize.GetActualValue().Value;
			block.TextProperties.BaselineAlignment = Extensions.GetBaselineAlignment(properties);
			block.GraphicProperties.FillColor = properties.ForegroundColor.ToColor(context);
			block.TextProperties.HighlightColor = properties.HighlightColor.ToColor(context);
			block.TextProperties.UnderlineColor = properties.UnderlineColor.ToColor(context, properties.ForegroundColor.GetActualValue());
			block.TextProperties.UnderlinePattern = Extensions.GetUnderlinePattern(properties);
		}

		public static void CopyPropertiesFrom(this Telerik.Windows.Documents.Fixed.Model.Editing.Flow.CharacterProperties fixedProperties, PdfExportContext context, Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties properties)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Fixed.Model.Editing.Flow.CharacterProperties>(fixedProperties, "fixedProperties");
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties>(properties, "properties");
			FontFamily actualValue = properties.FontFamily.GetActualValue().GetActualValue(context.Document.Theme);
			FontStyle? actualValue2 = properties.FontStyle.GetActualValue();
			FontWeight? actualValue3 = properties.FontWeight.GetActualValue();
			FontStyle fontStyle = ((actualValue2 != null) ? actualValue2.Value : FontStyles.Normal);
			FontWeight fontWeight = ((actualValue3 != null) ? actualValue3.Value : FontWeights.Normal);
			fixedProperties.TrySetFont(actualValue, fontStyle, fontWeight);
			fixedProperties.FontSize = properties.FontSize.GetActualValue().Value;
			fixedProperties.BaselineAlignment = Extensions.GetBaselineAlignment(properties);
			fixedProperties.ForegroundColor = properties.ForegroundColor.ToColor(context);
			fixedProperties.HighlightColor = properties.HighlightColor.ToColor(context);
			fixedProperties.UnderlineColor = properties.UnderlineColor.ToColor(context);
			fixedProperties.UnderlinePattern = Extensions.GetUnderlinePattern(properties);
		}

		public static void CopyPropertiesFrom(this TextWatermarkSettings fixedSettings, TextWatermarkSettings textWatermarkSettings)
		{
			Guard.ThrowExceptionIfNull<TextWatermarkSettings>(fixedSettings, "fixedSettings");
			Guard.ThrowExceptionIfNull<TextWatermarkSettings>(textWatermarkSettings, "textWatermarkSettings");
			RgbColor rgbColor = (RgbColor)textWatermarkSettings.ForegroundColor.ToColor();
			rgbColor.A = (byte)((double)rgbColor.A * textWatermarkSettings.Opacity);
			fixedSettings.ForegroundColor = rgbColor;
			fixedSettings.TrySetFont(textWatermarkSettings.FontFamily);
			fixedSettings.Angle = textWatermarkSettings.Angle;
			fixedSettings.Height = textWatermarkSettings.Height;
			fixedSettings.Width = textWatermarkSettings.Width;
			fixedSettings.Text = textWatermarkSettings.Text;
		}

		public static void CopyPropertiesFrom(this ImageWatermarkSettings fixedSettings, ImageWatermarkSettings imageWatermarkSettings)
		{
			Guard.ThrowExceptionIfNull<ImageWatermarkSettings>(fixedSettings, "fixedSettings");
			Guard.ThrowExceptionIfNull<ImageWatermarkSettings>(imageWatermarkSettings, "imageWatermarkSettings");
			using (MemoryStream memoryStream = new MemoryStream(imageWatermarkSettings.ImageSource.Data))
			{
				fixedSettings.ImageSource = new Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource(memoryStream, ImageQuality.High);
			}
			fixedSettings.Width = imageWatermarkSettings.Width;
			fixedSettings.Height = imageWatermarkSettings.Height;
			fixedSettings.Angle = imageWatermarkSettings.Angle;
		}

		public static double GetAvailableContentWidth(this RadFixedDocumentEditor editor)
		{
			Padding pageMargins = editor.SectionProperties.PageMargins;
			double val = editor.SectionProperties.PageSize.Width - pageMargins.Left - pageMargins.Right;
			return Math.Max(0.0, val);
		}

		public static double? ToPreferedWidth(this TableWidthUnit tableWidthUnit, double hundredPercentWidth)
		{
			double? result = null;
			switch (tableWidthUnit.Type)
			{
			case TableWidthUnitType.Fixed:
				result = new double?(tableWidthUnit.Value);
				break;
			case TableWidthUnitType.Percent:
				result = new double?(tableWidthUnit.Value * hundredPercentWidth / 100.0);
				break;
			}
			if (result != null && (double.IsNaN(result.Value) || double.IsInfinity(result.Value)))
			{
				result = null;
			}
			return result;
		}

		public static Telerik.Windows.Documents.Fixed.Model.Editing.BorderStyle ToBorderStyle(this Telerik.Windows.Documents.Flow.Model.Styles.BorderStyle borderStyle)
		{
			switch (borderStyle)
			{
			case Telerik.Windows.Documents.Flow.Model.Styles.BorderStyle.None:
				return Telerik.Windows.Documents.Fixed.Model.Editing.BorderStyle.None;
			case Telerik.Windows.Documents.Flow.Model.Styles.BorderStyle.Single:
				return Telerik.Windows.Documents.Fixed.Model.Editing.BorderStyle.Single;
			default:
				return Telerik.Windows.Documents.Fixed.Model.Editing.BorderStyle.None;
			}
		}

		public static ColorBase ToColor(this ThemableColor color, PdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<ThemableColor>(color, "color");
			return color.GetActualValue(context.Document.Theme).ToColor();
		}

		public static ColorBase ToColor(this IStyleProperty<Color?> property, PdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IStyleProperty<Color?>>(property, "property");
			Color? actualValue = property.GetActualValue();
			if (actualValue != null)
			{
				return actualValue.Value.ToColor();
			}
			return null;
		}

		public static ColorBase ToColor(this IStyleProperty<ThemableColor> property, PdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<IStyleProperty<ThemableColor>>(property, "property");
			Guard.ThrowExceptionIfNull<PdfExportContext>(context, "context");
			return property.GetActualValue().ToColor(context);
		}

		public static ColorBase ToColor(this IStyleProperty<ThemableColor> property, PdfExportContext context, ThemableColor alternative)
		{
			Guard.ThrowExceptionIfNull<IStyleProperty<ThemableColor>>(property, "property");
			Guard.ThrowExceptionIfNull<PdfExportContext>(context, "context");
			ThemableColor themableColor = property.GetActualValue();
			if (themableColor.IsAutomatic && alternative != null)
			{
				themableColor = alternative;
			}
			return themableColor.ToColor(context);
		}

		public static Thickness ToThickness(this Padding padding)
		{
			return new Thickness(padding.Left, padding.Top, padding.Right, padding.Bottom);
		}

		static double GetValue(double? value)
		{
			if (value != null)
			{
				return value.Value;
			}
			return 0.0;
		}

		static Telerik.Windows.Documents.Fixed.Model.Editing.Flow.UnderlinePattern GetUnderlinePattern(Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties properties)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties>(properties, "properties");
			Telerik.Windows.Documents.Flow.Model.Styles.UnderlinePattern? actualValue = properties.UnderlinePattern.GetActualValue();
			if (actualValue != null)
			{
				Telerik.Windows.Documents.Flow.Model.Styles.UnderlinePattern value = actualValue.Value;
				if (value == Telerik.Windows.Documents.Flow.Model.Styles.UnderlinePattern.Single)
				{
					return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.UnderlinePattern.Single;
				}
			}
			return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.UnderlinePattern.None;
		}

		static Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment GetBaselineAlignment(Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties properties)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties>(properties, "properties");
			Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment? actualValue = properties.BaselineAlignment.GetActualValue();
			if (actualValue != null)
			{
				switch (actualValue.Value)
				{
				case Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Baseline:
					return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Baseline;
				case Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Subscript:
					return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Subscript;
				case Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Superscript:
					return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Superscript;
				}
			}
			return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Baseline;
		}

		static Size GetPageSize(Telerik.Windows.Documents.Flow.Model.Styles.SectionProperties properties)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Flow.Model.Styles.SectionProperties>(properties, "properties");
			return properties.PageSize.GetActualValue().Value;
		}

		static Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment GetHorizontalAlignment(Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties properties)
		{
			Guard.ThrowExceptionIfNull<Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties>(properties, "properties");
			Alignment? actualValue = properties.TextAlignment.GetActualValue();
			if (actualValue != null)
			{
				switch (actualValue.Value)
				{
				case Alignment.Left:
					return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left;
				case Alignment.Center:
					return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
				case Alignment.Right:
					return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Right;
				}
			}
			return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left;
		}

		static Telerik.Windows.Documents.Fixed.Model.Editing.HeightType GetHeightType(Telerik.Windows.Documents.Flow.Model.Styles.HeightType? value)
		{
			if (value != null)
			{
				switch (value.Value)
				{
				case Telerik.Windows.Documents.Flow.Model.Styles.HeightType.Auto:
					return Telerik.Windows.Documents.Fixed.Model.Editing.HeightType.Auto;
				case Telerik.Windows.Documents.Flow.Model.Styles.HeightType.AtLeast:
					return Telerik.Windows.Documents.Fixed.Model.Editing.HeightType.AtLeast;
				case Telerik.Windows.Documents.Flow.Model.Styles.HeightType.Exact:
					return Telerik.Windows.Documents.Fixed.Model.Editing.HeightType.Exact;
				}
			}
			return Telerik.Windows.Documents.Fixed.Model.Editing.HeightType.Auto;
		}

		static Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TableLayoutType GetLayoutType(this TableProperties properties)
		{
			Guard.ThrowExceptionIfNull<TableProperties>(properties, "properties");
			Telerik.Windows.Documents.Flow.Model.Styles.TableLayoutType? actualValue = properties.LayoutType.GetActualValue();
			if (actualValue != null)
			{
				switch (actualValue.Value)
				{
				case Telerik.Windows.Documents.Flow.Model.Styles.TableLayoutType.FixedWidth:
					return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TableLayoutType.FixedWidth;
				case Telerik.Windows.Documents.Flow.Model.Styles.TableLayoutType.AutoFit:
					return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TableLayoutType.AutoFit;
				}
			}
			return Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TableLayoutType.FixedWidth;
		}

		static TabStopCollection GetTabStops(TabStopCollection tabStopCollection, Block parentBlock)
		{
			TabStopCollection tabStopCollection2 = new TabStopCollection();
			foreach (TabStop tabStop in tabStopCollection)
			{
				tabStopCollection2.Add(Extensions.GetTabStop(tabStop, parentBlock));
			}
			return tabStopCollection2;
		}

		static TabStop GetTabStop(TabStop tabStop, Block parentBlock)
		{
			TabStop tabStop2 = new TabStop(tabStop.Position);
			switch (tabStop.Type)
			{
			case TabStopType.Left:
				tabStop2.Type = TabStopType.Left;
				break;
			case TabStopType.Center:
				tabStop2.Type = TabStopType.Center;
				break;
			case TabStopType.Right:
				tabStop2.Type = TabStopType.Right;
				tabStop2.Position -= parentBlock.LeftIndent;
				break;
			case TabStopType.Decimal:
				tabStop2.Type = TabStopType.Decimal;
				break;
			case TabStopType.Bar:
				tabStop2.Type = TabStopType.Bar;
				break;
			case TabStopType.Clear:
				tabStop2.Type = TabStopType.Clear;
				break;
			default:
				throw new ArgumentException(string.Format("Unknown tab stop type: {0}", tabStop.Type));
			}
			switch (tabStop.Leader)
			{
			case TabStopLeader.None:
				tabStop2.Leader = TabStopLeader.None;
				break;
			case TabStopLeader.Dot:
				tabStop2.Leader = TabStopLeader.Dot;
				break;
			case TabStopLeader.Hyphen:
				tabStop2.Leader = TabStopLeader.Hyphen;
				break;
			case TabStopLeader.Underscore:
				tabStop2.Leader = TabStopLeader.Underscore;
				break;
			case TabStopLeader.MiddleDot:
				tabStop2.Leader = TabStopLeader.MiddleDot;
				break;
			default:
				throw new ArgumentException(string.Format("Unknown tab stop leader: {0}", tabStop.Leader));
			}
			return tabStop2;
		}
	}
}
