using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;
using Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class Top : Dict, IBuildCharHolder
	{
		public static OperatorDescriptor FamilyNameOperator { get; set; }

		public static OperatorDescriptor WeightOperator { get; set; }

		public static OperatorDescriptor EncodingOperator { get; set; }

		public static OperatorDescriptor CharStringsOperator { get; set; }

		public static OperatorDescriptor ItalicAngleOperator { get; set; }

		public static OperatorDescriptor UnderlinePositionOperator { get; set; }

		public static OperatorDescriptor UnderlineThicknessOperator { get; set; }

		public static OperatorDescriptor CharstringTypeOperator { get; set; }

		public static OperatorDescriptor CharsetOperator { get; set; }

		public static OperatorDescriptor FontMatrixOperator { get; set; } = new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 7 }), new PostScriptArray(new object[] { 0.001, 0, 0, 0.001, 0, 0 }));

		public static OperatorDescriptor PrivateOperator { get; set; }

		public static OperatorDescriptor ROSOperator { get; set; }

		public static OperatorDescriptor FDArrayOperator { get; set; }

		public static OperatorDescriptor FDSelectOperator { get; set; }

		static Top()
		{
			Top.FamilyNameOperator = new OperatorDescriptor(3);
			Top.WeightOperator = new OperatorDescriptor(4);
			Top.ItalicAngleOperator = new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 2 }), 0);
			Top.UnderlinePositionOperator = new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 3 }), -100);
			Top.UnderlineThicknessOperator = new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 3 }), 50);
			Top.CharstringTypeOperator = new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 6 }), 2);
			Top.CharsetOperator = new OperatorDescriptor(15, 0);
			Top.EncodingOperator = new OperatorDescriptor(16, 0);
			Top.CharStringsOperator = new OperatorDescriptor(17);
			Top.PrivateOperator = new OperatorDescriptor(18);
			Top.ROSOperator = new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 30 }));
			Top.FDArrayOperator = new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 36 }));
			Top.FDSelectOperator = new OperatorDescriptor(Helper.CreateByteArray(new byte[] { 12, 37 }));
		}

		public Top(ICFFFontFile file, long offset, int length)
			: base(file, offset, length)
		{
		}

		public double UnderlinePosition
		{
			get
			{
				if (this.underlinePosition == null)
				{
					this.underlinePosition = new double?(base.GetNumber(Top.UnderlinePositionOperator));
				}
				return this.underlinePosition.Value;
			}
		}

		public double UnderlineThickness
		{
			get
			{
				if (this.underlineThickness == null)
				{
					this.underlineThickness = new double?(base.GetNumber(Top.UnderlineThicknessOperator));
				}
				return this.underlineThickness.Value;
			}
		}

		public int CharstringType
		{
			get
			{
				if (this.charstringType == null)
				{
					this.charstringType = new int?(base.GetInt(Top.CharstringTypeOperator));
				}
				return this.charstringType.Value;
			}
		}

		public IEncoding Encoding
		{
			get
			{
				if (this.encoding == null)
				{
					this.ReadEncoding();
				}
				return this.encoding;
			}
		}

		public Charset Charset
		{
			get
			{
				if (this.charset == null)
				{
					this.ReadCharset();
				}
				return this.charset;
			}
		}

		public CharString CharString
		{
			get
			{
				if (this.charString == null)
				{
					this.ReadCharString();
				}
				return this.charString;
			}
		}

		public string FamilyName
		{
			get
			{
				if (this.familyName == null)
				{
					this.familyName = base.File.ReadString((ushort)base.GetInt(Top.FamilyNameOperator));
				}
				return this.familyName;
			}
		}

		public Matrix FontMatrix
		{
			get
			{
				if (this.fontMatrix == null)
				{
					this.fontMatrix = new Matrix?(base.GetArray(Top.FontMatrixOperator).ToMatrix());
				}
				return this.fontMatrix.Value;
			}
		}

		public Private Private
		{
			get
			{
				if (this.priv == null)
				{
					this.ReadPrivate();
				}
				return this.priv;
			}
		}

		public int DefaultWidthX
		{
			get
			{
				if (this.Private != null)
				{
					return this.Private.DefaultWidthX;
				}
				return 0;
			}
		}

		public int NominalWidthX
		{
			get
			{
				if (this.Private != null)
				{
					return this.Private.NominalWidthX;
				}
				return 0;
			}
		}

		public bool UsesCIDFontOperators
		{
			get
			{
				if (this.usesCIDFontOperators == null)
				{
					this.usesCIDFontOperators = new bool?(base.Data.ContainsKey(Top.ROSOperator));
				}
				return this.usesCIDFontOperators.Value;
			}
		}

		public byte[] GetSubr(int index)
		{
			return this.Private.Subrs[index];
		}

		public byte[] GetGlobalSubr(int index)
		{
			return base.File.GlobalSubrs[index];
		}

		public GlyphData GetGlyphData(string name)
		{
			ushort index;
			this.TryGetGlyphIdByCharacterName(name, out index);
			return this.CharString[index];
		}

		public bool TryGetGlyphIdByCharacterName(string name, out ushort glyphId)
		{
			return this.Charset.TryGetGlyphIdByCharacterName(name, out glyphId);
		}

		public bool TryGetGlyphIdByCharacterIdentifier(ushort cid, out ushort glyphId)
		{
			return this.Charset.TryGetGlyphIdByCharacterIdentifier(cid, out glyphId);
		}

		internal string GetGlyphName(ushort cid)
		{
			return this.Encoding.GetGlyphName(base.File, cid);
		}

		public ushort GetAdvancedWidth(ushort glyphId)
		{
			return (ushort)this.CharString.GetAdvancedWidth(glyphId, this.DefaultWidthX, this.NominalWidthX);
		}

		public void GetGlyphOutlines(Glyph glyph, double fontSize)
		{
			this.CharString.GetGlyphOutlines(glyph, fontSize);
		}

		void ReadEncoding()
		{
			int @int = base.GetInt(Top.EncodingOperator);
			if (PredefinedEncoding.IsPredefinedEncoding(@int))
			{
				this.encoding = PredefinedEncoding.GetPredefinedEncoding(@int);
				return;
			}
			Encoding table = new Encoding(base.File, this.Charset, (long)@int);
			base.File.ReadTable(table);
			this.encoding = table;
		}

		void ReadPrivate()
		{
			OperandsCollection operandsCollection;
			if (base.TryGetOperands(Top.PrivateOperator, out operandsCollection))
			{
				this.priv = new Private(this, (long)operandsCollection.GetLastAsInt(), operandsCollection.GetLastAsInt());
				base.File.ReadTable(this.priv);
			}
		}

		void ReadCharset()
		{
			int @int = base.GetInt(Top.CharsetOperator);
			if (PredefinedCharset.IsPredefinedCharset(@int))
			{
				this.charset = new Charset(base.File, PredefinedCharset.GetPredefinedCodes(@int));
				return;
			}
			this.charset = new Charset(base.File, (long)@int, (int)this.CharString.Count);
			base.File.ReadTable(this.charset);
		}

		void ReadCharString()
		{
			this.charString = new CharString(this, (long)base.GetInt(Top.CharStringsOperator));
			base.File.ReadTable(this.charString);
		}

		IEncoding encoding;

		CharString charString;

		Charset charset;

		string familyName;

		Matrix? fontMatrix;

		Private priv;

		int? charstringType;

		bool? usesCIDFontOperators;

		double? underlinePosition;

		double? underlineThickness;
	}
}
