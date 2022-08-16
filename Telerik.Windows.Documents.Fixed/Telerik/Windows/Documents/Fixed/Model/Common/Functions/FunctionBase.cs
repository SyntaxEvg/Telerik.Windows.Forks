using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Common.Functions
{
	abstract class FunctionBase
	{
		protected FunctionBase(double[] domain, double[] range)
		{
			Guard.ThrowExceptionIfNull<double[]>(domain, "domain");
			if (this.IsRangeRequired)
			{
				Guard.ThrowExceptionIfNull<double[]>(range, "range");
			}
			this.domain = domain;
			this.range = range;
		}

		public abstract FunctionType FunctionType { get; }

		public double[] Domain
		{
			get
			{
				return this.domain;
			}
		}

		public double[] Range
		{
			get
			{
				return this.range;
			}
		}

		public int OutputValuesCount
		{
			get
			{
				return this.Range.Length / 2;
			}
		}

		protected abstract bool IsRangeRequired { get; }

		public static double Interpolate(double x, double xMin, double xMax, double yMin, double yMax)
		{
			return yMin + (x - xMin) * ((yMax - yMin) / (xMax - xMin));
		}

		public static double ClipValue(double value, double min, double max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		public double[] Execute(double[] inputValues)
		{
			Guard.ThrowExceptionIfNull<double[]>(inputValues, "inputValues");
			double[] clippedInputValues = this.ClipInputValues(inputValues);
			double[] outputValues = this.ExecuteOverride(clippedInputValues);
			return this.ClipOutputValues(outputValues);
		}

		public virtual byte[] GetFunctionData()
		{
			throw new InvalidCastException(string.Format("Function type {0} cannot provide data.", this.FunctionType));
		}

		protected abstract double[] ExecuteOverride(double[] clippedInputValues);

		double[] ClipInputValues(double[] inputValues)
		{
			double[] array = new double[inputValues.Length];
			for (int i = 0; i < inputValues.Length; i++)
			{
				double min = this.Domain[2 * i];
				double max = this.Domain[2 * i + 1];
				array[i] = FunctionBase.ClipValue(inputValues[i], min, max);
			}
			return array;
		}

		double[] ClipOutputValues(double[] outputValues)
		{
			if (this.Range == null)
			{
				return outputValues;
			}
			double[] array = new double[outputValues.Length];
			for (int i = 0; i < outputValues.Length; i++)
			{
				double min = this.Range[2 * i];
				double max = this.Range[2 * i + 1];
				array[i] = FunctionBase.ClipValue(outputValues[i], min, max);
			}
			return array;
		}

		double[] domain;

		double[] range;
	}
}
