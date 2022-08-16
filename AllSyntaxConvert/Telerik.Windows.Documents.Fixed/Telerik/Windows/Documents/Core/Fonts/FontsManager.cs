using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Utils;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts
{
	class FontsManager : FontsManagerBase
	{
		public FontsManager() : base()
		{
			this.fonts = new Dictionary<FontProperties, OpenTypeFontSource>();
			this.fallbacks = new Dictionary<FallbackRecord, OpenTypeFontSource>();
		}

		public override OpenTypeFontSource GetOpenTypeFontSource(FontProperties fontProperties)
		{
			OpenTypeFontSource fontSourceFromGlyphTypeface;
			if (!this.fonts.TryGetValue(fontProperties, out fontSourceFromGlyphTypeface))
			{
				GlyphTypeface glyphTypeface = FontsManager.GetGlyphTypeface(fontProperties.FontFamilyName, fontProperties.FontStyle, fontProperties.FontWeight);
				fontSourceFromGlyphTypeface = this.GetFontSourceFromGlyphTypeface(fontProperties.FontFamilyName, glyphTypeface);
				this.fonts[fontProperties] = fontSourceFromGlyphTypeface;
			}
			return fontSourceFromGlyphTypeface;
		}

		public OpenTypeFontSource GetFallbackFontSource(FontProperties descr, string text)
		{
			FallbackRange fallbackRange = FallbackRange.GetFallbackRange(text[0]);
			FallbackRecord key = new FallbackRecord(fallbackRange, descr);
			OpenTypeFontSource fontSourceFromGlyphTypeface;
			if (!this.fallbacks.TryGetValue(key, out fontSourceFromGlyphTypeface))
			{
				foreach (string text2 in fallbackRange.FallbackFontFamilies)
				{
					GlyphTypeface glyphTypeface = FontsManager.GetGlyphTypeface(text2, descr.FontStyle, descr.FontWeight);
					fontSourceFromGlyphTypeface = this.GetFontSourceFromGlyphTypeface(text2, glyphTypeface);
					if (fontSourceFromGlyphTypeface != null)
					{
						break;
					}
				}
				if (fontSourceFromGlyphTypeface == null)
				{
					throw new NotSupportedFontFamilyException();
				}
				this.fallbacks[key] = fontSourceFromGlyphTypeface;
			}
			return fontSourceFromGlyphTypeface;
		}

		private static GlyphTypeface GetGlyphTypeface(string familyName, FontStyle style, FontWeight weight)
		{
			Typeface typeface = new Typeface(new FontFamily(familyName), style, weight, FontStretches.Normal);
			GlyphTypeface result;
			typeface.TryGetGlyphTypeface(out result);
			return result;
		}

		private static IEnumerable<OpenTypeFontSourceBase> GetFontSources(Stream fontUri)
		{
			OpenTypeFontReader reader = new OpenTypeFontReader(fontUri.ReadAllBytes());
			switch (OpenTypeFontSource.GetFontType(reader))
			{
			case OpenTypeFontSourceType.TrueType:
				yield return FontsManager.ReadTrueTypeFont(reader);
				break;
			case OpenTypeFontSourceType.TrueTypeCollection:
			{
				TrueTypeCollection ttc = FontsManager.ReadTrueTypeCollection(reader);
				foreach (OpenTypeFontSourceBase fs in ttc.Fonts)
				{
					yield return fs;
				}
				break;
			}
			}
			yield break;
		}

		private static FontProperties CreateFontDescriptorFromFontSource(FontSource source)
		{
			FontStyle fontStyle = (source.IsItalic ? FontStyles.Italic : FontStyles.Normal);
			FontWeight fontWeight = (source.IsBold ? FontWeights.Bold : FontWeights.Normal);
			return new FontProperties(new FontFamily(source.FontFamily), fontStyle, fontWeight);
		}

		private static OpenTypeFontSourceBase ReadTrueTypeFont(OpenTypeFontReader reader)
		{
			return new OpenTypeFontSource(reader);
		}

		private static TrueTypeCollection ReadTrueTypeCollection(OpenTypeFontReader reader)
		{
			TrueTypeCollection trueTypeCollection = new TrueTypeCollection(reader);
			trueTypeCollection.Initialize();
			return trueTypeCollection;
		}

		private void RegisterFontSource(FontProperties descr, OpenTypeFontSource source)
		{
			this.fonts[descr] = source;
		}

		private OpenTypeFontSource GetFontSourceFromGlyphTypeface(string fontFamily, GlyphTypeface gtf)
		{
			OpenTypeFontSource result = null;
			if (gtf == null)
			{
				return null;
			}
			bool flag = gtf.Weight == FontWeights.Bold && gtf.StyleSimulations != StyleSimulations.BoldItalicSimulation && gtf.StyleSimulations != StyleSimulations.BoldSimulation;
			bool flag2 = gtf.Style != FontStyles.Normal && gtf.StyleSimulations != StyleSimulations.BoldItalicSimulation && gtf.StyleSimulations != StyleSimulations.ItalicSimulation;
			foreach (OpenTypeFontSourceBase openTypeFontSourceBase in FontsManager.GetFontSources(gtf.GetFontStream()))
			{
				OpenTypeFontSource openTypeFontSource = (OpenTypeFontSource)openTypeFontSourceBase;
				if (openTypeFontSource.FontFamily == fontFamily && flag == openTypeFontSource.IsBold && flag2 == openTypeFontSource.IsItalic)
				{
					result = openTypeFontSource;
				}
				else
				{
					this.RegisterFontSource(FontsManager.CreateFontDescriptorFromFontSource(openTypeFontSource), openTypeFontSource);
				}
			}
			return result;
		}

		private readonly Dictionary<FontProperties, OpenTypeFontSource> fonts;

		private readonly Dictionary<FallbackRecord, OpenTypeFontSource> fallbacks;

		//[CompilerGenerated]
		//private sealed class GetFontSourcesd__0 : IEnumerable<OpenTypeFontSourceBase>, IEnumerable, IEnumerator<OpenTypeFontSourceBase>, IEnumerator, IDisposable
		//{
		//	[DebuggerHidden]
		//	IEnumerator<OpenTypeFontSourceBase> IEnumerable<OpenTypeFontSourceBase>.GetEnumerator()
		//	{
		//		FontsManager.GetFontSourcesd__0 GetFontSourcesd__;
		//		if (Thread.CurrentThread.ManagedThreadId == this._l__initialThreadId && this._1__state == -2)
		//		{
		//			this._1__state = 0;
		//			GetFontSourcesd__ = this;
		//		}
		//		else
		//		{
		//			<GetFontSources>d__ = new FontsManager.GetFontSourcesd__0(0);
		//		}
		//		<GetFontSources>d__.fontUri = this._3__fontUri;
		//		return <GetFontSources>d__;
		//	}

		//	[DebuggerHidden]
		//	IEnumerator IEnumerable.GetEnumerator()
		//	{
		//		return this.System.Collections.Generic.IEnumerable<Telerik.Windows.Documents.Core.Fonts.OpenType.OpenTypeFontSourceBase>.GetEnumerator();
		//	}

		//	bool IEnumerator.MoveNext()
		//	{
		//		bool result;
		//		try
		//		{
		//			switch (this._1__state)
		//			{
		//			case 0:
		//				this._1__state = -1;
		//				this.<reader>5__1 = new OpenTypeFontReader(this.fontUri.ReadAllBytes());
		//				switch (OpenTypeFontSource.GetFontType(this.<reader>5__1))
		//				{
		//				case OpenTypeFontSourceType.TrueType:
		//					this._2__current = FontsManager.ReadTrueTypeFont(this.<reader>5__1);
		//					this._1__state = 1;
		//					return true;
		//				case OpenTypeFontSourceType.TrueTypeCollection:
		//					this.<ttc>5__2 = FontsManager.ReadTrueTypeCollection(this.<reader>5__1);
		//					this._7__wrap4 = this.<ttc>5__2.Fonts.GetEnumerator();
		//					this._1__state = 2;
		//					break;
		//				default:
		//					goto IL_FA;
		//				}
		//				break;
		//			case 1:
		//				this._1__state = -1;
		//				goto IL_FA;
		//			case 2:
		//				goto IL_FA;
		//			case 3:
		//				this._1__state = 2;
		//				break;
		//			default:
		//				goto IL_FA;
		//			}
		//			if (this._7__wrap4.MoveNext())
		//			{
		//				this.<fs>5__3 = this._7__wrap4.Current;
		//				this._2__current = this.<fs>5__3;
		//				this._1__state = 3;
		//				return true;
		//			}
		//			this._m__Finally5();
		//			IL_FA:
		//			result = false;
		//		}
		//		catch
		//		{
		//			this.System.IDisposable.Dispose();
		//			throw;
		//		}
		//		return result;
		//	}

		//	OpenTypeFontSourceBase IEnumerator<OpenTypeFontSourceBase>.Current
		//	{
		//		[DebuggerHidden]
		//		get
		//		{
		//			return this._2__current;
		//		}
		//	}

		//	[DebuggerHidden]
		//	void IEnumerator.Reset()
		//	{
		//		throw new NotSupportedException();
		//	}

		//	void IDisposable.Dispose()
		//	{
		//		switch (this._1__state)
		//		{
		//		case 2:
		//		case 3:
		//			try
		//			{
		//			}
		//			finally
		//			{
		//				this._m__Finally5();
		//			}
		//			return;
		//		default:
		//			return;
		//		}
		//	}

		//	object IEnumerator.Current
		//	{
		//		[DebuggerHidden]
		//		get
		//		{
		//			return this._2__current;
		//		}
		//	}

		//	[DebuggerHidden]
		//	public GetFontSourcesd__0(int _1__state)
		//	{
		//		this._1__state = _1__state;
		//		this._l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
		//	}

		//	private void _m__Finally5()
		//	{
		//		this._1__state = -1;
		//		if (this._7__wrap4 != null)
		//		{
		//			this._7__wrap4.Dispose();
		//		}
		//	}

		//	private OpenTypeFontSourceBase _2__current;

		//	private int _1__state;

		//	private int _l__initialThreadId;

		//	public Stream fontUri;

		//	public Stream _3__fontUri;

		//	public OpenTypeFontReader <reader>5__1;

		//	public TrueTypeCollection <ttc>5__2;

		//	public OpenTypeFontSourceBase <fs>5__3;

		//	public IEnumerator<OpenTypeFontSourceBase> _7__wrap4;
		//}
	}
}
