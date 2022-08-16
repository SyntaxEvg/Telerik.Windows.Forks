using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core
{
	class CsvWriter : IDisposable
	{
		public CsvWriter(Stream stream, CsvSettings settings = null)
			: this(settings)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.writer = new StreamWriter(stream, this.settings.Encoding);
		}

		public CsvWriter(TextWriter writer, CsvSettings settings = null)
			: this(settings)
		{
			Guard.ThrowExceptionIfNull<TextWriter>(writer, "writer");
			this.writer = writer;
		}

		CsvWriter(CsvSettings settings)
		{
			this.settings = settings ?? new CsvSettings();
			this.isHeaderRow = this.settings.HasHeaderRow;
		}

		public void WriteRecord(IEnumerable<string> values)
		{
			bool flag = true;
			int num = 0;
			foreach (string value in values)
			{
				if (string.IsNullOrEmpty(value))
				{
					num++;
				}
				else
				{
					num = (flag ? num : (num + 1));
					for (int i = 0; i < num; i++)
					{
						this.writer.Write(this.settings.Delimiter);
					}
					string value2 = this.EscapeValue(value);
					this.writer.Write(value2);
					flag = false;
					num = 0;
				}
			}
			this.isHeaderRow = false;
			this.writer.WriteLine();
			this.writer.Flush();
		}

		string EscapeValue(string value)
		{
			StringBuilder stringBuilder = new StringBuilder(value);
			bool flag = this.isHeaderRow;
			foreach (char c in value)
			{
				if (c == this.settings.Delimiter || c == '\n' || c == '\r')
				{
					flag = true;
				}
				if (c == this.settings.Quote)
				{
					string text = this.settings.Quote.ToString();
					string newValue = text + text;
					stringBuilder.Replace(text, newValue);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				stringBuilder.Insert(0, this.settings.Quote.ToString());
				stringBuilder.Append(this.settings.Quote);
			}
			return stringBuilder.ToString();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool cleanUpManagedResources)
		{
			if (this.alreadyDisposed)
			{
				return;
			}
			if (cleanUpManagedResources && this.writer != null)
			{
				this.writer.Dispose();
			}
			this.alreadyDisposed = true;
		}

		readonly TextWriter writer;

		readonly CsvSettings settings;

		bool isHeaderRow;

		bool alreadyDisposed;
	}
}
