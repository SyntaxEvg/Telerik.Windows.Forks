using System;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Core.PostScript;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers
{
	class PdfReader : PdfReaderBase
	{
		public PdfReader(Stream stream)
			: base(stream)
		{
		}

		Token ReadKeyword()
		{
			string text = PostScriptReaderHelper.ReadKeyword(this);
			base.SetToken(text);
			string key;
			switch (key = text)
			{
			case "startxref":
				return base.TokenType = Token.StartXRef;
			case "true":
			case "false":
				return base.TokenType = Token.Boolean;
			case "xref":
				return base.TokenType = Token.XRef;
			case "stream":
				return base.TokenType = Token.StreamStart;
			case "endstream":
				return base.TokenType = Token.StreamEnd;
			case "null":
				return base.TokenType = Token.Null;
			case "obj":
				return base.TokenType = Token.IndirectObjectStart;
			case "endobj":
				return base.TokenType = Token.IndirectObjectEnd;
			case "R":
				return base.TokenType = Token.IndirectReference;
			case "trailer":
				return base.TokenType = Token.Trailer;
			}
			return base.TokenType = Token.Unknown;
		}

		public bool SeekToStartXRef()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder("startxref");
			long num = System.Math.Min((long)PdfReader.BufferSize, base.Stream.Length);
			base.Stream.Seek(-num, SeekOrigin.End);
			long num2 = -1L;
			byte[] array = new byte[PdfReader.BufferSize];
			int num3 = base.Stream.Read(array, 0, PdfReader.BufferSize);
			for (int i = 0; i < num3; i++)
			{
				stringBuilder.Append((char)array[i]);
				if (stringBuilder2.Length < stringBuilder.Length)
				{
					stringBuilder.Remove(0, 1);
				}
				if (stringBuilder.Equals(stringBuilder2))
				{
					num2 = base.Stream.Position - (long)(num3 - i - 1) - (long)stringBuilder.Length;
				}
			}
			if (num2 == -1L)
			{
				return false;
			}
			base.Seek(num2, SeekOrigin.Begin);
			return true;
		}

		public byte[] ReadStream(int length)
		{
			byte[] array = new byte[length];
			base.Stream.Seek(-2L, SeekOrigin.Current);
			base.Stream.Read(array, 0, length);
			base.ReadBufferedChars();
			return array;
		}

		public override Token ReadToken()
		{
			base.SkipUnusedCharacters();
			char c = (char)base.Peek(0);
			if (c <= '/')
			{
				if (c == '(')
				{
					return base.ReadLiteralString();
				}
				if (c == '/')
				{
					return base.TokenType = base.ReadName();
				}
			}
			else
			{
				switch (c)
				{
				case '<':
					if (base.Peek(1) == 60)
					{
						base.Read(true);
						base.Read(true);
						return base.TokenType = Token.DictionaryStart;
					}
					return base.ReadHexadecimalString();
				case '=':
					break;
				case '>':
					if (base.Peek(1) == 62)
					{
						base.Read(true);
						base.Read(true);
						return base.TokenType = Token.DictionaryEnd;
					}
					break;
				default:
					switch (c)
					{
					case '[':
						base.Read(true);
						return base.TokenType = Token.ArrayStart;
					case ']':
						base.Read(true);
						return base.TokenType = Token.ArrayEnd;
					}
					break;
				}
			}
			if (Characters.IsValidNumberChar(this))
			{
				return base.ReadNumber();
			}
			if (Characters.IsLetter((int)base.Peek(0)))
			{
				return this.ReadKeyword();
			}
			base.Read(true);
			return base.TokenType = Token.Unknown;
		}

		public static readonly int BufferSize = 1024;
	}
}
