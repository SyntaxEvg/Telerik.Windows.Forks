using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers
{
	class CMapStreamParser
	{
		public CMapStreamParser(PdfContentManager contentManager, CMapStream cmap)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<CMapStream>(cmap, "cmap");
			this.contentManager = contentManager;
			this.cmap = cmap;
			this.reader = new CMapStreamReader(this.ContentManager.ReadData(this.cmap.Reference));
		}

		public CMapStreamReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public PdfContentManager ContentManager
		{
			get
			{
				return this.contentManager;
			}
		}

		public CMapOld ParseCMap()
		{
			this.mappings = new CMapOld();
			for (;;)
			{
				object[] pars = this.ReadObjectsToToken(Token.Operator);
				if (this.Reader.TokenType == Token.EndOfFile)
				{
					break;
				}
				this.InvokeOperator(this.Reader.Result, pars);
			}
			if (this.cmap.UseCMap != null)
			{
				CMapStreamParser cmapStreamParser = new CMapStreamParser(this.ContentManager, this.cmap.UseCMap);
				this.mappings.UseCMap = cmapStreamParser.ParseCMap();
			}
			return this.mappings;
		}

		object[] ReadObjectsToToken(Token endToken)
		{
			Stack<object> stack = new Stack<object>();
			Token token;
			while ((token = this.Reader.ReadToken()) != endToken)
			{
				switch (token)
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
				case Token.EndOfFile:
					return new object[0];
				}
			}
			if (stack.Count == 0)
			{
				return null;
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

		void BeginCodeSpaceRange(int count)
		{
			for (int i = 0; i < count; i++)
			{
				CharCodeOld begin = this.ReadCharCode();
				CharCodeOld end = this.ReadCharCode();
				this.mappings.AddCodeRange(begin, end);
			}
		}

		void BeginBFChar(int count)
		{
			for (int i = 0; i < count; i++)
			{
				CharCodeOld cid = this.ReadCharCode();
				Token token = this.Reader.ReadToken();
				if (token == Token.String)
				{
					PdfStringOld pdfStringOld = this.ParseCurrentTokenToString();
					this.mappings.AddCIDtoUnicodeMapping(cid, pdfStringOld.ToUnicodeString());
				}
				else if (token == Token.Integer)
				{
					this.mappings.AddCIDtoUnicodeMapping(cid, BytesHelperOld.GetUnicodeChar(this.ParseCurrentTokenToInt()).ToString());
				}
			}
			this.Reader.ReadToken();
		}

		void BeginBFRange(int count)
		{
			for (int i = 0; i < count; i++)
			{
				CharCodeOld begin = this.ReadCharCode();
				CharCodeOld end = this.ReadCharCode();
				Token token = this.Reader.ReadToken();
				if (token == Token.String)
				{
					PdfStringOld unicode = this.ParseCurrentTokenToString();
					this.MapRange(begin, end, unicode);
				}
				else if (token == Token.Integer)
				{
					this.MapRange(begin, end, this.ParseCurrentTokenToInt());
				}
				else if (token == Token.ArrayStart)
				{
					PdfArrayOld unicodes = this.ParseCurrentTokenToArray();
					this.MapRange(begin, end, unicodes);
				}
			}
			this.Reader.ReadToken();
		}

		void BeginCIDChar(int count)
		{
			for (int i = 0; i < count; i++)
			{
				CharCodeOld icc = this.ReadCharCode();
				Token token = this.Reader.ReadToken();
				if (token == Token.Integer)
				{
					this.mappings.AddICCtoCIDMapping(icc, this.ParseCurrentTokenToInt());
				}
			}
			this.Reader.ReadToken();
		}

		void BeginCIDRange(int count)
		{
			for (int i = 0; i < count; i++)
			{
				CharCodeOld begin = this.ReadCharCode();
				CharCodeOld end = this.ReadCharCode();
				Token token = this.Reader.ReadToken();
				if (token == Token.Integer)
				{
					this.MapCIDRange(begin, end, this.ParseCurrentTokenToInt());
				}
			}
			this.Reader.ReadToken();
		}

		void BeginNotDefChar(int count)
		{
			for (int i = 0; i < count; i++)
			{
				CharCodeOld charCodeOld = this.ReadCharCode();
				Token token = this.Reader.ReadToken();
				if (token == Token.Integer)
				{
					this.mappings.AddNotDefRange(charCodeOld, charCodeOld, this.ParseCurrentTokenToInt());
				}
			}
			this.Reader.ReadToken();
		}

		void BeginNotDefRange(int count)
		{
			for (int i = 0; i < count; i++)
			{
				CharCodeOld begin = this.ReadCharCode();
				CharCodeOld end = this.ReadCharCode();
				Token token = this.Reader.ReadToken();
				if (token == Token.Integer)
				{
					this.mappings.AddNotDefRange(begin, end, this.ParseCurrentTokenToInt());
				}
			}
			this.Reader.ReadToken();
		}

		void MapCIDRange(CharCodeOld begin, CharCodeOld end, int cid)
		{
			CharCodeOld charCodeOld = begin;
			while (charCodeOld < end)
			{
				this.mappings.AddICCtoCIDMapping(charCodeOld, cid);
				charCodeOld = ++charCodeOld;
				cid++;
			}
			this.mappings.AddICCtoCIDMapping(end, cid++);
		}

		void MapRange(CharCodeOld begin, CharCodeOld end, int unicode)
		{
			CharCodeOld charCodeOld = begin;
			while (charCodeOld < end)
			{
				this.mappings.AddCIDtoUnicodeMapping(charCodeOld, BytesHelperOld.GetUnicodeChar(unicode).ToString());
				charCodeOld = ++charCodeOld;
				unicode++;
			}
			this.mappings.AddCIDtoUnicodeMapping(end, BytesHelperOld.GetUnicodeChar(unicode++).ToString());
		}

		void MapRange(CharCodeOld begin, CharCodeOld end, PdfStringOld unicode)
		{
			CharCodeOld charCodeOld = begin;
			while (charCodeOld < end)
			{
				this.mappings.AddCIDtoUnicodeMapping(charCodeOld, unicode.ToUnicodeString());
				unicode.Increment();
				charCodeOld = ++charCodeOld;
			}
			this.mappings.AddCIDtoUnicodeMapping(end, unicode.ToUnicodeString());
		}

		void MapRange(CharCodeOld begin, CharCodeOld end, PdfArrayOld unicodes)
		{
			int num = 0;
			CharCodeOld charCodeOld = begin;
			PdfStringOld element;
			while (charCodeOld < end)
			{
				element = unicodes.GetElement<PdfStringOld>(num++);
				this.mappings.AddCIDtoUnicodeMapping(charCodeOld, element.ToUnicodeString());
				charCodeOld = ++charCodeOld;
			}
			element = unicodes.GetElement<PdfStringOld>(num++);
			this.mappings.AddCIDtoUnicodeMapping(end, element.ToUnicodeString());
		}

		void InvokeOperator(string op, object[] pars)
		{
			int num = pars.Length;
			switch (op)
			{
			case "begincodespacerange":
				this.BeginCodeSpaceRange((int)pars[num - 1]);
				return;
			case "beginbfchar":
				this.BeginBFChar((int)pars[num - 1]);
				return;
			case "beginbfrange":
				this.BeginBFRange((int)pars[num - 1]);
				return;
			case "begincidchar":
				this.BeginCIDChar((int)pars[num - 1]);
				return;
			case "begincidrange":
				this.BeginCIDRange((int)pars[num - 1]);
				return;
			case "beginnotdefchar,":
				this.BeginNotDefChar((int)pars[num - 1]);
				return;
			case "beginnotdefrange,":
				this.BeginNotDefRange((int)pars[num - 1]);
				break;

				return;
			}
		}

		bool ParseCurrentTokenToBool()
		{
			return this.Reader.Result == "true";
		}

		int ParseCurrentTokenToInt()
		{
			int result;
			int.TryParse(this.Reader.Result, out result);
			return result;
		}

		double ParseCurrentTokenToReal()
		{
			double result;
			double.TryParse(this.Reader.Result, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out result);
			return result;
		}

		PdfStringOld ParseCurrentTokenToString()
		{
			if (this.Reader.TokenType == Token.String)
			{
				return new PdfStringOld(this.ContentManager, this.Reader.BytesToken);
			}
			return null;
		}

		PdfNameOld ParseCurrentTokenToName()
		{
			if (this.Reader.TokenType == Token.Name)
			{
				return new PdfNameOld(this.ContentManager, this.Reader.Result);
			}
			return null;
		}

		PdfArrayOld ParseCurrentTokenToArray()
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

		PdfDictionaryOld ParseCurrentTokenToDictionary()
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

		CharCodeOld ReadCharCode()
		{
			this.Reader.ReadToken();
			return new CharCodeOld(this.Reader.BytesToken);
		}

		readonly CMapStream cmap;

		readonly PdfContentManager contentManager;

		readonly CMapStreamReader reader;

		CMapOld mappings;
	}
}
