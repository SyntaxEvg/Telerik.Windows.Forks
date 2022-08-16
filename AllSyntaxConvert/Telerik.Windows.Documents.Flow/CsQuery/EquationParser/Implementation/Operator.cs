using System;
using System.Collections.Generic;
using CsQuery.EquationParser.Implementation.Functions;

namespace CsQuery.EquationParser.Implementation
{
	class Operator : IOperator, ICloneable
	{
		public Operator()
		{
		}

		public Operator(string op)
		{
			this.Set(op);
		}

		public Operator(OperationType op)
		{
			this._OperationType = op;
		}

		public static implicit operator Operator(string op)
		{
			return new Operator(op);
		}

		public static IEnumerable<string> Operators
		{
			get
			{
				return Operator._Operators;
			}
		}

		public bool IsInverted
		{
			get
			{
				return this.OperationType == OperationType.Subtraction || this.OperationType == OperationType.Division;
			}
		}

		public AssociationType AssociationType
		{
			get
			{
				switch (this.OperationType)
				{
				case OperationType.Addition:
				case OperationType.Subtraction:
					return AssociationType.Addition;
				case OperationType.Multiplication:
				case OperationType.Division:
					return AssociationType.Multiplicaton;
				case OperationType.Modulus:
				case OperationType.Power:
					return AssociationType.Power;
				default:
					throw new NotImplementedException("Unknown operation type, can't determine association");
				}
			}
		}

		public OperationType OperationType
		{
			get
			{
				return this._OperationType;
			}
		}

		public IOperation GetFunction()
		{
			switch (this.OperationType)
			{
			case OperationType.Addition:
			case OperationType.Subtraction:
				return new Sum();
			case OperationType.Multiplication:
			case OperationType.Division:
				return new Product();
			case OperationType.Power:
				return new Power();
			}
			throw new NotImplementedException("Not yet supported");
		}

		public void Set(string op)
		{
			if (!this.TrySet(op))
			{
				throw new ArgumentException("'" + op + "' is not a valid operator.");
			}
		}

		public bool TrySet(string value)
		{
			if (!Operator.ValidOperators.Contains(value))
			{
				return false;
			}
			if (value != null)
			{
				if (!(value == "+"))
				{
					if (!(value == "-"))
					{
						if (!(value == "*"))
						{
							if (!(value == "/"))
							{
								if (!(value == "^"))
								{
									if (value == "%")
									{
										this._OperationType = OperationType.Modulus;
									}
								}
								else
								{
									this._OperationType = OperationType.Power;
								}
							}
							else
							{
								this._OperationType = OperationType.Division;
							}
						}
						else
						{
							this._OperationType = OperationType.Multiplication;
						}
					}
					else
					{
						this._OperationType = OperationType.Subtraction;
					}
				}
				else
				{
					this._OperationType = OperationType.Addition;
				}
			}
			return true;
		}

		public IOperator Clone()
		{
			return new Operator(this.OperationType);
		}

		public override string ToString()
		{
			return Operator._Operators[this.OperationType - OperationType.Addition];
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		protected static List<string> _Operators = new List<string>(new string[] { "+", "-", "*", "/", "%", "^" });

		protected static HashSet<string> ValidOperators = new HashSet<string>(Operator.Operators);

		protected OperationType _OperationType;
	}
}
