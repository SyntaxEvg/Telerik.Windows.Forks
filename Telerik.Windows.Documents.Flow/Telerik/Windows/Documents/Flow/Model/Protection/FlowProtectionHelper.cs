using System;
using Telerik.Windows.Documents.Common.Model.Protection;

namespace Telerik.Windows.Documents.Flow.Model.Protection
{
	class FlowProtectionHelper : ProtectionHelperBase
	{
		internal static FlowProtectionHelper Instance
		{
			get
			{
				return FlowProtectionHelper.instance;
			}
		}

		protected override string PreprocessPassword(string password)
		{
			return FlowProtectionHelper.CalculateLegacyHash(password).ToString("X");
		}

		static int CalculateLegacyHash(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				return 0;
			}
			byte[] array = FlowProtectionHelper.Pretransform(password);
			int num = FlowProtectionHelper.initialCodeArray[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					if (((array[i] >> j) & 1) != 0)
					{
						num ^= FlowProtectionHelper.cryptoMatrix[15 - array.Length + i][j];
					}
				}
			}
			int num2 = 0;
			for (int k = array.Length - 1; k >= 0; k--)
			{
				num2 = (((num2 >> 14) & 1) | ((num2 << 1) & 32767)) ^ (int)array[k];
			}
			num2 = (((num2 >> 14) & 1) | ((num2 << 1) & 32767)) ^ password.Length ^ 52811;
			int num3 = num2 & 255;
			int num4 = (num2 >> 8) & 255;
			int num5 = num & 255;
			int num6 = (num >> 8) & 255;
			return (num3 << 24) | (num4 << 16) | (num5 << 8) | num6;
		}

		static byte[] Pretransform(string password)
		{
			password = password.Substring(0, Math.Min(password.Length, 15));
			byte[] array = new byte[password.Length];
			for (int i = 0; i < password.Length; i++)
			{
				char c = password[i];
				if ((c & 'ÿ') == '\0')
				{
					array[i] = (byte)(c >> 8);
				}
				else
				{
					array[i] = (byte)(c & 'ÿ');
				}
			}
			return array;
		}

		static readonly int[] initialCodeArray = new int[]
		{
			0, 57840, 7439, 52380, 33984, 4364, 3600, 61902, 12606, 6258,
			57657, 54287, 34041, 10252, 43370, 20163
		};

		static readonly int[][] cryptoMatrix = new int[][]
		{
			new int[] { 44796, 19929, 39858, 10053, 20106, 40212, 10761 },
			new int[] { 31585, 63170, 64933, 60267, 50935, 40399, 11199 },
			new int[] { 17763, 35526, 1453, 2906, 5812, 11624, 23248 },
			new int[] { 885, 1770, 3540, 7080, 14160, 28320, 56640 },
			new int[] { 55369, 41139, 20807, 41614, 21821, 43642, 17621 },
			new int[] { 28485, 56970, 44341, 19019, 38038, 14605, 29210 },
			new int[] { 60195, 50791, 40175, 10751, 21502, 43004, 24537 },
			new int[] { 18387, 36774, 3949, 7898, 15796, 31592, 63184 },
			new int[] { 47201, 24803, 49606, 37805, 14203, 28406, 56812 },
			new int[] { 17824, 35648, 1697, 3394, 6788, 13576, 27152 },
			new int[] { 43601, 17539, 35078, 557, 1114, 2228, 4456 },
			new int[] { 30388, 60776, 51953, 34243, 7079, 14158, 28316 },
			new int[] { 14128, 28256, 56512, 43425, 17251, 34502, 7597 },
			new int[] { 13105, 26210, 52420, 35241, 883, 1766, 3532 },
			new int[] { 4129, 8258, 16516, 33032, 4657, 9314, 18628 }
		};

		static readonly FlowProtectionHelper instance = new FlowProtectionHelper();
	}
}
