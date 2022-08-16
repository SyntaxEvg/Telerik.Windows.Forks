using System;
using System.Collections.Generic;
using CsQuery.StringScanner;

namespace CsQuery.HtmlParser
{
	class HtmlData
	{
		static HtmlData()
		{
			HtmlData.defaultPadding = "";
			for (int i = 1; i < 1; i++)
			{
				HtmlData.defaultPadding += "0";
			}
			HtmlData.MustBeQuotedAll = new char[CharacterData.charsHtmlSpaceArray.Length + HtmlData.MustBeQuoted.Length];
			HtmlData.MustBeQuoted.CopyTo(HtmlData.MustBeQuotedAll, 0);
			CharacterData.charsHtmlSpaceArray.CopyTo(HtmlData.MustBeQuotedAll, HtmlData.MustBeQuoted.Length);
			string[] tokens = new string[] { "SCRIPT", "TEXTAREA", "STYLE" };
			string[] tokens2 = new string[]
			{
				"BASE", "BASEFONT", "FRAME", "LINK", "META", "AREA", "COL", "HR", "PARAM", "IMG",
				"INPUT", "BR", "!DOCTYPE", "!--", "COMMAND", "EMBED", "KEYGEN", "SOURCE", "TRACK", "WBR"
			};
			string[] tokens3 = new string[]
			{
				"BODY", "BR", "ADDRESS", "BLOCKQUOTE", "CENTER", "DIV", "DIR", "FORM", "FRAMESET", "H1",
				"H2", "H3", "H4", "H5", "H6", "HR", "ISINDEX", "LI", "NOFRAMES", "NOSCRIPT",
				"OL", "P", "PRE", "TABLE", "TR", "TEXTAREA", "UL", "ARTICLE", "ASIDE", "BUTTON",
				"CANVAS", "CAPTION", "COL", "COLGROUP", "DD", "DL", "DT", "EMBED", "FIELDSET", "FIGCAPTION",
				"FIGURE", "FOOTER", "HEADER", "HGROUP", "PROGRESS", "SECTION", "TBODY", "THEAD", "TFOOT", "VIDEO",
				"APPLET", "LAYER", "LEGEND"
			};
			string[] tokens4 = new string[]
			{
				"ADDRESS", "ARTICLE", "ASIDE", "BLOCKQUOTE", "DIR", "DIV", "DL", "FIELDSET", "FOOTER", "FORM",
				"H1", "H2", "H3", "H4", "H5", "H6", "HEADER", "HGROUP", "HR", "MENU",
				"NAV", "OL", "P", "PRE", "SECTION", "TABLE", "UL"
			};
			string[] tokens5 = new string[]
			{
				"AUTOBUFFER", "AUTOFOCUS", "AUTOPLAY", "ASYNC", "CHECKED", "COMPACT", "CONTROLS", "DECLARE", "DEFAULTMUTED", "DEFAULTSELECTED",
				"DEFER", "DISABLED", "DRAGGABLE", "FORMNOVALIDATE", "HIDDEN", "INDETERMINATE", "ISMAP", "ITEMSCOPE", "LOOP", "MULTIPLE",
				"MUTED", "NOHREF", "NORESIZE", "NOSHADE", "NOWRAP", "NOVALIDATE", "OPEN", "PUBDATE", "READONLY", "REQUIRED",
				"REVERSED", "SCOPED", "SEAMLESS", "SELECTED", "SPELLCHECK", "TRUESPEED", " VISIBLE"
			};
			string[] tokens6 = new string[]
			{
				"P", "LI", "TR", "TD", "TH", "THEAD", "TBODY", "TFOOT", "OPTION", "HEAD",
				"DT", "DD", "COLGROUP", "OPTGROUP", "TABLE", "HTML"
			};
			string[] tokens7 = new string[] { "BASE", "COMMAND", "LINK", "META", "NOSCRIPT", "SCRIPT", "STYLE", "TITLE" };
			string[] tokens8 = new string[] { "type", "target" };
			string[] tokens9 = new string[] { "input", "select", "option", "param", "button", "progress", "output", "meter", "script" };
			string[] tokens10 = new string[] { "input", "select", "button", "textarea" };
			string[] array = new string[]
			{
				"unused", "class", "value", "id", "selected", "readonly", "checked", "input", "select", "option",
				"p", "tr", "td", "th", "head", "body", "dt", "colgroup", "dd", "li",
				"dl", "table", "optgroup", "ul", "ol", "tbody", "tfoot", "thead", "rt", "rp",
				"script", "textarea", "style", "col", "html", "button", "multiple", "a", "span", "form",
				"required", "autofocus", "type", "progress", "label", "disabled", "meter", "img", "link"
			};
			HtmlData.TokenIDs = new Dictionary<string, ushort>();
			foreach (string name in array)
			{
				HtmlData.Tokenize(name);
			}
			if (HtmlData.nextID != 51)
			{
				throw new InvalidOperationException("Something went wrong with the constant map in DomData");
			}
			HtmlData.PopulateTokenHashset(tokens);
			HtmlData.PopulateTokenHashset(tokens2);
			HtmlData.PopulateTokenHashset(tokens3);
			HtmlData.PopulateTokenHashset(tokens4);
			HtmlData.PopulateTokenHashset(tokens5);
			HtmlData.PopulateTokenHashset(tokens6);
			HtmlData.PopulateTokenHashset(tokens7);
			HtmlData.PopulateTokenHashset(tokens8);
			HtmlData.PopulateTokenHashset(tokens9);
			while (HtmlData.nextID < (ushort)HtmlData.TokenMetadata.Length)
			{
				HtmlData.Tokens.Add(null);
				HtmlData.nextID += 1;
			}
			HtmlData.setBit(tokens, TokenProperties.HtmlChildrenNotAllowed);
			HtmlData.setBit(tokens2, TokenProperties.ChildrenNotAllowed | TokenProperties.HtmlChildrenNotAllowed);
			HtmlData.setBit(tokens6, TokenProperties.AutoOpenOrClose);
			HtmlData.setBit(tokens3, TokenProperties.BlockElement);
			HtmlData.setBit(tokens5, TokenProperties.BooleanProperty);
			HtmlData.setBit(tokens4, TokenProperties.ParagraphCloser);
			HtmlData.setBit(tokens7, TokenProperties.MetaDataTags);
			HtmlData.setBit(tokens8, TokenProperties.CaseInsensitiveValues);
			HtmlData.setBit(tokens9, TokenProperties.HasValue);
			HtmlData.setBit(tokens10, TokenProperties.FormInputControl);
		}

		static HashSet<ushort> PopulateTokenHashset(IEnumerable<string> tokens)
		{
			HashSet<ushort> hashSet = new HashSet<ushort>();
			foreach (string name in tokens)
			{
				hashSet.Add(HtmlData.Tokenize(name));
			}
			return hashSet;
		}

		public static IEnumerable<string> Keys
		{
			get
			{
				return HtmlData.Tokens;
			}
		}

		public static bool HtmlChildrenNotAllowed(ushort nodeId)
		{
			return (nodeId & 65280) == 0 && (HtmlData.TokenMetadata[(int)nodeId] & 16) > 0;
		}

		public static bool HtmlChildrenNotAllowed(string nodeName)
		{
			return HtmlData.HtmlChildrenNotAllowed(HtmlData.Tokenize(nodeName));
		}

		public static bool ChildrenAllowed(ushort tokenId)
		{
			return (tokenId & 65280) != 0 || (HtmlData.TokenMetadata[(int)tokenId] & 8) == 0;
		}

		public static bool ChildrenAllowed(string nodeName)
		{
			return HtmlData.ChildrenAllowed(HtmlData.Tokenize(nodeName));
		}

		public static bool IsBlock(ushort tokenId)
		{
			return (tokenId & 65280) == 0 && (HtmlData.TokenMetadata[(int)tokenId] & 1) != 0;
		}

		public static bool IsBlock(string nodeName)
		{
			return HtmlData.IsBlock(HtmlData.Tokenize(nodeName));
		}

		public static bool IsBoolean(ushort tokenId)
		{
			return (tokenId & 65280) == 0 && (HtmlData.TokenMetadata[(int)tokenId] & 2) != 0;
		}

		public static bool IsBoolean(string propertyName)
		{
			return HtmlData.IsBoolean(HtmlData.Tokenize(propertyName));
		}

		public static bool IsCaseInsensitiveValues(string attributeName)
		{
			return HtmlData.IsCaseInsensitiveValues(HtmlData.Tokenize(attributeName));
		}

		public static bool IsCaseInsensitiveValues(ushort attributeToken)
		{
			return (attributeToken & 65280) == 0 && (HtmlData.TokenMetadata[(int)attributeToken] & 128) != 0;
		}

		public static bool HasValueProperty(string nodeName)
		{
			return HtmlData.HasValueProperty(HtmlData.Tokenize(nodeName));
		}

		public static bool HasValueProperty(ushort nodeNameToken)
		{
			return (nodeNameToken & 65280) == 0 && (HtmlData.TokenMetadata[(int)nodeNameToken] & 256) != 0;
		}

		public static bool IsFormInputControl(string nodeName)
		{
			return HtmlData.IsFormInputControl(HtmlData.Tokenize(nodeName));
		}

		public static bool IsFormInputControl(ushort nodeNameToken)
		{
			return (nodeNameToken & 65280) == 0 && (HtmlData.TokenMetadata[(int)nodeNameToken] & 256) != 0;
		}

		public static ushort Tokenize(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return 0;
			}
			return HtmlData.TokenizeImpl(name.ToLower());
		}

		public static ushort TokenizeCaseSensitive(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return 0;
			}
			return HtmlData.TokenizeImpl(name);
		}

		static ushort TokenizeImpl(string tokenName)
		{
			ushort result;
			if (!HtmlData.TokenIDs.TryGetValue(tokenName, out result))
			{
				lock (HtmlData.locker)
				{
					if (!HtmlData.TokenIDs.TryGetValue(tokenName, out result))
					{
						HtmlData.Tokens.Add(tokenName);
						HtmlData.TokenIDs.Add(tokenName, HtmlData.nextID);
						ushort num = HtmlData.nextID;
						HtmlData.nextID = (ushort)(num + 1);
						result = num;
					}
				}
			}
			return result;
		}

		public static string TokenName(ushort tokenId)
		{
			if (tokenId > 0)
			{
				return HtmlData.Tokens[(int)(tokenId - 2)];
			}
			return "";
		}

		public static string AttributeEncode(string text, bool alwaysQuote, out string quoteChar)
		{
			if (text == "")
			{
				quoteChar = "\"";
				return "";
			}
			bool flag = text.IndexOf("\"") >= 0;
			bool flag2 = text.IndexOf("'") >= 0;
			string text2 = text;
			if (flag || flag2)
			{
				if (flag && flag2)
				{
					text2 = text2.Replace("'", "&#39;");
					quoteChar = "'";
				}
				else if (flag)
				{
					quoteChar = "'";
				}
				else
				{
					quoteChar = "\"";
				}
			}
			else if (alwaysQuote)
			{
				quoteChar = "\"";
			}
			else
			{
				quoteChar = ((text2.IndexOfAny(HtmlData.MustBeQuotedAll) >= 0) ? "\"" : "");
			}
			return text2;
		}

		public static ushort SpecialTagAction(string tag, string newTag, bool isDocument = true)
		{
			if (!isDocument)
			{
				return HtmlData.SpecialTagAction(HtmlData.Tokenize(tag), HtmlData.Tokenize(newTag));
			}
			return HtmlData.SpecialTagActionForDocument(HtmlData.Tokenize(tag), HtmlData.Tokenize(newTag));
		}

		public static ushort SpecialTagActionForDocument(ushort parentTagId, ushort newTagId)
		{
			if (parentTagId != 36)
			{
				return HtmlData.SpecialTagAction(parentTagId, newTagId);
			}
			if ((newTagId & 65280) == 0 && (HtmlData.TokenMetadata[(int)newTagId] & 64) != 0)
			{
				return 16;
			}
			if (newTagId == 17 || newTagId == 16)
			{
				return 0;
			}
			return 17;
		}

		public static ushort SpecialTagAction(ushort parentTagId, ushort newTagId)
		{
			if ((parentTagId & 65280) != 0)
			{
				return 0;
			}
			switch (parentTagId)
			{
			case 11:
				if (newTagId != 11)
				{
					return 0;
				}
				return 1;
			case 12:
				if ((newTagId & 65280) != 0 || (HtmlData.TokenMetadata[(int)newTagId] & 32) == 0)
				{
					return 0;
				}
				return 1;
			case 13:
				if (newTagId != 13 && newTagId != 27 && newTagId != 28)
				{
					return 0;
				}
				return 1;
			case 14:
			case 15:
				if (newTagId != 27 && newTagId != 28 && newTagId != 15 && newTagId != 14 && newTagId != 13)
				{
					return 0;
				}
				return 1;
			case 16:
				if ((newTagId & 65280) != 0 || (HtmlData.TokenMetadata[(int)newTagId] & 64) != 0)
				{
					return 0;
				}
				return 1;
			case 18:
			case 20:
				if (newTagId != 18 && newTagId != 20)
				{
					return 0;
				}
				return 1;
			case 19:
				if (newTagId != 19 && newTagId != 13 && newTagId != 23 && newTagId != 29 && newTagId != 27 && newTagId != 28)
				{
					return 0;
				}
				return 1;
			case 21:
				if (newTagId != 21)
				{
					return 0;
				}
				return 1;
			case 23:
				if (newTagId != 13)
				{
					return 0;
				}
				return 27;
			case 24:
				if (newTagId != 24)
				{
					return 0;
				}
				return 1;
			case 27:
			case 29:
				if (newTagId != 27 && newTagId != 28)
				{
					return 0;
				}
				return 1;
			case 28:
				if (newTagId != 17 && newTagId != 29)
				{
					return 0;
				}
				return 1;
			case 30:
			case 31:
				if (newTagId != 30 && newTagId != 31)
				{
					return 0;
				}
				return 1;
			}
			return 0;
		}

		static void setBit(IEnumerable<string> tokens, TokenProperties bit)
		{
			foreach (string name in tokens)
			{
				HtmlData.setBit(HtmlData.Tokenize(name), bit);
			}
		}

		static void setBit(IEnumerable<ushort> tokens, TokenProperties bit)
		{
			foreach (ushort token in tokens)
			{
				HtmlData.setBit(token, bit);
			}
		}

		static void setBit(ushort token, TokenProperties bit)
		{
			ushort[] tokenMetadata = HtmlData.TokenMetadata;
			tokenMetadata[(int)token] = (ushort)(tokenMetadata[(int)token] | (ushort)bit);
		}

		public const int pathIdLength = 1;

		public const ushort indexSeparator = 65535;

		public const ushort tagActionNothing = 0;

		public const ushort tagActionClose = 1;

		public const ushort ClassAttrId = 3;

		public const ushort ValueAttrId = 4;

		public const ushort IDAttrId = 5;

		public const ushort SelectedAttrId = 6;

		public const ushort ReadonlyAttrId = 7;

		public const ushort CheckedAttrId = 8;

		public const ushort tagINPUT = 9;

		public const ushort tagSELECT = 10;

		public const ushort tagOPTION = 11;

		public const ushort tagP = 12;

		public const ushort tagTR = 13;

		public const ushort tagTD = 14;

		public const ushort tagTH = 15;

		public const ushort tagHEAD = 16;

		public const ushort tagBODY = 17;

		public const ushort tagDT = 18;

		public const ushort tagCOLGROUP = 19;

		public const ushort tagDD = 20;

		public const ushort tagLI = 21;

		public const ushort tagDL = 22;

		public const ushort tagTABLE = 23;

		public const ushort tagOPTGROUP = 24;

		public const ushort tagUL = 25;

		public const ushort tagOL = 26;

		public const ushort tagTBODY = 27;

		public const ushort tagTFOOT = 28;

		public const ushort tagTHEAD = 29;

		public const ushort tagRT = 30;

		public const ushort tagRP = 31;

		public const ushort tagSCRIPT = 32;

		public const ushort tagTEXTAREA = 33;

		public const ushort tagSTYLE = 34;

		public const ushort tagCOL = 35;

		public const ushort tagHTML = 36;

		public const ushort tagBUTTON = 37;

		public const ushort attrMULTIPLE = 38;

		public const ushort tagA = 39;

		public const ushort tagSPAN = 40;

		public const ushort tagFORM = 41;

		public const ushort attrREQUIRED = 42;

		public const ushort attrAUTOFOCUS = 43;

		public const ushort attrTYPE = 44;

		public const ushort tagPROGRESS = 45;

		public const ushort tagLABEL = 46;

		public const ushort attrDISABLED = 47;

		public const ushort tagMETER = 48;

		public const ushort tagIMG = 49;

		public const ushort tagLINK = 50;

		const ushort maxHardcodedTokenId = 50;

		const ushort NonSpecialTokenMask = 65280;

		public static bool Debug = false;

		static char[] MustBeQuoted = new char[] { '/', '"', '\'', '=', '<', '>', '`' };

		static char[] MustBeQuotedAll;

		public static HashSet<char> NumberChars = new HashSet<char>("-+0123456789.,");

		public static HashSet<string> Units = new HashSet<string>(new string[]
		{
			"%", "cm", "mm", "in", "px", "pc", "pt", "em", "ex", "vmin",
			"vw", "rem", "vh", "deg", "rad", "grad", "turn", "s", "ms", "Hz",
			"KHz", "dpi", "dpcm", "dppx"
		});

		static ushort nextID = 2;

		static List<string> Tokens = new List<string>();

		static Dictionary<string, ushort> TokenIDs;

		static object locker = new object();

		static string defaultPadding;

		static ushort[] TokenMetadata = new ushort[256];
	}
}
