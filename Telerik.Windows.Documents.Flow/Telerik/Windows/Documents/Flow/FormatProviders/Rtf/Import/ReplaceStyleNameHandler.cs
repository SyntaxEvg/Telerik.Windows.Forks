using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	static class ReplaceStyleNameHandler
	{
		static Dictionary<string, ReplaceStyleNameHandler.BuiltInStyleInfo> StyleNameToStyleId
		{
			get
			{
				if (ReplaceStyleNameHandler.styleNameToStyleId == null)
				{
					ReplaceStyleNameHandler.InitializeStyleNameToStyleIdDictionary();
				}
				return ReplaceStyleNameHandler.styleNameToStyleId;
			}
		}

		public static void Replace(ReplaceStyleNameContext context)
		{
			ReplaceStyleNameHandler.BuiltInStyleInfo builtInStyleInfo;
			if (ReplaceStyleNameHandler.TryGetStyleIdByStyleName(context.InputStyleName, out builtInStyleInfo))
			{
				context.StyleDefinition.Id = builtInStyleInfo.Id;
				context.StyleDefinition.Name = builtInStyleInfo.Name;
				return;
			}
			context.StyleDefinition.Id = context.InputStyleName.Replace(" ", string.Empty);
			context.StyleDefinition.Name = context.InputStyleName;
		}

		static bool TryGetStyleIdByStyleName(string name, out ReplaceStyleNameHandler.BuiltInStyleInfo defaultStyleInfo)
		{
			return ReplaceStyleNameHandler.StyleNameToStyleId.TryGetValue(name, out defaultStyleInfo);
		}

		static void InitializeStyleNameToStyleIdDictionary()
		{
			ReplaceStyleNameHandler.styleNameToStyleId = new Dictionary<string, ReplaceStyleNameHandler.BuiltInStyleInfo>(StringComparer.OrdinalIgnoreCase);
			ReplaceStyleNameHandler.Add("Table Normal", "TableNormal");
			ReplaceStyleNameHandler.Add("Table Grid", "TableGrid");
			ReplaceStyleNameHandler.Add("Normal", "Normal");
			ReplaceStyleNameHandler.Add("Normal (Web)", "NormalWeb");
			ReplaceStyleNameHandler.Add("Hyperlink", "Hyperlink");
			ReplaceStyleNameHandler.Add("Caption", "Caption");
			ReplaceStyleNameHandler.Add("table of figures", "TableofFigures");
			ReplaceStyleNameHandler.Add("footnote reference", "FootnoteReference");
			ReplaceStyleNameHandler.Add("footnote text", "FootnoteText");
			ReplaceStyleNameHandler.Add("Footnote Text Char", "FootnoteTextChar");
			ReplaceStyleNameHandler.Add("endnote reference", "EndnoteReference");
			ReplaceStyleNameHandler.Add("endnote text", "EndnoteText");
			ReplaceStyleNameHandler.Add("Endnote Text Char", "EndnoteTextChar");
			for (int i = 1; i <= 9; i++)
			{
				ReplaceStyleNameHandler.Add(BuiltInStyleNames.GetHeadingStyleNameByIndex(i), BuiltInStyleNames.GetHeadingStyleIdByIndex(i));
				ReplaceStyleNameHandler.Add(BuiltInStyleNames.GetHeadingCharStyleNameByIndex(i), BuiltInStyleNames.GetHeadingCharStyleIdByIndex(i));
				ReplaceStyleNameHandler.Add(BuiltInStyleNames.GetTocStyleNameByIndex(i), BuiltInStyleNames.GetTocStyleIdByIndex(i));
			}
		}

		static void Add(string name, string id)
		{
			ReplaceStyleNameHandler.styleNameToStyleId.Add(name, new ReplaceStyleNameHandler.BuiltInStyleInfo(id, name));
		}

		static Dictionary<string, ReplaceStyleNameHandler.BuiltInStyleInfo> styleNameToStyleId;

		class BuiltInStyleInfo
		{
			public BuiltInStyleInfo(string id, string name)
			{
				this.id = id;
				this.name = name;
			}

			public string Id
			{
				get
				{
					return this.id;
				}
			}

			public string Name
			{
				get
				{
					return this.name;
				}
			}

			readonly string id;

			readonly string name;
		}
	}
}
