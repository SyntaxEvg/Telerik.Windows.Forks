using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts
{
	class WorksheetProtectionInfo : ProtectionInfoBase
	{
		public bool DeleteRows
		{
			get
			{
				return this.deleteRows;
			}
			set
			{
				this.deleteRows = value;
			}
		}

		public bool InsertRows
		{
			get
			{
				return this.insertRows;
			}
			set
			{
				this.insertRows = value;
			}
		}

		public bool DeleteColumns
		{
			get
			{
				return this.deleteColumns;
			}
			set
			{
				this.deleteColumns = value;
			}
		}

		public bool InsertColumns
		{
			get
			{
				return this.insertColumns;
			}
			set
			{
				this.insertColumns = value;
			}
		}

		public bool FormatColumns
		{
			get
			{
				return this.formatColumns;
			}
			set
			{
				this.formatColumns = value;
			}
		}

		public bool FormatCells
		{
			get
			{
				return this.formatCells;
			}
			set
			{
				this.formatCells = value;
			}
		}

		public bool FormatRows
		{
			get
			{
				return this.formatRows;
			}
			set
			{
				this.formatRows = value;
			}
		}

		public bool AutoFilter
		{
			get
			{
				return this.autoFilter;
			}
			set
			{
				this.autoFilter = value;
			}
		}

		public bool Sort
		{
			get
			{
				return this.sort;
			}
			set
			{
				this.sort = value;
			}
		}

		bool deleteRows;

		bool insertRows;

		bool deleteColumns;

		bool insertColumns;

		bool formatCells;

		bool formatRows;

		bool formatColumns;

		bool autoFilter;

		bool sort;
	}
}
