using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	class FormatDescriptorItem
	{
		internal string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		public bool IsTransparent
		{
			get
			{
				return this.isTransparent;
			}
		}

		public bool ShouldExpand
		{
			get
			{
				return this.shouldExpand;
			}
		}

		public bool ApplyFormat
		{
			get
			{
				return this.applyFormat;
			}
		}

		FormatDescriptorItem(bool isTransparent, bool shouldExapnd, bool applyFormat)
		{
			this.text = string.Empty;
			this.isTransparent = isTransparent;
			this.shouldExpand = shouldExapnd;
			this.applyFormat = applyFormat;
		}

		public FormatDescriptorItem(string text, bool isTransparent, bool shouldExapnd = false, bool applyFormat = true)
			: this(isTransparent, shouldExapnd, applyFormat)
		{
			this.text = text;
		}

		public FormatDescriptorItem(Func<double?, string> applyFormatToDoubleValue, bool isTransparent, bool shouldExapnd = false, bool applyFormat = true)
			: this(isTransparent, shouldExapnd, applyFormat)
		{
			this.applyFormatToDoubleValue = applyFormatToDoubleValue;
		}

		public FormatDescriptorItem(Func<DateTime?, string> applyFormatToDateTimeValue, bool isTransparent, bool shouldExapnd = false, bool applyFormat = true)
			: this(isTransparent, shouldExapnd, applyFormat)
		{
			this.applyFormatToDateTimeValue = applyFormatToDateTimeValue;
		}

		public string ApplyFormatToValue(double? doubleValue)
		{
			if (this.applyFormatToDoubleValue != null)
			{
				return this.applyFormatToDoubleValue(doubleValue);
			}
			return this.Text;
		}

		public string ApplyFormatToValue(DateTime? dateTimeValue)
		{
			if (this.applyFormatToDateTimeValue != null)
			{
				return this.applyFormatToDateTimeValue(dateTimeValue);
			}
			return this.Text;
		}

		public virtual IEnumerable<FormatDescriptorItem> GetItems(double? doubleValue)
		{
			yield return this;
			yield break;
		}

		public string GetTextReplacePlaceholder()
		{
			string text = this.Text.Replace(FormatHelper.TextPlaceholder, "{0}");
			text = text.Replace("\\", string.Empty);
			return text.Replace("\"", string.Empty);
		}

		string text;

		readonly bool isTransparent;

		readonly bool shouldExpand;

		readonly bool applyFormat;

		readonly Func<double?, string> applyFormatToDoubleValue;

		readonly Func<DateTime?, string> applyFormatToDateTimeValue;
	}
}
