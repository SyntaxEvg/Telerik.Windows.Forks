using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class OperatorInfo
	{
		public string Symbol
		{
			get
			{
				return this.symbol;
			}
		}

		public int Precedence
		{
			get
			{
				return this.precedence;
			}
		}

		public OperatorAssociativity Associativity
		{
			get
			{
				return this.associativity;
			}
		}

		public OperatorInfo(string symbol, int precedence, OperatorAssociativity associativity)
		{
			Guard.ThrowExceptionIfNullOrEmpty(symbol, "symbol");
			this.symbol = symbol;
			this.associativity = associativity;
			this.precedence = precedence;
		}

		readonly string symbol;

		readonly int precedence;

		readonly OperatorAssociativity associativity;
	}
}
