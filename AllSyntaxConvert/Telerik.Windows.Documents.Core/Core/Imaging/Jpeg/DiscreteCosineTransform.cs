using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg
{
	static class DiscreteCosineTransform
	{
		public static FloatBlock ForwardDCT(Block input)
		{
			FloatBlock floatBlock = new FloatBlock(input);
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					floatBlock[i, j] -= 128f;
				}
			}
			for (int k = 0; k < 8; k++)
			{
				float num = floatBlock[k, 0] + floatBlock[k, 7];
				float num2 = floatBlock[k, 0] - floatBlock[k, 7];
				float num3 = floatBlock[k, 1] + floatBlock[k, 6];
				float num4 = floatBlock[k, 1] - floatBlock[k, 6];
				float num5 = floatBlock[k, 2] + floatBlock[k, 5];
				float num6 = floatBlock[k, 2] - floatBlock[k, 5];
				float num7 = floatBlock[k, 3] + floatBlock[k, 4];
				float num8 = floatBlock[k, 3] - floatBlock[k, 4];
				float num9 = num + num7;
				float num10 = num - num7;
				float num11 = num3 + num5;
				float num12 = num3 - num5;
				floatBlock[k, 0] = num9 + num11;
				floatBlock[k, 4] = num9 - num11;
				float num13 = (num12 + num10) * 0.70710677f;
				floatBlock[k, 2] = num10 + num13;
				floatBlock[k, 6] = num10 - num13;
				num9 = num8 + num6;
				num11 = num6 + num4;
				num12 = num4 + num2;
				float num14 = (num9 - num12) * 0.38268343f;
				float num15 = 0.5411961f * num9 + num14;
				float num16 = 1.306563f * num12 + num14;
				float num17 = num11 * 0.70710677f;
				float num18 = num2 + num17;
				float num19 = num2 - num17;
				floatBlock[k, 5] = num19 + num15;
				floatBlock[k, 3] = num19 - num15;
				floatBlock[k, 1] = num18 + num16;
				floatBlock[k, 7] = num18 - num16;
			}
			for (int l = 0; l < 8; l++)
			{
				float num = floatBlock[0, l] + floatBlock[7, l];
				float num2 = floatBlock[0, l] - floatBlock[7, l];
				float num3 = floatBlock[1, l] + floatBlock[6, l];
				float num4 = floatBlock[1, l] - floatBlock[6, l];
				float num5 = floatBlock[2, l] + floatBlock[5, l];
				float num6 = floatBlock[2, l] - floatBlock[5, l];
				float num7 = floatBlock[3, l] + floatBlock[4, l];
				float num8 = floatBlock[3, l] - floatBlock[4, l];
				float num9 = num + num7;
				float num10 = num - num7;
				float num11 = num3 + num5;
				float num12 = num3 - num5;
				floatBlock[0, l] = num9 + num11;
				floatBlock[4, l] = num9 - num11;
				float num13 = (num12 + num10) * 0.70710677f;
				floatBlock[2, l] = num10 + num13;
				floatBlock[6, l] = num10 - num13;
				num9 = num8 + num6;
				num11 = num6 + num4;
				num12 = num4 + num2;
				float num14 = (num9 - num12) * 0.38268343f;
				float num15 = 0.5411961f * num9 + num14;
				float num16 = 1.306563f * num12 + num14;
				float num17 = num11 * 0.70710677f;
				float num18 = num2 + num17;
				float num19 = num2 - num17;
				floatBlock[5, l] = num19 + num15;
				floatBlock[3, l] = num19 - num15;
				floatBlock[1, l] = num18 + num16;
				floatBlock[7, l] = num18 - num16;
			}
			return floatBlock;
		}

		public static Block InverseDCT(Block input)
		{
			Block block = new Block();
			double[] array = new double[64];
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					double num2 = 0.0;
					for (int k = 0; k < 8; k++)
					{
						num2 += (double)input[i * 8 + k] * DiscreteCosineTransform.coefficients[k, j];
					}
					array[num++] = num2;
				}
			}
			for (int l = 0; l < 8; l++)
			{
				for (int m = 0; m < 8; m++)
				{
					double num2 = 128.0;
					for (int n = 0; n < 8; n++)
					{
						num2 += DiscreteCosineTransform.transposedCoefficients[l, n] * array[n * 8 + m];
					}
					block[l, m] = (int)DiscreteCosineTransform.Round(num2);
				}
			}
			return block;
		}

		static byte Round(double f)
		{
			if (f < 0.0)
			{
				return 0;
			}
			if (f > 255.0)
			{
				return byte.MaxValue;
			}
			return (byte)(f + 0.5);
		}

		static double[,] BuildCoefficients()
		{
			double[,] array = new double[8, 8];
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					array[i, j] = ((i == 0) ? 0.353553391 : (0.5 * Math.Cos((2.0 * (double)j + 1.0) * (double)i * 3.141592653589793 / 16.0)));
				}
			}
			return array;
		}

		static double[,] BuildTransposedCoefitients(double[,] coefitients)
		{
			double[,] array = new double[8, 8];
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					array[j, i] = coefitients[i, j];
				}
			}
			return array;
		}

		static readonly double[,] coefficients = DiscreteCosineTransform.BuildCoefficients();

		static readonly double[,] transposedCoefficients = DiscreteCosineTransform.BuildTransposedCoefitients(DiscreteCosineTransform.coefficients);
	}
}
