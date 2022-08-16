using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	class LzwDecoder
	{
		public LzwDecoder(byte[] data, int earlyChange)
		{
			this.lzwState = new LzwDecoder.LzwDecoderState(earlyChange);
			this.data = data;
		}

		public byte[] GetBytes()
		{
			List<byte> list = new List<byte>();
			while (!this.eof)
			{
				list.AddRange(this.ReadBlock());
			}
			return list.ToArray();
		}

		int? ReadBits(int n)
		{
			int i = this.bitsCached;
			int num = this.cachedData;
			while (i < n)
			{
				byte? @byte = this.GetByte();
				byte? b = @byte;
				int? num2 = ((b != null) ? new int?((int)b.GetValueOrDefault()) : null);
				if (num2 == null)
				{
					this.eof = true;
					return null;
				}
				num = (num << 8) | (int)@byte.Value;
				i += 8;
			}
			i = (this.bitsCached = i - n);
			this.cachedData = num;
			return new int?((int)(((uint)num >> i) & ((1U << n) - 1U)));
		}

		byte? GetByte()
		{
			if (this.position >= this.data.Length)
			{
				return null;
			}
			return new byte?(this.data[this.position++]);
		}

		List<byte> ReadBlock()
		{
			List<byte> list = new List<byte>();
			int num = 512;
			LzwDecoder.LzwDecoderState lzwDecoderState = this.lzwState;
			if (lzwDecoderState == null)
			{
				return list;
			}
			int earlyChange = lzwDecoderState.EarlyChange;
			int num2 = lzwDecoderState.NextCode;
			byte[] dictionaryValues = lzwDecoderState.DictionaryValues;
			int[] dictionaryLengths = lzwDecoderState.DictionaryLengths;
			int[] dictionaryPrevCodes = lzwDecoderState.DictionaryPrevCodes;
			int num3 = lzwDecoderState.CodeLength;
			int num4 = lzwDecoderState.PrevCode;
			byte[] currentSequence = lzwDecoderState.CurrentSequence;
			int num5 = lzwDecoderState.CurrentSequenceLength;
			int i = 0;
			while (i < num)
			{
				int? num6 = this.ReadBits(num3);
				bool flag = num5 > 0;
				if (num6 < 256)
				{
					currentSequence[0] = (byte)num6.Value;
					num5 = 1;
					goto IL_176;
				}
				if (num6 >= 258)
				{
					if (num6 < num2)
					{
						num5 = dictionaryLengths[num6.Value];
						int j = num5 - 1;
						int num7 = num6.Value;
						while (j >= 0)
						{
							currentSequence[j] = dictionaryValues[num7];
							num7 = dictionaryPrevCodes[num7];
							j--;
						}
						goto IL_176;
					}
					currentSequence[num5++] = currentSequence[0];
					goto IL_176;
				}
				else
				{
					if (!(num6 == 256))
					{
						this.eof = true;
						this.lzwState = null;
						break;
					}
					num3 = 9;
					num2 = 258;
					num5 = 0;
				}
				IL_1FA:
				i++;
				continue;
				IL_176:
				if (flag)
				{
					dictionaryPrevCodes[num2] = num4;
					dictionaryLengths[num2] = dictionaryLengths[num4] + 1;
					dictionaryValues[num2] = currentSequence[0];
					num2++;
					if (((num2 + earlyChange) & (num2 + earlyChange - 1)) == 0)
					{
						num3 = (int)Math.Min(Math.Log((double)(num2 + earlyChange)) / this.Log2 + 1.0, 12.0);
					}
				}
				num4 = num6.Value;
				for (int k = 0; k < num5; k++)
				{
					list.Add(currentSequence[k]);
				}
				goto IL_1FA;
			}
			lzwDecoderState.NextCode = num2;
			lzwDecoderState.CodeLength = num3;
			lzwDecoderState.PrevCode = num4;
			lzwDecoderState.CurrentSequenceLength = num5;
			return list;
		}

		readonly double Log2 = Math.Log(2.0);

		LzwDecoder.LzwDecoderState lzwState;

		int bitsCached;

		int cachedData;

		bool eof;

		int position;

		byte[] data;

		class LzwDecoderState
		{
			public int EarlyChange
			{
				get
				{
					return this.earlyChange;
				}
			}

			public int NextCode
			{
				get
				{
					return this.nextCode;
				}
				set
				{
					this.nextCode = value;
				}
			}

			public int PrevCode
			{
				get
				{
					return this.prevCode;
				}
				set
				{
					this.prevCode = value;
				}
			}

			public int CodeLength
			{
				get
				{
					return this.codeLength;
				}
				set
				{
					this.codeLength = value;
				}
			}

			public int CurrentSequenceLength
			{
				get
				{
					return this.currentSequenceLength;
				}
				set
				{
					this.currentSequenceLength = value;
				}
			}

			public byte[] DictionaryValues
			{
				get
				{
					return this.dictionaryValues;
				}
			}

			public int[] DictionaryPrevCodes
			{
				get
				{
					return this.dictionaryPrevCodes;
				}
			}

			public int[] DictionaryLengths
			{
				get
				{
					return this.dictionaryLengths;
				}
			}

			public byte[] CurrentSequence
			{
				get
				{
					return this.currentSequence;
				}
			}

			public LzwDecoderState(int earlyChange)
			{
				this.earlyChange = earlyChange;
				for (int i = 0; i < 256; i++)
				{
					this.DictionaryValues[i] = (byte)i;
					this.DictionaryLengths[i] = 1;
				}
			}

			const int maxLzwDictionarySize = 4096;

			int earlyChange;

			int codeLength = 9;

			int nextCode = 258;

			int prevCode;

			byte[] dictionaryValues = new byte[4096];

			int[] dictionaryLengths = new int[4096];

			int[] dictionaryPrevCodes = new int[4096];

			byte[] currentSequence = new byte[4096];

			int currentSequenceLength;
		}
	}
}
