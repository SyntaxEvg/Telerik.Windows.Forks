using System;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class SpreadsheetNameExpression : RadExpression
	{
		internal bool IsNameReferringWorbook
		{
			get
			{
				return this.qualifierInfo.HasQualifier && this.qualifierInfo.IsQualifierReferringWorkbook;
			}
		}

		internal bool IsNameReferringWorksheet
		{
			get
			{
				return this.qualifierInfo.HasQualifier && !this.qualifierInfo.IsQualifierReferringWorkbook;
			}
		}

		internal SpreadsheetNameExpression(ExpressionQualifierInfo qualifierInfo, CellIndex cellIndex, string spreadsheetName)
			: this(qualifierInfo, cellIndex.RowIndex, cellIndex.ColumnIndex, spreadsheetName)
		{
		}

		internal SpreadsheetNameExpression(ExpressionQualifierInfo qualifierInfo, int rowIndex, int columnIndex, string spreadsheetName)
		{
			Guard.ThrowExceptionIfNull<ExpressionQualifierInfo>(qualifierInfo, "qualifierInfo");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			Guard.ThrowExceptionIfNullOrEmpty(spreadsheetName, "spreadsheetName");
			this.qualifierInfo = qualifierInfo;
			this.spreadsheetName = spreadsheetName;
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.AttachWeekEventNamesUpdated();
		}

		void AttachWeekEventNamesUpdated()
		{
			RadWeakEventListener<SpreadsheetNameExpression, NameManager, EventArgs> radWeakEventListener = new RadWeakEventListener<SpreadsheetNameExpression, NameManager, EventArgs>(this, this.qualifierInfo.Workbook.NameManager);
			this.qualifierInfo.Workbook.NameManager.Changed += radWeakEventListener.OnEvent;
			radWeakEventListener.OnEventAction = new Action<SpreadsheetNameExpression, object, EventArgs>(SpreadsheetNameExpression.InnerValueInvalidatedWeakEventAction);
		}

		static void InnerValueInvalidatedWeakEventAction(SpreadsheetNameExpression instance, object source, EventArgs e)
		{
			instance.InnerValueInvalidated(instance, e);
		}

		void InnerValueInvalidated(object sender, EventArgs e)
		{
			base.InvalidateValue();
		}

		protected override RadExpression GetValueOverride()
		{
			DefinedName definedName = this.qualifierInfo.Workbook.NameManager.FindSpreadsheetName(this.qualifierInfo.Qualifier, this.spreadsheetName, this.qualifierInfo.CurrentWorksheet, this.qualifierInfo.Workbook) as DefinedName;
			if (definedName != null)
			{
				RadExpression value = definedName.FormulaCellValue.Value;
				RadExpression value2 = value.GetValue();
				if (value2 != ErrorExpressions.NameError && value2 != ErrorExpressions.CyclicReference)
				{
					ExpressionCloneAndTranslateContext context = new ExpressionCloneAndTranslateContext(this.qualifierInfo.CurrentWorksheet, this.rowIndex, this.columnIndex, false);
					RadExpression radExpression = value.CloneAndTranslate(context);
					if (!radExpression.Equals(this.expressionCache))
					{
						if (this.expressionCache != null)
						{
							this.expressionCache.ValueInvalidated -= this.InnerValueInvalidated;
						}
						this.expressionCache = radExpression;
						this.expressionCache.ValueInvalidated += this.InnerValueInvalidated;
					}
					return this.expressionCache.GetValueAsConstantOrCellReference();
				}
			}
			return ErrorExpressions.NameError;
		}

		internal override RadExpression CloneAndTranslate(ExpressionCloneAndTranslateContext context)
		{
			Worksheet worksheet = context.Worksheet;
			ExpressionQualifierInfo expressionQualifierInfo = this.qualifierInfo;
			if (this.qualifierInfo.CurrentWorksheet != worksheet)
			{
				expressionQualifierInfo = new ExpressionQualifierInfo(context.Worksheet.Workbook, worksheet, this.qualifierInfo.Qualifier);
			}
			RadExpression radExpression = new SpreadsheetNameExpression(expressionQualifierInfo, context.RowIndex, context.ColumnIndex, this.spreadsheetName);
			if (context.NewFormatCollection != null && context.OldFormatCollection != null)
			{
				for (int i = 0; i < context.OldFormatCollection.Count; i++)
				{
					DefinedNameInputString definedNameInputString = context.OldFormatCollection[i] as DefinedNameInputString;
					if (definedNameInputString != null && object.ReferenceEquals(this, definedNameInputString.Expression))
					{
						((DefinedNameInputString)context.NewFormatCollection[i]).Expression = (SpreadsheetNameExpression)radExpression;
					}
				}
			}
			return radExpression;
		}

		internal override void Translate(ExpressionTranslateContext context)
		{
			if (context.IsWorksheetRenamed && this.IsNameReferringWorksheet)
			{
				this.qualifierInfo.UpdateQualifier(TextHelper.EncodeWorksheetName(context.RenamedWorksheet.Name, false));
			}
			else if (context.IsNameRenamed)
			{
				if (this.spreadsheetName.Equals(context.OldSpreadsheetName))
				{
					this.spreadsheetName = context.NewSpreadsheetName;
				}
			}
			else if (context.IsWorkbookRenamed && this.qualifierInfo.IsQualifierReferringWorkbook)
			{
				this.qualifierInfo.UpdateQualifier(TextHelper.EncodeWorksheetName(context.RenamedWorkbook.Name, false));
			}
			base.InvalidateValue();
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.qualifierInfo.HasQualifier)
			{
				stringBuilder.Append(this.qualifierInfo.EscapedQualifier);
				stringBuilder.Append("!");
			}
			stringBuilder.Append(this.spreadsheetName);
			return stringBuilder.ToString().ToString(cultureInfo.CultureInfo);
		}

		internal override bool SimilarEquals(object obj)
		{
			return this.Equals(obj);
		}

		public override bool Equals(object obj)
		{
			SpreadsheetNameExpression spreadsheetNameExpression = obj as SpreadsheetNameExpression;
			return spreadsheetNameExpression != null && TelerikHelper.EqualsOfT<int>(this.rowIndex, spreadsheetNameExpression.rowIndex) && TelerikHelper.EqualsOfT<int>(this.columnIndex, spreadsheetNameExpression.columnIndex) && TelerikHelper.EqualsOfT<ExpressionQualifierInfo>(this.qualifierInfo, spreadsheetNameExpression.qualifierInfo);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.rowIndex.GetHashCode(), this.columnIndex.GetHashCode(), this.qualifierInfo.GetHashCodeOrZero());
		}

		readonly ExpressionQualifierInfo qualifierInfo;

		readonly int rowIndex;

		readonly int columnIndex;

		string spreadsheetName;

		RadExpression expressionCache;
	}
}
