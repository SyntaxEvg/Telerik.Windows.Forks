using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class ExpressionLexerContext
	{
		public ExpressionLexerContext()
		{
			this.functionStack = new Stack<bool>();
		}

		public void RegisterFunction()
		{
			this.functionStack.Push(true);
		}

		public void RegisterLeftParenthesis()
		{
			this.functionStack.Push(false);
		}

		public void RegisterRightParenthesis()
		{
			if (this.functionStack.Count > 0)
			{
				this.functionStack.Pop();
			}
		}

		public bool IsUnionOperator
		{
			get
			{
				return this.functionStack.Count == 0 || !this.functionStack.Peek();
			}
		}

		readonly Stack<bool> functionStack;
	}
}
