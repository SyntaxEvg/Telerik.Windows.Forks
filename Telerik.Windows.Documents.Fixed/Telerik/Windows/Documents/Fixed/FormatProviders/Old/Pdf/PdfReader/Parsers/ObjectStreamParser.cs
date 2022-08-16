using System;
using System.IO;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers
{
	class ObjectStreamParser : PdfParserBase
	{
		public ObjectStreamParser(PdfContentManager contentManager, Stream stream)
			: base(contentManager, stream)
		{
		}

		public PdfObjectOld ReadPdfObject(long offset)
		{
			base.Reader.Seek(offset, SeekOrigin.Begin);
			switch (base.Reader.ReadToken())
			{
			case Token.Integer:
				return base.ParseCurrentTokenToInt();
			case Token.Real:
				return base.ParseCurrentTokenToReal();
			case Token.Name:
				return base.ParseCurrentTokenToName();
			case Token.ArrayStart:
				return base.ParseCurrentTokenToArray();
			case Token.String:
				return base.ParseCurrentTokenToString();
			case Token.Boolean:
				return base.ParseCurrentTokenToBool();
			case Token.DictionaryStart:
				return base.ParseCurrentTokenToDictionary();
			}
			return null;
		}

		public int[] ReadOffsets(int n, int first)
		{
			int[] array = new int[n];
			for (int i = 0; i < n; i++)
			{
				int num = base.ReadInt();
				num = base.ReadInt();
				array[i] = num + first;
			}
			return array;
		}
	}
}
