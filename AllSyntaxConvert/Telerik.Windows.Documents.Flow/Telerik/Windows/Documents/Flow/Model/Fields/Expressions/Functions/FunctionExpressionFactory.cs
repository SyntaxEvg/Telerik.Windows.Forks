using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions.Functions
{
	static class FunctionExpressionFactory
	{
		static FunctionExpressionFactory()
		{
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("ABS", 1, (Expression[] args) => new AbsFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("AND", 2, (Expression[] args) => new AndFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("AVERAGE", 0, (Expression[] args) => new AverageFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("COUNT", 0, (Expression[] args) => new CountFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("DEFINED", 1, (Expression[] args) => new DefinedFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("INT", 1, (Expression[] args) => new IntFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("MAX", 0, (Expression[] args) => new MaxFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("MIN", 0, (Expression[] args) => new MinFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("MOD", 2, (Expression[] args) => new ModFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("NOT", 1, (Expression[] args) => new NotFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("OR", 2, (Expression[] args) => new OrFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("PRODUCT", 0, (Expression[] args) => new ProductFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("ROUND", 2, (Expression[] args) => new RoundFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("SIGN", 1, (Expression[] args) => new SignFunctionExpression(args)));
			FunctionExpressionFactory.AddFunctionInfo(new FunctionExpressionFactory.FunctionInfo("SUM", 0, (Expression[] args) => new SumFunctionExpression(args)));
		}

		public static FunctionExpression CreateFunctionExpression(string funcName, Expression[] arguments)
		{
			funcName = funcName.Trim().ToUpperInvariant();
			FunctionExpressionFactory.FunctionInfo functionInfo = null;
			if (!FunctionExpressionFactory.Functions.TryGetValue(funcName, out functionInfo))
			{
				throw new ExpressionException("Unrecognized function: " + funcName);
			}
			if (functionInfo.Arguments > 0 && functionInfo.Arguments != arguments.Length)
			{
				throw new ExpressionException("Unexpected number of function arguments.");
			}
			return functionInfo.Constructor(arguments);
		}

		static void AddFunctionInfo(FunctionExpressionFactory.FunctionInfo info)
		{
			FunctionExpressionFactory.Functions.Add(info.Name, info);
		}

		static readonly Dictionary<string, FunctionExpressionFactory.FunctionInfo> Functions = new Dictionary<string, FunctionExpressionFactory.FunctionInfo>();

		class FunctionInfo
		{
			public FunctionInfo(string name, int arguments, Func<Expression[], FunctionExpression> constructor)
			{
				this.name = name;
				this.arguments = arguments;
				this.constructor = constructor;
			}

			public int Arguments
			{
				get
				{
					return this.arguments;
				}
			}

			public string Name
			{
				get
				{
					return this.name;
				}
			}

			public Func<Expression[], FunctionExpression> Constructor
			{
				get
				{
					return this.constructor;
				}
			}

			readonly int arguments;

			readonly string name;

			readonly Func<Expression[], FunctionExpression> constructor;
		}
	}
}
