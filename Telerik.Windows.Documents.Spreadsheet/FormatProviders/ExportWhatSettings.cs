using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	public class ExportWhatSettings
	{
		internal ExportWhatSettings()
			: this(ExportWhat.ActiveSheet, false)
		{
		}

		public ExportWhatSettings(ExportWhat exportWhat, bool ignorePrintArea)
		{
			this.ExportWhat = exportWhat;
			this.IgnorePrintArea = ignorePrintArea;
		}

		public ExportWhat ExportWhat { get; set; }

		public bool IgnorePrintArea { get; set; }

		public override bool Equals(object obj)
		{
			ExportWhatSettings exportWhatSettings = obj as ExportWhatSettings;
			return exportWhatSettings != null && this.ExportWhat == exportWhatSettings.ExportWhat && this.IgnorePrintArea == exportWhatSettings.IgnorePrintArea;
		}

		public override int GetHashCode()
		{
			return this.ExportWhat.GetHashCode() ^ this.IgnorePrintArea.GetHashCode();
		}
	}
}
