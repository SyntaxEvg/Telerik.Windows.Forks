using System;
using Telerik.Windows.Documents.Core.PostScript;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Fixed.Model.Common.Functions
{
	class PostScriptCalculatorFunction : FunctionBase
	{
		public PostScriptCalculatorFunction(byte[] data, double[] domain, double[] range)
			: base(domain, range)
		{
			this.data = data ?? new byte[0];
			Interpreter interpreter = new Interpreter();
			interpreter.Execute(data);
			this.procedure = interpreter.Operands.GetLastAs<PostScriptArray>();
		}

		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		public override FunctionType FunctionType
		{
			get
			{
				return FunctionType.PostScriptCalculator;
			}
		}

		protected override bool IsRangeRequired
		{
			get
			{
				return true;
			}
		}

		public override byte[] GetFunctionData()
		{
			return this.Data;
		}

		protected override double[] ExecuteOverride(double[] clippedInputValues)
		{
			Interpreter interpreter = new Interpreter();
			interpreter.Operands = new OperandsCollection();
			foreach (double num in clippedInputValues)
			{
				interpreter.Operands.AddLast(num);
			}
			interpreter.ExecuteProcedure(this.procedure);
			int num2 = base.Range.Length / 2;
			double[] array = new double[num2];
			for (int j = 0; j < num2; j++)
			{
				array[j] = interpreter.Operands.GetFirstAsReal();
			}
			return array;
		}

		readonly byte[] data;

		readonly PostScriptArray procedure;
	}
}
