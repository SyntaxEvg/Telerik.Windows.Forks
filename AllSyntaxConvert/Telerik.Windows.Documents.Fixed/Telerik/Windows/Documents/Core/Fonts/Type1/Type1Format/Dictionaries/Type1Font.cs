using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Converters;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Dictionaries
{
	class Type1Font : PostScriptObject, IBuildCharHolder
	{
		public Type1Font()
		{
			this.buildChar = new BuildChar(this);
			this.glyphOutlines = new Dictionary<string, GlyphData>();
			this.fontMatrix = base.CreateProperty<PostScriptArray>(new PropertyDescriptor
			{
				Name = "FontMatrix"
			}, PostScriptArray.MatrixIdentity);
			this.fontInfo = base.CreateProperty<FontInfo>(new PropertyDescriptor
			{
				Name = "FontInfo"
			}, Type1Converters.PostScriptObjectConverter);
			this.encoding = base.CreateProperty<PostScriptArray>(new PropertyDescriptor
			{
				Name = "Encoding"
			}, Type1Converters.EncodingConverter, PredefinedEncoding.StandardEncoding.ToArray());
			this.charStrings = base.CreateProperty<PostScriptDictionary>(new PropertyDescriptor
			{
				Name = "CharStrings"
			});
			this.priv = base.CreateProperty<Private>(new PropertyDescriptor
			{
				Name = "Private"
			}, Type1Converters.PostScriptObjectConverter);
			this.fontBBox = base.CreateProperty<PostScriptArray>(new PropertyDescriptor
			{
				Name = "FontBBox"
			});
		}

		public PostScriptArray FontMatrix
		{
			get
			{
				return this.fontMatrix.GetValue();
			}
		}

		public PostScriptArray FontBBox
		{
			get
			{
				return this.fontBBox.GetValue();
			}
		}

		public FontInfo FontInfo
		{
			get
			{
				return this.fontInfo.GetValue();
			}
		}

		public PostScriptArray Encoding
		{
			get
			{
				return this.encoding.GetValue();
			}
		}

		public PostScriptDictionary CharStrings
		{
			get
			{
				return this.charStrings.GetValue();
			}
		}

		public Private Private
		{
			get
			{
				return this.priv.GetValue();
			}
		}

		public ushort GetAdvancedWidth(string glyphName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(glyphName, "glyphName");
			GlyphData glyphData = this.GetGlyphData(glyphName);
			return glyphData.AdvancedWidth;
		}

		public void GetGlyphOutlines(Glyph glyph, double fontSize)
		{
			Guard.ThrowExceptionIfNull<Glyph>(glyph, "glyph");
			GlyphData glyphData = this.GetGlyphData(glyph.Name);
			GlyphOutlinesCollection glyphOutlinesCollection = glyphData.Oultlines.Clone();
			Matrix transformMatrix = this.FontMatrix.ToMatrix();
			transformMatrix.ScaleAppend(fontSize, -fontSize, 0.0, 0.0);
			glyphOutlinesCollection.Transform(transformMatrix);
			glyph.Outlines = glyphOutlinesCollection;
		}

		public GlyphData GetGlyphData(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			GlyphData glyphData;
			if (!this.glyphOutlines.TryGetValue(name, out glyphData))
			{
				glyphData = this.ReadGlyphData(name);
				this.glyphOutlines[name] = glyphData;
			}
			return glyphData;
		}

		public bool TryGetCharCode(ushort unicode, out ushort charId)
		{
			charId = 0;
			string name = AdobeGlyphList.GetName((char)unicode);
			if (this.Encoding.Contains(name))
			{
				charId = (ushort)this.Encoding.IndexOf(name);
				return true;
			}
			return false;
		}

		public byte[] GetSubr(int index)
		{
			return this.Private.GetSubr(index);
		}

		public byte[] GetGlobalSubr(int index)
		{
			throw new NotImplementedException();
		}

		internal string GetGlyphName(ushort cid)
		{
			if (this.Encoding == null)
			{
				return ".notdef";
			}
			string elementAs = this.Encoding.GetElementAs<string>((int)cid);
			return elementAs ?? ".notdef";
		}

		GlyphData ReadGlyphData(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			PostScriptString elementAs = this.CharStrings.GetElementAs<PostScriptString>(name);
			this.buildChar.Execute(elementAs.ToByteArray());
			GlyphOutlinesCollection outlines = this.buildChar.GlyphOutlines;
			int? width = this.buildChar.Width;
			return new GlyphData(outlines, (width != null) ? new ushort?((ushort)width.GetValueOrDefault()) : null);
		}

		readonly BuildChar buildChar;

		readonly Dictionary<string, GlyphData> glyphOutlines;

		readonly Property<PostScriptArray> fontMatrix;

		readonly Property<FontInfo> fontInfo;

		readonly Property<PostScriptArray> encoding;

		readonly Property<PostScriptDictionary> charStrings;

		readonly Property<Private> priv;

		readonly Property<PostScriptArray> fontBBox;
	}
}
