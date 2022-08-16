using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class DefinedNameInfo
	{
		public DefinedNameInfo(string name, string value, int localSheetId, string comment, bool hidden)
		{
			this.Name = name;
			this.Value = value;
			this.LocalSheetId = localSheetId;
			this.Comment = comment;
			this.Hidden = hidden;
		}

		public string Name { get; set; }

		public string Value { get; set; }

		public int LocalSheetId { get; set; }

		public CellIndex CellIndex { get; set; }

		public string Comment { get; set; }

		public bool Hidden { get; set; }

		public bool IsGlobal
		{
			get
			{
				return this.LocalSheetId == -1;
			}
		}
	}
}
