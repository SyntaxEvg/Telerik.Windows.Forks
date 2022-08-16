using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Operators
{
	class OperatorInfo
	{
		public OperatorInfo(string symbol, int precedence, OperatorAssociativity associativity)
		{
			Guard.ThrowExceptionIfNullOrEmpty(symbol, "symbol");
			this.symbol = symbol;
			this.associativity = associativity;
			this.precedence = precedence;
		}

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

		readonly string symbol;

		readonly int precedence;

		readonly OperatorAssociativity associativity;
	}
}
