using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts
{
	static class SystemFontsManager
	{
		static SystemFontsManager()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string name = string.Empty;
			name = "AllSyntaxConvert.Resours.Syntax.Windows.Documents.Core.TextMeasurer.FontsMetaInfo.Fonts.meta";
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						if (!text.StartsWith("//"))
						{
							string[] array = text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
							string text2 = array[0];
							if (array[1] == "1")
							{
								SystemFontsManager.monospacedFonts.Add(text2);
							}
							if (array[2] == "1")
							{
								SystemFontsManager.fontsWithKerning.Add(text2);
							}
							for (int i = 3; i < array.Length; i++)
							{
								dictionary[array[i].ToLower(CultureInfo.InvariantCulture)] = text2;
							}
						}
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (Typeface typeface in System.Windows.Media.Fonts.SystemTypefaces)
			{
				try
				{
					GlyphTypeface typeFace;
					if (typeface.TryGetGlyphTypeface(out typeFace))
					{
						string key = SystemFontsManager.SanitizeFontFileName(Path.GetFileName(typeFace.GetFontFileName()));
						string item;
						if (dictionary.TryGetValue(key, out item))
						{
							hashSet.Add(item);
						}
					}
				}
				catch
				{
				}
			}
			using (InstalledFontCollection installedFontCollection = new InstalledFontCollection())
			{
				foreach (System.Drawing.FontFamily fontFamily in installedFontCollection.Families)
				{
					if (!hashSet.Contains(fontFamily.Name))
					{
						hashSet.Add(fontFamily.Name);
					}
				}
			}
			SystemFontsManager.systemFonts = new List<string>(hashSet);
			SystemFontsManager.systemFonts.Sort();
		}

		public static IEnumerable<string> GetAvailableFonts()
		{
			return SystemFontsManager.systemFonts;
		}

		public static bool HasKerning(string fontName)
		{
			return SystemFontsManager.fontsWithKerning.Contains(fontName);
		}

		public static bool IsMonospaced(string fontFamily)
		{
			if (fontFamily == null)
			{
				return false;
			}
			string[] array = fontFamily.Split(new char[] { ',' });
			foreach (string text in array)
			{
				if (!SystemFontsManager.monospacedFonts.Contains(text.Trim()))
				{
					return false;
				}
			}
			return true;
		}

		internal static void WarmUp()
		{
		}

		static string SanitizeFontFileName(string fileName)
		{
			fileName = fileName.ToLower(CultureInfo.InvariantCulture);
			return Regex.Replace(Path.GetFileNameWithoutExtension(fileName), "_[0-9]+$", string.Empty) + Path.GetExtension(fileName);
		}

		static readonly HashSet<string> fontsWithKerning = new HashSet<string>();

		static readonly HashSet<string> monospacedFonts = new HashSet<string>();

		static readonly List<string> systemFonts;
	}
}
