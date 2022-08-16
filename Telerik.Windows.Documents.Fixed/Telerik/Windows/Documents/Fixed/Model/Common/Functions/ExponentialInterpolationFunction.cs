using System;

namespace Telerik.Windows.Documents.Fixed.Model.Common.Functions
{
	class ExponentialInterpolationFunction : FunctionBase
	{
		public ExponentialInterpolationFunction(double n, double[] c0, double[] c1, double[] domain, double[] range)
			: base(domain, range)
		{
			this.n = n;
			double[] array = c0;
			if (c0 == null)
			{
				double[] array2 = new double[1];
				array = array2;
			}
			this.c0Array = array;
			this.c1Array = c1 ?? new double[] { 1.0 };
		}

		public double N
		{
			get
			{
				return this.n;
			}
		}

		public double[] C0
		{
			get
			{
				return this.c0Array;
			}
		}

		public double[] C1
		{
			get
			{
				return this.c1Array;
			}
		}

		public override FunctionType FunctionType
		{
			get
			{
				return FunctionType.ExponentialInterpolation;
			}
		}

		protected override bool IsRangeRequired
		{
			get
			{
				return false;
			}
		}

		int SingleResultValuesCount
		{
			get
			{
				return this.C0.Length;
			}
		}

		protected override double[] ExecuteOverride(double[] clippedInputValues)
		{
			double[] array = new double[clippedInputValues.Length * this.SingleResultValuesCount];
			for (int i = 0; i < clippedInputValues.Length; i++)
			{
				double[] array2 = this.ExecuteSingle(clippedInputValues[i]);
				for (int j = 0; j < array2.Length; j++)
				{
					array[i * this.SingleResultValuesCount + j] = array2[j];
				}
			}
			return array;
		}

		double[] ExecuteSingle(double x)
		{
			double[] array = new double[this.SingleResultValuesCount];
			for (int i = 0; i < array.Length; i++)
			{
				double num = this.C0[i];
				double num2 = this.C1[i];
				double num3 = num + Math.Pow(x, this.N) * (num2 - num);
				array[i] = num3;
			}
			return array;
		}

		readonly double n;

		readonly double[] c0Array;

		readonly double[] c1Array;
	}
}
