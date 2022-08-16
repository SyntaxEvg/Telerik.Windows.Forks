using System;

namespace CsQuery.EquationParser
{
	interface IEquation : IOperand, IConvertible, ICloneable, IVariableContainer
	{
		IOrderedDictionary<string, IConvertible> VariableValues { get; }

		void SetVariable(string name, IConvertible value);

		void SetVariable<U>(string name, U value) where U : IConvertible;

		IConvertible GetValue(params IConvertible[] values);

		bool TryGetValue(out IConvertible result, params IConvertible[] values);

		bool TryGetValue(out IConvertible result);

		IEquation Clone();

		IOperand Operand { get; set; }

		void Compile();
	}
}
