using System;

namespace Telerik.Windows.Zip
{
	sealed class Tree
	{
		internal short[] DynamicTree { get; set; }

		internal int MaxCode { get; set; }

		internal StaticTree StaticTree { get; set; }

		internal static int BitReverse(int code, int length)
		{
			int num = 0;
			do
			{
				num |= code & 1;
				code >>= 1;
				num <<= 1;
			}
			while (--length > 0);
			return num >> 1;
		}

		internal static int GetDistanceCode(int distance)
		{
			if (distance >= 256)
			{
				return (int)Tree.DistanceCode[(int)(256U + ((uint)distance >> 7))];
			}
			return (int)Tree.DistanceCode[distance];
		}

		internal void BuildTree(DeflateCompressor manager)
		{
			short[] dynamicTree = this.DynamicTree;
			short[] treeCodes = this.StaticTree.TreeCodes;
			int elements = this.StaticTree.Elements;
			int num = -1;
			manager.HeapLength = 0;
			manager.HeapMax = Tree.HeapSize;
			for (int i = 0; i < elements; i++)
			{
				if (dynamicTree[i * 2] != 0)
				{
					num = (manager.Heap[++manager.HeapLength] = i);
					manager.Depth[i] = 0;
				}
				else
				{
					dynamicTree[i * 2 + 1] = 0;
				}
			}
			int num2;
			while (manager.HeapLength < 2)
			{
				num2 = (manager.Heap[++manager.HeapLength] = ((num < 2) ? (++num) : 0));
				dynamicTree[num2 * 2] = 1;
				manager.Depth[num2] = 0;
				manager.OptimalLength--;
				if (treeCodes != null)
				{
					manager.StaticLength -= (int)treeCodes[num2 * 2 + 1];
				}
			}
			this.MaxCode = num;
			for (int i = manager.HeapLength / 2; i >= 1; i--)
			{
				manager.DownHeap(dynamicTree, i);
			}
			num2 = elements;
			do
			{
				int i = manager.Heap[1];
				manager.Heap[1] = manager.Heap[manager.HeapLength--];
				manager.DownHeap(dynamicTree, 1);
				int num3 = manager.Heap[1];
				manager.Heap[--manager.HeapMax] = i;
				manager.Heap[--manager.HeapMax] = num3;
				dynamicTree[num2 * 2] = (short)(dynamicTree[i * 2] + dynamicTree[num3 * 2]);
				manager.Depth[num2] = (sbyte)(Math.Max((byte)manager.Depth[i], (byte)manager.Depth[num3]) + 1);
				dynamicTree[i * 2 + 1] = (dynamicTree[num3 * 2 + 1] = (short)num2);
				manager.Heap[1] = num2++;
				manager.DownHeap(dynamicTree, 1);
			}
			while (manager.HeapLength >= 2);
			manager.Heap[--manager.HeapMax] = manager.Heap[1];
			this.GenerateBitLengths(manager);
			Tree.GenerateCodes(dynamicTree, num, manager.BitLengthCount);
		}

		static void GenerateCodes(short[] tree, int maxCode, short[] bitLengthCount)
		{
			short[] array = new short[16];
			short num = 0;
			for (int i = 1; i <= 15; i++)
			{
				num = (array[i] = (short)(num + bitLengthCount[i - 1] << 1));
			}
			for (int j = 0; j <= maxCode; j++)
			{
				int num2 = (int)tree[j * 2 + 1];
				if (num2 != 0)
				{
					int num3 = j * 2;
					short[] array2 = array;
					int num4 = num2;
					short code;
					array2[num4] = (short)((code = array2[num4]) + 1);
					tree[num3] = (short)Tree.BitReverse((int)code, num2);
				}
			}
		}

		void GenerateBitLengths(DeflateCompressor manager)
		{
			short[] dynamicTree = this.DynamicTree;
			short[] treeCodes = this.StaticTree.TreeCodes;
			int[] extraBits = this.StaticTree.ExtraBits;
			int extraBase = this.StaticTree.ExtraBase;
			int maxLength = this.StaticTree.MaxLength;
			int num = 0;
			for (int i = 0; i <= 15; i++)
			{
				manager.BitLengthCount[i] = 0;
			}
			dynamicTree[manager.Heap[manager.HeapMax] * 2 + 1] = 0;
			int j;
			for (j = manager.HeapMax + 1; j < Tree.HeapSize; j++)
			{
				int num2 = manager.Heap[j];
				int i = (int)(dynamicTree[(int)(dynamicTree[num2 * 2 + 1] * 2 + 1)] + 1);
				if (i > maxLength)
				{
					i = maxLength;
					num++;
				}
				dynamicTree[num2 * 2 + 1] = (short)i;
				if (num2 <= this.MaxCode)
				{
					short[] bitLengthCount = manager.BitLengthCount;
					int num3 = i;
					bitLengthCount[num3] += 1;
					int num4 = 0;
					if (num2 >= extraBase)
					{
						num4 = extraBits[num2 - extraBase];
					}
					short num5 = dynamicTree[num2 * 2];
					manager.OptimalLength += (int)num5 * (i + num4);
					if (treeCodes != null)
					{
						manager.StaticLength += (int)num5 * ((int)treeCodes[num2 * 2 + 1] + num4);
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			do
			{
				int i = maxLength - 1;
				while (manager.BitLengthCount[i] == 0)
				{
					i--;
				}
				short[] bitLengthCount2 = manager.BitLengthCount;
				int num6 = i;
				bitLengthCount2[num6] -= 1;
				manager.BitLengthCount[i + 1] = (short)(manager.BitLengthCount[i + 1] + 2);
				short[] bitLengthCount3 = manager.BitLengthCount;
				int num7 = maxLength;
				bitLengthCount3[num7] -= 1;
				num -= 2;
			}
			while (num > 0);
			for (int i = maxLength; i != 0; i--)
			{
				int num2 = (int)manager.BitLengthCount[i];
				while (num2 != 0)
				{
					int num8 = manager.Heap[--j];
					if (num8 <= this.MaxCode)
					{
						int num9 = num8 * 2;
						int num10 = num9 + 1;
						if ((int)dynamicTree[num10] != i)
						{
							manager.OptimalLength = (int)((long)manager.OptimalLength + ((long)i - (long)dynamicTree[num10]) * (long)dynamicTree[num9]);
							dynamicTree[num10] = (short)i;
						}
						num2--;
					}
				}
			}
		}

		internal static readonly int[] ExtraLengthBits = new int[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
			1, 1, 2, 2, 2, 2, 3, 3, 3, 3,
			4, 4, 4, 4, 5, 5, 5, 5, 0
		};

		internal static readonly int[] ExtraDistanceBits = new int[]
		{
			0, 0, 0, 0, 1, 1, 2, 2, 3, 3,
			4, 4, 5, 5, 6, 6, 7, 7, 8, 8,
			9, 9, 10, 10, 11, 11, 12, 12, 13, 13
		};

		internal static readonly int[] ExtraBits = new int[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 2, 3, 7
		};

		internal static readonly sbyte[] BitLengthOrder = new sbyte[]
		{
			16, 17, 18, 0, 8, 7, 9, 6, 10, 5,
			11, 4, 12, 3, 13, 2, 14, 1, 15
		};

		internal static readonly sbyte[] LengthCode = new sbyte[]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 8,
			9, 9, 10, 10, 11, 11, 12, 12, 12, 12,
			13, 13, 13, 13, 14, 14, 14, 14, 15, 15,
			15, 15, 16, 16, 16, 16, 16, 16, 16, 16,
			17, 17, 17, 17, 17, 17, 17, 17, 18, 18,
			18, 18, 18, 18, 18, 18, 19, 19, 19, 19,
			19, 19, 19, 19, 20, 20, 20, 20, 20, 20,
			20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
			21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
			21, 21, 21, 21, 21, 21, 22, 22, 22, 22,
			22, 22, 22, 22, 22, 22, 22, 22, 22, 22,
			22, 22, 23, 23, 23, 23, 23, 23, 23, 23,
			23, 23, 23, 23, 23, 23, 23, 23, 24, 24,
			24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 26, 26, 26, 26, 26, 26, 26, 26,
			26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
			26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
			26, 26, 26, 26, 27, 27, 27, 27, 27, 27,
			27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
			27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
			27, 27, 27, 27, 27, 28
		};

		internal static readonly int[] LengthBase = new int[]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 10,
			12, 14, 16, 20, 24, 28, 32, 40, 48, 56,
			64, 80, 96, 112, 128, 160, 192, 224, 0
		};

		internal static readonly int[] DistanceBase = new int[]
		{
			0, 1, 2, 3, 4, 6, 8, 12, 16, 24,
			32, 48, 64, 96, 128, 192, 256, 384, 512, 768,
			1024, 1536, 2048, 3072, 4096, 6144, 8192, 12288, 16384, 24576
		};

		static readonly int HeapSize = 573;

		static readonly sbyte[] DistanceCode = new sbyte[]
		{
			0, 1, 2, 3, 4, 4, 5, 5, 6, 6,
			6, 6, 7, 7, 7, 7, 8, 8, 8, 8,
			8, 8, 8, 8, 9, 9, 9, 9, 9, 9,
			9, 9, 10, 10, 10, 10, 10, 10, 10, 10,
			10, 10, 10, 10, 10, 10, 10, 10, 11, 11,
			11, 11, 11, 11, 11, 11, 11, 11, 11, 11,
			11, 11, 11, 11, 12, 12, 12, 12, 12, 12,
			12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
			12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
			12, 12, 12, 12, 12, 12, 13, 13, 13, 13,
			13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
			13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
			13, 13, 13, 13, 13, 13, 13, 13, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 15, 15, 15, 15, 15, 15, 15, 15,
			15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
			15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
			15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
			15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
			15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
			15, 15, 15, 15, 15, 15, 0, 0, 16, 17,
			18, 18, 19, 19, 20, 20, 20, 20, 21, 21,
			21, 21, 22, 22, 22, 22, 22, 22, 22, 22,
			23, 23, 23, 23, 23, 23, 23, 23, 24, 24,
			24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			24, 24, 24, 24, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
			26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
			26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
			26, 26, 27, 27, 27, 27, 27, 27, 27, 27,
			27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
			27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
			27, 27, 27, 27, 28, 28, 28, 28, 28, 28,
			28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
			28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
			28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
			28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
			28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
			28, 28, 28, 28, 28, 28, 28, 28, 29, 29,
			29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
			29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
			29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
			29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
			29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
			29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
			29, 29
		};
	}
}
