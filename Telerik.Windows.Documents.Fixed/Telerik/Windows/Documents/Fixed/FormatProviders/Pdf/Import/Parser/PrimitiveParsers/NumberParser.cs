using System;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PrimitiveParsers
{
	class NumberParser
	{
		public NumberParser(PostScriptReader parser)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(parser, "parser");
			this.parser = parser;
			this.token = new StringBuilder();
			this.isDouble = false;
		}

		public void Start(char sign)
		{
			this.token.Append(sign);
		}

		public virtual void Append(PostScriptReaderArgs args)
		{
			char @byte = (char)args.Byte;
			this.token.Append(@byte);
			this.isDouble |= PdfHelper.CultureInfo.IsDecimalSeparator(@byte);
		}

		public void Complete()
		{
			if (this.isDouble)
			{
				double initialValue;
				PdfHelper.CultureInfo.TryParseDouble(this.token.ToString(), out initialValue);
				this.parser.PushToken(new PdfReal(initialValue));
			}
			else
			{
				int defaultValue;
				int.TryParse(this.token.ToString(), out defaultValue);
				this.parser.PushToken(new PdfInt(defaultValue));
			}
			this.token.Clear();
			this.isDouble = false;
		}

		public bool IsNumber(PostScriptReaderArgs args)
		{
			return char.IsDigit((char)args.Byte) || (!this.isDouble && PdfHelper.CultureInfo.IsDecimalSeparator((char)args.Byte));
		}

		readonly PostScriptReader parser;

		readonly StringBuilder token;

		bool isDouble;
	}
}
