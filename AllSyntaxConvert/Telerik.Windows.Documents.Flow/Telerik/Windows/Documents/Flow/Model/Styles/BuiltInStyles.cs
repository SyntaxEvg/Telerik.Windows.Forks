using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public static class BuiltInStyles
	{
		static BuiltInStyles()
		{
			BuiltInStyles.InitializeCreationOfStyles();
		}

		public static Style GetStyle(string styleId)
		{
			if (!BuiltInStyles.IsBuiltInStyle(styleId))
			{
				throw new ArgumentException("The style with ID '{0}' is not built-in style.", styleId);
			}
			BuiltInStyleMetaData valueOrNull = BuiltInStyles.styleGenerationMetadatas.GetValueOrNull(styleId);
			return valueOrNull.GetStyle();
		}

		public static bool IsBuiltInStyle(string styleId)
		{
			return BuiltInStyles.styleGenerationMetadatas.ContainsKey(styleId);
		}

		public static IEnumerable<Style> GetAllPrimaryStyles()
		{
			LinkedList<Style> linkedList = new LinkedList<Style>();
			foreach (KeyValuePair<string, BuiltInStyleMetaData> keyValuePair in BuiltInStyles.styleGenerationMetadatas)
			{
				BuiltInStyleMetaData value = keyValuePair.Value;
				if (value.IsPrimary)
				{
					linkedList.AddLast(value.GetStyle());
				}
			}
			return linkedList;
		}

		public static IEnumerable<Style> GetAllStyles(StyleType type)
		{
			LinkedList<Style> linkedList = new LinkedList<Style>();
			foreach (KeyValuePair<string, BuiltInStyleMetaData> keyValuePair in BuiltInStyles.styleGenerationMetadatas)
			{
				BuiltInStyleMetaData value = keyValuePair.Value;
				if (value.StyleType == type)
				{
					linkedList.AddLast(value.GetStyle());
				}
			}
			return linkedList;
		}

		public static IEnumerable<Style> GetAllStyles()
		{
			LinkedList<Style> linkedList = new LinkedList<Style>();
			foreach (KeyValuePair<string, BuiltInStyleMetaData> keyValuePair in BuiltInStyles.styleGenerationMetadatas)
			{
				BuiltInStyleMetaData value = keyValuePair.Value;
				linkedList.AddLast(value.GetStyle());
			}
			return linkedList;
		}

		internal static BuiltInStyleMetaData GetStyleMetadata(string styleId)
		{
			if (!BuiltInStyles.IsBuiltInStyle(styleId))
			{
				throw new ArgumentException("The style with ID '{0}' is not built-in style.", styleId);
			}
			return BuiltInStyles.styleGenerationMetadatas.GetValueOrNull(styleId);
		}

		static void InitializeCreationOfStyles()
		{
			BuiltInStyles.styleGenerationMetadatas.Add("Normal", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 0,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.NormalStyle)
			});
			BuiltInStyles.styleGenerationMetadatas.Add("NormalWeb", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 99,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.NormalWebStyle)
			});
			BuiltInStyles.InitializeHeadingStyles();
			BuiltInStyles.InitializeTableStyles();
			BuiltInStyles.InitializeTocStyles();
			BuiltInStyles.InitializeFootnotesAndEndnotes();
			BuiltInStyles.styleGenerationMetadatas.Add("Caption", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 35,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.CaptionStyle)
			});
			BuiltInStyles.styleGenerationMetadatas.Add("Hyperlink", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Character,
				IsPrimary = true,
				UIPriority = 99,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.HyperlinkStyle)
			});
			BuiltInStyles.styleGenerationMetadatas.Add("TableofFigures", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 99,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.TofStyle)
			});
		}

		static void InitializeTableStyles()
		{
			BuiltInStyles.AddTableNormalTableStyle();
			BuiltInStyles.AddPlainTableStyles();
		}

		static void AddTableNormalTableStyle()
		{
			BuiltInStyles.styleGenerationMetadatas.Add("TableNormal", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Table,
				IsPrimary = false,
				UIPriority = 59,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.TableNormalStyle)
			});
		}

		static void AddPlainTableStyles()
		{
			BuiltInStyles.styleGenerationMetadatas.Add("TableGrid", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Table,
				IsPrimary = false,
				UIPriority = 59,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.TableGridStyle)
			});
		}

		static void InitializeHeadingStyles()
		{
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetHeadingStyleIdByIndex(1), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 9,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.Heading1Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetHeadingStyleIdByIndex(2), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 9,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.Heading2Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetHeadingStyleIdByIndex(3), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 9,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.Heading3Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetHeadingStyleIdByIndex(4), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 9,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.Heading4Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetHeadingStyleIdByIndex(5), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 9,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.Heading5Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetHeadingStyleIdByIndex(6), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 9,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.Heading6Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetHeadingStyleIdByIndex(7), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 9,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.Heading7Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetHeadingStyleIdByIndex(8), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 9,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.Heading8Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetHeadingStyleIdByIndex(9), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = true,
				UIPriority = 9,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.Heading9Style)
			});
		}

		static void InitializeTocStyles()
		{
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetTocStyleIdByIndex(1), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 39,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.GetToc1Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetTocStyleIdByIndex(2), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 39,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.GetToc2Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetTocStyleIdByIndex(3), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 39,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.GetToc3Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetTocStyleIdByIndex(4), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 39,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.GetToc4Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetTocStyleIdByIndex(5), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 39,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.GetToc5Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetTocStyleIdByIndex(6), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 39,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.GetToc6Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetTocStyleIdByIndex(7), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 39,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.GetToc7Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetTocStyleIdByIndex(8), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 39,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.GetToc8Style)
			});
			BuiltInStyles.styleGenerationMetadatas.Add(BuiltInStyleNames.GetTocStyleIdByIndex(9), new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 39,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.GetToc9Style)
			});
		}

		static void InitializeFootnotesAndEndnotes()
		{
			BuiltInStyles.styleGenerationMetadatas.Add("FootnoteReference", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Character,
				IsPrimary = false,
				UIPriority = 99,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.FootnoteReferenceStyle)
			});
			BuiltInStyles.styleGenerationMetadatas.Add("FootnoteText", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 99,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.FootnoteTextStyle)
			});
			BuiltInStyles.styleGenerationMetadatas.Add("EndnoteReference", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Character,
				IsPrimary = false,
				UIPriority = 99,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.EndnoteReferenceStyle)
			});
			BuiltInStyles.styleGenerationMetadatas.Add("EndnoteText", new BuiltInStyleMetaData
			{
				StyleType = StyleType.Paragraph,
				IsPrimary = false,
				UIPriority = 99,
				CreateStyleMethod = new CreateBuiltInStyleCallback(BuiltInStylesGenerator.EndnoteTextStyle)
			});
		}

		static readonly Dictionary<string, BuiltInStyleMetaData> styleGenerationMetadatas = new Dictionary<string, BuiltInStyleMetaData>();
	}
}
