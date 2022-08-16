using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Web.Spreadsheet
{
	public class JsonFormatProvider : WorkbookFormatProviderBase
	{
		public JsonFormatProvider()
		{
			this.ImportSettings = new JsonImportSettings();
			this.ExportSettings = new JsonExportSettings();
		}

		public override string Name
		{
			get
			{
				return "Json";
			}
		}

		public override IEnumerable<string> SupportedExtensions
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

		public JsonExportSettings ExportSettings { get; set; }

		public JsonImportSettings ImportSettings { get; set; }

		protected override Workbook ImportOverride(Stream input)
		{
			JsonSerializer jsonSerializer = new JsonSerializer();
			Workbook result;
			using (StreamReader streamReader = new StreamReader(input, this.ImportSettings.Encoding, true, this.ImportSettings.BufferSize))
			{
				using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
				{
					result = jsonSerializer.Deserialize<Workbook>(jsonTextReader).ToDocument();
				}
			}
			return result;
		}

		protected override void ExportOverride(Workbook document, Stream output)
		{
			Workbook workbook = Workbook.FromDocument(document);
			JsonSerializer jsonSerializer = new JsonSerializer();
			using (StreamWriter streamWriter = new StreamWriter(output, this.ExportSettings.Encoding, this.ExportSettings.BufferSize))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
				{
					jsonSerializer.Serialize(jsonTextWriter, workbook);
				}
			}
		}
	}
}
