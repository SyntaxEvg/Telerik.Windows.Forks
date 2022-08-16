using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts
{
	abstract class FontsManagerBase
	{
		static FontsManagerBase()
		{
			FontsManagerBase.alternativeNames = new Dictionary<string, string>();
			FontsManagerBase.InitializeStandardFontStreams();
			FontsManagerBase.InitializeStandardFontDescriptors();
			FontsManagerBase.InitializeAlternativeNames();
		}

		public FontsManagerBase()
		{
			this.standardFontSources = new Dictionary<string, Type1FontSource>();
			this.InitializeStandardFontSources();
		}

		public abstract OpenTypeFontSource GetOpenTypeFontSource(FontProperties fontProperties);

		public static bool IsStandardFontName(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return FontsManagerBase.standardFontDescriptors.ContainsKey(name) || FontsManagerBase.alternativeNames.ContainsKey(name);
		}

		public static StandardFontDescriptor GetStandardFontDescriptor(string fontName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			string standardFontName = FontsManagerBase.GetStandardFontName(fontName);
			StandardFontDescriptor result;
			if (!FontsManagerBase.standardFontDescriptors.TryGetValue(standardFontName, out result))
			{
				throw new ArgumentException("Font name is not a standard font.", fontName);
			}
			return result;
		}

		public Type1FontSource GetStandardFontSource(string fontName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			string standardFontName = FontsManagerBase.GetStandardFontName(fontName);
			Type1FontSource result;
			if (!this.standardFontSources.TryGetValue(standardFontName, out result))
			{
				throw new ArgumentException("Font name is not a standard font.", fontName);
			}
			return result;
		}

		public Type1FontSource GetType1FallbackFontSource(string fontName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			string styles = fontName.ToLower();
			bool flag = FontsHelper.IsBold(styles);
			bool flag2 = FontsHelper.IsItalic(styles);
			string fontName2 = "Helvetica";
			if (flag)
			{
				if (flag2)
				{
					fontName2 = "Helvetica-BoldOblique";
				}
				else
				{
					fontName2 = "Helvetica-Bold";
				}
			}
			else if (flag2)
			{
				fontName2 = "Helvetica-Oblique";
			}
			return this.GetStandardFontSource(fontName2);
		}

		static Stream GetApplicationResourceStream(string fontName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			string resourceName = string.Format("Telerik.Windows.Documents.Fixed.Resources.Fonts.{0}.pfb", fontName);
			return ResourcesHelper.GetApplicationResourceStream(resourceName);
		}

		static string GetStandardFontName(string fontName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			string result = fontName;
			if (FontsManagerBase.alternativeNames.ContainsKey(fontName))
			{
				result = FontsManagerBase.alternativeNames[fontName];
			}
			return result;
		}

		static void RegisterStandardFontStream(string fontName, Stream stream)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			FontsManagerBase.standardFontStreams[fontName] = stream.ReadAllBytes();
		}

		static void RegisterStandardFontDescriptor(string name, StandardFontDescriptor fontDescriptor)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNull<StandardFontDescriptor>(fontDescriptor, "fontDescriptor");
			FontsManagerBase.standardFontDescriptors[name] = fontDescriptor;
		}

		static void InitializeStandardFontDescriptors()
		{
			StandardFontDescriptor fontDescriptor = new StandardFontDescriptor(629.0, -157.0);
			StandardFontDescriptor fontDescriptor2 = new StandardFontDescriptor(718.0, -207.0);
			StandardFontDescriptor fontDescriptor3 = new StandardFontDescriptor(683.0, -217.0);
			StandardFontDescriptor fontDescriptor4 = new StandardFontDescriptor(1000.0, 0.0);
			FontsManagerBase.RegisterStandardFontDescriptor("Courier", fontDescriptor);
			FontsManagerBase.RegisterStandardFontDescriptor("Courier-Bold", fontDescriptor);
			FontsManagerBase.RegisterStandardFontDescriptor("Courier-BoldOblique", fontDescriptor);
			FontsManagerBase.RegisterStandardFontDescriptor("Courier-Oblique", fontDescriptor);
			FontsManagerBase.RegisterStandardFontDescriptor("Helvetica", fontDescriptor2);
			FontsManagerBase.RegisterStandardFontDescriptor("Helvetica-Bold", fontDescriptor2);
			FontsManagerBase.RegisterStandardFontDescriptor("Helvetica-BoldOblique", fontDescriptor2);
			FontsManagerBase.RegisterStandardFontDescriptor("Helvetica-Oblique", fontDescriptor2);
			FontsManagerBase.RegisterStandardFontDescriptor("Times-Roman", fontDescriptor3);
			FontsManagerBase.RegisterStandardFontDescriptor("Times-Bold", fontDescriptor3);
			FontsManagerBase.RegisterStandardFontDescriptor("Times-BoldItalic", fontDescriptor3);
			FontsManagerBase.RegisterStandardFontDescriptor("Times-Italic", fontDescriptor3);
			FontsManagerBase.RegisterStandardFontDescriptor("Symbol", fontDescriptor4);
			FontsManagerBase.RegisterStandardFontDescriptor("ZapfDingbats", fontDescriptor4);
		}

		static void InitializeAlternativeNames()
		{
			FontsManagerBase.RegisterAlternativeName("Helvetica", new string[] { "Arial", "Arial MT" });
			FontsManagerBase.RegisterAlternativeName("Helvetica-Bold", new string[] { "Arial,Bold", "Arial MT,Bold" });
			FontsManagerBase.RegisterAlternativeName("Helvetica-BoldOblique", new string[] { "Arial,BoldItalic", "Arial MT,BoldItalic" });
			FontsManagerBase.RegisterAlternativeName("Helvetica-Oblique", new string[] { "Arial,Italic", "Arial MT,Italic" });
			FontsManagerBase.RegisterAlternativeName("Courier", new string[] { "Courier New" });
			FontsManagerBase.RegisterAlternativeName("Courier-Bold", new string[] { "Courier New,Bold" });
			FontsManagerBase.RegisterAlternativeName("Courier-BoldOblique", new string[] { "Courier New,BoldItalic" });
			FontsManagerBase.RegisterAlternativeName("Courier-Oblique", new string[] { "Courier New,Italic" });
			FontsManagerBase.RegisterAlternativeName("Times-Roman", new string[] { "Times New Roman" });
			FontsManagerBase.RegisterAlternativeName("Times-Bold", new string[] { "Times New Roman,Bold" });
			FontsManagerBase.RegisterAlternativeName("Times-BoldItalic", new string[] { "Times New Roman,BoldItalic" });
			FontsManagerBase.RegisterAlternativeName("Times-Italic", new string[] { "Times New Roman,Italic" });
		}

		static void RegisterAlternativeName(string original, params string[] alternatives)
		{
			foreach (string key in alternatives)
			{
				FontsManagerBase.alternativeNames[key] = original;
			}
		}

		static void InitializeStandardFontStreams()
		{
			if (FontsManagerBase.standardFontStreams == null)
			{
				FontsManagerBase.standardFontStreams = new Dictionary<string, byte[]>();
				FontsManagerBase.RegisterStandardFontStream("Courier", FontsManagerBase.GetApplicationResourceStream("Courier"));
				FontsManagerBase.RegisterStandardFontStream("Courier-Bold", FontsManagerBase.GetApplicationResourceStream("Courier-Bold"));
				FontsManagerBase.RegisterStandardFontStream("Courier-BoldOblique", FontsManagerBase.GetApplicationResourceStream("Courier-BoldOblique"));
				FontsManagerBase.RegisterStandardFontStream("Courier-Oblique", FontsManagerBase.GetApplicationResourceStream("Courier-Oblique"));
				FontsManagerBase.RegisterStandardFontStream("Helvetica", FontsManagerBase.GetApplicationResourceStream("Helvetica"));
				FontsManagerBase.RegisterStandardFontStream("Helvetica-Bold", FontsManagerBase.GetApplicationResourceStream("Helvetica-Bold"));
				FontsManagerBase.RegisterStandardFontStream("Helvetica-BoldOblique", FontsManagerBase.GetApplicationResourceStream("Helvetica-BoldOblique"));
				FontsManagerBase.RegisterStandardFontStream("Helvetica-Oblique", FontsManagerBase.GetApplicationResourceStream("Helvetica-Oblique"));
				FontsManagerBase.RegisterStandardFontStream("Times-Roman", FontsManagerBase.GetApplicationResourceStream("Times-Roman"));
				FontsManagerBase.RegisterStandardFontStream("Times-Bold", FontsManagerBase.GetApplicationResourceStream("Times-Bold"));
				FontsManagerBase.RegisterStandardFontStream("Times-BoldItalic", FontsManagerBase.GetApplicationResourceStream("Times-BoldItalic"));
				FontsManagerBase.RegisterStandardFontStream("Times-Italic", FontsManagerBase.GetApplicationResourceStream("Times-Italic"));
				FontsManagerBase.RegisterStandardFontStream("Symbol", FontsManagerBase.GetApplicationResourceStream("Symbol"));
				FontsManagerBase.RegisterStandardFontStream("ZapfDingbats", FontsManagerBase.GetApplicationResourceStream("ZapfDingbats"));
			}
		}

		static Type1FontSource CreateType1FontSource(byte[] data)
		{
			Type1FontSource result;
			try
			{
				Type1FontSource type1FontSource = new Type1FontSource(data);
				result = type1FontSource;
			}
			catch
			{
				throw new InvalidOperationException("Cannot create standard font.");
			}
			return result;
		}

		void InitializeStandardFontSources()
		{
			foreach (KeyValuePair<string, byte[]> keyValuePair in FontsManagerBase.standardFontStreams)
			{
				this.RegisterStandardFontSource(keyValuePair.Key, FontsManagerBase.CreateType1FontSource(keyValuePair.Value));
			}
		}

		void RegisterStandardFontSource(string fontName, Type1FontSource fontSource)
		{
			Guard.ThrowExceptionIfNullOrEmpty(fontName, "fontName");
			Guard.ThrowExceptionIfNull<Type1FontSource>(fontSource, "fontSource");
			this.standardFontSources[fontName] = fontSource;
		}

		public const string Courier = "Courier";

		public const string CourierBold = "Courier-Bold";

		public const string CourierBoldOblique = "Courier-BoldOblique";

		public const string CourierOblique = "Courier-Oblique";

		public const string Helvetica = "Helvetica";

		public const string HelveticaBold = "Helvetica-Bold";

		public const string HelveticaBoldOblique = "Helvetica-BoldOblique";

		public const string HelveticaOblique = "Helvetica-Oblique";

		public const string TimesRoman = "Times-Roman";

		public const string TimesBold = "Times-Bold";

		public const string TimesBoldItalic = "Times-BoldItalic";

		public const string TimesItalic = "Times-Italic";

		public const string Symbol = "Symbol";

		public const string ZapfDingbats = "ZapfDingbats";

		static readonly Dictionary<string, string> alternativeNames;

		static readonly Dictionary<string, StandardFontDescriptor> standardFontDescriptors = new Dictionary<string, StandardFontDescriptor>();

		static Dictionary<string, byte[]> standardFontStreams;

		readonly Dictionary<string, Type1FontSource> standardFontSources;
	}
}
