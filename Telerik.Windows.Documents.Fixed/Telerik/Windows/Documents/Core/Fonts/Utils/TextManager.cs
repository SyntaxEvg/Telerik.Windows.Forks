using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.Fonts.Utils
{
	class TextManager
	{
		internal static GlyphForm GetGlyphForm(string text, int index)
		{
			if (!char.IsLetter(text[index]))
			{
				return GlyphForm.Undefined;
			}
			if (TextManager.charactersWithOnlyIsolatedOrFinalForm.Contains(text[index]))
			{
				if (index - 1 >= 0 && char.IsLetter(text[index - 1]) && !TextManager.charactersWithOnlyIsolatedOrFinalForm.Contains(text[index - 1]))
				{
					return GlyphForm.Final;
				}
				return GlyphForm.Isolated;
			}
			else if (index - 1 >= 0 && char.IsLetter(text[index - 1]) && !TextManager.charactersWithOnlyIsolatedOrFinalForm.Contains(text[index - 1]))
			{
				if (index + 1 < text.Length && char.IsLetter(text[index + 1]))
				{
					return GlyphForm.Medial;
				}
				return GlyphForm.Final;
			}
			else
			{
				if (index + 1 < text.Length && char.IsLetter(text[index + 1]))
				{
					return GlyphForm.Initial;
				}
				return GlyphForm.Isolated;
			}
		}

		static readonly HashSet<char> charactersWithOnlyIsolatedOrFinalForm = new HashSet<char>(new char[] { 'ا', 'أ', 'إ', 'آ', 'د', 'ذ', 'ر', 'ز', 'و' });
	}
}
