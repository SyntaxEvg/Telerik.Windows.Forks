using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class WildcardStringExtensions
	{
		static WildcardStringExtensions()
		{
			WildcardStringExtensions.wildcardSymbolToRegex["*"] = ".*";
			WildcardStringExtensions.wildcardSymbolToRegex["?"] = ".{1}";
			WildcardStringExtensions.wildcardSymbolToRegex["~"] = Regex.Escape("~");
		}

		public static string FromWildcardStringToRegex(this string wildcardString)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in wildcardString.TokenizeWildcardString())
			{
				if (text.IsWildcardCharacter())
				{
					stringBuilder.Append(WildcardStringExtensions.wildcardSymbolToRegex[text]);
				}
				else
				{
					stringBuilder.Append(Regex.Escape(text));
				}
			}
			return stringBuilder.ToString();
		}

		public static bool IsWildcardCharacter(this char character)
		{
			return character == "*"[0] || character == "?"[0] || character == "~"[0];
		}

		public static bool IsWildcardCharacter(this string character)
		{
			return character.Length == 1 && character[0].IsWildcardCharacter();
		}

		public static bool IsNonTildeWildcardCharacter(this char character)
		{
			return character != "~"[0] && character.IsWildcardCharacter();
		}

		static IEnumerable<string> TokenizeWildcardString(this string wildcardString)
		{
			StringBuilder token = new StringBuilder();
			for (int i = 0; i < wildcardString.Length; i++)
			{
				char character = wildcardString[i];
				if (character.IsWildcardCharacter())
				{
					if (character == "~"[0])
					{
						if (i < wildcardString.Length - 1 && wildcardString[i + 1].IsWildcardCharacter())
						{
							token.Append(wildcardString[i + 1]);
							i++;
						}
					}
					else
					{
						string tokenToYield = token.ToString();
						if (tokenToYield.Length > 0)
						{
							yield return tokenToYield;
							token.Clear();
						}
						yield return character.ToString();
					}
				}
				else
				{
					token.Append(character);
				}
			}
			string lastToken = token.ToString();
			if (lastToken.Length > 0)
			{
				yield return lastToken;
			}
			yield break;
		}

		static readonly Dictionary<string, string> wildcardSymbolToRegex = new Dictionary<string, string>();
	}
}
