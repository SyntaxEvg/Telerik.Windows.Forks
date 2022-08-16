using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg
{
	class ZigZagScan
	{
		public static Block UnZigZag(Block input)
		{
			Block block = new Block();
			block[0] = input[0];
			block[1] = input[1];
			block[8] = input[2];
			block[16] = input[3];
			block[9] = input[4];
			block[2] = input[5];
			block[3] = input[6];
			block[10] = input[7];
			block[17] = input[8];
			block[24] = input[9];
			block[32] = input[10];
			block[25] = input[11];
			block[18] = input[12];
			block[11] = input[13];
			block[4] = input[14];
			block[5] = input[15];
			block[12] = input[16];
			block[19] = input[17];
			block[26] = input[18];
			block[33] = input[19];
			block[40] = input[20];
			block[48] = input[21];
			block[41] = input[22];
			block[34] = input[23];
			block[27] = input[24];
			block[20] = input[25];
			block[13] = input[26];
			block[6] = input[27];
			block[7] = input[28];
			block[14] = input[29];
			block[21] = input[30];
			block[28] = input[31];
			block[35] = input[32];
			block[42] = input[33];
			block[49] = input[34];
			block[56] = input[35];
			block[57] = input[36];
			block[50] = input[37];
			block[43] = input[38];
			block[36] = input[39];
			block[29] = input[40];
			block[22] = input[41];
			block[15] = input[42];
			block[23] = input[43];
			block[30] = input[44];
			block[37] = input[45];
			block[44] = input[46];
			block[51] = input[47];
			block[58] = input[48];
			block[59] = input[49];
			block[52] = input[50];
			block[45] = input[51];
			block[38] = input[52];
			block[31] = input[53];
			block[39] = input[54];
			block[46] = input[55];
			block[53] = input[56];
			block[60] = input[57];
			block[61] = input[58];
			block[54] = input[59];
			block[47] = input[60];
			block[55] = input[61];
			block[62] = input[62];
			block[63] = input[63];
			return block;
		}

		public static Block ZigZag(Block input)
		{
			Block block = new Block();
			block[0] = input[0];
			block[1] = input[1];
			block[5] = input[2];
			block[6] = input[3];
			block[14] = input[4];
			block[15] = input[5];
			block[27] = input[6];
			block[28] = input[7];
			block[2] = input[8];
			block[4] = input[9];
			block[7] = input[10];
			block[13] = input[11];
			block[16] = input[12];
			block[26] = input[13];
			block[29] = input[14];
			block[42] = input[15];
			block[3] = input[16];
			block[8] = input[17];
			block[12] = input[18];
			block[17] = input[19];
			block[25] = input[20];
			block[30] = input[21];
			block[41] = input[22];
			block[43] = input[23];
			block[9] = input[24];
			block[11] = input[25];
			block[18] = input[26];
			block[24] = input[27];
			block[31] = input[28];
			block[40] = input[29];
			block[44] = input[30];
			block[53] = input[31];
			block[10] = input[32];
			block[19] = input[33];
			block[23] = input[34];
			block[32] = input[35];
			block[39] = input[36];
			block[45] = input[37];
			block[52] = input[38];
			block[54] = input[39];
			block[20] = input[40];
			block[22] = input[41];
			block[33] = input[42];
			block[38] = input[43];
			block[46] = input[44];
			block[51] = input[45];
			block[55] = input[46];
			block[60] = input[47];
			block[21] = input[48];
			block[34] = input[49];
			block[37] = input[50];
			block[47] = input[51];
			block[50] = input[52];
			block[56] = input[53];
			block[59] = input[54];
			block[61] = input[55];
			block[35] = input[56];
			block[36] = input[57];
			block[48] = input[58];
			block[49] = input[59];
			block[57] = input[60];
			block[58] = input[61];
			block[62] = input[62];
			block[63] = input[63];
			return block;
		}

		public static FloatBlock ZigZag(FloatBlock input)
		{
			FloatBlock floatBlock = new FloatBlock();
			floatBlock[0] = input[0];
			floatBlock[1] = input[1];
			floatBlock[5] = input[2];
			floatBlock[6] = input[3];
			floatBlock[14] = input[4];
			floatBlock[15] = input[5];
			floatBlock[27] = input[6];
			floatBlock[28] = input[7];
			floatBlock[2] = input[8];
			floatBlock[4] = input[9];
			floatBlock[7] = input[10];
			floatBlock[13] = input[11];
			floatBlock[16] = input[12];
			floatBlock[26] = input[13];
			floatBlock[29] = input[14];
			floatBlock[42] = input[15];
			floatBlock[3] = input[16];
			floatBlock[8] = input[17];
			floatBlock[12] = input[18];
			floatBlock[17] = input[19];
			floatBlock[25] = input[20];
			floatBlock[30] = input[21];
			floatBlock[41] = input[22];
			floatBlock[43] = input[23];
			floatBlock[9] = input[24];
			floatBlock[11] = input[25];
			floatBlock[18] = input[26];
			floatBlock[24] = input[27];
			floatBlock[31] = input[28];
			floatBlock[40] = input[29];
			floatBlock[44] = input[30];
			floatBlock[53] = input[31];
			floatBlock[10] = input[32];
			floatBlock[19] = input[33];
			floatBlock[23] = input[34];
			floatBlock[32] = input[35];
			floatBlock[39] = input[36];
			floatBlock[45] = input[37];
			floatBlock[52] = input[38];
			floatBlock[54] = input[39];
			floatBlock[20] = input[40];
			floatBlock[22] = input[41];
			floatBlock[33] = input[42];
			floatBlock[38] = input[43];
			floatBlock[46] = input[44];
			floatBlock[51] = input[45];
			floatBlock[55] = input[46];
			floatBlock[60] = input[47];
			floatBlock[21] = input[48];
			floatBlock[34] = input[49];
			floatBlock[37] = input[50];
			floatBlock[47] = input[51];
			floatBlock[50] = input[52];
			floatBlock[56] = input[53];
			floatBlock[59] = input[54];
			floatBlock[61] = input[55];
			floatBlock[35] = input[56];
			floatBlock[36] = input[57];
			floatBlock[48] = input[58];
			floatBlock[49] = input[59];
			floatBlock[57] = input[60];
			floatBlock[58] = input[61];
			floatBlock[62] = input[62];
			floatBlock[63] = input[63];
			return floatBlock;
		}

		public static readonly int[] DirectZigZagIndices = new int[]
		{
			0, 1, 5, 6, 14, 15, 27, 28, 2, 4,
			7, 13, 16, 26, 29, 42, 3, 8, 12, 17,
			25, 30, 41, 43, 9, 11, 18, 24, 31, 40,
			44, 53, 10, 19, 23, 32, 39, 45, 52, 54,
			20, 22, 33, 38, 46, 51, 55, 60, 21, 34,
			37, 47, 50, 56, 59, 61, 35, 36, 48, 49,
			57, 58, 62, 63
		};

		public static readonly int[] ZigZagMap = new int[]
		{
			0, 1, 8, 16, 9, 2, 3, 10, 17, 24,
			32, 25, 18, 11, 4, 5, 12, 19, 26, 33,
			40, 48, 41, 34, 27, 20, 13, 6, 7, 14,
			21, 28, 35, 42, 49, 56, 57, 50, 43, 36,
			29, 22, 15, 23, 30, 37, 44, 51, 58, 59,
			52, 45, 38, 31, 39, 46, 53, 60, 61, 54,
			47, 55, 62, 63
		};
	}
}
