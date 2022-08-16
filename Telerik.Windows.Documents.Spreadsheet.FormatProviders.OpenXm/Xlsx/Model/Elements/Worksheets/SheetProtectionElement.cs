using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class SheetProtectionElement : WorksheetElementBase
	{
		public SheetProtectionElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.algorithmName = base.RegisterAttribute<string>("algorithmName", string.Empty, false);
			this.hashValue = base.RegisterAttribute<string>("hashValue", string.Empty, false);
			this.saltValue = base.RegisterAttribute<string>("saltValue", string.Empty, false);
			this.spinCount = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("spinCount", false));
			this.password = base.RegisterAttribute<string>("password", string.Empty, false);
			this.sheet = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("sheet"));
			this.formatCells = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("formatCells", true, false));
			this.formatColumns = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("formatColumns", true, false));
			this.formatRows = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("formatRows", true, false));
			this.insertColumns = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("insertColumns", true, false));
			this.insertRows = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("insertRows", true, false));
			this.deleteColumns = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("deleteColumns", true, false));
			this.deleteRows = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("deleteRows", true, false));
			this.autoFilter = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("autoFilter", true, false));
			this.sort = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("sort", true, false));
		}

		public override string ElementName
		{
			get
			{
				return "sheetProtection";
			}
		}

		public bool Sheet
		{
			get
			{
				return this.sheet.Value;
			}
			set
			{
				this.sheet.Value = value;
			}
		}

		public string AlgorithmName
		{
			get
			{
				return this.algorithmName.Value;
			}
			set
			{
				this.algorithmName.Value = value;
			}
		}

		public string HashValue
		{
			get
			{
				return this.hashValue.Value;
			}
			set
			{
				this.hashValue.Value = value;
			}
		}

		public string SaltValue
		{
			get
			{
				return this.saltValue.Value;
			}
			set
			{
				this.saltValue.Value = value;
			}
		}

		public int SpinCount
		{
			get
			{
				return this.spinCount.Value;
			}
			set
			{
				this.spinCount.Value = value;
			}
		}

		public string Password
		{
			get
			{
				return this.password.Value;
			}
			set
			{
				this.password.Value = value;
			}
		}

		public bool FormatCells
		{
			get
			{
				return this.formatCells.Value;
			}
			set
			{
				this.formatCells.Value = value;
			}
		}

		public bool FormatColumns
		{
			get
			{
				return this.formatColumns.Value;
			}
			set
			{
				this.formatColumns.Value = value;
			}
		}

		public bool FormatRows
		{
			get
			{
				return this.formatRows.Value;
			}
			set
			{
				this.formatRows.Value = value;
			}
		}

		public bool InsertColumns
		{
			get
			{
				return this.insertColumns.Value;
			}
			set
			{
				this.insertColumns.Value = value;
			}
		}

		public bool InsertRows
		{
			get
			{
				return this.insertRows.Value;
			}
			set
			{
				this.insertRows.Value = value;
			}
		}

		public bool DeleteColumns
		{
			get
			{
				return this.deleteColumns.Value;
			}
			set
			{
				this.deleteColumns.Value = value;
			}
		}

		public bool DeleteRows
		{
			get
			{
				return this.deleteRows.Value;
			}
			set
			{
				this.deleteRows.Value = value;
			}
		}

		public bool AutoFilter
		{
			get
			{
				return this.autoFilter.Value;
			}
			set
			{
				this.autoFilter.Value = value;
			}
		}

		public bool Sort
		{
			get
			{
				return this.sort.Value;
			}
			set
			{
				this.sort.Value = value;
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			return context.GetSheetProtectionInfo().Enforced;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			WorksheetProtectionInfo sheetProtectionInfo = context.GetSheetProtectionInfo();
			if (sheetProtectionInfo.Enforced)
			{
				this.Sheet = true;
				if (!string.IsNullOrEmpty(sheetProtectionInfo.Password))
				{
					this.Password = sheetProtectionInfo.Password;
				}
				else if (!string.IsNullOrEmpty(sheetProtectionInfo.HashValue))
				{
					this.AlgorithmName = sheetProtectionInfo.AlgorithmName;
					this.HashValue = sheetProtectionInfo.HashValue;
					this.SpinCount = sheetProtectionInfo.SpinCount;
					if (!string.IsNullOrEmpty(sheetProtectionInfo.SaltValue))
					{
						this.SaltValue = sheetProtectionInfo.SaltValue;
					}
				}
			}
			if (this.formatCells.DefaultValue != sheetProtectionInfo.FormatCells)
			{
				this.FormatCells = sheetProtectionInfo.FormatCells;
			}
			if (this.formatColumns.DefaultValue != sheetProtectionInfo.FormatColumns)
			{
				this.FormatColumns = sheetProtectionInfo.FormatColumns;
			}
			if (this.formatRows.DefaultValue != sheetProtectionInfo.FormatRows)
			{
				this.FormatRows = sheetProtectionInfo.FormatRows;
			}
			if (this.insertColumns.DefaultValue != sheetProtectionInfo.InsertColumns)
			{
				this.InsertColumns = sheetProtectionInfo.InsertColumns;
			}
			if (this.insertRows.DefaultValue != sheetProtectionInfo.InsertRows)
			{
				this.InsertRows = sheetProtectionInfo.InsertRows;
			}
			if (this.deleteColumns.DefaultValue != sheetProtectionInfo.DeleteColumns)
			{
				this.DeleteColumns = sheetProtectionInfo.DeleteColumns;
			}
			if (this.deleteRows.DefaultValue != sheetProtectionInfo.DeleteRows)
			{
				this.DeleteRows = sheetProtectionInfo.DeleteRows;
			}
			if (this.autoFilter.DefaultValue != sheetProtectionInfo.AutoFilter)
			{
				this.AutoFilter = sheetProtectionInfo.AutoFilter;
			}
			if (this.sort.DefaultValue != sheetProtectionInfo.Sort)
			{
				this.Sort = sheetProtectionInfo.Sort;
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			context.ApplySheetProtectionInfo(new WorksheetProtectionInfo
			{
				Password = this.Password,
				AlgorithmName = this.AlgorithmName,
				HashValue = this.HashValue,
				SaltValue = this.SaltValue,
				SpinCount = this.SpinCount,
				Enforced = this.Sheet,
				FormatCells = this.FormatCells,
				FormatColumns = this.FormatColumns,
				FormatRows = this.FormatRows,
				InsertColumns = this.InsertColumns,
				InsertRows = this.InsertRows,
				DeleteColumns = this.DeleteColumns,
				DeleteRows = this.DeleteRows,
				AutoFilter = this.AutoFilter,
				Sort = this.Sort
			});
		}

		readonly OpenXmlAttribute<string> algorithmName;

		readonly OpenXmlAttribute<string> hashValue;

		readonly OpenXmlAttribute<string> saltValue;

		readonly IntOpenXmlAttribute spinCount;

		readonly OpenXmlAttribute<string> password;

		readonly BoolOpenXmlAttribute sheet;

		readonly BoolOpenXmlAttribute formatCells;

		readonly BoolOpenXmlAttribute formatColumns;

		readonly BoolOpenXmlAttribute formatRows;

		readonly BoolOpenXmlAttribute insertColumns;

		readonly BoolOpenXmlAttribute insertRows;

		readonly BoolOpenXmlAttribute deleteColumns;

		readonly BoolOpenXmlAttribute deleteRows;

		readonly BoolOpenXmlAttribute autoFilter;

		readonly BoolOpenXmlAttribute sort;
	}
}
