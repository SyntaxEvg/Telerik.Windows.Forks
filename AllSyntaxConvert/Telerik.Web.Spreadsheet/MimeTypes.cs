using System;
using System.Collections.Generic;

namespace Telerik.Web.Spreadsheet
{
	public static class MimeTypes
	{
		public const string CSV = "text/csv";

		public const string JSON = "application/json";

		public const string PDF = "application/pdf";

		public const string TXT = "text/tab-separated-values";

		public const string XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

		public static readonly IDictionary<string, string> ByExtension = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ ".csv", "text/csv" },
			{ ".json", "application/json" },
			{ ".pdf", "application/pdf" },
			{ ".txt", "text/tab-separated-values" },
			{ ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
		};
	}
}
