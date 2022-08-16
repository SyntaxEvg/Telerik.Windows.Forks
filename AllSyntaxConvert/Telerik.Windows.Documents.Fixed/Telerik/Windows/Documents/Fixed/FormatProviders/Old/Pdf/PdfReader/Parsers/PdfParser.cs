using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers
{
	class PdfParser : global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers.PdfParserBase
	{
		public PdfParser(PdfContentManager contentManager, Stream stream)
			: base(contentManager, stream)
		{
		}

		public bool TryReadIndirectObject(long offset, bool isEncrypted, out IndirectObjectOld indirectObject)
		{
			if (offset == -1L)
			{
				indirectObject = null;
				return false;
			}
			indirectObject = new IndirectObjectOld();
			long position = base.Reader.Position;
			base.Reader.Seek(offset, SeekOrigin.Begin);
			indirectObject.ObjectNumber = base.ReadInt();
			indirectObject.GenerationNumber = base.ReadInt();
			base.Reader.ReadToken();
			object[] array = (isEncrypted ? this.ReadObjectsToToken(indirectObject, Token.IndirectObjectEnd) : base.ReadObjectsToToken(Token.IndirectObjectEnd));
			indirectObject.Value = array[0];
			base.Reader.Seek(position, SeekOrigin.Begin);
			return true;
		}

		public TrailerOld InitializeCrossReferences()
		{
			base.Reader.SeekToStartXRef();
			base.Reader.GoToNextLine(true);
			long num = this.ReadLong();
			if (num == -1L)
			{
				throw new NotSupportedException("Startxref keyword cannot be found");
			}
			return this.ReadCrossReferenceCollection(num);
		}

		public PdfDictionaryOld ParseCurrentTokenToDictionary(IndirectObjectOld containingObject)
		{
			if (base.Reader.TokenType == Token.DictionaryStart)
			{
				PdfDictionaryOld pdfDictionaryOld = new PdfDictionaryOld(base.ContentManager);
				object[] content = this.ReadObjectsToToken(containingObject, Token.DictionaryEnd);
				pdfDictionaryOld.Load(content);
				return pdfDictionaryOld;
			}
			return null;
		}

		protected override void ParseCurrentTokenOverride(Token token, Stack<object> stack)
		{
			if (token != Token.StreamStart)
			{
				return;
			}
			stack.Push(this.ParseCurrentTokenToStream(stack.Pop() as PdfDictionaryOld, null));
		}

		protected object[] ReadObjectsToToken(IndirectObjectOld containingObject, Token endToken)
		{
			Stack<object> stack = new Stack<object>();
			Token token;
			while (!base.Reader.EndOfFile && (token = base.Reader.ReadToken()) != endToken)
			{
				switch (token)
				{
				case Token.Integer:
					stack.Push(base.ParseCurrentTokenToInt());
					break;
				case Token.Real:
					stack.Push(base.ParseCurrentTokenToReal());
					break;
				case Token.Name:
					stack.Push(base.ParseCurrentTokenToName());
					break;
				case Token.ArrayStart:
					stack.Push(this.ParseCurrentTokenToArray(containingObject));
					break;
				case Token.String:
					stack.Push(this.ParseCurrentTokenToString(containingObject));
					break;
				case Token.Boolean:
					stack.Push(base.ParseCurrentTokenToBool());
					break;
				case Token.DictionaryStart:
					stack.Push(this.ParseCurrentTokenToDictionary(containingObject));
					break;
				case Token.Null:
					stack.Push(null);
					break;
				case Token.StreamStart:
					stack.Push(this.ParseCurrentTokenToStream(stack.Pop() as PdfDictionaryOld, containingObject));
					break;
				case Token.IndirectReference:
				{
					int value = ((PdfIntOld)stack.Pop()).Value;
					int value2 = ((PdfIntOld)stack.Pop()).Value;
					stack.Push(base.ParseCurrentTokenToIndirectReference(value2, value));
					break;
				}
				}
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

		long ReadLong()
		{
			if (base.Reader.ReadToken() == Token.Integer)
			{
				long result;
				long.TryParse(base.Reader.Result, out result);
				return result;
			}
			return -1L;
		}

		PdfDictionaryOld ReadDictionary()
		{
			base.Reader.ReadToken();
			return base.ParseCurrentTokenToDictionary();
		}

		PdfDataStream ParseCurrentTokenToStream(PdfDictionaryOld dict, IndirectObjectOld containingObject = null)
		{
			PdfDataStream pdfDataStream = new PdfDataStream(base.Reader, dict, containingObject);
			pdfDataStream.FixLengthAndPosition(base.Reader.Position);
			return pdfDataStream;
		}

		PdfStringOld ParseCurrentTokenToString(IndirectObjectOld containingObject)
		{
			if (base.Reader.TokenType == Token.String)
			{
				return new PdfStringOld(base.ContentManager, base.ContentManager.DecryptString(containingObject, base.Reader.BytesToken));
			}
			return null;
		}

		PdfArrayOld ParseCurrentTokenToArray(IndirectObjectOld containingObject)
		{
			if (base.Reader.TokenType == Token.ArrayStart)
			{
				PdfArrayOld pdfArrayOld = new PdfArrayOld(base.ContentManager);
				object[] content = this.ReadObjectsToToken(containingObject, Token.ArrayEnd);
				pdfArrayOld.Load(content);
				return pdfArrayOld;
			}
			return null;
		}

		CrossReferenceEntryOld ReadCrossReferenceEntry()
		{
			string text = base.Reader.ReadLine();
			while (string.IsNullOrEmpty(text))
			{
				text = base.Reader.ReadLine();
			}
			string[] array = text.Split(new char[] { ' ' });
			int num;
			int.TryParse(array[0], out num);
			int field;
			int.TryParse(array[1], out field);
			return new CrossReferenceEntryOld
			{
				Field2 = (long)num,
				Field3 = field,
				Type = ((array[2] == "f") ? CrossReferenceEntryTypeOld.Free : CrossReferenceEntryTypeOld.Used)
			};
		}

		TrailerOld InitTrailerAndCrossReferenceTable(long offset)
		{
			base.Reader.Seek(offset, SeekOrigin.Begin);
			Token token;
			if ((token = base.Reader.ReadToken()) == Token.XRef)
			{
				token = this.ParseCrossReferenceTable();
			}
			if (token == Token.Trailer)
			{
				PdfDictionaryOld dictionary = this.ReadDictionary();
				TrailerOld trailerOld = new TrailerOld(base.ContentManager);
				trailerOld.Load(dictionary);
				if (trailerOld.XRefStm != null)
				{
					this.InitTrailerAndCrossReferenceStream((long)trailerOld.XRefStm.Value);
				}
				if (trailerOld.Prev != null)
				{
					this.ReadCrossReferenceCollection((long)trailerOld.Prev.Value);
				}
				return trailerOld;
			}
			return null;
		}

		Token ParseCrossReferenceTable()
		{
			Token result;
			while ((result = base.Reader.ReadToken()) == Token.Integer)
			{
				int value = base.ParseCurrentTokenToInt().Value;
				int num = base.ReadInt();
				base.Reader.GoToNextLine(true);
				for (int i = 0; i < num; i++)
				{
					base.ContentManager.CrossReferences.AddCrossReferenceEntry(value + i, this.ReadCrossReferenceEntry());
				}
			}
			return result;
		}

		TrailerOld InitTrailerAndCrossReferenceStream(long offset)
		{
			CrossReferenceStreamOld crossReferenceStreamOld;
			byte[] data;
			this.ReadCrossReferenceStream(offset, out crossReferenceStreamOld, out data);
			crossReferenceStreamOld.Append(crossReferenceStreamOld, data);
			TrailerOld trailerOld = new TrailerOld(base.ContentManager);
			trailerOld.CopyPropertiesFrom(crossReferenceStreamOld);
			if (crossReferenceStreamOld.Prev != null)
			{
				this.ReadCrossReferenceCollection((long)crossReferenceStreamOld.Prev.Value);
			}
			return trailerOld;
		}

		void ReadCrossReferenceStream(long offset, out CrossReferenceStreamOld stream, out byte[] data)
		{
			stream = null;
			data = null;
			IndirectObjectOld indirectObjectOld;
			if (this.TryReadIndirectObject(offset, false, out indirectObjectOld))
			{
				stream = new CrossReferenceStreamOld(base.ContentManager);
				PdfDataStream pdfDataStream = (PdfDataStream)indirectObjectOld.Value;
				stream.Load(indirectObjectOld);
				data = pdfDataStream.ReadData(base.ContentManager);
			}
		}

		TrailerOld ReadCrossReferenceCollection(long offset)
		{
			base.Reader.Seek(offset, SeekOrigin.Begin);
			Token token = base.Reader.ReadToken();
			TrailerOld result;
			if (token == Token.XRef)
			{
				result = this.InitTrailerAndCrossReferenceTable(offset);
			}
			else
			{
				result = this.InitTrailerAndCrossReferenceStream(offset);
			}
			return result;
		}
	}
}
