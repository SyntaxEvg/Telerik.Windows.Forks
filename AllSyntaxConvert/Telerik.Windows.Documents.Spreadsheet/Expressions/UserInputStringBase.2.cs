using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	abstract class UserInputStringBase<T> : UserInputStringBase where T : RadExpression
	{
		public T Expression
		{
			get
			{
				return this.expression;
			}
			set
			{
				this.expression = value;
			}
		}

		public UserInputStringBase(T expression)
		{
			this.expression = expression;
		}

		T expression;
	}
}
