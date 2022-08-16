﻿using System;
using System.IO;
using Telerik.Windows.Documents.Core.PostScript;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers
{
	class CMapStreamReader : PdfReaderBase
	{
		public CMapStreamReader(byte[] data)
			: base(new MemoryStream(data))
		{
		}

		public override Token ReadToken()
		{
			PostScriptReaderHelper.SkipUnusedCharacters(this);
			if (base.EndOfFile)
			{
				return base.TokenType = Token.EndOfFile;
			}
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
				return this.ReadKeywordOrOperator();
			}
			base.Read(true);
			return base.TokenType = Token.Unknown;
		}

		Token ReadKeywordOrOperator()
		{
			string text = PostScriptReaderHelper.ReadKeyword(this);
			base.SetToken(text);
			string a;
			if ((a = text) != null)
			{
				if (a == "true" || a == "false")
				{
					return base.TokenType = Token.Boolean;
				}
				if (a == "null")
				{
					return base.TokenType = Token.Null;
				}
			}
			if (CMapOperators.IsOperator(text))
			{
				return base.TokenType = Token.Operator;
			}
			return base.TokenType = Token.Unknown;
		}
	}
}
