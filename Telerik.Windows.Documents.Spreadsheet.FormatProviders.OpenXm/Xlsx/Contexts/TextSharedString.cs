using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class TextSharedString : SharedString
	{
		public TextSharedString(string text)
		{
			Guard.ThrowExceptionIfNull<string>(text, "text");
			this.text = text;
		}

		public override SharedStringType Type
		{
			get
			{
				return SharedStringType.Text;
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
		}

		public override int GetHashCode()
		{
			int num = 17;
			return num * 23 + ((this.text != null) ? this.text.GetHashCode() : 0);
		}

		public override bool Equals(object obj)
		{
			TextSharedString textSharedString = obj as TextSharedString;
			return obj != null && this.Text == textSharedString.Text;
		}

		readonly string text;
	}
}
