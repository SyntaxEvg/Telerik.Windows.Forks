using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Common.Functions
{
	class StitchingFunction : FunctionBase
	{
		public StitchingFunction(FunctionBase[] functions, double[] bounds, double[] encode, double[] domain, double[] range)
			: base(domain, range)
		{
			Guard.ThrowExceptionIfNull<FunctionBase[]>(functions, "functions");
			Guard.ThrowExceptionIfNull<double[]>(bounds, "bounds");
			Guard.ThrowExceptionIfNull<double[]>(encode, "encode");
			this.functions = functions;
			this.bounds = bounds;
			this.encode = encode;
		}

		public override FunctionType FunctionType
		{
			get
			{
				return FunctionType.Stitching;
			}
		}

		public double[] Encode
		{
			get
			{
				return this.encode;
			}
		}

		public double[] Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		public FunctionBase[] Functions
		{
			get
			{
				return this.functions;
			}
		}

		protected override bool IsRangeRequired
		{
			get
			{
				return false;
			}
		}

		protected override double[] ExecuteOverride(double[] clippedInputValues)
		{
			double x = clippedInputValues[0];
			int num;
			FunctionBase functionBase = this.FindFunction(x, out num);
			double xMin;
			if (num > 0)
			{
				xMin = this.Bounds[num - 1];
			}
			else
			{
				xMin = base.Domain[0];
			}
			double xMax;
			if (num < this.Functions.Length - 1)
			{
				xMax = this.Bounds[num];
			}
			else
			{
				xMax = base.Domain[1];
			}
			double yMin = this.Encode[2 * num];
			double yMax = this.Encode[2 * num + 1];
			double[] inputValues = new double[] { FunctionBase.Interpolate(x, xMin, xMax, yMin, yMax) };
			return functionBase.Execute(inputValues);
		}

		FunctionBase FindFunction(double x, out int index)
		{
			for (int i = 0; i < this.Bounds.Length; i++)
			{
				double num = this.Bounds[i];
				if (x < num)
				{
					index = i;
					return this.Functions[i];
				}
			}
			index = this.Functions.Length - 1;
			return this.Functions[index];
		}

		readonly FunctionBase[] functions;

		readonly double[] bounds;

		readonly double[] encode;
	}
}
