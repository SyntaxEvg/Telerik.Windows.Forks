using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class HeaderFooterSection
	{
		internal HeaderFooterSection(Action updateOnChange)
		{
			Guard.ThrowExceptionIfNull<Action>(updateOnChange, "updateOnChange");
			this.updateOnChange = updateOnChange;
			this.text = string.Empty;
		}

		internal HeaderFooterSection(HeaderFooterSection other, Action updateOnChange)
		{
			Guard.ThrowExceptionIfNull<Action>(updateOnChange, "updateOnChange");
			this.updateOnChange = updateOnChange;
			this.text = other.text;
		}

		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (this.text != value)
				{
					this.text = value;
					this.updateOnChange();
				}
			}
		}

		internal bool IsEmpty
		{
			get
			{
				HeaderFooterSectionTextSanitizer headerFooterSectionTextSanitizer = new HeaderFooterSectionTextSanitizer();
				string value = headerFooterSectionTextSanitizer.Sanitize(this.text);
				return string.IsNullOrEmpty(value);
			}
		}

		internal void CopyFrom(HeaderFooterSection fromHeaderFooterSection)
		{
			this.Text = fromHeaderFooterSection.Text;
		}

		internal void Clear()
		{
			this.Text = string.Empty;
		}

		public override bool Equals(object obj)
		{
			HeaderFooterSection headerFooterSection = obj as HeaderFooterSection;
			return headerFooterSection != null && this.Text.Equals(headerFooterSection.Text);
		}

		public override int GetHashCode()
		{
			return this.Text.GetHashCode();
		}

		readonly Action updateOnChange;

		string text;
	}
}
