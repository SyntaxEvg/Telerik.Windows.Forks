using System;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	class CssWriter
	{
		public CssWriter()
		{
			this.writer = new StringBuilder();
		}

		protected StringBuilder Writer
		{
			get
			{
				return this.writer;
			}
		}

		public void WriteStyleProperty(string name, string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			this.writer.Append(string.Format("{0}: {1};", name, value));
			this.WriteStylePropertyOverride(name, value);
		}

		public string GetData()
		{
			return this.writer.ToString();
		}

		protected virtual void WriteStylePropertyOverride(string name, string value)
		{
		}

		const string CssPropertyFormat = "{0}: {1};";

		readonly StringBuilder writer;
	}
}
