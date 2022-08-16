using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers
{
	class PdfParserBase
	{
		public PdfParserBase(PdfContentManager contentManager, Stream stream)
		{
			this.Reader = new PdfReader(stream);
			this.ContentManager = contentManager;
		}

		internal PdfReader Reader { get; set; }

		protected PdfContentManager ContentManager { get; set; }

		public object ParseCurrentTokenToIndirectReference(int objectNo, int generationNo)
		{
			return new IndirectReferenceOld
			{
				GenerationNumber = generationNo,
				ObjectNumber = objectNo
			};
		}

		public PdfBoolOld ParseCurrentTokenToBool()
		{
			if (this.Reader.TokenType == Token.Boolean)
			{
				return new PdfBoolOld(this.ContentManager, this.Reader.Result == "true");
			}
			return null;
		}

		public PdfIntOld ParseCurrentTokenToInt()
		{
			if (this.Reader.TokenType == Token.Integer)
			{
				int value;
				int.TryParse(this.Reader.Result, out value);
				return new PdfIntOld(this.ContentManager, value);
			}
			return null;
		}

		public PdfRealOld ParseCurrentTokenToReal()
		{
			if (this.Reader.TokenType == Token.Real)
			{
				double val;
				double.TryParse(this.Reader.Result, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out val);
				return new PdfRealOld(this.ContentManager, val);
			}
			return null;
		}

		public PdfNameOld ParseCurrentTokenToName()
		{
			if (this.Reader.TokenType == Token.Name)
			{
				return new PdfNameOld(this.ContentManager, this.Reader.Result);
			}
			return null;
		}

		public PdfStringOld ParseCurrentTokenToString()
		{
			if (this.Reader.TokenType == Token.String)
			{
				return new PdfStringOld(this.ContentManager, this.Reader.BytesToken);
			}
			return null;
		}

		public PdfArrayOld ParseCurrentTokenToArray()
		{
			if (this.Reader.TokenType == Token.ArrayStart)
			{
				PdfArrayOld pdfArrayOld = new PdfArrayOld(this.ContentManager);
				object[] content = this.ReadObjectsToToken(Token.ArrayEnd);
				pdfArrayOld.Load(content);
				return pdfArrayOld;
			}
			return null;
		}

		public PdfDictionaryOld ParseCurrentTokenToDictionary()
		{
			if (this.Reader.TokenType == Token.DictionaryStart)
			{
				PdfDictionaryOld pdfDictionaryOld = new PdfDictionaryOld(this.ContentManager);
				object[] content = this.ReadObjectsToToken(Token.DictionaryEnd);
				pdfDictionaryOld.Load(content);
				return pdfDictionaryOld;
			}
			return null;
		}

		protected object[] ReadObjectsToToken(Token endToken)
		{
			Stack<object> stack = new Stack<object>();
			Token token;
			while ((token = this.Reader.ReadToken()) != endToken)
			{
				Token token2 = token;
				switch (token2)
				{
				case Token.Integer:
					stack.Push(this.ParseCurrentTokenToInt());
					break;
				case Token.Real:
					stack.Push(this.ParseCurrentTokenToReal());
					break;
				case Token.Name:
					stack.Push(this.ParseCurrentTokenToName());
					break;
				case Token.ArrayStart:
					stack.Push(this.ParseCurrentTokenToArray());
					break;
				case Token.ArrayEnd:
				case Token.Keyword:
					goto IL_F1;
				case Token.Unknown:
					break;
				case Token.String:
					stack.Push(this.ParseCurrentTokenToString());
					break;
				case Token.Boolean:
					stack.Push(this.ParseCurrentTokenToBool());
					break;
				case Token.DictionaryStart:
					stack.Push(this.ParseCurrentTokenToDictionary());
					break;
				case Token.Null:
					stack.Push(null);
					break;
				default:
				{
					if (token2 != Token.IndirectReference)
					{
						goto IL_F1;
					}
					int value = ((PdfIntOld)stack.Pop()).Value;
					int value2 = ((PdfIntOld)stack.Pop()).Value;
					stack.Push(this.ParseCurrentTokenToIndirectReference(value2, value));
					break;
				}
				}
				IL_F9:
				if (!this.Reader.EndOfFile)
				{
					continue;
				}
				break;
				IL_F1:
				this.ParseCurrentTokenOverride(token, stack);
				goto IL_F9;
			}
			int i = stack.Count - 1;
			object[] array = new object[i + 1];
			while (i >= 0)
			{
				array[i] = stack.Pop();
				i--;
			}
			return array;
		}

		protected int ReadInt()
		{
			if (this.Reader.ReadToken() == Token.Integer)
			{
				int result;
				int.TryParse(this.Reader.Result, out result);
				return result;
			}
			return -1;
		}

		protected virtual void ParseCurrentTokenOverride(Token token, Stack<object> stack)
		{
		}
	}
}
