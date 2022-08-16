using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Common.Functions
{
	class SampledFunction : FunctionBase
	{
		public SampledFunction(int[][] samples, int[] size, int bitsPerSample, double[] domain, double[] range, double[] encode, double[] decode)
			: base(domain, range)
		{
			Guard.ThrowExceptionIfNull<int[][]>(samples, "samples");
			SampledFunction.GuardRequiredInitializationValues(size, bitsPerSample);
			this.samples = samples;
			this.size = size;
			this.bitsPerSample = bitsPerSample;
			this.encode = encode;
			this.decode = decode;
		}

		public SampledFunction(byte[] data, int[] size, int bitsPerSample, double[] domain, double[] range, double[] encode, double[] decode)
			: base(domain, range)
		{
			SampledFunction.GuardRequiredInitializationValues(size, bitsPerSample);
			this.data = data ?? new byte[0];
			this.size = size;
			this.bitsPerSample = bitsPerSample;
			this.encode = encode;
			this.decode = decode;
		}

		public override FunctionType FunctionType
		{
			get
			{
				return FunctionType.Sampled;
			}
		}

		public double[] Encode
		{
			get
			{
				return this.encode;
			}
		}

		public double[] Decode
		{
			get
			{
				return this.decode;
			}
		}

		public int[] Size
		{
			get
			{
				return this.size;
			}
		}

		public int BitsPerSample
		{
			get
			{
				return this.bitsPerSample;
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
			this.EnsureData();
			return this.data;
		}

		protected override double[] ExecuteOverride(double[] clippedInputValues)
		{
			int[] array = new int[clippedInputValues.Length];
			double[] array2 = new double[clippedInputValues.Length];
			int num = 0;
			for (int i = 0; i < clippedInputValues.Length; i++)
			{
				double xMin = base.Domain[2 * i];
				double xMax = base.Domain[2 * i + 1];
				double yMin = this.GetEncode(2 * i);
				double yMax = this.GetEncode(2 * i + 1);
				int num2 = this.Size[i];
				double x = clippedInputValues[i];
				double value = FunctionBase.Interpolate(x, xMin, xMax, yMin, yMax);
				double num3 = FunctionBase.ClipValue(value, 0.0, (double)(num2 - 1));
				int num4 = (int)num3;
				array[i] = num4;
				double num5 = num3 - (double)num4;
				if (SampledFunction.IsNonZeroInterpolationCoeficient(num5))
				{
					array2[i] = num5;
					num++;
				}
			}
			return this.CalculateMultidimensionalLinearInterpolationResult(array, array2, num);
		}

		static bool IsNonZeroInterpolationCoeficient(double interpolationCoeficient)
		{
			return interpolationCoeficient != 0.0;
		}

		static void GuardSupportedDimensionsCount(int nonZeroInterpolationCoeficientsCount)
		{
			if (nonZeroInterpolationCoeficientsCount > 31)
			{
				throw new NotSupportedException(string.Format("Functions with input dimension bigger than 31 are not supported.", new object[0]));
			}
		}

		static void GuardRequiredInitializationValues(int[] size, int bitsPerSample)
		{
			Guard.ThrowExceptionIfNull<int[]>(size, "size");
			if (bitsPerSample <= 12)
			{
				switch (bitsPerSample)
				{
				case 1:
				case 2:
				case 4:
					return;
				case 3:
					break;
				default:
					if (bitsPerSample == 8 || bitsPerSample == 12)
					{
						return;
					}
					break;
				}
			}
			else if (bitsPerSample == 16 || bitsPerSample == 24 || bitsPerSample == 32)
			{
				return;
			}
			throw new ArgumentException(string.Format("{0} is not a valid bitsPerSample value!", bitsPerSample));
		}

		double GetEncode(int index)
		{
			double result;
			if (this.Encode == null)
			{
				bool flag = (index & 1) == 0;
				if (flag)
				{
					result = 0.0;
				}
				else
				{
					int num = index / 2;
					int num2 = this.Size[num];
					result = (double)(num2 - 1);
				}
			}
			else
			{
				result = this.Encode[index];
			}
			return result;
		}

		double GetDecode(int index)
		{
			double[] array = this.Decode ?? base.Range;
			return array[index];
		}

		double[] CalculateMultidimensionalLinearInterpolationResult(int[] flooredEncodedValues, double[] interpolationCoeficients, int nonZeroInterpolationCoeficientsCount)
		{
			SampledFunction.GuardSupportedDimensionsCount(nonZeroInterpolationCoeficientsCount);
			this.EnsureSamples();
			double[] array = new double[base.OutputValuesCount];
			int num = 1 << nonZeroInterpolationCoeficientsCount;
			for (int i = 0; i < num; i++)
			{
				int[] array2 = new int[flooredEncodedValues.Length];
				double num2 = 1.0;
				int j = 0;
				int num3 = 0;
				while (j < array2.Length)
				{
					int num4 = flooredEncodedValues[j];
					double num5 = interpolationCoeficients[j];
					if (SampledFunction.IsNonZeroInterpolationCoeficient(num5))
					{
						bool flag = ((i >> num3) & 1) == 1;
						if (flag)
						{
							num4++;
							num2 *= num5;
						}
						else
						{
							num2 *= 1.0 - num5;
						}
						num3++;
					}
					array2[j] = num4;
					j++;
				}
				int sampleIndex = this.GetSampleIndex(array2);
				int[] array3 = this.samples[sampleIndex];
				for (int k = 0; k < array3.Length; k++)
				{
					array[k] += num2 * (double)array3[k];
				}
			}
			this.DecodeResult(array);
			return array;
		}

		void EnsureSamples()
		{
			if (this.samples == null)
			{
				BitReader bitReader = new BitReader(this.data, this.BitsPerSample);
				int num = 1;
				for (int i = 0; i < this.Size.Length; i++)
				{
					int num2 = this.Size[i];
					num *= num2;
				}
				this.samples = new int[num][];
				int outputValuesCount = base.OutputValuesCount;
				for (int j = 0; j < num; j++)
				{
					this.samples[j] = new int[outputValuesCount];
					for (int k = 0; k < outputValuesCount; k++)
					{
						this.samples[j][k] = bitReader.Read();
					}
				}
			}
		}

		void EnsureData()
		{
			if (this.data == null)
			{
				int num = this.samples.Length;
				int num2 = base.Range.Length / 2;
				int numberOfRecords = num * num2;
				BitWriter bitWriter = new BitWriter(numberOfRecords, this.BitsPerSample);
				for (int i = 0; i < num; i++)
				{
					int[] array = this.samples[i];
					for (int j = 0; j < num2; j++)
					{
						int bits = array[j];
						bitWriter.Write(bits);
					}
				}
				this.data = bitWriter.ResultBits;
			}
		}

		void DecodeResult(double[] result)
		{
			int num = (1 << this.BitsPerSample) - 1;
			for (int i = 0; i < result.Length; i++)
			{
				double yMin = this.GetDecode(2 * i);
				double yMax = this.GetDecode(2 * i + 1);
				result[i] = FunctionBase.Interpolate(result[i], 0.0, (double)num, yMin, yMax);
			}
		}

		int GetSampleIndex(int[] encodedValues)
		{
			int num = encodedValues[0];
			int num2 = 1;
			for (int i = 1; i < encodedValues.Length; i++)
			{
				int num3 = this.Size[i - 1];
				num2 *= num3;
				num += num2 * encodedValues[i];
			}
			return num;
		}

		readonly int[] size;

		readonly int bitsPerSample;

		readonly double[] encode;

		readonly double[] decode;

		int[][] samples;

		byte[] data;
	}
}
