using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery.EquationParser.Implementation.Functions;
using CsQuery.StringScanner;

namespace CsQuery.EquationParser.Implementation
{
	class EquationParserEngine : IEquationParser
	{
		protected bool IsTyped { get; set; }

		protected HashSet<IVariable> UniqueVariables
		{
			get
			{
				if (this._UniqueVariables == null)
				{
					this._UniqueVariables = new HashSet<IVariable>();
				}
				return this._UniqueVariables;
			}
		}

		public string Error { get; set; }

		public bool TryParse(string text, out IOperand operand)
		{
			bool result;
			try
			{
				operand = this.Parse(text);
				result = true;
			}
			catch (Exception ex)
			{
				operand = null;
				this.Error = ex.Message;
				this.Clause = null;
				result = false;
			}
			return result;
		}

		public IOperand Parse(string text)
		{
			return this.Parse<IConvertible>(text);
		}

		public IOperand Parse<T>(string text) where T : IConvertible
		{
			this.IsTyped = typeof(T) != typeof(IConvertible);
			this.scanner = Scanner.Create(text);
			this.Clause = (this.IsTyped ? new Sum<T>() : new Sum());
			IOperand operand = this.GetOperand<T>();
			this.Clause.AddOperand(operand);
			IOperation operation = this.Clause;
			while (!this.ParseEnd)
			{
				IOperator operation2 = this.GetOperation();
				IOperand operand2 = this.GetOperand<T>();
				if (operation2.AssociationType == operation.AssociationType)
				{
					operation.AddOperand(operand2, operation2.IsInverted);
				}
				else
				{
					switch (operation2.AssociationType)
					{
					case AssociationType.Addition:
						if (!object.ReferenceEquals(operation, this.Clause))
						{
							operation = this.Clause;
						}
						operation.AddOperand(operand2, operation2.IsInverted);
						break;
					case AssociationType.Multiplicaton:
					{
						IOperation function = operation2.GetFunction();
						function.AddOperand(operand);
						function.AddOperand(operand2, operation2.IsInverted);
						this.Clause.ReplaceLastOperand(function);
						operation = function;
						break;
					}
					case AssociationType.Power:
					{
						IOperation function = operation2.GetFunction();
						function.AddOperand(operand);
						function.AddOperand(operand2, operation2.IsInverted);
						this.Clause.ReplaceLastOperand(function);
						break;
					}
					case AssociationType.Function:
					{
						IOperation function = operation2.GetFunction();
						function.AddOperand(operand2, operation2.IsInverted);
						this.Clause.ReplaceLastOperand(function);
						break;
					}
					default:
						throw new NotImplementedException("Unknown association type.");
					}
				}
				operand = operand2;
			}
			this.Error = "";
			return this.Clause;
		}

		protected IOperand GetOperand<T>() where T : IConvertible
		{
			string text = "";
			IOperand operand = null;
			this.scanner.SkipWhitespace();
			if (this.scanner.Current == '-')
			{
				this.scanner.Next();
				if (this.scanner.Finished)
				{
					throw new ArgumentException("Unexpected end of string found, expected an operand (a number or variable name)");
				}
				if (CharacterData.IsType(this.scanner.Current, CharacterType.Number))
				{
					text += "-";
				}
				else
				{
					operand = new Literal<T>(-1);
				}
			}
			else if (this.scanner.Current == '+')
			{
				this.scanner.Next();
			}
			if (operand == null)
			{
				if (this.scanner.Info.Numeric)
				{
					text += this.scanner.Get(MatchFunctions.Number(false));
					double num;
					if (!double.TryParse(text, out num))
					{
						throw new InvalidCastException("Unable to parse number from '" + text + "'");
					}
					operand = (this.IsTyped ? new Literal<T>(num) : new Literal(num));
				}
				else if (this.scanner.Info.Alpha)
				{
					text += this.scanner.GetAlpha();
					if (this.scanner.CurrentOrEmpty == "(")
					{
						IFunction function = Utils.GetFunction<T>(text);
						IStringScanner stringScanner = this.scanner.ExpectBoundedBy('(', true).ToNewScanner("{0},");
						while (!stringScanner.Finished)
						{
							string text2 = stringScanner.Get(MatchFunctions.BoundedBy(null, ",", false));
							EquationParserEngine equationParserEngine = new EquationParserEngine();
							IOperand operand2 = equationParserEngine.Parse<T>(text2);
							function.AddOperand(operand2);
						}
						this.CacheVariables(function);
						operand = function;
					}
					else
					{
						IVariable variable = this.GetVariable<T>(text);
						operand = variable;
					}
				}
				else
				{
					if (this.scanner.Current != '(')
					{
						throw new ArgumentException("Unexpected character '" + this.scanner.Match + "' found, expected an operand (a number or variable name)");
					}
					string text3 = this.scanner.Get(MatchFunctions.BoundedBy("(", null, false));
					EquationParserEngine equationParserEngine2 = new EquationParserEngine();
					equationParserEngine2.Parse<T>(text3);
					operand = equationParserEngine2.Clause;
					this.CacheVariables(operand);
				}
			}
			this.scanner.SkipWhitespace();
			this.ParseEnd = this.scanner.Finished;
			return operand;
		}

		protected IOperator GetOperation()
		{
			IOperator result;
			if (this.scanner.Info.Alpha || this.scanner.Current == '(')
			{
				result = new Operator("*");
			}
			else
			{
				result = new Operator(this.scanner.Get(new Func<int, char, bool>(MatchFunctions.Operator)));
			}
			return result;
		}

		protected IVariable GetVariable<T>(string name) where T : IConvertible
		{
			IVariable variable = this.UniqueVariables.FirstOrDefault((IVariable item) => item.Name == name);
			if (variable == null)
			{
				Variable variable2 = (this.IsTyped ? new Variable<T>(name) : new Variable(name));
				variable = variable2;
				this.CacheVariables(variable);
			}
			return variable;
		}

		protected void CacheVariables(IOperand oper)
		{
			if (oper is IVariableContainer)
			{
				foreach (IVariable item in ((IVariableContainer)oper).Variables)
				{
					this.UniqueVariables.Add(item);
				}
			}
		}

		protected HashSet<IVariable> _UniqueVariables;

		protected int CurPos;

		protected bool ParseEnd;

		protected IStringScanner scanner;

		protected IOperation Clause;
	}
}
