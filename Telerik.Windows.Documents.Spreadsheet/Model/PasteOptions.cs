using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class PasteOptions
	{
		public PasteType PasteType
		{
			get
			{
				return this.pasteType;
			}
		}

		public bool SkipBlanks
		{
			get
			{
				return this.skipBlanks;
			}
		}

		internal bool SkipMergedCells
		{
			get
			{
				return this.skipMergedCells;
			}
		}

		public PasteOptions(PasteType pasteType, bool skipBlanks = false)
		{
			this.pasteType = pasteType;
			this.skipBlanks = skipBlanks;
			this.skipMergedCells = false;
		}

		internal PasteOptions(PasteType pasteType, bool skipBlanks, bool skipMergedCells)
			: this(pasteType, skipBlanks)
		{
			this.skipMergedCells = skipMergedCells;
		}

		public static readonly PasteOptions All = new PasteOptions(PasteType.All, false);

		readonly PasteType pasteType;

		readonly bool skipBlanks;

		readonly bool skipMergedCells;
	}
}
