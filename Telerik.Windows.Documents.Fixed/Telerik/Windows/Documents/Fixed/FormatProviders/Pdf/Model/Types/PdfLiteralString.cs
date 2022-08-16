using System;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfLiteralString : PdfString
	{
		public PdfLiteralString()
		{
			this.stringType = StringType.Byte;
		}

		public PdfLiteralString(byte[] initialValue)
			: base(initialValue)
		{
			this.stringType = (PdfString.IsUnicode(base.Value) ? StringType.UTFEncodedTextString : StringType.Byte);
		}

		public PdfLiteralString(string initialValue, StringType stringType)
			: base(PdfLiteralString.GetBytes(initialValue, stringType))
		{
			this.stringType = stringType;
		}

		public StringType StringType
		{
			get
			{
				return this.stringType;
			}
		}

		public override StringObjectType StringObjectType
		{
			get
			{
				return StringObjectType.Literal;
			}
		}

		public override string ToString()
		{
			if (base.Value == null)
			{
				return string.Empty;
			}
			return PdfLiteralString.GetString(base.Value, this.StringType);
		}

		public string ToStringAs(StringType stringType)
		{
			return PdfLiteralString.GetString(base.Value, stringType);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			byte[] bytes = context.EncryptString(base.Value);
			string @string = PdfLiteralString.GetString(bytes, this.StringType);
			string arg = PdfLiteralString.EscapeSpecialCharacters(@string);
			writer.Write(string.Format("{0}{1}{2}", '(', arg, ')'));
		}

		static string GetString(byte[] bytes, StringType stringType)
		{
			switch (stringType)
			{
			case StringType.ASCII:
				return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			case StringType.Byte:
				return PdfString.GetPdfEncodedTextString(bytes);
			case StringType.UTFEncodedTextString:
				return PdfString.GetUtfEncodedTextString(bytes);
			default:
				throw new NotImplementedException(string.Format("{0} is not supported.", stringType.ToString()));
			}
		}

		static byte[] GetBytes(string value, StringType stringType)
		{
			switch (stringType)
			{
			case StringType.ASCII:
				return Encoding.UTF8.GetBytes(value);
			case StringType.Byte:
				return BytesHelper.ToByteArray(value);
			case StringType.UTFEncodedTextString:
				return BytesHelper.MergeBytes(new byte[] { 254, byte.MaxValue }, Encoding.BigEndianUnicode.GetBytes(value));
			default:
				throw new NotImplementedException(string.Format("{0} is not supported.", stringType.ToString()));
			}
		}

		static string EscapeSpecialCharacters(string text)
		{
			return Regex.Replace(text, "\\\\|\\)|\\(", new MatchEvaluator(PdfLiteralString.EscapeSpecialCharacterMatch));
		}

		static string EscapeSpecialCharacterMatch(Match m)
		{
			string value;
			if ((value = m.Value) != null)
			{
				if (value == "\\")
				{
					return "\\\\";
				}
				if (value == ")")
				{
					return "\\)";
				}
				if (value == "(")
				{
					return "\\(";
				}
			}
			return m.Value;
		}

		readonly StringType stringType;
	}
}
