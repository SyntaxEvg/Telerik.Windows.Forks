using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class PaperTypesConverter : IStringConverter<PaperTypes>
	{
		static PaperTypesConverter()
		{
			PaperTypesConverter.AddId("1", PaperTypes.Letter);
			PaperTypesConverter.AddId("2", PaperTypes.Letter);
			PaperTypesConverter.AddId("3", PaperTypes.Tabloid);
			PaperTypesConverter.AddId("4", PaperTypes.Ledger);
			PaperTypesConverter.AddId("5", PaperTypes.Legal);
			PaperTypesConverter.AddId("6", PaperTypes.Statement);
			PaperTypesConverter.AddId("7", PaperTypes.Executive);
			PaperTypesConverter.AddId("8", PaperTypes.A3);
			PaperTypesConverter.AddId("9", PaperTypes.A4);
			PaperTypesConverter.AddId("10", PaperTypes.A4);
			PaperTypesConverter.AddId("11", PaperTypes.A5);
			PaperTypesConverter.AddId("12", PaperTypes.B4);
			PaperTypesConverter.AddId("13", PaperTypes.B5);
			PaperTypesConverter.AddId("14", PaperTypes.Folio);
			PaperTypesConverter.AddId("15", PaperTypes.Quarto);
			PaperTypesConverter.AddId("42", PaperTypes.B4);
			PaperTypesConverter.AddId("50", PaperTypes.Letter);
			PaperTypesConverter.AddId("51", PaperTypes.Legal);
			PaperTypesConverter.AddId("52", PaperTypes.Tabloid);
			PaperTypesConverter.AddId("53", PaperTypes.A4);
			PaperTypesConverter.AddId("59", PaperTypes.Letter);
			PaperTypesConverter.AddId("60", PaperTypes.A4);
			PaperTypesConverter.AddId("63", PaperTypes.A3);
			PaperTypesConverter.AddId("64", PaperTypes.A5);
			PaperTypesConverter.AddId("65", PaperTypes.B5);
			PaperTypesConverter.AddId("66", PaperTypes.A2);
			PaperTypesConverter.defaultId = PaperTypesConverter.paperTypesToId[PaperTypes.Letter];
		}

		public PaperTypes ConvertFromString(string value)
		{
			PaperTypes result;
			if (PaperTypesConverter.paperIdToPaperTypes.TryGetValue(value, out result))
			{
				return result;
			}
			return PaperTypes.Letter;
		}

		public string ConvertToString(PaperTypes value)
		{
			string result;
			if (PaperTypesConverter.paperTypesToId.TryGetValue(value, out result))
			{
				return result;
			}
			return PaperTypesConverter.defaultId;
		}

		static void AddId(string id, PaperTypes paperType)
		{
			PaperTypesConverter.paperIdToPaperTypes[id] = paperType;
			if (!PaperTypesConverter.paperTypesToId.ContainsKey(paperType))
			{
				PaperTypesConverter.paperTypesToId[paperType] = id;
			}
		}

		public const PaperTypes DefaultExportPaperType = PaperTypes.Letter;

		static readonly Dictionary<PaperTypes, string> paperTypesToId = new Dictionary<PaperTypes, string>();

		static readonly Dictionary<string, PaperTypes> paperIdToPaperTypes = new Dictionary<string, PaperTypes>();

		static readonly string defaultId;
	}
}
