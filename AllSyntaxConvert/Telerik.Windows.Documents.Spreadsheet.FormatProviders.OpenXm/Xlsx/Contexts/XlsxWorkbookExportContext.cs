using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class XlsxWorkbookExportContext : XlsxWorkbookContextBase<XlsxWorksheetExportContext>, IXlsxWorkbookExportContext, IOpenXmlExportContext
	{
		public XlsxWorkbookExportContext(Workbook workbook)
			: base(true, workbook)
		{
			this.styleNameToFormattingRecordIndex = new Dictionary<string, int>();
			this.worksheetContextToRelationshipMapping = new Dictionary<IXlsxWorksheetExportContext, string>();
			this.relationshipIdToWorksheetContextMapping = new Dictionary<string, IXlsxWorksheetExportContext>();
			this.formatsContext = new DifferentialFormatsExportContext();
			this.Initialize();
		}

		public IEnumerable<IXlsxWorksheetExportContext> WorksheetContexts
		{
			get
			{
				return base.GetWorksheetContexts();
			}
		}

		public DifferentialFormatsExportContext DifferentialFormatsContext
		{
			get
			{
				return this.formatsContext;
			}
		}

		public int GetStyleFormattingRecordId(string styleName)
		{
			return this.styleNameToFormattingRecordIndex[styleName];
		}

		public string GetRelationshipIdByResource(IResource resource)
		{
			throw new NotImplementedException();
		}

		public void RegisterResource(string relationshipId, IResource resource)
		{
			throw new NotImplementedException();
		}

		WorkbookProtectionInfo IXlsxWorkbookExportContext.GetWorkbookProtectionInfo()
		{
			return new WorkbookProtectionInfo
			{
				Enforced = base.Workbook.IsProtected,
				AlgorithmName = base.Workbook.ProtectionData.AlgorithmName,
				HashValue = base.Workbook.ProtectionData.Hash,
				SaltValue = base.Workbook.ProtectionData.Salt,
				SpinCount = base.Workbook.ProtectionData.SpinCount,
				Password = base.Workbook.ProtectionData.Password,
				LockStructure = base.Workbook.ProtectionOptions.LockStructure,
				LockWindows = base.Workbook.ProtectionOptions.LockWindows
			};
		}

		void IXlsxWorkbookExportContext.RegisterWorksheetContext(string relationshipId, IXlsxWorksheetExportContext context)
		{
			this.relationshipIdToWorksheetContextMapping[relationshipId] = context;
			this.worksheetContextToRelationshipMapping[context] = relationshipId;
		}

		IXlsxWorksheetExportContext IXlsxWorkbookExportContext.GetWorksheetContextByRelationshipId(string relationshipId)
		{
			return this.relationshipIdToWorksheetContextMapping[relationshipId];
		}

		string IXlsxWorkbookExportContext.GetRelationshipIdFromWorksheetContext(IXlsxWorksheetExportContext context)
		{
			return this.worksheetContextToRelationshipMapping[context];
		}

		public IXlsxWorksheetExportContext GetWorksheetContextFromWorksheetPart(WorksheetPart worksheetPart)
		{
			Guard.ThrowExceptionIfNull<WorksheetPart>(worksheetPart, "worksheetPart");
			return base.GetWorksheetContext(base.GetWorksheetFromWorksheetPart(worksheetPart));
		}

		public IXlsxWorksheetExportContext GetWorksheetContextFromDrawingPart(DrawingPart drawingPart)
		{
			Guard.ThrowExceptionIfNull<DrawingPart>(drawingPart, "drawingPart");
			return base.GetWorksheetContext(base.GetWorksheetFromDrawingPart(drawingPart));
		}

		public int GetActiveTabIndex()
		{
			return base.Workbook.Sheets.IndexOf(base.Workbook.ActiveSheet);
		}

		public IEnumerable<WorksheetEntityBase> GetWorksheetEntitiesFromWorksheet(Worksheet worksheet)
		{
			return new WorksheetEntityBase[] { worksheet.Cells, worksheet.Rows, worksheet.Columns };
		}

		static void CollectWorksheetSharedStrings(Worksheet worksheet, ISet<SharedString> sharedStringsSet)
		{
			ICompressedList<ICellValue> propertyValueCollection = worksheet.Cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty);
			foreach (Range<long, ICellValue> range in propertyValueCollection)
			{
				TextCellValue textCellValue = range.Value as TextCellValue;
				if (textCellValue != null)
				{
					sharedStringsSet.Add(new TextSharedString(textCellValue.Value));
				}
			}
		}

		void Initialize()
		{
			this.InitializeWorksheetContexts();
			this.InitializeSharedStrings();
			this.InitializeStyleSheet();
			this.InitializeDefinedNames();
		}

		void InitializeWorksheetContexts()
		{
			for (int i = 0; i < base.Workbook.Worksheets.Count; i++)
			{
				base.AddWorksheetContext(new XlsxWorksheetExportContext(this, base.Workbook.Worksheets[i], i + 1));
			}
		}

		void InitializeSharedStrings()
		{
			List<Worksheet> list = new List<Worksheet>(base.Workbook.Worksheets);
			ISet<SharedString> set = new HashSet<SharedString>();
			for (int i = 0; i < list.Count; i++)
			{
				int index = i;
				XlsxWorkbookExportContext.CollectWorksheetSharedStrings(list[index], set);
			}
			base.SharedStrings.AddRange(set);
		}

		void InitializeStyleSheet()
		{
			this.InitDefaultFormatting();
			this.InitStyleFormatting();
			this.InitDirectFormatting();
		}

		void InitDefaultFormatting()
		{
			base.StyleSheet.FillTable.Add(NoneFill.Instance);
			base.StyleSheet.FillTable.Add(new PatternFill(PatternType.Gray12Percent, null, null));
			base.InitDefaultNumberFormatting();
		}

		void InitStyleFormatting()
		{
			ResourceIndexedTable<string> resourceIndexedTable = new ResourceIndexedTable<string>(true, 0);
			this.InitStyleNameTable(resourceIndexedTable);
			foreach (string styleName in resourceIndexedTable)
			{
				this.InitStyleFormatting(styleName);
			}
		}

		void InitStyleFormatting(string styleName)
		{
			FormattingRecord formattingRecord = default(FormattingRecord);
			CellStyle cellStyle = base.Workbook.Styles[styleName];
			if (cellStyle.IsPropertyValueSet<IFill>(CellPropertyDefinitions.FillProperty) && !TelerikHelper.EqualsOfT<IFill>(cellStyle.Fill, CellPropertyDefinitions.FillProperty.DefaultValue))
			{
				formattingRecord.FillId = new int?(base.StyleSheet.FillTable.Add(cellStyle.Fill));
			}
			formattingRecord.NumberFormatId = new int?(base.StyleSheet.CellValueFormatTable.Add(cellStyle.Format));
			if (cellStyle.IsPropertyValueSet<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty))
			{
				formattingRecord.HorizontalAlignment = new RadHorizontalAlignment?(cellStyle.HorizontalAlignment);
			}
			if (cellStyle.IsPropertyValueSet<RadVerticalAlignment>(CellPropertyDefinitions.VerticalAlignmentProperty))
			{
				formattingRecord.VerticalAlignment = new RadVerticalAlignment?(cellStyle.VerticalAlignment);
			}
			if (cellStyle.IsPropertyValueSet<int>(CellPropertyDefinitions.IndentProperty))
			{
				formattingRecord.Indent = new int?(cellStyle.Indent);
			}
			if (cellStyle.IsPropertyValueSet<bool>(CellPropertyDefinitions.IsWrappedProperty))
			{
				formattingRecord.WrapText = new bool?(cellStyle.IsWrapped);
			}
			if (cellStyle.IsPropertyValueSet<bool>(CellPropertyDefinitions.IsLockedProperty))
			{
				formattingRecord.IsLocked = new bool?(cellStyle.IsLocked);
			}
			formattingRecord.ApplyNumberFormat = new bool?(cellStyle.IncludeNumber);
			formattingRecord.ApplyAlignment = new bool?(cellStyle.IncludeAlignment);
			formattingRecord.ApplyFont = new bool?(cellStyle.IncludeFont);
			formattingRecord.ApplyBorder = new bool?(cellStyle.IncludeBorder);
			formattingRecord.ApplyFill = new bool?(cellStyle.IncludeFill);
			formattingRecord.ApplyProtection = new bool?(cellStyle.IncludeProtection);
			formattingRecord.FontInfoId = new int?(base.StyleSheet.FontInfoTable.Add(new FontInfo(cellStyle)));
			formattingRecord.BordersInfoId = new int?(base.StyleSheet.BordersInfoTable.Add(new BordersInfo(cellStyle)));
			int num = base.StyleSheet.StyleFormattingTable.Add(formattingRecord);
			this.styleNameToFormattingRecordIndex.Add(styleName, num);
			base.StyleSheet.DirectFormattingTable.Add(new FormattingRecord(formattingRecord)
			{
				StyleFormattingRecordId = new int?(num)
			});
			base.StyleSheet.StyleInfoTable.Add(new StyleInfo
			{
				Name = styleName,
				BuiltInId = cellStyle.BuiltInId,
				FormattingRecordId = num
			});
		}

		void InitStyleNameTable(ResourceIndexedTable<string> styleNameTable)
		{
			styleNameTable.Add("Normal");
			foreach (XlsxWorksheetExportContext xlsxWorksheetExportContext in base.GetWorksheetContexts())
			{
				foreach (WorksheetEntityBase worksheetEntityBase in this.GetWorksheetEntitiesFromWorksheet(xlsxWorksheetExportContext.Worksheet))
				{
					ICompressedList<string> compressedList = xlsxWorksheetExportContext.StyleNameCompressedList[worksheetEntityBase];
					foreach (Range<long, string> range in compressedList.GetNonDefaultRanges())
					{
						this.MarkRangeAsNonDefaultFormatted<string>(xlsxWorksheetExportContext, worksheetEntityBase, compressedList, range);
						styleNameTable.Add(range.Value);
					}
				}
			}
		}

		void MarkRangeAsNonDefaultFormatted<T>(XlsxWorksheetExportContext worksheetContext, WorksheetEntityBase entity, ICompressedList<T> compressedList, Range<long, T> range)
		{
			T defaultValue = compressedList.GetDefaultValue();
			if (!TelerikHelper.EqualsOfT<T>(range.Value, defaultValue))
			{
				worksheetContext.NonDefaultFormattingCompressedList[entity].SetValue(range.Start, range.End, true);
			}
		}

		void InitDirectFormatting()
		{
			this.InitDirectFormattingFontInfoTable();
			this.InitDirectFormattingFills();
			this.InitDirectFormattingCellValueFormats();
			this.InitDirectFormattingBordersInfoTable();
			this.InitDirectFormattingTable();
		}

		void InitDirectFormattingFontInfoTable()
		{
			foreach (XlsxWorksheetExportContext xlsxWorksheetExportContext in base.GetWorksheetContexts())
			{
				xlsxWorksheetExportContext.InitFontInfos();
				foreach (WorksheetEntityBase worksheetEntityBase in this.GetWorksheetEntitiesFromWorksheet(xlsxWorksheetExportContext.Worksheet))
				{
					ICompressedList<FontInfo> compressedList = xlsxWorksheetExportContext.FontInfoCompressedList[worksheetEntityBase];
					foreach (Range<long, FontInfo> range in compressedList.GetNonDefaultRanges())
					{
						this.MarkRangeAsNonDefaultFormatted<FontInfo>(xlsxWorksheetExportContext, worksheetEntityBase, compressedList, range);
						base.StyleSheet.FontInfoTable.Add(range.Value);
					}
				}
			}
		}

		void InitDirectFormattingFills()
		{
			foreach (XlsxWorksheetExportContext xlsxWorksheetExportContext in base.GetWorksheetContexts())
			{
				foreach (WorksheetEntityBase worksheetEntityBase in this.GetWorksheetEntitiesFromWorksheet(xlsxWorksheetExportContext.Worksheet))
				{
					ICompressedList<IFill> compressedList = xlsxWorksheetExportContext.FillCompressedList[worksheetEntityBase];
					foreach (Range<long, IFill> range in xlsxWorksheetExportContext.FillCompressedList[worksheetEntityBase].GetNonDefaultRanges())
					{
						this.MarkRangeAsNonDefaultFormatted<IFill>(xlsxWorksheetExportContext, worksheetEntityBase, compressedList, range);
						base.StyleSheet.FillTable.Add(range.Value);
					}
				}
			}
		}

		void InitDirectFormattingCellValueFormats()
		{
			foreach (XlsxWorksheetExportContext xlsxWorksheetExportContext in base.GetWorksheetContexts())
			{
				foreach (WorksheetEntityBase worksheetEntityBase in this.GetWorksheetEntitiesFromWorksheet(xlsxWorksheetExportContext.Worksheet))
				{
					ICompressedList<CellValueFormat> compressedList = xlsxWorksheetExportContext.CellValueFormatCompressedList[worksheetEntityBase];
					foreach (Range<long, CellValueFormat> range in compressedList.GetNonDefaultRanges())
					{
						this.MarkRangeAsNonDefaultFormatted<CellValueFormat>(xlsxWorksheetExportContext, worksheetEntityBase, compressedList, range);
						base.StyleSheet.CellValueFormatTable.Add(range.Value);
					}
				}
			}
		}

		void InitDirectFormattingBordersInfoTable()
		{
			foreach (XlsxWorksheetExportContext xlsxWorksheetExportContext in base.GetWorksheetContexts())
			{
				xlsxWorksheetExportContext.InitBordersInfos();
				foreach (WorksheetEntityBase worksheetEntityBase in this.GetWorksheetEntitiesFromWorksheet(xlsxWorksheetExportContext.Worksheet))
				{
					ICompressedList<BordersInfo> compressedList = xlsxWorksheetExportContext.BordersInfoCompressedList[worksheetEntityBase];
					foreach (Range<long, BordersInfo> range in compressedList.GetNonDefaultRanges())
					{
						this.MarkRangeAsNonDefaultFormatted<BordersInfo>(xlsxWorksheetExportContext, worksheetEntityBase, compressedList, range);
						base.StyleSheet.BordersInfoTable.Add(range.Value);
					}
				}
			}
		}

		void InitDirectFormattingTable()
		{
			foreach (XlsxWorksheetExportContext xlsxWorksheetExportContext in base.GetWorksheetContexts())
			{
				xlsxWorksheetExportContext.InitDirectFormattingRecords();
				foreach (WorksheetEntityBase worksheetEntityBase in this.GetWorksheetEntitiesFromWorksheet(xlsxWorksheetExportContext.Worksheet))
				{
					ICompressedList<FormattingRecord> compressedList = xlsxWorksheetExportContext.DirectFormattingCompressedList[worksheetEntityBase];
					foreach (Range<long, FormattingRecord> range in compressedList.GetNonDefaultRanges())
					{
						this.MarkRangeAsNonDefaultFormatted<FormattingRecord>(xlsxWorksheetExportContext, worksheetEntityBase, compressedList, range);
						base.StyleSheet.DirectFormattingTable.Add(range.Value);
					}
				}
			}
		}

		void InitializeDefinedNames()
		{
			foreach (KeyValuePair<int, IEnumerable<ISpreadsheetName>> keyValuePair in base.Workbook.NameManager.NameWithWorksheetIdCollections)
			{
				foreach (ISpreadsheetName spreadsheetName in keyValuePair.Value)
				{
					DefinedName definedName = spreadsheetName as DefinedName;
					if (definedName != null)
					{
						string value = definedName.FormulaCellValue.Value.ToString(XlsxHelper.CultureInfo);
						DefinedNameInfo definedNameInfo = new DefinedNameInfo(definedName.Name, value, keyValuePair.Key, definedName.Comment, !definedName.IsVisible);
						base.AddDefinedNameInfo(definedNameInfo);
					}
				}
			}
			this.ExportRequiredDefinedNames();
		}

		void ExportRequiredDefinedNames()
		{
			for (int i = 0; i < base.Workbook.Worksheets.Count; i++)
			{
				Worksheet worksheet = base.Workbook.Worksheets[i];
				PrintArea printArea = worksheet.WorksheetPageSetup.PrintArea;
				if (printArea.HasPrintAreaRanges)
				{
					string formula = TextHelper.BuildAbsoluteCellReferenceString(worksheet.Name, printArea.Ranges, XlsxHelper.CultureInfo);
					string value = SpreadsheetCultureHelper.ClearFormulaValue(formula);
					DefinedNameInfo definedNameInfo = new DefinedNameInfo(DefinedName.PrintAreaDefinedName, value, i, string.Empty, false);
					base.AddDefinedNameInfo(definedNameInfo);
				}
				AutoFilter filter = worksheet.Filter;
				if (filter.FilterRange != null)
				{
					string formula2 = TextHelper.BuildAbsoluteCellReferenceString(worksheet.Name, filter.FilterRange, XlsxHelper.CultureInfo);
					string value2 = SpreadsheetCultureHelper.ClearFormulaValue(formula2);
					DefinedNameInfo definedNameInfo2 = new DefinedNameInfo(DefinedName.FilterDefinedName, value2, i, string.Empty, true);
					base.AddDefinedNameInfo(definedNameInfo2);
				}
			}
		}

		readonly Dictionary<string, int> styleNameToFormattingRecordIndex;

		readonly Dictionary<string, IXlsxWorksheetExportContext> relationshipIdToWorksheetContextMapping;

		readonly Dictionary<IXlsxWorksheetExportContext, string> worksheetContextToRelationshipMapping;

		readonly DifferentialFormatsExportContext formatsContext;
	}
}
