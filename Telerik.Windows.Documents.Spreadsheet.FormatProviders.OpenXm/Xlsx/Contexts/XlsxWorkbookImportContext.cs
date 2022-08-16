using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Charts;
using Telerik.Windows.Documents.Spreadsheet.Model.Protection;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class XlsxWorkbookImportContext : XlsxWorkbookContextBase<XlsxWorksheetImportContext>, IXlsxWorkbookImportContext, IOpenXmlImportContext
	{
		public XlsxWorkbookImportContext()
			: base(false, new Workbook())
		{
			this.resourcesMapping = new Dictionary<string, IResource>();
			this.formats = new DifferentialFormatsImportContext();
			this.axesGroupNamesToAxesIndices = new ValueMapper<AxisGroupName, Tuple<int, int>>();
			this.seriesGroupsToAxesIndices = new ValueMapper<ISupportAxes, Tuple<int, int>>();
		}

		public DifferentialFormatsImportContext DifferentialFormatsContext
		{
			get
			{
				return this.formats;
			}
		}

		public bool IsImportSuspended
		{
			get
			{
				return false;
			}
		}

		public void BeginImport()
		{
			base.Workbook.History.IsEnabled = false;
			base.Workbook.SuspendLayoutUpdate();
			base.Workbook.SuspendPropertyChanged();
			this.InitDefaultFormatting();
		}

		public void EndImport()
		{
			foreach (XlsxWorksheetImportContext xlsxWorksheetImportContext in base.GetWorksheetContexts())
			{
				xlsxWorksheetImportContext.EndImport(this);
			}
			this.ImportDefinedNames();
			base.Workbook.ResumePropertyChanged();
			base.Workbook.ResumeLayoutUpdate();
			base.Workbook.History.IsEnabled = true;
		}

		public IXlsxWorksheetImportContext AddWorksheet(string worksheetName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(worksheetName, "worksheetName");
			Worksheet worksheet = base.Workbook.Worksheets.Add();
			worksheet.Name = worksheetName;
			XlsxWorksheetImportContext xlsxWorksheetImportContext = new XlsxWorksheetImportContext(this, worksheet);
			base.AddWorksheetContext(xlsxWorksheetImportContext);
			return xlsxWorksheetImportContext;
		}

		public IXlsxWorksheetImportContext GetWorksheetContextFromWorksheetPart(WorksheetPart worksheetPart)
		{
			Guard.ThrowExceptionIfNull<WorksheetPart>(worksheetPart, "worksheetPart");
			return base.GetWorksheetContext(base.GetWorksheetFromWorksheetPart(worksheetPart));
		}

		public IXlsxWorksheetImportContext GetWorksheetContextFromDrawingPart(DrawingPart drawingPart)
		{
			Guard.ThrowExceptionIfNull<DrawingPart>(drawingPart, "drawingPart");
			return base.GetWorksheetContext(base.GetWorksheetFromDrawingPart(drawingPart));
		}

		IXlsxWorksheetImportContext IXlsxWorkbookImportContext.GetWorksheetContext(string worksheetName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(worksheetName, "worksheetName");
			return base.GetWorksheetContext(worksheetName);
		}

		public void SetActiveTabIndex(int activeTabIndex)
		{
			base.Workbook.ActiveTabIndex = activeTabIndex;
		}

		public void ApplyWorkbookProtectionInfo(WorkbookProtectionInfo workbookProtectionInfo)
		{
			Guard.ThrowExceptionIfNull<WorkbookProtectionInfo>(workbookProtectionInfo, "workbookProtectionInfo");
			base.Workbook.ProtectionData.Enforced = true;
			if (string.IsNullOrEmpty(workbookProtectionInfo.Password))
			{
				base.Workbook.ProtectionData.AlgorithmName = workbookProtectionInfo.AlgorithmName;
				base.Workbook.ProtectionData.Salt = workbookProtectionInfo.SaltValue;
				base.Workbook.ProtectionData.Hash = workbookProtectionInfo.HashValue;
				base.Workbook.ProtectionData.SpinCount = workbookProtectionInfo.SpinCount;
			}
			else
			{
				base.Workbook.ProtectionData.Password = workbookProtectionInfo.Password;
			}
			if (workbookProtectionInfo.Enforced)
			{
				base.Workbook.ProtectionOptions = new WorkbookProtectionOptions(workbookProtectionInfo.LockStructure, workbookProtectionInfo.LockWindows);
				return;
			}
			base.Workbook.ProtectionOptions = WorkbookProtectionOptions.Default;
		}

		public void RegisterResource(string resourceKey, IResource resource)
		{
			this.resourcesMapping[resourceKey] = resource;
		}

		public IResource GetResourceByResourceKey(string resourceKey)
		{
			return this.resourcesMapping[resourceKey];
		}

		public void ImportStyles()
		{
			foreach (StyleInfo styleInfo in base.StyleSheet.StyleInfoTable)
			{
				CellStyle cellStyle;
				if (styleInfo.BuiltInId != null)
				{
					CellStyle byName = base.Workbook.Styles.GetByName(styleInfo.Name);
					if (byName != null)
					{
						cellStyle = this.CreateStyleFromStyleInfo(styleInfo, byName);
						using (new UpdateScope(new Action(byName.SuspendForcingLayoutUpdate), new Action(byName.ResumeForcingLayoutUpdate)))
						{
							byName.CopyPropertiesFrom(cellStyle);
							continue;
						}
					}
				}
				cellStyle = this.CreateStyleFromStyleInfo(styleInfo, null);
				CellStyle byName2 = base.Workbook.Styles.GetByName(cellStyle.Name);
				if (byName2 == null || base.Workbook.Styles.Remove(cellStyle.Name))
				{
					CellStyle cellStyle2 = base.Workbook.Styles.Add(cellStyle.Name, CellStyleCategory.Custom, true);
					using (new UpdateScope(new Action(cellStyle2.SuspendForcingLayoutUpdate), new Action(cellStyle2.ResumeForcingLayoutUpdate)))
					{
						cellStyle2.CopyPropertiesFrom(cellStyle);
						continue;
					}
				}
				byName2.CopyPropertiesFrom(cellStyle);
			}
		}

		public FormulaChartData GetFormulaChartData(string formula)
		{
			return new WorkbookFormulaChartData(base.Workbook, formula);
		}

		public void RegisterSeriesGroupAwaitingAxisGroupName(ISupportAxes seriesGroup, int catAxisId, int valAxisId)
		{
			this.seriesGroupsToAxesIndices.AddPair(seriesGroup, new Tuple<int, int>(catAxisId, valAxisId));
		}

		public void RegisterAxisGroup(AxisGroupName groupName, int thisId, int otherId)
		{
			if (!this.axesGroupNamesToAxesIndices.ContainsFromValue(groupName))
			{
				this.axesGroupNamesToAxesIndices.AddPair(groupName, new Tuple<int, int>(thisId, otherId));
			}
		}

		public void PairSeriesGroupsWithAxes()
		{
			foreach (ISupportAxes supportAxes in this.seriesGroupsToAxesIndices.FromValues)
			{
				Tuple<int, int> expectedIndices = this.seriesGroupsToAxesIndices.GetToValue(supportAxes);
				Tuple<int, int> tuple = this.axesGroupNamesToAxesIndices.ToValues.FirstOrDefault((Tuple<int, int> registeredPair) => (expectedIndices.Item1 == registeredPair.Item1 && expectedIndices.Item2 == registeredPair.Item2) || (expectedIndices.Item1 == registeredPair.Item2 && expectedIndices.Item2 == registeredPair.Item1));
				if (tuple == null)
				{
					throw new InvalidOperationException("The axes are incorrectly paired.");
				}
				AxisGroupName fromValue = this.axesGroupNamesToAxesIndices.GetFromValue(tuple);
				supportAxes.AxisGroupName = fromValue;
			}
			this.seriesGroupsToAxesIndices = new ValueMapper<ISupportAxes, Tuple<int, int>>();
			this.axesGroupNamesToAxesIndices = new ValueMapper<AxisGroupName, Tuple<int, int>>();
		}

		void InitDefaultFormatting()
		{
			base.InitDefaultNumberFormatting();
			this.InitDefaultIndexedColors();
		}

		void InitDefaultIndexedColors()
		{
			for (int i = 0; i < IndexedColors.PredefinedIndexedColors.Length; i++)
			{
				base.StyleSheet.IndexedColorTable.Add(IndexedColors.PredefinedIndexedColors[i].Color, i);
			}
		}

		void ImportDefinedNames()
		{
			foreach (DefinedNameInfo definedNameInfo in base.DefinedNames)
			{
				int rowIndex = 0;
				int columnIndex = 0;
				Worksheet worksheet = (definedNameInfo.IsGlobal ? base.Workbook.Worksheets[0] : base.Workbook.Worksheets[definedNameInfo.LocalSheetId]);
				RadExpression radExpression;
				InputStringCollection inputStringCollection;
				ParseResult parseResult = RadExpression.TryParse(definedNameInfo.Value, worksheet, rowIndex, columnIndex, XlsxHelper.CultureInfo, out radExpression, out inputStringCollection, false);
				if (parseResult == ParseResult.Successful)
				{
					string refersTo = SpreadsheetCultureHelper.PrepareFormulaValue(radExpression.ToString(FormatHelper.DefaultSpreadsheetCulture));
					if (definedNameInfo.IsGlobal)
					{
						if (!DefinedName.AreNamesEqual(definedNameInfo.Name, DefinedName.PrintAreaDefinedName) && !DefinedName.AreNamesEqual(definedNameInfo.Name, DefinedName.FilterDefinedName))
						{
							base.Workbook.Names.Add(definedNameInfo.Name, refersTo, rowIndex, columnIndex, definedNameInfo.Comment, !definedNameInfo.Hidden);
						}
					}
					else if (definedNameInfo.Name == DefinedName.PrintAreaDefinedName)
					{
						this.ImportPrintArea(radExpression, worksheet);
					}
					else if (!string.Equals(DefinedName.FilterDefinedName, definedNameInfo.Name, StringComparison.OrdinalIgnoreCase))
					{
						worksheet.Names.Add(definedNameInfo.Name, refersTo, rowIndex, columnIndex, definedNameInfo.Comment, !definedNameInfo.Hidden);
					}
				}
			}
		}

		void ImportPrintArea(RadExpression expression, Worksheet worksheet)
		{
			CellReferenceRangeExpression cellReferenceRangeExpression = expression.GetValueAsConstantOrCellReference() as CellReferenceRangeExpression;
			if (cellReferenceRangeExpression == null || !cellReferenceRangeExpression.IsValid)
			{
				return;
			}
			List<CellRange> list = new List<CellRange>();
			foreach (CellReferenceRange cellReferenceRange in cellReferenceRangeExpression.CellReferenceRanges)
			{
				list.Add(new CellRange(cellReferenceRange.FromCellReference.ActualRowIndex, cellReferenceRange.FromCellReference.ActualColumnIndex, cellReferenceRange.ToCellReference.ActualRowIndex, cellReferenceRange.ToCellReference.ActualColumnIndex));
			}
			worksheet.WorksheetPageSetup.PrintArea.SetPrintArea(list);
		}

		CellStyle CreateStyleFromStyleInfo(StyleInfo styleInfo, CellStyle builtInStyle = null)
		{
			CellStyle cellStyle = new CellStyle(base.Workbook, styleInfo.Name, CellStyleCategory.Custom, true, null);
			using (new UpdateScope(new Action(cellStyle.SuspendForcingLayoutUpdate), new Action(cellStyle.ResumeForcingLayoutUpdate)))
			{
				if (builtInStyle != null)
				{
					cellStyle.CopyPropertiesFrom(builtInStyle);
				}
				FormattingRecord formattingRecord = base.StyleSheet.StyleFormattingTable[styleInfo.FormattingRecordId];
				if (formattingRecord.FillId != null)
				{
					IFill fill = base.StyleSheet.FillTable[formattingRecord.FillId.Value];
					if (!(fill is NoneFill))
					{
						cellStyle.Fill = fill;
					}
				}
				if (formattingRecord.NumberFormatId != null)
				{
					cellStyle.Format = base.StyleSheet.CellValueFormatTable[formattingRecord.NumberFormatId.Value];
				}
				if (formattingRecord.FontInfoId != null)
				{
					FontInfo fontInfo = base.StyleSheet.FontInfoTable[formattingRecord.FontInfoId.Value];
					this.ApplyFontInfoToStyle(fontInfo, cellStyle);
				}
				if (formattingRecord.BordersInfoId != null)
				{
					BordersInfo borderInfo = base.StyleSheet.BordersInfoTable[formattingRecord.BordersInfoId.Value];
					this.ApplyBorderInfoToStyle(borderInfo, cellStyle);
				}
				if (formattingRecord.HorizontalAlignment != null)
				{
					cellStyle.HorizontalAlignment = formattingRecord.HorizontalAlignment.Value;
				}
				if (formattingRecord.VerticalAlignment != null)
				{
					cellStyle.VerticalAlignment = formattingRecord.VerticalAlignment.Value;
				}
				if (formattingRecord.Indent != null)
				{
					cellStyle.Indent = formattingRecord.Indent.Value;
				}
				if (formattingRecord.WrapText != null)
				{
					cellStyle.IsWrapped = formattingRecord.WrapText.Value;
				}
				if (formattingRecord.IsLocked != null)
				{
					cellStyle.IsLocked = formattingRecord.IsLocked.Value;
				}
				if (formattingRecord.ApplyNumberFormat != null)
				{
					cellStyle.IncludeNumber = formattingRecord.ApplyNumberFormat.Value;
				}
				if (formattingRecord.ApplyAlignment != null)
				{
					cellStyle.IncludeAlignment = formattingRecord.ApplyAlignment.Value;
				}
				if (formattingRecord.ApplyFont != null)
				{
					cellStyle.IncludeFont = formattingRecord.ApplyFont.Value;
				}
				if (formattingRecord.ApplyBorder != null)
				{
					cellStyle.IncludeBorder = formattingRecord.ApplyBorder.Value;
				}
				if (formattingRecord.ApplyFill != null)
				{
					cellStyle.IncludeFill = formattingRecord.ApplyFill.Value;
				}
				if (formattingRecord.ApplyProtection != null)
				{
					cellStyle.IncludeProtection = formattingRecord.ApplyProtection.Value;
				}
			}
			return cellStyle;
		}

		void ApplyFontInfoToStyle(FontInfo fontInfo, CellStyle cellStyle)
		{
			if (fontInfo.Bold != null)
			{
				cellStyle.IsBold = fontInfo.Bold.Value;
			}
			if (fontInfo.Italic != null)
			{
				cellStyle.IsItalic = fontInfo.Italic.Value;
			}
			if (fontInfo.FontSize != null)
			{
				cellStyle.FontSize = fontInfo.FontSize.Value;
			}
			if (fontInfo.FontFamily != null)
			{
				cellStyle.FontFamily = fontInfo.FontFamily;
			}
			if (fontInfo.ForeColor != null)
			{
				cellStyle.ForeColor = fontInfo.ForeColor;
			}
			if (fontInfo.UnderlineType != null)
			{
				cellStyle.Underline = fontInfo.UnderlineType.Value;
			}
		}

		void ApplyBorderInfoToStyle(BordersInfo borderInfo, CellStyle cellStyle)
		{
			if (borderInfo.Left != null)
			{
				cellStyle.LeftBorder = borderInfo.Left;
			}
			if (borderInfo.Top != null)
			{
				cellStyle.TopBorder = borderInfo.Top;
			}
			if (borderInfo.Right != null)
			{
				cellStyle.RightBorder = borderInfo.Right;
			}
			if (borderInfo.Bottom != null)
			{
				cellStyle.BottomBorder = borderInfo.Bottom;
			}
			if (borderInfo.DiagonalUp != null)
			{
				cellStyle.DiagonalUpBorder = borderInfo.DiagonalUp;
			}
			if (borderInfo.DiagonalDown != null)
			{
				cellStyle.DiagonalDownBorder = borderInfo.DiagonalDown;
			}
		}

		readonly Dictionary<string, IResource> resourcesMapping;

		readonly DifferentialFormatsImportContext formats;

		ValueMapper<AxisGroupName, Tuple<int, int>> axesGroupNamesToAxesIndices;

		ValueMapper<ISupportAxes, Tuple<int, int>> seriesGroupsToAxesIndices;
	}
}
