using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public abstract class OperatorExpression : RadExpression
	{
		public abstract OperatorInfo OperatorInfo { get; }

		internal virtual string GetOperandAsString(RadExpression expression, SpreadsheetCultureHelper cultureInfo)
		{
			Guard.ThrowExceptionIfNull<RadExpression>(expression, "expression");
			Guard.ThrowExceptionIfNull<SpreadsheetCultureHelper>(cultureInfo, "cultureInfo");
			string format = "{0}";
			OperatorExpression operatorExpression = expression as OperatorExpression;
			if (operatorExpression != null)
			{
				int num = OperatorInfos.ComparePrecedence(this.OperatorInfo.Precedence, operatorExpression.OperatorInfo.Precedence);
				if (num <= 0 && (!(this.OperatorInfo.Symbol == operatorExpression.OperatorInfo.Symbol) || !this.IsOperatorAssociative()))
				{
					format = "({0})";
				}
			}
			return string.Format(format, expression.ToString(cultureInfo));
		}

		bool IsOperatorAssociative()
		{
			return (this.OperatorInfo.Symbol != "-" || this.OperatorInfo.Associativity == OperatorAssociativity.Right) && this.OperatorInfo.Symbol != "^" && this.OperatorInfo.Symbol != "/";
		}
	}
}
