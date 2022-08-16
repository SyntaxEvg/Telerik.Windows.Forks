using System;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core
{
	public class CsvSettings
	{
		public char Delimiter
		{
			get
			{
				if (this.delimiter == null)
				{
					string listSeparator = FormatHelper.DefaultSpreadsheetCulture.ListSeparator;
					this.delimiter = new char?((listSeparator.Length == 1) ? listSeparator[0] : '\0');
				}
				return this.delimiter.Value;
			}
			set
			{
				this.delimiter = new char?(value);
			}
		}

		public char Quote
		{
			get
			{
				if (this.quote == null)
				{
					this.quote = new char?('"');
				}
				return this.quote.Value;
			}
			set
			{
				this.quote = new char?(value);
			}
		}

		public bool HasHeaderRow
		{
			get
			{
				if (this.hasHeaderRow == null)
				{
					this.hasHeaderRow = new bool?(false);
				}
				return this.hasHeaderRow.Value;
			}
			set
			{
				this.hasHeaderRow = new bool?(value);
			}
		}

		public Encoding Encoding
		{
			get
			{
				if (this.encoding == null)
				{
					this.encoding = new UTF8Encoding(true);
				}
				return this.encoding;
			}
			set
			{
				this.encoding = value;
			}
		}

		public override bool Equals(object obj)
		{
			CsvSettings csvSettings = obj as CsvSettings;
			return !(csvSettings == null) && (TelerikHelper.EqualsOfT<char?>(new char?(this.Delimiter), new char?(csvSettings.Delimiter)) && TelerikHelper.EqualsOfT<char?>(new char?(this.Quote), new char?(csvSettings.Quote)) && TelerikHelper.EqualsOfT<bool?>(new bool?(this.HasHeaderRow), new bool?(csvSettings.HasHeaderRow))) && TelerikHelper.EqualsOfT<Encoding>(this.Encoding, csvSettings.Encoding);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Delimiter.GetHashCode(), this.Quote.GetHashCode(), this.HasHeaderRow.GetHashCode(), this.Encoding.GetHashCodeOrZero());
		}

		public static bool operator ==(CsvSettings first, CsvSettings second)
		{
			return first.Equals(second);
		}

		public static bool operator !=(CsvSettings first, CsvSettings second)
		{
			return !first.Equals(second);
		}

		char? delimiter;

		char? quote;

		bool? hasHeaderRow;

		Encoding encoding;
	}
}
