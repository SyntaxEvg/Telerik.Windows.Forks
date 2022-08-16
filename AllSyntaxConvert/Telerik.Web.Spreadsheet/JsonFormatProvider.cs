using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Web.Spreadsheet
{
	public class JsonFormatProvider : global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.WorkbookFormatProviderBase
	{
		public JsonFormatProvider()
		{
			this.ImportSettings = new global::Telerik.Web.Spreadsheet.JsonImportSettings();
			this.ExportSettings = new global::Telerik.Web.Spreadsheet.JsonExportSettings();
		}

		public override string Name
		{
			get
			{
				return "Json";
			}
		}

		public override global::System.Collections.Generic.IEnumerable<string> SupportedExtensions
		{
			get
			{
				return new string[] { ".json" };
			}
		}

		public override bool CanImport
		{
			get
			{
				return true;
			}
		}

		public override bool CanExport
		{
			get
			{
				return true;
			}
		}

		public global::Telerik.Web.Spreadsheet.JsonExportSettings ExportSettings { get; private set; }

		public global::Telerik.Web.Spreadsheet.JsonImportSettings ImportSettings { get; private set; }

		protected override global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook ImportOverride(global::System.IO.Stream input)
		{
			global::Newtonsoft.Json.JsonSerializer jsonSerializer = new global::Newtonsoft.Json.JsonSerializer();
			global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook result;
			using (global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(input, this.ImportSettings.Encoding, true, this.ImportSettings.BufferSize))
			{
				using (global::Newtonsoft.Json.JsonTextReader jsonTextReader = new global::Newtonsoft.Json.JsonTextReader(streamReader))
				{
					result = jsonSerializer.Deserialize<global::Telerik.Web.Spreadsheet.Workbook>(jsonTextReader).ToDocument();
				}
			}
			return result;
		}

		protected override void ExportOverride(global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook document, global::System.IO.Stream output)
		{
			global::Telerik.Web.Spreadsheet.Workbook workbook = global::Telerik.Web.Spreadsheet.Workbook.FromDocument(document);
			global::Newtonsoft.Json.JsonSerializer jsonSerializer = new global::Newtonsoft.Json.JsonSerializer();
			using (global::System.IO.StreamWriter streamWriter = new global::System.IO.StreamWriter(output, this.ExportSettings.Encoding, this.ExportSettings.BufferSize))
			{
				using (global::Newtonsoft.Json.JsonTextWriter jsonTextWriter = new global::Newtonsoft.Json.JsonTextWriter(streamWriter))
				{
					jsonSerializer.Serialize(jsonTextWriter, workbook);
				}
			}
		}
	}
}
