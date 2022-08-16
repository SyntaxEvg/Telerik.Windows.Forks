using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class DataValidationElement : WorksheetElementBase
	{
		public DataValidationElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.type = base.RegisterAttribute<ConvertedOpenXmlAttribute<DataValidationRuleType>>(new ConvertedOpenXmlAttribute<DataValidationRuleType>("type", XlsxConverters.DataValidationRuleTypeConverter, DataValidationRuleType.None, false));
			this.errorStyle = base.RegisterAttribute<ConvertedOpenXmlAttribute<ErrorStyle>>(new ConvertedOpenXmlAttribute<ErrorStyle>("errorStyle", XlsxConverters.ErrorStyleConverter, ErrorStyle.Stop, false));
			this.comparisonOperator = base.RegisterAttribute<ConvertedOpenXmlAttribute<ComparisonOperator>>(new ConvertedOpenXmlAttribute<ComparisonOperator>("operator", XlsxConverters.ComparisonOperatorConverter, ComparisonOperator.Between, false));
			this.allowBlank = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("allowBlank", 0, false));
			this.showDropDown = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("showDropDown", 0, false));
			this.showInputMessage = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("showInputMessage", 0, false));
			this.showErrorMessage = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("showErrorMessage", 0, false));
			this.errorTitle = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("errorTitle", string.Empty, false));
			this.error = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("error", string.Empty, false));
			this.promptTitle = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("promptTitle", string.Empty, false));
			this.prompt = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("prompt", string.Empty, false));
			this.sqref = base.RegisterAttribute<ConvertedOpenXmlAttribute<CellRefRangeSequence>>(new ConvertedOpenXmlAttribute<CellRefRangeSequence>("sqref", XlsxConverters.CellRefRangeSequenceConverter, true));
			this.formula1Element = base.RegisterChildElement<Formula1Element>("formula1");
			this.formula2Element = base.RegisterChildElement<Formula2Element>("formula2");
			this.sqrefElement = base.RegisterChildElement<SqRefElement>("sqref");
			this.copyActions = new List<Action<IXlsxWorksheetExportContext, IDataValidationRule, CellIndex, IEnumerable<CellRange>>>();
			this.InitializeCopyActions();
			this.typeToDataValidationRuleType = new Dictionary<Type, DataValidationRuleType>();
			this.InitializeTypeToDataValidationRuleType();
			this.ruleTypeToFactory = new Dictionary<DataValidationRuleType, Func<IXlsxWorksheetImportContext, IDataValidationRule>>();
			this.InitializeRuleTypeToFactory();
		}

		public override string ElementName
		{
			get
			{
				return "dataValidation";
			}
		}

		public bool AllowBlank
		{
			get
			{
				return this.allowBlank.Value != 0;
			}
			set
			{
				this.allowBlank.Value = (value ? 1 : 0);
			}
		}

		public string Error
		{
			get
			{
				return this.error.Value;
			}
			set
			{
				this.error.Value = value;
			}
		}

		public ErrorStyle ErrorStyle
		{
			get
			{
				return this.errorStyle.Value;
			}
			set
			{
				this.errorStyle.Value = value;
			}
		}

		public string ErrorTitle
		{
			get
			{
				return this.errorTitle.Value;
			}
			set
			{
				this.errorTitle.Value = value;
			}
		}

		public ComparisonOperator ComparisonOperator
		{
			get
			{
				return this.comparisonOperator.Value;
			}
			set
			{
				this.comparisonOperator.Value = value;
			}
		}

		public string Prompt
		{
			get
			{
				return this.prompt.Value;
			}
			set
			{
				this.prompt.Value = value;
			}
		}

		public string PromptTitle
		{
			get
			{
				return this.promptTitle.Value;
			}
			set
			{
				this.promptTitle.Value = value;
			}
		}

		public bool ShowDropDown
		{
			get
			{
				return this.showDropDown.Value == 0;
			}
			set
			{
				this.showDropDown.Value = (value ? 0 : 1);
			}
		}

		public bool ShowErrorMessage
		{
			get
			{
				return this.showErrorMessage.Value != 0;
			}
			set
			{
				this.showErrorMessage.Value = (value ? 1 : 0);
			}
		}

		public bool ShowInputMessage
		{
			get
			{
				return this.showInputMessage.Value != 0;
			}
			set
			{
				this.showInputMessage.Value = (value ? 1 : 0);
			}
		}

		public CellRefRangeSequence SqRef
		{
			get
			{
				CellRefRangeSequence cellRefRangeSequence = this.sqref.Value;
				if (cellRefRangeSequence == null)
				{
					cellRefRangeSequence = new CellRefRangeSequence(this.sqrefElement.Element.InnerText);
				}
				return cellRefRangeSequence;
			}
			set
			{
				this.sqref.Value = value;
			}
		}

		public DataValidationRuleType Type
		{
			get
			{
				return this.type.Value;
			}
			set
			{
				this.type.Value = value;
			}
		}

		public Formula1Element Formula1Element
		{
			get
			{
				return this.formula1Element.Element;
			}
			set
			{
				this.formula1Element.Element = value;
			}
		}

		public Formula2Element Formula2Element
		{
			get
			{
				return this.formula2Element.Element;
			}
			set
			{
				this.formula2Element.Element = value;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, IDataValidationRule rule)
		{
			base.CreateElement(this.formula1Element);
			base.CreateElement(this.formula2Element);
			IEnumerable<CellRange> enumerable = context.GetDataValidationRuleCellRanges(rule);
			CellIndex topLeftCellIndex = null;
			foreach (CellRange cellRange in enumerable)
			{
				if (topLeftCellIndex == null)
				{
					topLeftCellIndex = cellRange.FromIndex;
				}
				else if (cellRange.FromIndex.ColumnIndex <= topLeftCellIndex.ColumnIndex || cellRange.FromIndex.RowIndex <= topLeftCellIndex.RowIndex)
				{
					topLeftCellIndex = cellRange.FromIndex;
				}
			}
			enumerable = from p in enumerable
				orderby p.FromIndex == topLeftCellIndex
				select p;
			foreach (Action<IXlsxWorksheetExportContext, IDataValidationRule, CellIndex, IEnumerable<CellRange>> action in this.copyActions)
			{
				action(context, rule, topLeftCellIndex, enumerable);
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			IDataValidationRule dataValidationRule = this.ruleTypeToFactory[this.Type](context);
			foreach (CellRange cellRange in this.SqRef.CellRanges)
			{
				context.Worksheet.Cells[cellRange].SetDataValidationRule(dataValidationRule);
			}
			base.ReleaseElement(this.formula1Element);
			base.ReleaseElement(this.formula2Element);
		}

		void InitializeRuleTypeToFactory()
		{
			this.ruleTypeToFactory.Add(DataValidationRuleType.Custom, new Func<IXlsxWorksheetImportContext, IDataValidationRule>(this.CustomRuleFactory));
			this.ruleTypeToFactory.Add(DataValidationRuleType.Date, new Func<IXlsxWorksheetImportContext, IDataValidationRule>(this.DateRuleFactory));
			this.ruleTypeToFactory.Add(DataValidationRuleType.Decimal, new Func<IXlsxWorksheetImportContext, IDataValidationRule>(this.DecimalRuleFactory));
			this.ruleTypeToFactory.Add(DataValidationRuleType.List, new Func<IXlsxWorksheetImportContext, IDataValidationRule>(this.ListRuleFactory));
			this.ruleTypeToFactory.Add(DataValidationRuleType.None, new Func<IXlsxWorksheetImportContext, IDataValidationRule>(this.AnyRuleFactory));
			this.ruleTypeToFactory.Add(DataValidationRuleType.TextLength, new Func<IXlsxWorksheetImportContext, IDataValidationRule>(this.TextLengthRuleFactory));
			this.ruleTypeToFactory.Add(DataValidationRuleType.Time, new Func<IXlsxWorksheetImportContext, IDataValidationRule>(this.TimeRuleFactory));
			this.ruleTypeToFactory.Add(DataValidationRuleType.Whole, new Func<IXlsxWorksheetImportContext, IDataValidationRule>(this.WholeRuleFactory));
		}

		IDataValidationRule CustomRuleFactory(IXlsxWorksheetImportContext context)
		{
			SingleArgumentDataValidationRuleContext context2 = new SingleArgumentDataValidationRuleContext(context.Worksheet, this.SqRef.CellRanges.First<CellRange>().FromIndex);
			this.CopyPropertiesToDataValidationRuleContext(context2);
			return new CustomDataValidationRule(context2);
		}

		IDataValidationRule DateRuleFactory(IXlsxWorksheetImportContext context)
		{
			NumberDataValidationRuleContext context2 = new NumberDataValidationRuleContext(context.Worksheet, this.SqRef.CellRanges.First<CellRange>().FromIndex);
			this.CopyPropertiesToDataValidationRuleContext(context2);
			return new DateDataValidationRule(context2);
		}

		IDataValidationRule DecimalRuleFactory(IXlsxWorksheetImportContext context)
		{
			NumberDataValidationRuleContext context2 = new NumberDataValidationRuleContext(context.Worksheet, this.SqRef.CellRanges.First<CellRange>().FromIndex);
			this.CopyPropertiesToDataValidationRuleContext(context2);
			return new DecimalDataValidationRule(context2);
		}

		IDataValidationRule ListRuleFactory(IXlsxWorksheetImportContext context)
		{
			ListDataValidationRuleContext context2 = new ListDataValidationRuleContext(context.Worksheet, this.SqRef.CellRanges.First<CellRange>().FromIndex);
			this.CopyPropertiesToDataValidationRuleContext(context2);
			return new ListDataValidationRule(context2);
		}

		IDataValidationRule AnyRuleFactory(IXlsxWorksheetImportContext arg)
		{
			AnyValueDataValidationRuleContext context = new AnyValueDataValidationRuleContext();
			this.CopyPropertiesToDataValidationRuleContext(context);
			return new AnyValueDataValidationRule(context);
		}

		IDataValidationRule TextLengthRuleFactory(IXlsxWorksheetImportContext context)
		{
			NumberDataValidationRuleContext context2 = new NumberDataValidationRuleContext(context.Worksheet, this.SqRef.CellRanges.First<CellRange>().FromIndex);
			this.CopyPropertiesToDataValidationRuleContext(context2);
			return new TextLengthDataValidationRule(context2);
		}

		IDataValidationRule TimeRuleFactory(IXlsxWorksheetImportContext context)
		{
			NumberDataValidationRuleContext context2 = new NumberDataValidationRuleContext(context.Worksheet, this.SqRef.CellRanges.First<CellRange>().FromIndex);
			this.CopyPropertiesToDataValidationRuleContext(context2);
			return new TimeDataValidationRule(context2);
		}

		IDataValidationRule WholeRuleFactory(IXlsxWorksheetImportContext context)
		{
			NumberDataValidationRuleContext context2 = new NumberDataValidationRuleContext(context.Worksheet, this.SqRef.CellRanges.First<CellRange>().FromIndex);
			this.CopyPropertiesToDataValidationRuleContext(context2);
			return new WholeNumberDataValidationRule(context2);
		}

		void CopyPropertiesToDataValidationRuleContext(DataValidationRuleContextBase context)
		{
			context.ErrorAlertContent = this.Error;
			context.ErrorAlertTitle = this.ErrorTitle;
			context.ErrorStyle = this.ErrorStyle;
			context.InputMessageContent = this.Prompt;
			context.InputMessageTitle = this.PromptTitle;
			context.ShowErrorMessage = this.ShowErrorMessage;
			context.ShowInputMessage = this.ShowInputMessage;
			SingleArgumentDataValidationRuleContext singleArgumentDataValidationRuleContext = context as SingleArgumentDataValidationRuleContext;
			if (singleArgumentDataValidationRuleContext != null)
			{
				this.CopyPropertiesToSingleArgumentContext(singleArgumentDataValidationRuleContext);
			}
			ListDataValidationRuleContext listDataValidationRuleContext = context as ListDataValidationRuleContext;
			if (listDataValidationRuleContext != null)
			{
				this.CopyPropertiesToListDataValidationRuleContext(listDataValidationRuleContext);
			}
			NumberDataValidationRuleContext numberDataValidationRuleContext = context as NumberDataValidationRuleContext;
			if (numberDataValidationRuleContext != null)
			{
				this.CopyPropertiesToNumberDataValidationRuleContext(numberDataValidationRuleContext);
			}
		}

		void CopyPropertiesToSingleArgumentContext(SingleArgumentDataValidationRuleContext context)
		{
			string value = this.Formula1Element.GetValue();
			context.Argument1 = value;
			context.IgnoreBlank = this.AllowBlank;
			context.CultureInfo = XlsxHelper.CultureInfo;
		}

		void CopyPropertiesToListDataValidationRuleContext(ListDataValidationRuleContext context)
		{
			context.InCellDropdown = this.ShowDropDown;
		}

		void CopyPropertiesToNumberDataValidationRuleContext(NumberDataValidationRuleContext context)
		{
			context.ComparisonOperator = this.ComparisonOperator;
			if (this.Formula2Element != null)
			{
				string value = this.Formula2Element.GetValue();
				context.Argument2 = value;
			}
		}

		void InitializeTypeToDataValidationRuleType()
		{
			this.typeToDataValidationRuleType.Add(typeof(CustomDataValidationRule), DataValidationRuleType.Custom);
			this.typeToDataValidationRuleType.Add(typeof(DateDataValidationRule), DataValidationRuleType.Date);
			this.typeToDataValidationRuleType.Add(typeof(DecimalDataValidationRule), DataValidationRuleType.Decimal);
			this.typeToDataValidationRuleType.Add(typeof(ListDataValidationRule), DataValidationRuleType.List);
			this.typeToDataValidationRuleType.Add(typeof(AnyValueDataValidationRule), DataValidationRuleType.None);
			this.typeToDataValidationRuleType.Add(typeof(TextLengthDataValidationRule), DataValidationRuleType.TextLength);
			this.typeToDataValidationRuleType.Add(typeof(TimeDataValidationRule), DataValidationRuleType.Time);
			this.typeToDataValidationRuleType.Add(typeof(WholeNumberDataValidationRule), DataValidationRuleType.Whole);
		}

		void InitializeCopyActions()
		{
			this.copyActions.Add(new Action<IXlsxWorksheetExportContext, IDataValidationRule, CellIndex, IEnumerable<CellRange>>(this.CopyPropertiesFromDataValidationRule));
			this.copyActions.Add(new Action<IXlsxWorksheetExportContext, IDataValidationRule, CellIndex, IEnumerable<CellRange>>(this.CopyPropertiesFromSingleArgumentRule));
			this.copyActions.Add(new Action<IXlsxWorksheetExportContext, IDataValidationRule, CellIndex, IEnumerable<CellRange>>(this.CopyPropertiesFromListRule));
			this.copyActions.Add(new Action<IXlsxWorksheetExportContext, IDataValidationRule, CellIndex, IEnumerable<CellRange>>(this.CopyPropertiesFromNumberRule));
		}

		void CopyPropertiesFromDataValidationRule(IXlsxWorksheetExportContext context, IDataValidationRule rule, CellIndex topLeftCellIndex, IEnumerable<CellRange> ruleCellRanges)
		{
			this.Error = rule.ErrorAlertContent;
			this.ErrorStyle = rule.ErrorStyle;
			this.ErrorTitle = rule.ErrorAlertTitle;
			this.Prompt = rule.InputMessageContent;
			this.PromptTitle = rule.InputMessageTitle;
			this.ShowErrorMessage = rule.ShowErrorMessage;
			this.ShowInputMessage = rule.ShowInputMessage;
			this.SqRef = new CellRefRangeSequence(from range in ruleCellRanges
				select new CellRefRange(range));
			this.Type = this.typeToDataValidationRuleType[rule.GetType()];
		}

		void CopyPropertiesFromSingleArgumentRule(IXlsxWorksheetExportContext context, IDataValidationRule rule, CellIndex topLeftCellIndex, IEnumerable<CellRange> ruleCellRanges)
		{
			SingleArgumentDataValidationRuleBase singleArgumentDataValidationRuleBase = rule as SingleArgumentDataValidationRuleBase;
			if (singleArgumentDataValidationRuleBase != null)
			{
				this.AllowBlank = singleArgumentDataValidationRuleBase.IgnoreBlank;
				this.Formula1Element.InnerText = this.GetFormulaString(singleArgumentDataValidationRuleBase.Argument1, context, topLeftCellIndex, singleArgumentDataValidationRuleBase);
			}
		}

		void CopyPropertiesFromListRule(IXlsxWorksheetExportContext context, IDataValidationRule rule, CellIndex topLeftCellIndex, IEnumerable<CellRange> ruleCellRanges)
		{
			ListDataValidationRule listDataValidationRule = rule as ListDataValidationRule;
			if (listDataValidationRule != null)
			{
				this.ShowDropDown = listDataValidationRule.InCellDropdown;
			}
		}

		void CopyPropertiesFromNumberRule(IXlsxWorksheetExportContext context, IDataValidationRule rule, CellIndex topLeftCellIndex, IEnumerable<CellRange> ruleCellRanges)
		{
			NumberDataValidationRuleBase numberDataValidationRuleBase = rule as NumberDataValidationRuleBase;
			if (numberDataValidationRuleBase != null)
			{
				this.ComparisonOperator = numberDataValidationRuleBase.ComparisonOperator;
				this.Formula2Element.InnerText = this.GetFormulaString(numberDataValidationRuleBase.Argument2, context, topLeftCellIndex, numberDataValidationRuleBase);
			}
		}

		string GetFormulaString(ICellValue argument, IXlsxWorksheetExportContext context, CellIndex topLeftCellIndex, SingleArgumentDataValidationRuleBase singleArgumentRule)
		{
			string text;
			if (argument.ValueType == CellValueType.Formula)
			{
				FormulaCellValue formulaCellValue = argument as FormulaCellValue;
				formulaCellValue = singleArgumentRule.GetTranslatedArgument(argument, context.Worksheet, topLeftCellIndex.RowIndex, topLeftCellIndex.ColumnIndex) as FormulaCellValue;
				text = formulaCellValue.GetExportString();
			}
			else if (argument.ValueType == CellValueType.Number)
			{
				text = XlsxHelper.CultureInfo.ToString(((NumberCellValue)argument).Value);
			}
			else
			{
				text = singleArgumentRule.GetExpressionString(argument, context.Worksheet, topLeftCellIndex.RowIndex, topLeftCellIndex.ColumnIndex);
			}
			if (argument.ValueType == CellValueType.Text)
			{
				if (singleArgumentRule is ListDataValidationRule)
				{
					text = text.Replace(FormatHelper.DefaultSpreadsheetCulture.ListSeparator, SpreadsheetCultureHelper.InvariantSpreadsheetCultureInfo.ListSeparator);
				}
				text = string.Format("\"{0}\"", text);
			}
			return text;
		}

		readonly IntOpenXmlAttribute allowBlank;

		readonly OpenXmlAttribute<string> error;

		readonly ConvertedOpenXmlAttribute<ErrorStyle> errorStyle;

		readonly OpenXmlAttribute<string> errorTitle;

		readonly ConvertedOpenXmlAttribute<ComparisonOperator> comparisonOperator;

		readonly OpenXmlAttribute<string> prompt;

		readonly OpenXmlAttribute<string> promptTitle;

		readonly IntOpenXmlAttribute showDropDown;

		readonly IntOpenXmlAttribute showErrorMessage;

		readonly IntOpenXmlAttribute showInputMessage;

		readonly ConvertedOpenXmlAttribute<CellRefRangeSequence> sqref;

		readonly ConvertedOpenXmlAttribute<DataValidationRuleType> type;

		readonly OpenXmlChildElement<Formula1Element> formula1Element;

		readonly OpenXmlChildElement<Formula2Element> formula2Element;

		readonly OpenXmlChildElement<SqRefElement> sqrefElement;

		readonly List<Action<IXlsxWorksheetExportContext, IDataValidationRule, CellIndex, IEnumerable<CellRange>>> copyActions;

		readonly Dictionary<Type, DataValidationRuleType> typeToDataValidationRuleType;

		readonly Dictionary<DataValidationRuleType, Func<IXlsxWorksheetImportContext, IDataValidationRule>> ruleTypeToFactory;
	}
}
