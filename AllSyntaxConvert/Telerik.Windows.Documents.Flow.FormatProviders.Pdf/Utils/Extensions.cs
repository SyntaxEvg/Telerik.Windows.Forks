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
		public static void CopyPropertiesFrom(this global::Telerik.Windows.Documents.Fixed.Model.Editing.RadFixedDocumentEditor editor, global::Telerik.Windows.Documents.Flow.Model.Styles.SectionProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Editing.RadFixedDocumentEditor>(editor, "editor");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.SectionProperties>(properties, "properties");
			editor.SectionProperties.PageMargins = properties.PageMargins.GetActualValue();
			editor.SectionProperties.PageSize = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetPageSize(properties);
		}

		public static void CopyPropertiesFrom(this global::Telerik.Windows.Documents.Fixed.Model.Editing.Block block, global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext context, global::Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Editing.Block>(block, "block");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext>(context, "context");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties>(properties, "properties");
			block.BackgroundColor = properties.BackgroundColor.ToColor(context);
			block.HorizontalAlignment = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetHorizontalAlignment(properties);
			block.FirstLineIndent = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetFirstLineIndent(properties);
			block.LeftIndent = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.CalculateActualLeftIndent(properties.LeftIndent.GetActualValue(), block.FirstLineIndent);
			block.RightIndent = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetValue(properties.RightIndent.GetActualValue());
			block.LineSpacing = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetValue(properties.LineSpacing.GetActualValue());
			block.LineSpacingType = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetHeightType(properties.LineSpacingType.GetActualValue());
			block.TabStops = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetTabStops(properties.TabStops.GetActualValue(), block);
			block.DefaultTabStopWidth = context.Document.DefaultTabStopWidth;
		}

		public static double CalculateActualLeftIndent(double? wordsProcessingLeftIndent, double firstLineIndent)
		{
			double value = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetValue(wordsProcessingLeftIndent);
			if (firstLineIndent < 0.0)
			{
				return value + firstLineIndent;
			}
			return value;
		}

		public static double GetFirstLineIndent(global::Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties>(properties, "properties");
			double? actualValue = properties.HangingIndent.GetActualValue();
			if (actualValue != null && actualValue.Value != 0.0)
			{
				return -actualValue.Value;
			}
			actualValue = new double?(global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetValue(properties.FirstLineIndent.GetActualValue()));
			return actualValue.Value;
		}

		public static void CopyPropertiesFrom(this global::Telerik.Windows.Documents.Fixed.Model.Editing.Tables.Table fixedProperties, global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext context, global::Telerik.Windows.Documents.Flow.Model.Styles.TableProperties properties, double availableContentWidth)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Editing.Tables.Table>(fixedProperties, "fixedProperties");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext>(context, "context");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.TableProperties>(properties, "properties");
			fixedProperties.LayoutType = properties.GetLayoutType();
			global::Telerik.Windows.Documents.Flow.Model.Styles.TableWidthUnit actualValue = properties.PreferredWidth.GetActualValue();
			fixedProperties.PreferredWidth = actualValue.ToPreferedWidth(availableContentWidth);
		}

		public static void CopyPropertiesFrom(this global::Telerik.Windows.Documents.Fixed.Model.Editing.Block block, global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext context, global::Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Editing.Block>(block, "block");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties>(properties, "properties");
			global::System.Windows.Media.FontFamily actualValue = properties.FontFamily.GetActualValue().GetActualValue(context.Document.Theme);
			global::System.Windows.FontStyle? actualValue2 = properties.FontStyle.GetActualValue();
			global::System.Windows.FontWeight? actualValue3 = properties.FontWeight.GetActualValue();
			global::System.Windows.FontStyle fontStyle = ((actualValue2 != null) ? actualValue2.Value : global::System.Windows.FontStyles.Normal);
			global::System.Windows.FontWeight fontWeight = ((actualValue3 != null) ? actualValue3.Value : global::System.Windows.FontWeights.Normal);
			block.TextProperties.TrySetFont(actualValue, fontStyle, fontWeight);
			block.TextProperties.FontSize = properties.FontSize.GetActualValue().Value;
			block.TextProperties.BaselineAlignment = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetBaselineAlignment(properties);
			block.GraphicProperties.FillColor = properties.ForegroundColor.ToColor(context);
			block.TextProperties.HighlightColor = properties.HighlightColor.ToColor(context);
			block.TextProperties.UnderlineColor = properties.UnderlineColor.ToColor(context, properties.ForegroundColor.GetActualValue());
			block.TextProperties.UnderlinePattern = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetUnderlinePattern(properties);
		}

		public static void CopyPropertiesFrom(this global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.CharacterProperties fixedProperties, global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext context, global::Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.CharacterProperties>(fixedProperties, "fixedProperties");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties>(properties, "properties");
			global::System.Windows.Media.FontFamily actualValue = properties.FontFamily.GetActualValue().GetActualValue(context.Document.Theme);
			global::System.Windows.FontStyle? actualValue2 = properties.FontStyle.GetActualValue();
			global::System.Windows.FontWeight? actualValue3 = properties.FontWeight.GetActualValue();
			global::System.Windows.FontStyle fontStyle = ((actualValue2 != null) ? actualValue2.Value : global::System.Windows.FontStyles.Normal);
			global::System.Windows.FontWeight fontWeight = ((actualValue3 != null) ? actualValue3.Value : global::System.Windows.FontWeights.Normal);
			fixedProperties.TrySetFont(actualValue, fontStyle, fontWeight);
			fixedProperties.FontSize = properties.FontSize.GetActualValue().Value;
			fixedProperties.BaselineAlignment = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetBaselineAlignment(properties);
			fixedProperties.ForegroundColor = properties.ForegroundColor.ToColor(context);
			fixedProperties.HighlightColor = properties.HighlightColor.ToColor(context);
			fixedProperties.UnderlineColor = properties.UnderlineColor.ToColor(context);
			fixedProperties.UnderlinePattern = global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetUnderlinePattern(properties);
		}

		public static void CopyPropertiesFrom(this global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TextWatermarkSettings fixedSettings, global::Telerik.Windows.Documents.Flow.Model.Watermarks.TextWatermarkSettings textWatermarkSettings)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TextWatermarkSettings>(fixedSettings, "fixedSettings");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Watermarks.TextWatermarkSettings>(textWatermarkSettings, "textWatermarkSettings");
			global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor rgbColor = (global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.RgbColor)textWatermarkSettings.ForegroundColor.ToColor();
			rgbColor.A = (byte)((double)rgbColor.A * textWatermarkSettings.Opacity);
			fixedSettings.ForegroundColor = rgbColor;
			fixedSettings.TrySetFont(textWatermarkSettings.FontFamily);
			fixedSettings.Angle = textWatermarkSettings.Angle;
			fixedSettings.Height = textWatermarkSettings.Height;
			fixedSettings.Width = textWatermarkSettings.Width;
			fixedSettings.Text = textWatermarkSettings.Text;
		}

		public static void CopyPropertiesFrom(this global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.ImageWatermarkSettings fixedSettings, global::Telerik.Windows.Documents.Flow.Model.Watermarks.ImageWatermarkSettings imageWatermarkSettings)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.ImageWatermarkSettings>(fixedSettings, "fixedSettings");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Watermarks.ImageWatermarkSettings>(imageWatermarkSettings, "imageWatermarkSettings");
			using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(imageWatermarkSettings.ImageSource.Data))
			{
				fixedSettings.ImageSource = new global::Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource(memoryStream, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ImageQuality.High);
			}
			fixedSettings.Width = imageWatermarkSettings.Width;
			fixedSettings.Height = imageWatermarkSettings.Height;
			fixedSettings.Angle = imageWatermarkSettings.Angle;
		}

		public static double GetAvailableContentWidth(this global::Telerik.Windows.Documents.Fixed.Model.Editing.RadFixedDocumentEditor editor)
		{
			global::Telerik.Windows.Documents.Primitives.Padding pageMargins = editor.SectionProperties.PageMargins;
			double val = editor.SectionProperties.PageSize.Width - pageMargins.Left - pageMargins.Right;
			return global::System.Math.Max(0.0, val);
		}

		public static double? ToPreferedWidth(this global::Telerik.Windows.Documents.Flow.Model.Styles.TableWidthUnit tableWidthUnit, double hundredPercentWidth)
		{
			double? result = null;
			switch (tableWidthUnit.Type)
			{
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TableWidthUnitType.Fixed:
					result = new double?(tableWidthUnit.Value);
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TableWidthUnitType.Percent:
					result = new double?(tableWidthUnit.Value * hundredPercentWidth / 100.0);
					break;
			}
			if (result != null && (double.IsNaN(result.Value) || double.IsInfinity(result.Value)))
			{
				result = null;
			}
			return result;
		}

		public static global::Telerik.Windows.Documents.Fixed.Model.Editing.BorderStyle ToBorderStyle(this global::Telerik.Windows.Documents.Flow.Model.Styles.BorderStyle borderStyle)
		{
			switch (borderStyle)
			{
				case global::Telerik.Windows.Documents.Flow.Model.Styles.BorderStyle.None:
					return global::Telerik.Windows.Documents.Fixed.Model.Editing.BorderStyle.None;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.BorderStyle.Single:
					return global::Telerik.Windows.Documents.Fixed.Model.Editing.BorderStyle.Single;
				default:
					return global::Telerik.Windows.Documents.Fixed.Model.Editing.BorderStyle.None;
			}
		}

		public static global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.ColorBase ToColor(this global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor color, global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext context)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor>(color, "color");
			return color.GetActualValue(context.Document.Theme).ToColor();
		}

		public static global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.ColorBase ToColor(this global::Telerik.Windows.Documents.Flow.Model.Styles.Core.IStyleProperty<global::System.Windows.Media.Color?> property, global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext context)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext>(context, "context");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.Core.IStyleProperty<global::System.Windows.Media.Color?>>(property, "property");
			global::System.Windows.Media.Color? actualValue = property.GetActualValue();
			if (actualValue != null)
			{
				return actualValue.Value.ToColor();
			}
			return null;
		}

		public static global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.ColorBase ToColor(this global::Telerik.Windows.Documents.Flow.Model.Styles.Core.IStyleProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor> property, global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext context)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.Core.IStyleProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor>>(property, "property");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext>(context, "context");
			return property.GetActualValue().ToColor(context);
		}

		public static global::Telerik.Windows.Documents.Fixed.Model.ColorSpaces.ColorBase ToColor(this global::Telerik.Windows.Documents.Flow.Model.Styles.Core.IStyleProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor> property, global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext context, global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor alternative)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.Core.IStyleProperty<global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor>>(property, "property");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext>(context, "context");
			global::Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor themableColor = property.GetActualValue();
			if (themableColor.IsAutomatic && alternative != null)
			{
				themableColor = alternative;
			}
			return themableColor.ToColor(context);
		}

		public static global::System.Windows.Thickness ToThickness(this global::Telerik.Windows.Documents.Primitives.Padding padding)
		{
			return new global::System.Windows.Thickness(padding.Left, padding.Top, padding.Right, padding.Bottom);
		}

		private static double GetValue(double? value)
		{
			if (value != null)
			{
				return value.Value;
			}
			return 0.0;
		}

		private static global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.UnderlinePattern GetUnderlinePattern(global::Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties>(properties, "properties");
			global::Telerik.Windows.Documents.Flow.Model.Styles.UnderlinePattern? actualValue = properties.UnderlinePattern.GetActualValue();
			if (actualValue != null)
			{
				global::Telerik.Windows.Documents.Flow.Model.Styles.UnderlinePattern value = actualValue.Value;
				if (value == global::Telerik.Windows.Documents.Flow.Model.Styles.UnderlinePattern.Single)
				{
					return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.UnderlinePattern.Single;
				}
			}
			return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.UnderlinePattern.None;
		}

		private static global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment GetBaselineAlignment(global::Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.CharacterProperties>(properties, "properties");
			global::Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment? actualValue = properties.BaselineAlignment.GetActualValue();
			if (actualValue != null)
			{
				switch (actualValue.Value)
				{
					case global::Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Baseline:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Baseline;
					case global::Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Subscript:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Subscript;
					case global::Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment.Superscript:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Superscript;
				}
			}
			return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Baseline;
		}

		private static global::System.Windows.Size GetPageSize(global::Telerik.Windows.Documents.Flow.Model.Styles.SectionProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.SectionProperties>(properties, "properties");
			return properties.PageSize.GetActualValue().Value;
		}

		private static global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment GetHorizontalAlignment(global::Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.ParagraphProperties>(properties, "properties");
			global::Telerik.Windows.Documents.Flow.Model.Styles.Alignment? actualValue = properties.TextAlignment.GetActualValue();
			if (actualValue != null)
			{
				switch (actualValue.Value)
				{
					case global::Telerik.Windows.Documents.Flow.Model.Styles.Alignment.Left:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left;
					case global::Telerik.Windows.Documents.Flow.Model.Styles.Alignment.Center:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
					case global::Telerik.Windows.Documents.Flow.Model.Styles.Alignment.Right:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Right;
				}
			}
			return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Left;
		}

		private static global::Telerik.Windows.Documents.Fixed.Model.Editing.HeightType GetHeightType(global::Telerik.Windows.Documents.Flow.Model.Styles.HeightType? value)
		{
			if (value != null)
			{
				switch (value.Value)
				{
					case global::Telerik.Windows.Documents.Flow.Model.Styles.HeightType.Auto:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.HeightType.Auto;
					case global::Telerik.Windows.Documents.Flow.Model.Styles.HeightType.AtLeast:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.HeightType.AtLeast;
					case global::Telerik.Windows.Documents.Flow.Model.Styles.HeightType.Exact:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.HeightType.Exact;
				}
			}
			return global::Telerik.Windows.Documents.Fixed.Model.Editing.HeightType.Auto;
		}

		private static global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TableLayoutType GetLayoutType(this global::Telerik.Windows.Documents.Flow.Model.Styles.TableProperties properties)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Flow.Model.Styles.TableProperties>(properties, "properties");
			global::Telerik.Windows.Documents.Flow.Model.Styles.TableLayoutType? actualValue = properties.LayoutType.GetActualValue();
			if (actualValue != null)
			{
				switch (actualValue.Value)
				{
					case global::Telerik.Windows.Documents.Flow.Model.Styles.TableLayoutType.FixedWidth:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TableLayoutType.FixedWidth;
					case global::Telerik.Windows.Documents.Flow.Model.Styles.TableLayoutType.AutoFit:
						return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TableLayoutType.AutoFit;
				}
			}
			return global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TableLayoutType.FixedWidth;
		}

		private static global::Telerik.Windows.Documents.Fixed.Model.Editing.Collections.TabStopCollection GetTabStops(global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopCollection tabStopCollection, global::Telerik.Windows.Documents.Fixed.Model.Editing.Block parentBlock)
		{
			global::Telerik.Windows.Documents.Fixed.Model.Editing.Collections.TabStopCollection tabStopCollection2 = new global::Telerik.Windows.Documents.Fixed.Model.Editing.Collections.TabStopCollection();
			foreach (global::Telerik.Windows.Documents.Flow.Model.Styles.TabStop tabStop in tabStopCollection)
			{
				tabStopCollection2.Add(global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils.Extensions.GetTabStop(tabStop, parentBlock));
			}
			return tabStopCollection2;
		}

		private static global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStop GetTabStop(global::Telerik.Windows.Documents.Flow.Model.Styles.TabStop tabStop, global::Telerik.Windows.Documents.Fixed.Model.Editing.Block parentBlock)
		{
			global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStop tabStop2 = new global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStop(tabStop.Position);
			switch (tabStop.Type)
			{
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopType.Left:
					tabStop2.Type = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopType.Left;
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopType.Center:
					tabStop2.Type = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopType.Center;
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopType.Right:
					tabStop2.Type = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopType.Right;
					tabStop2.Position -= parentBlock.LeftIndent;
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopType.Decimal:
					tabStop2.Type = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopType.Decimal;
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopType.Bar:
					tabStop2.Type = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopType.Bar;
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopType.Clear:
					tabStop2.Type = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopType.Clear;
					break;
				default:
					throw new global::System.ArgumentException(string.Format("Unknown tab stop type: {0}", tabStop.Type));
			}
			switch (tabStop.Leader)
			{
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopLeader.None:
					tabStop2.Leader = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopLeader.None;
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopLeader.Dot:
					tabStop2.Leader = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopLeader.Dot;
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopLeader.Hyphen:
					tabStop2.Leader = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopLeader.Hyphen;
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopLeader.Underscore:
					tabStop2.Leader = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopLeader.Underscore;
					break;
				case global::Telerik.Windows.Documents.Flow.Model.Styles.TabStopLeader.MiddleDot:
					tabStop2.Leader = global::Telerik.Windows.Documents.Fixed.Model.Editing.Flow.TabStopLeader.MiddleDot;
					break;
				default:
					throw new global::System.ArgumentException(string.Format("Unknown tab stop leader: {0}", tabStop.Leader));
			}
			return tabStop2;
		}
	}
}
