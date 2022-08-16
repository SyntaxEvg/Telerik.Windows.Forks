using System;

namespace CsQuery.EquationParser.Implementation
{
	class VariableReadEventArgs : EventArgs
	{
		public VariableReadEventArgs(string name)
		{
			this.Name = name;
		}

		public IConvertible Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
			}
		}

		public Type Type { get; set; }

		public string Name { get; protected set; }

		protected IConvertible _Value;
	}
}
