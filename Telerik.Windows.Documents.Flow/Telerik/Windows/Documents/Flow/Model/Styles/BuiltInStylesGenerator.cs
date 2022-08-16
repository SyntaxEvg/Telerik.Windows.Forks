using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	static class BuiltInStylesGenerator
	{
		public static Style NormalStyle(StyleType type)
		{
			return new Style("Normal", type)
			{
				Name = "Normal",
				IsDefault = true,
				IsCustom = false
			};
		}

		public static Style NormalWebStyle(StyleType type)
		{
			Style style = new Style("NormalWeb", type)
			{
				Name = "Normal (Web)",
				IsDefault = false,
				IsCustom = false,
				BasedOnStyleId = "Normal",
				UIPriority = 99
			};
			style.CharacterProperties.FontSize.LocalValue = new double?(16.0);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(new FontFamily("Times New Roman"));
			return style;
		}

		public static Style Heading1Style(StyleType type)
		{
			Style style = new Style(BuiltInStyleNames.GetHeadingStyleIdByIndex(1), type);
			style.Name = BuiltInStyleNames.GetHeadingStyleNameByIndex(1);
			style.IsCustom = false;
			style.BasedOnStyleId = "Normal";
			style.NextStyleId = "Normal";
			style.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(true);
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(32.0);
			style.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(OutlineLevel.Level1);
			style.CharacterProperties.FontSize.LocalValue = new double?(18.66666603088379);
			style.CharacterProperties.FontWeight.LocalValue = new FontWeight?(FontWeights.Bold);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Major);
			style.CharacterProperties.ForegroundColor.LocalValue = new ThemableColor(ThemeColorType.Accent1, null);
			BuiltInStylesGenerator.LinkStyles(style, new Style(BuiltInStyleNames.GetHeadingCharStyleIdByIndex(1), StyleType.Character)
			{
				Name = BuiltInStyleNames.GetHeadingCharStyleNameByIndex(1),
				IsCustom = false,
				IsPrimary = false,
				CharacterProperties = 
				{
					FontSize = 
					{
						LocalValue = new double?(18.66666603088379)
					},
					FontWeight = 
					{
						LocalValue = new FontWeight?(FontWeights.Bold)
					},
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Major)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Accent1, null)
					}
				}
			});
			return style;
		}

		public static Style Heading2Style(StyleType type)
		{
			Style style = new Style(BuiltInStyleNames.GetHeadingStyleIdByIndex(2), type);
			style.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(true);
			style.IsCustom = false;
			style.Name = BuiltInStyleNames.GetHeadingStyleNameByIndex(2);
			style.BasedOnStyleId = "Normal";
			style.NextStyleId = "Normal";
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(13.333333015441895);
			style.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(OutlineLevel.Level2);
			style.CharacterProperties.FontSize.LocalValue = new double?(17.33333396911621);
			style.CharacterProperties.FontWeight.LocalValue = new FontWeight?(FontWeights.Bold);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Major);
			style.CharacterProperties.ForegroundColor.LocalValue = new ThemableColor(ThemeColorType.Accent1, null);
			BuiltInStylesGenerator.LinkStyles(style, new Style(BuiltInStyleNames.GetHeadingCharStyleIdByIndex(2), StyleType.Character)
			{
				IsCustom = false,
				IsPrimary = false,
				Name = BuiltInStyleNames.GetHeadingCharStyleNameByIndex(2),
				CharacterProperties = 
				{
					FontSize = 
					{
						LocalValue = new double?(17.33333396911621)
					},
					FontWeight = 
					{
						LocalValue = new FontWeight?(FontWeights.Bold)
					},
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Major)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Accent1, null)
					}
				}
			});
			return style;
		}

		public static Style Heading3Style(StyleType type)
		{
			Style style = new Style(BuiltInStyleNames.GetHeadingStyleIdByIndex(3), type);
			style.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(true);
			style.IsCustom = false;
			style.Name = BuiltInStyleNames.GetHeadingStyleNameByIndex(3);
			style.BasedOnStyleId = "Normal";
			style.NextStyleId = "Normal";
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(13.333333015441895);
			style.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(OutlineLevel.Level3);
			style.CharacterProperties.FontWeight.LocalValue = new FontWeight?(FontWeights.Bold);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Major);
			style.CharacterProperties.ForegroundColor.LocalValue = new ThemableColor(ThemeColorType.Accent1, null);
			BuiltInStylesGenerator.LinkStyles(style, new Style(BuiltInStyleNames.GetHeadingCharStyleIdByIndex(3), StyleType.Character)
			{
				IsCustom = false,
				IsPrimary = false,
				Name = BuiltInStyleNames.GetHeadingCharStyleNameByIndex(3),
				CharacterProperties = 
				{
					FontWeight = 
					{
						LocalValue = new FontWeight?(FontWeights.Bold)
					},
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Major)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Accent1, null)
					}
				}
			});
			return style;
		}

		public static Style Heading4Style(StyleType type)
		{
			Style style = new Style(BuiltInStyleNames.GetHeadingStyleIdByIndex(4), type);
			style.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(true);
			style.IsCustom = false;
			style.Name = BuiltInStyleNames.GetHeadingStyleNameByIndex(4);
			style.BasedOnStyleId = "Normal";
			style.NextStyleId = "Normal";
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(13.333333015441895);
			style.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(OutlineLevel.Level4);
			style.CharacterProperties.FontStyle.LocalValue = new FontStyle?(FontStyles.Italic);
			style.CharacterProperties.FontWeight.LocalValue = new FontWeight?(FontWeights.Bold);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Major);
			style.CharacterProperties.ForegroundColor.LocalValue = new ThemableColor(ThemeColorType.Accent1, null);
			BuiltInStylesGenerator.LinkStyles(style, new Style(BuiltInStyleNames.GetHeadingCharStyleIdByIndex(4), StyleType.Character)
			{
				IsCustom = false,
				IsPrimary = false,
				Name = BuiltInStyleNames.GetHeadingCharStyleNameByIndex(4),
				CharacterProperties = 
				{
					FontStyle = 
					{
						LocalValue = new FontStyle?(FontStyles.Italic)
					},
					FontWeight = 
					{
						LocalValue = new FontWeight?(FontWeights.Bold)
					},
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Major)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Accent1, null)
					}
				}
			});
			return style;
		}

		public static Style Heading5Style(StyleType type)
		{
			Style style = new Style(BuiltInStyleNames.GetHeadingStyleIdByIndex(5), type);
			style.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(true);
			style.IsCustom = false;
			style.Name = BuiltInStyleNames.GetHeadingStyleNameByIndex(5);
			style.BasedOnStyleId = "Normal";
			style.NextStyleId = "Normal";
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(13.333333015441895);
			style.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(OutlineLevel.Level5);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Major);
			style.CharacterProperties.ForegroundColor.LocalValue = new ThemableColor(ThemeColorType.Accent1, null);
			BuiltInStylesGenerator.LinkStyles(style, new Style(BuiltInStyleNames.GetHeadingCharStyleIdByIndex(5), StyleType.Character)
			{
				IsCustom = false,
				IsPrimary = false,
				Name = BuiltInStyleNames.GetHeadingCharStyleNameByIndex(5),
				CharacterProperties = 
				{
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Major)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Accent1, null)
					}
				}
			});
			return style;
		}

		public static Style Heading6Style(StyleType type)
		{
			Style style = new Style(BuiltInStyleNames.GetHeadingStyleIdByIndex(6), type);
			style.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(true);
			style.IsCustom = false;
			style.Name = BuiltInStyleNames.GetHeadingStyleNameByIndex(6);
			style.BasedOnStyleId = "Normal";
			style.NextStyleId = "Normal";
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(13.333333015441895);
			style.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(OutlineLevel.Level6);
			style.CharacterProperties.FontStyle.LocalValue = new FontStyle?(FontStyles.Italic);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Major);
			style.CharacterProperties.ForegroundColor.LocalValue = new ThemableColor(ThemeColorType.Accent1, null);
			BuiltInStylesGenerator.LinkStyles(style, new Style(BuiltInStyleNames.GetHeadingCharStyleIdByIndex(6), StyleType.Character)
			{
				IsCustom = false,
				IsPrimary = false,
				Name = BuiltInStyleNames.GetHeadingCharStyleNameByIndex(6),
				CharacterProperties = 
				{
					FontStyle = 
					{
						LocalValue = new FontStyle?(FontStyles.Italic)
					},
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Major)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Accent1, null)
					}
				}
			});
			return style;
		}

		public static Style Heading7Style(StyleType type)
		{
			Style style = new Style(BuiltInStyleNames.GetHeadingStyleIdByIndex(7), type);
			style.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(true);
			style.IsCustom = false;
			style.Name = BuiltInStyleNames.GetHeadingStyleNameByIndex(7);
			style.BasedOnStyleId = "Normal";
			style.NextStyleId = "Normal";
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(13.333333015441895);
			style.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(OutlineLevel.Level7);
			style.CharacterProperties.FontStyle.LocalValue = new FontStyle?(FontStyles.Italic);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Major);
			style.CharacterProperties.ForegroundColor.LocalValue = new ThemableColor(ThemeColorType.Text1, null);
			BuiltInStylesGenerator.LinkStyles(style, new Style(BuiltInStyleNames.GetHeadingCharStyleIdByIndex(7), StyleType.Character)
			{
				IsCustom = false,
				IsPrimary = false,
				Name = BuiltInStyleNames.GetHeadingCharStyleNameByIndex(7),
				CharacterProperties = 
				{
					FontStyle = 
					{
						LocalValue = new FontStyle?(FontStyles.Italic)
					},
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Major)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Text1, null)
					}
				}
			});
			return style;
		}

		public static Style Heading8Style(StyleType type)
		{
			Style style = new Style(BuiltInStyleNames.GetHeadingStyleIdByIndex(8), type);
			style.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(true);
			style.IsCustom = false;
			style.Name = BuiltInStyleNames.GetHeadingStyleNameByIndex(8);
			style.BasedOnStyleId = "Normal";
			style.NextStyleId = "Normal";
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(13.333333015441895);
			style.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(OutlineLevel.Level8);
			style.CharacterProperties.FontSize.LocalValue = new double?(13.333333015441895);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Major);
			style.CharacterProperties.ForegroundColor.LocalValue = new ThemableColor(ThemeColorType.Text1, null);
			BuiltInStylesGenerator.LinkStyles(style, new Style(BuiltInStyleNames.GetHeadingCharStyleIdByIndex(8), StyleType.Character)
			{
				IsCustom = false,
				IsPrimary = false,
				Name = BuiltInStyleNames.GetHeadingCharStyleNameByIndex(8),
				CharacterProperties = 
				{
					FontSize = 
					{
						LocalValue = new double?(13.333333015441895)
					},
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Major)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Text1, null)
					}
				}
			});
			return style;
		}

		public static Style Heading9Style(StyleType type)
		{
			Style style = new Style(BuiltInStyleNames.GetHeadingStyleIdByIndex(9), type);
			style.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(true);
			style.IsCustom = false;
			style.Name = BuiltInStyleNames.GetHeadingStyleNameByIndex(9);
			style.BasedOnStyleId = "Normal";
			style.NextStyleId = "Normal";
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(13.333333015441895);
			style.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(OutlineLevel.Level9);
			style.CharacterProperties.FontSize.LocalValue = new double?(13.333333015441895);
			style.CharacterProperties.FontStyle.LocalValue = new FontStyle?(FontStyles.Italic);
			style.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ThemeFontType.Major);
			style.CharacterProperties.ForegroundColor.LocalValue = new ThemableColor(ThemeColorType.Text1, null);
			BuiltInStylesGenerator.LinkStyles(style, new Style(BuiltInStyleNames.GetHeadingCharStyleIdByIndex(9), StyleType.Character)
			{
				IsCustom = false,
				IsPrimary = false,
				Id = BuiltInStyleNames.GetHeadingCharStyleIdByIndex(9),
				Name = BuiltInStyleNames.GetHeadingCharStyleNameByIndex(9),
				CharacterProperties = 
				{
					FontSize = 
					{
						LocalValue = new double?(13.333333015441895)
					},
					FontStyle = 
					{
						LocalValue = new FontStyle?(FontStyles.Italic)
					},
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Major)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Text1, null)
					}
				}
			});
			return style;
		}

		public static Style CaptionStyle(StyleType type)
		{
			return new Style("Caption", type)
			{
				IsCustom = false,
				Name = "Caption",
				BasedOnStyleId = "Normal",
				NextStyleId = "Normal",
				ParagraphProperties = 
				{
					LineSpacing = 
					{
						LocalValue = new double?(1.0)
					},
					LineSpacingType = 
					{
						LocalValue = new HeightType?(HeightType.Auto),
						LocalValue = new HeightType?(HeightType.Auto)
					}
				},
				CharacterProperties = 
				{
					FontSize = 
					{
						LocalValue = new double?(12.0)
					},
					FontWeight = 
					{
						LocalValue = new FontWeight?(FontWeights.Bold)
					},
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(ThemeColorType.Accent1, null)
					},
					FontFamily = 
					{
						LocalValue = new ThemableFontFamily(ThemeFontType.Minor)
					}
				}
			};
		}

		public static Style HyperlinkStyle(StyleType type)
		{
			return new Style("Hyperlink", type)
			{
				IsCustom = false,
				Name = "Hyperlink",
				CharacterProperties = 
				{
					ForegroundColor = 
					{
						LocalValue = new ThemableColor(Colors.Blue)
					},
					UnderlineColor = 
					{
						LocalValue = new ThemableColor(Colors.Blue)
					},
					UnderlinePattern = 
					{
						LocalValue = new UnderlinePattern?(UnderlinePattern.Single)
					}
				}
			};
		}

		public static Style FootnoteReferenceStyle(StyleType type)
		{
			Style style = BuiltInStylesGenerator.CreateNoteStyle("FootnoteReference", "footnote reference", type);
			style.CharacterProperties.BaselineAlignment.LocalValue = new BaselineAlignment?(BaselineAlignment.Superscript);
			return style;
		}

		public static Style FootnoteTextStyle(StyleType type)
		{
			Style style = BuiltInStylesGenerator.CreateNoteStyle("FootnoteText", "footnote text", type);
			style.ParagraphProperties.LineSpacingType.LocalValue = new HeightType?(HeightType.Auto);
			style.ParagraphProperties.LineSpacing.LocalValue = new double?(1.0);
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.CharacterProperties.FontSize.LocalValue = new double?(Unit.PointToDip(10.0));
			Style style2 = BuiltInStylesGenerator.CreateNoteStyle("FootnoteTextChar", "Footnote Text Char", StyleType.Character);
			style2.CharacterProperties.FontSize.LocalValue = new double?(Unit.PointToDip(10.0));
			BuiltInStylesGenerator.LinkStyles(style2, style);
			return style;
		}

		public static Style EndnoteReferenceStyle(StyleType type)
		{
			Style style = BuiltInStylesGenerator.CreateNoteStyle("EndnoteReference", "endnote reference", type);
			style.CharacterProperties.BaselineAlignment.LocalValue = new BaselineAlignment?(BaselineAlignment.Superscript);
			return style;
		}

		public static Style EndnoteTextStyle(StyleType type)
		{
			Style style = BuiltInStylesGenerator.CreateNoteStyle("EndnoteText", "endnote text", type);
			style.ParagraphProperties.LineSpacingType.LocalValue = new HeightType?(HeightType.Auto);
			style.ParagraphProperties.LineSpacing.LocalValue = new double?(1.0);
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(0.0);
			style.CharacterProperties.FontSize.LocalValue = new double?(Unit.PointToDip(10.0));
			Style style2 = BuiltInStylesGenerator.CreateNoteStyle("EndnoteTextChar", "Endnote Text Char", StyleType.Character);
			style2.CharacterProperties.FontSize.LocalValue = new double?(Unit.PointToDip(10.0));
			BuiltInStylesGenerator.LinkStyles(style2, style);
			return style;
		}

		public static Style TableNormalStyle(StyleType type)
		{
			return new Style("TableNormal", type)
			{
				Name = "Table Normal",
				IsDefault = true,
				IsCustom = false,
				TableProperties = 
				{
					TableCellPadding = 
					{
						LocalValue = new Padding(7.2, 0.0, 7.2, 0.0)
					}
				}
			};
		}

		public static Style TableGridStyle(StyleType type)
		{
			return new Style("TableGrid", StyleType.Table)
			{
				Name = "Table Grid",
				BasedOnStyleId = "TableNormal",
				ParagraphProperties = 
				{
					SpacingAfter = 
					{
						LocalValue = new double?(0.0)
					},
					LineSpacing = 
					{
						LocalValue = new double?(1.0)
					},
					LineSpacingType = 
					{
						LocalValue = new HeightType?(HeightType.Auto)
					}
				},
				IsCustom = false,
				TableProperties = 
				{
					Borders = 
					{
						LocalValue = new TableBorders(new Border(1.0, BorderStyle.Single, new ThemableColor(Colors.Black)))
					}
				}
			};
		}

		public static Style TofStyle(StyleType type)
		{
			return new Style("TableofFigures", type)
			{
				BasedOnStyleId = "Normal",
				IsCustom = false,
				IsDefault = false,
				IsPrimary = false,
				NextStyleId = "Normal",
				Name = "table of figures",
				ParagraphProperties = 
				{
					SpacingAfter = 
					{
						LocalValue = new double?(6.666666507720947),
						LocalValue = new double?(0.0)
					}
				}
			};
		}

		public static Style GetToc1Style(StyleType type)
		{
			return BuiltInStylesGenerator.GetTocStyle(1, type);
		}

		public static Style GetToc2Style(StyleType type)
		{
			return BuiltInStylesGenerator.GetTocStyle(2, type);
		}

		public static Style GetToc3Style(StyleType type)
		{
			return BuiltInStylesGenerator.GetTocStyle(3, type);
		}

		public static Style GetToc4Style(StyleType type)
		{
			return BuiltInStylesGenerator.GetTocStyle(4, type);
		}

		public static Style GetToc5Style(StyleType type)
		{
			return BuiltInStylesGenerator.GetTocStyle(5, type);
		}

		public static Style GetToc6Style(StyleType type)
		{
			return BuiltInStylesGenerator.GetTocStyle(6, type);
		}

		public static Style GetToc7Style(StyleType type)
		{
			return BuiltInStylesGenerator.GetTocStyle(7, type);
		}

		public static Style GetToc8Style(StyleType type)
		{
			return BuiltInStylesGenerator.GetTocStyle(8, type);
		}

		public static Style GetToc9Style(StyleType type)
		{
			return BuiltInStylesGenerator.GetTocStyle(9, type);
		}

		public static Style GetTocStyle(int index, StyleType type)
		{
			Style style = new Style(string.Format(BuiltInStyleNames.GetTocStyleIdByIndex(index), new object[0]), type);
			style.BasedOnStyleId = "Normal";
			style.IsCustom = false;
			style.IsDefault = false;
			style.NextStyleId = "Normal";
			style.Name = BuiltInStyleNames.GetTocStyleNameByIndex(index);
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(6.666666507720947);
			if (index > 1)
			{
				style.ParagraphProperties.LeftIndent.LocalValue = new double?(14.666666984558105 * (double)(index - 1));
			}
			return style;
		}

		static Style CreateNoteStyle(string name, string dispalyName, StyleType type)
		{
			return new Style(name, type)
			{
				Name = dispalyName,
				IsCustom = false,
				IsPrimary = false
			};
		}

		static void LinkStyles(Style paragraphStyle, Style charStyle)
		{
			paragraphStyle.LinkedStyleId = charStyle.Id;
			charStyle.LinkedStyleId = paragraphStyle.Id;
		}
	}
}
