using System;

namespace JBig2Decoder
{
	class Helpers
	{
		long[][] convertToJaggedArray(long[,] multiArray, int numOfColumns, int numOfRows)
		{
			long[][] array = new long[numOfColumns][];
			for (int i = 0; i < numOfColumns; i++)
			{
				array[i] = new long[numOfRows];
				for (int j = 0; j < numOfRows; j++)
				{
					array[i][j] = multiArray[i, j];
				}
			}
			return array;
		}
	}
}
