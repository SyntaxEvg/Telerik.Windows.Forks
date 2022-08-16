using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class RichTextRunSharedString : SharedString
	{
		public RichTextRunSharedString(string text)
		{
			this.text = text;
		}

		public string Text
		{
			get
			{
				return this.text;
			}
		}

		public override SharedStringType Type
		{
			get
			{
				return SharedStringType.RichText;
			}
		}

		readonly string text;
	}
}
