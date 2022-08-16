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
		public PdfParserBase(global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.PdfContentManager contentManager, global::System.IO.Stream stream)
		{
			this.Reader = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers.PdfReader(stream);
			this.ContentManager = contentManager;
		}

		internal global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers.PdfReader Reader { get; private set; }

		private protected global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.PdfContentManager ContentManager { get;  set; }

		public object ParseCurrentTokenToIndirectReference(int objectNo, int generationNo)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.IndirectReferenceOld
			{
				GenerationNumber = generationNo,
				ObjectNumber = objectNo
			};
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfBoolOld ParseCurrentTokenToBool()
		{
			if (this.Reader.TokenType == global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Boolean)
			{
				return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfBoolOld(this.ContentManager, this.Reader.Result == "true");
			}
			return null;
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfIntOld ParseCurrentTokenToInt()
		{
			if (this.Reader.TokenType == global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Integer)
			{
				int value;
				int.TryParse(this.Reader.Result, out value);
				return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfIntOld(this.ContentManager, value);
			}
			return null;
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfRealOld ParseCurrentTokenToReal()
		{
			if (this.Reader.TokenType == global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Real)
			{
				double val;
				double.TryParse(this.Reader.Result, global::System.Globalization.NumberStyles.Any, global::System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out val);
				return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfRealOld(this.ContentManager, val);
			}
			return null;
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfNameOld ParseCurrentTokenToName()
		{
			if (this.Reader.TokenType == global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Name)
			{
				return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfNameOld(this.ContentManager, this.Reader.Result);
			}
			return null;
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfStringOld ParseCurrentTokenToString()
		{
			if (this.Reader.TokenType == global::Telerik.Windows.Documents.Core.PostScript.Data.Token.String)
			{
				return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfStringOld(this.ContentManager, this.Reader.BytesToken);
			}
			return null;
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfArrayOld ParseCurrentTokenToArray()
		{
			if (this.Reader.TokenType == global::Telerik.Windows.Documents.Core.PostScript.Data.Token.ArrayStart)
			{
				global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfArrayOld pdfArrayOld = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfArrayOld(this.ContentManager);
				object[] content = this.ReadObjectsToToken(global::Telerik.Windows.Documents.Core.PostScript.Data.Token.ArrayEnd);
				pdfArrayOld.Load(content);
				return pdfArrayOld;
			}
			return null;
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfDictionaryOld ParseCurrentTokenToDictionary()
		{
			if (this.Reader.TokenType == global::Telerik.Windows.Documents.Core.PostScript.Data.Token.DictionaryStart)
			{
				global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfDictionaryOld pdfDictionaryOld = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfDictionaryOld(this.ContentManager);
				object[] content = this.ReadObjectsToToken(global::Telerik.Windows.Documents.Core.PostScript.Data.Token.DictionaryEnd);
				pdfDictionaryOld.Load(content);
				return pdfDictionaryOld;
			}
			return null;
		}

		protected object[] ReadObjectsToToken(global::Telerik.Windows.Documents.Core.PostScript.Data.Token endToken)
		{
			global::System.Collections.Generic.Stack<object> stack = new global::System.Collections.Generic.Stack<object>();
			global::Telerik.Windows.Documents.Core.PostScript.Data.Token token;
			while ((token = this.Reader.ReadToken()) != endToken)
			{
				global::Telerik.Windows.Documents.Core.PostScript.Data.Token token2 = token;
				switch (token2)
				{
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Integer:
						stack.Push(this.ParseCurrentTokenToInt());
						break;
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Real:
						stack.Push(this.ParseCurrentTokenToReal());
						break;
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Name:
						stack.Push(this.ParseCurrentTokenToName());
						break;
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.ArrayStart:
						stack.Push(this.ParseCurrentTokenToArray());
						break;
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.ArrayEnd:
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Keyword:
						goto IL_F1;
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Unknown:
						break;
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.String:
						stack.Push(this.ParseCurrentTokenToString());
						break;
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Boolean:
						stack.Push(this.ParseCurrentTokenToBool());
						break;
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.DictionaryStart:
						stack.Push(this.ParseCurrentTokenToDictionary());
						break;
					case global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Null:
						stack.Push(null);
						break;
					default:
						{
							if (token2 != global::Telerik.Windows.Documents.Core.PostScript.Data.Token.IndirectReference)
							{
								goto IL_F1;
							}
							int value = ((global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfIntOld)stack.Pop()).Value;
							int value2 = ((global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfIntOld)stack.Pop()).Value;
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
			if (this.Reader.ReadToken() == global::Telerik.Windows.Documents.Core.PostScript.Data.Token.Integer)
			{
				int result;
				int.TryParse(this.Reader.Result, out result);
				return result;
			}
			return -1;
		}

		protected virtual void ParseCurrentTokenOverride(global::Telerik.Windows.Documents.Core.PostScript.Data.Token token, global::System.Collections.Generic.Stack<object> stack)
		{
		}
	}
}
