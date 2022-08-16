using System;

namespace JBig2Decoder
{
	sealed class JBIG2Bitmap
	{
		public JBIG2Bitmap(long width, long height, ArithmeticDecoder arithmeticDecoder, HuffmanDecoder huffmanDecoder, MMRDecoder mmrDecoder)
		{
			this.width = width;
			this.height = height;
			this.arithmeticDecoder = arithmeticDecoder;
			this.huffmanDecoder = huffmanDecoder;
			this.mmrDecoder = mmrDecoder;
			this.line = width + 7L >> 3;
			this.data = new FastBitSet(width * height);
		}

		public void readBitmap(bool useMMR, int template, bool typicalPredictionGenericDecodingOn, bool useSkip, JBIG2Bitmap skipBitmap, short[] adaptiveTemplateX, short[] adaptiveTemplateY, long mmrDataLength)
		{
			if (useMMR)
			{
				this.mmrDecoder.reset();
				long[] array = new long[this.width + 2L];
				long[] array2 = new long[this.width + 2L];
				array2[0] = (array2[1] = this.width);
				int num = 0;
				while ((long)num < this.height)
				{
					int num2 = 0;
					while (array2[num2] < this.width)
					{
						array[num2] = array2[num2];
						num2++;
					}
					array[num2] = (array[num2 + 1] = this.width);
					long num3 = 0L;
					long num4 = 0L;
					long num5 = 0L;
					do
					{
						switch (this.mmrDecoder.get2DCode())
						{
						case 0:
							if (array[(int)(checked((IntPtr)num3))] < this.width)
							{
								num5 = array[(int)(checked((IntPtr)(unchecked(num3 + 1L))))];
								num3 += 2L;
							}
							break;
						case 1:
						{
							int num6;
							int num8;
							if ((num4 & 1L) != 0L)
							{
								num6 = 0;
								int num7;
								do
								{
									num6 += (num7 = this.mmrDecoder.getBlackCode());
								}
								while (num7 >= 64);
								num8 = 0;
								do
								{
									num8 += (num7 = this.mmrDecoder.getWhiteCode());
								}
								while (num7 >= 64);
							}
							else
							{
								num6 = 0;
								int num7;
								do
								{
									num6 += (num7 = this.mmrDecoder.getWhiteCode());
								}
								while (num7 >= 64);
								num8 = 0;
								do
								{
									num8 += (num7 = this.mmrDecoder.getBlackCode());
								}
								while (num7 >= 64);
							}
							if (num6 > 0 || num8 > 0)
							{
								long[] array3 = array2;
								long num9 = num4;
								num4 = num9 + 1L;
								num5 = (array3[(int)(checked((IntPtr)num9))] = num5 + (long)num6);
								long[] array4 = array2;
								long num10 = num4;
								num4 = num10 + 1L;
								num5 = (array4[(int)(checked((IntPtr)num10))] = num5 + (long)num8);
								while (array[(int)(checked((IntPtr)num3))] <= num5)
								{
									if (array[(int)(checked((IntPtr)num3))] >= this.width)
									{
										break;
									}
									num3 += 2L;
								}
							}
							break;
						}
						case 2:
						{
							long[] array5 = array2;
							long num11 = num4;
							num4 = num11 + 1L;
							num5 = checked(array5[(int)((IntPtr)num11)] = array[(int)((IntPtr)num3)]);
							if (array[(int)(checked((IntPtr)num3))] < this.width)
							{
								num3 += 1L;
							}
							break;
						}
						case 3:
						{
							long[] array6 = array2;
							long num12 = num4;
							num4 = num12 + 1L;
							num5 = (array6[(int)(checked((IntPtr)num12))] = array[(int)(checked((IntPtr)num3))] + 1L);
							if (array[(int)(checked((IntPtr)num3))] < this.width)
							{
								num3 += 1L;
								while (array[(int)(checked((IntPtr)num3))] <= num5)
								{
									if (array[(int)(checked((IntPtr)num3))] >= this.width)
									{
										break;
									}
									num3 += 2L;
								}
							}
							break;
						}
						case 4:
						{
							long[] array7 = array2;
							long num13 = num4;
							num4 = num13 + 1L;
							num5 = (array7[(int)(checked((IntPtr)num13))] = array[(int)(checked((IntPtr)num3))] - 1L);
							if (num3 > 0L)
							{
								num3 -= 1L;
							}
							else
							{
								num3 += 1L;
							}
							while (array[(int)(checked((IntPtr)num3))] <= num5)
							{
								if (array[(int)(checked((IntPtr)num3))] >= this.width)
								{
									break;
								}
								num3 += 2L;
							}
							break;
						}
						case 5:
						{
							long[] array8 = array2;
							long num14 = num4;
							num4 = num14 + 1L;
							num5 = (array8[(int)(checked((IntPtr)num14))] = array[(int)(checked((IntPtr)num3))] + 2L);
							if (array[(int)(checked((IntPtr)num3))] < this.width)
							{
								num3 += 1L;
								while (array[(int)(checked((IntPtr)num3))] <= num5)
								{
									if (array[(int)(checked((IntPtr)num3))] >= this.width)
									{
										break;
									}
									num3 += 2L;
								}
							}
							break;
						}
						case 6:
						{
							long[] array9 = array2;
							long num15 = num4;
							num4 = num15 + 1L;
							num5 = (array9[(int)(checked((IntPtr)num15))] = array[(int)(checked((IntPtr)num3))] - 2L);
							if (num3 > 0L)
							{
								num3 -= 1L;
							}
							else
							{
								num3 += 1L;
							}
							while (array[(int)(checked((IntPtr)num3))] <= num5)
							{
								if (array[(int)(checked((IntPtr)num3))] >= this.width)
								{
									break;
								}
								num3 += 2L;
							}
							break;
						}
						case 7:
						{
							long[] array10 = array2;
							long num16 = num4;
							num4 = num16 + 1L;
							num5 = (array10[(int)(checked((IntPtr)num16))] = array[(int)(checked((IntPtr)num3))] + 3L);
							if (array[(int)(checked((IntPtr)num3))] < this.width)
							{
								num3 += 1L;
								while (array[(int)(checked((IntPtr)num3))] <= num5)
								{
									if (array[(int)(checked((IntPtr)num3))] >= this.width)
									{
										break;
									}
									num3 += 2L;
								}
							}
							break;
						}
						case 8:
						{
							long[] array11 = array2;
							long num17 = num4;
							num4 = num17 + 1L;
							num5 = (array11[(int)(checked((IntPtr)num17))] = array[(int)(checked((IntPtr)num3))] - 3L);
							if (num3 > 0L)
							{
								num3 -= 1L;
							}
							else
							{
								num3 += 1L;
							}
							while (array[(int)(checked((IntPtr)num3))] <= num5)
							{
								if (array[(int)(checked((IntPtr)num3))] >= this.width)
								{
									break;
								}
								num3 += 2L;
							}
							break;
						}
						default:
							if (JBIG2StreamDecoder.debug)
							{
								Console.WriteLine("Illegal code in JBIG2 MMR bitmap data");
							}
							break;
						}
					}
					while (num5 < this.width);
					long[] array12 = array2;
					long num18 = num4;
					num4 = num18 + 1L;
					array12[(int)(checked((IntPtr)num18))] = this.width;
					int num19 = 0;
					while (array2[num19] < this.width)
					{
						for (long num20 = array2[num19]; num20 < array2[num19 + 1]; num20 += 1L)
						{
							this.setPixel(num20, (long)num, 1L);
						}
						num19 += 2;
					}
					num++;
				}
				if (mmrDataLength >= 0L)
				{
					this.mmrDecoder.skipTo(mmrDataLength);
					return;
				}
				if (this.mmrDecoder.get24Bits() != 4097L && JBIG2StreamDecoder.debug)
				{
					Console.WriteLine("Missing EOFB in JBIG2 MMR bitmap data");
					return;
				}
			}
			else
			{
				BitmapPointer bitmapPointer = new BitmapPointer(this);
				BitmapPointer bitmapPointer2 = new BitmapPointer(this);
				BitmapPointer bitmapPointer3 = new BitmapPointer(this);
				BitmapPointer bitmapPointer4 = new BitmapPointer(this);
				BitmapPointer bitmapPointer5 = new BitmapPointer(this);
				BitmapPointer bitmapPointer6 = new BitmapPointer(this);
				long context = 0L;
				if (typicalPredictionGenericDecodingOn)
				{
					switch (template)
					{
					case 0:
						context = 14675L;
						break;
					case 1:
						context = 1946L;
						break;
					case 2:
						context = 227L;
						break;
					case 3:
						context = 394L;
						break;
					}
				}
				bool flag = false;
				int num21 = 0;
				while ((long)num21 < this.height)
				{
					if (!typicalPredictionGenericDecodingOn)
					{
						goto IL_577;
					}
					int num22 = this.arithmeticDecoder.decodeBit(context, this.arithmeticDecoder.genericRegionStats);
					if (num22 != 0)
					{
						flag = !flag;
					}
					if (!flag)
					{
						goto IL_577;
					}
					this.duplicateRow(num21, num21 - 1);
					IL_AB6:
					num21++;
					continue;
					IL_577:
					switch (template)
					{
					case 0:
					{
						bitmapPointer.setPointer(0L, (long)(num21 - 2));
						long num23 = (long)((long)bitmapPointer.nextPixel() << 1);
						num23 |= (long)bitmapPointer.nextPixel();
						bitmapPointer2.setPointer(0L, (long)(num21 - 1));
						long num24 = (long)((long)bitmapPointer2.nextPixel() << 2);
						num24 |= (long)((long)bitmapPointer2.nextPixel() << 1);
						num24 |= (long)bitmapPointer2.nextPixel();
						long number = 0L;
						bitmapPointer3.setPointer((long)adaptiveTemplateX[0], (long)(num21 + (int)adaptiveTemplateY[0]));
						bitmapPointer4.setPointer((long)adaptiveTemplateX[1], (long)(num21 + (int)adaptiveTemplateY[1]));
						bitmapPointer5.setPointer((long)adaptiveTemplateX[2], (long)(num21 + (int)adaptiveTemplateY[2]));
						bitmapPointer6.setPointer((long)adaptiveTemplateX[3], (long)(num21 + (int)adaptiveTemplateY[3]));
						int num25 = 0;
						while ((long)num25 < this.width)
						{
							long context2 = BinaryOperation.bit32ShiftL(num23, 13) | BinaryOperation.bit32ShiftL(num24, 8) | BinaryOperation.bit32ShiftL(number, 4) | (long)((long)bitmapPointer3.nextPixel() << 3) | (long)((long)bitmapPointer4.nextPixel() << 2) | (long)((long)bitmapPointer5.nextPixel() << 1) | (long)bitmapPointer6.nextPixel();
							int num26;
							if (useSkip && skipBitmap.getPixel(num25, num21) != 0)
							{
								num26 = 0;
							}
							else
							{
								num26 = this.arithmeticDecoder.decodeBit(context2, this.arithmeticDecoder.genericRegionStats);
								if (num26 != 0)
								{
									this.data.set((long)num21 * this.width + (long)num25);
								}
							}
							num23 = (BinaryOperation.bit32ShiftL(num23, 1) | (long)bitmapPointer.nextPixel()) & 7L;
							num24 = (BinaryOperation.bit32ShiftL(num24, 1) | (long)bitmapPointer2.nextPixel()) & 31L;
							number = (BinaryOperation.bit32ShiftL(number, 1) | (long)num26) & 15L;
							num25++;
						}
						goto IL_AB6;
					}
					case 1:
					{
						bitmapPointer.setPointer(0L, (long)(num21 - 2));
						long num23 = (long)((long)bitmapPointer.nextPixel() << 2);
						num23 |= (long)((long)bitmapPointer.nextPixel() << 1);
						num23 |= (long)bitmapPointer.nextPixel();
						bitmapPointer2.setPointer(0L, (long)(num21 - 1));
						long num24 = (long)((long)bitmapPointer2.nextPixel() << 2);
						num24 |= (long)((long)bitmapPointer2.nextPixel() << 1);
						num24 |= (long)bitmapPointer2.nextPixel();
						long number = 0L;
						bitmapPointer3.setPointer((long)adaptiveTemplateX[0], (long)(num21 + (int)adaptiveTemplateY[0]));
						int num27 = 0;
						while ((long)num27 < this.width)
						{
							long context2 = BinaryOperation.bit32ShiftL(num23, 9) | BinaryOperation.bit32ShiftL(num24, 4) | BinaryOperation.bit32ShiftL(number, 1) | (long)bitmapPointer3.nextPixel();
							int num26;
							if (useSkip && skipBitmap.getPixel(num27, num21) != 0)
							{
								num26 = 0;
							}
							else
							{
								num26 = this.arithmeticDecoder.decodeBit(context2, this.arithmeticDecoder.genericRegionStats);
								if (num26 != 0)
								{
									this.data.set((long)num21 * this.width + (long)num27);
								}
							}
							num23 = (BinaryOperation.bit32ShiftL(num23, 1) | (long)bitmapPointer.nextPixel()) & 15L;
							num24 = (BinaryOperation.bit32ShiftL(num24, 1) | (long)bitmapPointer2.nextPixel()) & 31L;
							number = (BinaryOperation.bit32ShiftL(number, 1) | (long)num26) & 7L;
							num27++;
						}
						goto IL_AB6;
					}
					case 2:
					{
						bitmapPointer.setPointer(0L, (long)(num21 - 2));
						long num23 = (long)((long)bitmapPointer.nextPixel() << 1);
						num23 |= (long)bitmapPointer.nextPixel();
						bitmapPointer2.setPointer(0L, (long)(num21 - 1));
						long num24 = (long)((long)bitmapPointer2.nextPixel() << 1);
						num24 |= (long)bitmapPointer2.nextPixel();
						long number = 0L;
						bitmapPointer3.setPointer((long)adaptiveTemplateX[0], (long)(num21 + (int)adaptiveTemplateY[0]));
						int num28 = 0;
						while ((long)num28 < this.width)
						{
							long context2 = BinaryOperation.bit32ShiftL(num23, 7) | BinaryOperation.bit32ShiftL(num24, 3) | BinaryOperation.bit32ShiftL(number, 1) | (long)bitmapPointer3.nextPixel();
							int num26;
							if (useSkip && skipBitmap.getPixel(num28, num21) != 0)
							{
								num26 = 0;
							}
							else
							{
								num26 = this.arithmeticDecoder.decodeBit(context2, this.arithmeticDecoder.genericRegionStats);
								if (num26 != 0)
								{
									this.data.set((long)num21 * this.width + (long)num28);
								}
							}
							num23 = (BinaryOperation.bit32ShiftL(num23, 1) | (long)bitmapPointer.nextPixel()) & 7L;
							num24 = (BinaryOperation.bit32ShiftL(num24, 1) | (long)bitmapPointer2.nextPixel()) & 15L;
							number = (BinaryOperation.bit32ShiftL(number, 1) | (long)num26) & 3L;
							num28++;
						}
						goto IL_AB6;
					}
					case 3:
					{
						bitmapPointer2.setPointer(0L, (long)(num21 - 1));
						long num24 = (long)((long)bitmapPointer2.nextPixel() << 1);
						num24 |= (long)bitmapPointer2.nextPixel();
						long number = 0L;
						bitmapPointer3.setPointer((long)adaptiveTemplateX[0], (long)(num21 + (int)adaptiveTemplateY[0]));
						int num29 = 0;
						while ((long)num29 < this.width)
						{
							long context2 = BinaryOperation.bit32ShiftL(num24, 5) | BinaryOperation.bit32ShiftL(number, 1) | (long)bitmapPointer3.nextPixel();
							int num26;
							if (useSkip && skipBitmap.getPixel(num29, num21) != 0)
							{
								num26 = 0;
							}
							else
							{
								num26 = this.arithmeticDecoder.decodeBit(context2, this.arithmeticDecoder.genericRegionStats);
								if (num26 != 0)
								{
									this.data.set((long)num21 * this.width + (long)num29);
								}
							}
							num24 = (BinaryOperation.bit32ShiftL(num24, 1) | (long)bitmapPointer2.nextPixel()) & 31L;
							number = (BinaryOperation.bit32ShiftL(number, 1) | (long)num26) & 15L;
							num29++;
						}
						goto IL_AB6;
					}
					default:
						goto IL_AB6;
					}
				}
			}
		}

		public void readGenericRefinementRegion(long template, bool typicalPredictionGenericRefinementOn, JBIG2Bitmap referredToBitmap, long referenceDX, long referenceDY, short[] adaptiveTemplateX, short[] adaptiveTemplateY)
		{
			long context;
			BitmapPointer bitmapPointer;
			BitmapPointer bitmapPointer2;
			BitmapPointer bitmapPointer3;
			BitmapPointer bitmapPointer4;
			BitmapPointer bitmapPointer5;
			BitmapPointer bitmapPointer6;
			BitmapPointer bitmapPointer7;
			BitmapPointer bitmapPointer8;
			BitmapPointer bitmapPointer9;
			BitmapPointer bitmapPointer10;
			if (template != 0L)
			{
				context = 8L;
				bitmapPointer = new BitmapPointer(this);
				bitmapPointer2 = new BitmapPointer(this);
				bitmapPointer3 = new BitmapPointer(referredToBitmap);
				bitmapPointer4 = new BitmapPointer(referredToBitmap);
				bitmapPointer5 = new BitmapPointer(referredToBitmap);
				bitmapPointer6 = new BitmapPointer(this);
				bitmapPointer7 = new BitmapPointer(this);
				bitmapPointer8 = new BitmapPointer(referredToBitmap);
				bitmapPointer9 = new BitmapPointer(referredToBitmap);
				bitmapPointer10 = new BitmapPointer(referredToBitmap);
			}
			else
			{
				context = 16L;
				bitmapPointer = new BitmapPointer(this);
				bitmapPointer2 = new BitmapPointer(this);
				bitmapPointer3 = new BitmapPointer(referredToBitmap);
				bitmapPointer4 = new BitmapPointer(referredToBitmap);
				bitmapPointer5 = new BitmapPointer(referredToBitmap);
				bitmapPointer6 = new BitmapPointer(this);
				bitmapPointer7 = new BitmapPointer(referredToBitmap);
				bitmapPointer8 = new BitmapPointer(referredToBitmap);
				bitmapPointer9 = new BitmapPointer(referredToBitmap);
				bitmapPointer10 = new BitmapPointer(referredToBitmap);
			}
			bool flag = false;
			int num = 0;
			while ((long)num < this.height)
			{
				if (template != 0L)
				{
					bitmapPointer.setPointer(0L, (long)(num - 1));
					long number = (long)bitmapPointer.nextPixel();
					bitmapPointer2.setPointer(-1L, (long)num);
					bitmapPointer3.setPointer(-referenceDX, (long)(num - 1) - referenceDY);
					bitmapPointer4.setPointer(-1L - referenceDX, (long)num - referenceDY);
					long number2 = (long)bitmapPointer4.nextPixel();
					number2 = BinaryOperation.bit32ShiftL(number2, 1) | (long)bitmapPointer4.nextPixel();
					bitmapPointer5.setPointer(-referenceDX, (long)(num + 1) - referenceDY);
					long num2 = (long)bitmapPointer5.nextPixel();
					long num5;
					long num4;
					long num3 = (num4 = (num5 = 0L));
					if (typicalPredictionGenericRefinementOn)
					{
						bitmapPointer8.setPointer(-1L - referenceDX, (long)(num - 1) - referenceDY);
						num4 = (long)bitmapPointer8.nextPixel();
						num4 = BinaryOperation.bit32ShiftL(num4, 1) | (long)bitmapPointer8.nextPixel();
						num4 = BinaryOperation.bit32ShiftL(num4, 1) | (long)bitmapPointer8.nextPixel();
						bitmapPointer9.setPointer(-1L - referenceDX, (long)num - referenceDY);
						num3 = (long)bitmapPointer9.nextPixel();
						num3 = BinaryOperation.bit32ShiftL(num3, 1) | (long)bitmapPointer9.nextPixel();
						num3 = BinaryOperation.bit32ShiftL(num3, 1) | (long)bitmapPointer9.nextPixel();
						bitmapPointer10.setPointer(-1L - referenceDX, (long)(num + 1) - referenceDY);
						num5 = (long)bitmapPointer10.nextPixel();
						num5 = BinaryOperation.bit32ShiftL(num5, 1) | (long)bitmapPointer10.nextPixel();
						num5 = BinaryOperation.bit32ShiftL(num5, 1) | (long)bitmapPointer10.nextPixel();
					}
					int num6 = 0;
					while ((long)num6 < this.width)
					{
						number = (BinaryOperation.bit32ShiftL(number, 1) | (long)bitmapPointer.nextPixel()) & 7L;
						number2 = (BinaryOperation.bit32ShiftL(number2, 1) | (long)bitmapPointer4.nextPixel()) & 7L;
						num2 = (BinaryOperation.bit32ShiftL(num2, 1) | (long)bitmapPointer5.nextPixel()) & 3L;
						if (!typicalPredictionGenericRefinementOn)
						{
							goto IL_30B;
						}
						num4 = (BinaryOperation.bit32ShiftL(num4, 1) | (long)bitmapPointer8.nextPixel()) & 7L;
						num3 = (BinaryOperation.bit32ShiftL(num3, 1) | (long)bitmapPointer9.nextPixel()) & 7L;
						num5 = (BinaryOperation.bit32ShiftL(num5, 1) | (long)bitmapPointer10.nextPixel()) & 7L;
						int num7 = this.arithmeticDecoder.decodeBit(context, this.arithmeticDecoder.refinementRegionStats);
						if (num7 != 0)
						{
							flag = !flag;
						}
						if (num4 == 0L && num3 == 0L && num5 == 0L)
						{
							this.setPixel((long)num6, (long)num, 0L);
						}
						else
						{
							if (num4 != 7L || num3 != 7L || num5 != 7L)
							{
								goto IL_30B;
							}
							this.setPixel((long)num6, (long)num, 1L);
						}
						IL_36D:
						num6++;
						continue;
						IL_30B:
						long context2 = BinaryOperation.bit32ShiftL(number, 7) | (long)((long)bitmapPointer2.nextPixel() << 6) | (long)((long)bitmapPointer3.nextPixel() << 5) | BinaryOperation.bit32ShiftL(number2, 2) | num2;
						int num8 = this.arithmeticDecoder.decodeBit(context2, this.arithmeticDecoder.refinementRegionStats);
						if (num8 == 1)
						{
							this.data.set((long)num * this.width + (long)num6);
							goto IL_36D;
						}
						goto IL_36D;
					}
				}
				else
				{
					bitmapPointer.setPointer(0L, (long)(num - 1));
					long number = (long)bitmapPointer.nextPixel();
					bitmapPointer2.setPointer(-1L, (long)num);
					bitmapPointer3.setPointer(-referenceDX, (long)(num - 1) - referenceDY);
					long number3 = (long)bitmapPointer3.nextPixel();
					bitmapPointer4.setPointer(-1L - referenceDX, (long)num - referenceDY);
					long number2 = (long)bitmapPointer4.nextPixel();
					number2 = BinaryOperation.bit32ShiftL(number2, 1) | (long)bitmapPointer4.nextPixel();
					bitmapPointer5.setPointer(-1L - referenceDX, (long)(num + 1) - referenceDY);
					long num2 = (long)bitmapPointer5.nextPixel();
					num2 = BinaryOperation.bit32ShiftL(num2, 1) | (long)bitmapPointer5.nextPixel();
					bitmapPointer6.setPointer((long)adaptiveTemplateX[0], (long)(num + (int)adaptiveTemplateY[0]));
					bitmapPointer7.setPointer((long)adaptiveTemplateX[1] - referenceDX, (long)(num + (int)adaptiveTemplateY[1]) - referenceDY);
					long num5;
					long num4;
					long num3 = (num4 = (num5 = 0L));
					if (typicalPredictionGenericRefinementOn)
					{
						bitmapPointer8.setPointer(-1L - referenceDX, (long)(num - 1) - referenceDY);
						num4 = (long)bitmapPointer8.nextPixel();
						num4 = BinaryOperation.bit32ShiftL(num4, 1) | (long)bitmapPointer8.nextPixel();
						num4 = BinaryOperation.bit32ShiftL(num4, 1) | (long)bitmapPointer8.nextPixel();
						bitmapPointer9.setPointer(-1L - referenceDX, (long)num - referenceDY);
						num3 = (long)bitmapPointer9.nextPixel();
						num3 = BinaryOperation.bit32ShiftL(num3, 1) | (long)bitmapPointer9.nextPixel();
						num3 = BinaryOperation.bit32ShiftL(num3, 1) | (long)bitmapPointer9.nextPixel();
						bitmapPointer10.setPointer(-1L - referenceDX, (long)(num + 1) - referenceDY);
						num5 = (long)bitmapPointer10.nextPixel();
						num5 = BinaryOperation.bit32ShiftL(num5, 1) | (long)bitmapPointer10.nextPixel();
						num5 = BinaryOperation.bit32ShiftL(num5, 1) | (long)bitmapPointer10.nextPixel();
					}
					int num9 = 0;
					while ((long)num9 < this.width)
					{
						number = (BinaryOperation.bit32ShiftL(number, 1) | (long)bitmapPointer.nextPixel()) & 3L;
						number3 = (BinaryOperation.bit32ShiftL(number3, 1) | (long)bitmapPointer3.nextPixel()) & 3L;
						number2 = (BinaryOperation.bit32ShiftL(number2, 1) | (long)bitmapPointer4.nextPixel()) & 7L;
						num2 = (BinaryOperation.bit32ShiftL(num2, 1) | (long)bitmapPointer5.nextPixel()) & 7L;
						if (!typicalPredictionGenericRefinementOn)
						{
							goto IL_638;
						}
						num4 = (BinaryOperation.bit32ShiftL(num4, 1) | (long)bitmapPointer8.nextPixel()) & 7L;
						num3 = (BinaryOperation.bit32ShiftL(num3, 1) | (long)bitmapPointer9.nextPixel()) & 7L;
						num5 = (BinaryOperation.bit32ShiftL(num5, 1) | (long)bitmapPointer10.nextPixel()) & 7L;
						int num10 = this.arithmeticDecoder.decodeBit(context, this.arithmeticDecoder.refinementRegionStats);
						if (num10 == 1)
						{
							flag = !flag;
						}
						if (num4 == 0L && num3 == 0L && num5 == 0L)
						{
							this.setPixel((long)num9, (long)num, 0L);
						}
						else
						{
							if (num4 != 7L || num3 != 7L || num5 != 7L)
							{
								goto IL_638;
							}
							this.setPixel((long)num9, (long)num, 1L);
						}
						IL_6AA:
						num9++;
						continue;
						IL_638:
						long context2 = BinaryOperation.bit32ShiftL(number, 11) | (long)((long)bitmapPointer2.nextPixel() << 10) | BinaryOperation.bit32ShiftL(number3, 8) | BinaryOperation.bit32ShiftL(number2, 5) | BinaryOperation.bit32ShiftL(num2, 2) | (long)((long)bitmapPointer6.nextPixel() << 1) | (long)bitmapPointer7.nextPixel();
						int num11 = this.arithmeticDecoder.decodeBit(context2, this.arithmeticDecoder.refinementRegionStats);
						if (num11 == 1)
						{
							this.setPixel((long)num9, (long)num, 1L);
							goto IL_6AA;
						}
						goto IL_6AA;
					}
				}
				num++;
			}
		}

		public void readTextRegion(bool huffman, bool symbolRefine, long noOfSymbolInstances, long logStrips, long noOfSymbols, long[,] symbolCodeTable, long symbolCodeLength, JBIG2Bitmap[] symbols, int defaultPixel, int combinationOperator, bool transposed, int referenceCorner, int sOffset, long[,] huffmanFSTable, long[,] huffmanDSTable, long[,] huffmanDTTable, long[,] huffmanRDWTable, long[,] huffmanRDHTable, long[,] huffmanRDXTable, long[,] huffmanRDYTable, long[,] huffmanRSizeTable, int template, short[] symbolRegionAdaptiveTemplateX, short[] symbolRegionAdaptiveTemplateY, JBIG2StreamDecoder decoder)
		{
			int num = 1 << (int)logStrips;
			this.clear(defaultPixel);
			long num2;
			if (huffman)
			{
				num2 = this.huffmanDecoder.decodeInt(huffmanDTTable).intResult();
			}
			else
			{
				num2 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iadtStats).intResult();
			}
			num2 *= (long)(-(long)num);
			int num3 = 0;
			long num4 = 0L;
			while ((long)num3 < noOfSymbolInstances)
			{
				long num5;
				if (huffman)
				{
					num5 = this.huffmanDecoder.decodeInt(huffmanDTTable).intResult();
				}
				else
				{
					num5 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iadtStats).intResult();
				}
				num2 += num5 * (long)num;
				long num6;
				if (huffman)
				{
					num6 = this.huffmanDecoder.decodeInt(huffmanFSTable).intResult();
				}
				else
				{
					num6 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iafsStats).intResult();
				}
				num4 += num6;
				long num7 = num4;
				for (;;)
				{
					if (num == 1)
					{
						num5 = 0L;
					}
					else if (huffman)
					{
						num5 = (long)decoder.readBits(logStrips);
					}
					else
					{
						num5 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iaitStats).intResult();
					}
					long num8 = num2 + num5;
					long num9;
					if (huffman)
					{
						if (symbolCodeTable != null)
						{
							num9 = this.huffmanDecoder.decodeInt(symbolCodeTable).intResult();
						}
						else
						{
							num9 = (long)decoder.readBits(symbolCodeLength);
						}
					}
					else
					{
						num9 = this.arithmeticDecoder.decodeIAID(symbolCodeLength, this.arithmeticDecoder.iaidStats);
					}
					if (num9 >= noOfSymbols)
					{
						if (JBIG2StreamDecoder.debug)
						{
							Console.WriteLine("Invalid symbol number in JBIG2 text region");
						}
					}
					else
					{
						long num10;
						if (symbolRefine)
						{
							if (huffman)
							{
								num10 = (long)decoder.readBit();
							}
							else
							{
								num10 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iariStats).intResult();
							}
						}
						else
						{
							num10 = 0L;
						}
						JBIG2Bitmap jbig2Bitmap;
						if (num10 != 0L)
						{
							long num11;
							long num12;
							long num13;
							long num14;
							if (huffman)
							{
								num11 = this.huffmanDecoder.decodeInt(huffmanRDWTable).intResult();
								num12 = this.huffmanDecoder.decodeInt(huffmanRDHTable).intResult();
								num13 = this.huffmanDecoder.decodeInt(huffmanRDXTable).intResult();
								num14 = this.huffmanDecoder.decodeInt(huffmanRDYTable).intResult();
								decoder.consumeRemainingBits();
								this.arithmeticDecoder.start();
							}
							else
							{
								num11 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardwStats).intResult();
								num12 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardhStats).intResult();
								num13 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardxStats).intResult();
								num14 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardyStats).intResult();
							}
							num13 = ((num11 >= 0L) ? num11 : (num11 - 1L)) / 2L + num13;
							num14 = ((num12 >= 0L) ? num12 : (num12 - 1L)) / 2L + num14;
							jbig2Bitmap = new JBIG2Bitmap(num11 + symbols[(int)num9].width, num12 + symbols[(int)num9].height, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
							jbig2Bitmap.readGenericRefinementRegion((long)template, false, symbols[(int)num9], num13, num14, symbolRegionAdaptiveTemplateX, symbolRegionAdaptiveTemplateY);
						}
						else
						{
							jbig2Bitmap = symbols[(int)num9];
						}
						long num15 = jbig2Bitmap.width - 1L;
						long num16 = jbig2Bitmap.height - 1L;
						if (transposed)
						{
							switch (referenceCorner)
							{
							case 0:
								this.combine(jbig2Bitmap, num8, num7, (long)combinationOperator);
								break;
							case 1:
								this.combine(jbig2Bitmap, num8, num7, (long)combinationOperator);
								break;
							case 2:
								this.combine(jbig2Bitmap, num8 - num15, num7, (long)combinationOperator);
								break;
							case 3:
								this.combine(jbig2Bitmap, num8 - num15, num7, (long)combinationOperator);
								break;
							}
							num7 += num16;
						}
						else
						{
							switch (referenceCorner)
							{
							case 0:
								this.combine(jbig2Bitmap, num7, num8 - num16, (long)combinationOperator);
								break;
							case 1:
								this.combine(jbig2Bitmap, num7, num8, (long)combinationOperator);
								break;
							case 2:
								this.combine(jbig2Bitmap, num7, num8 - num16, (long)combinationOperator);
								break;
							case 3:
								this.combine(jbig2Bitmap, num7, num8, (long)combinationOperator);
								break;
							}
							num7 += num15;
						}
					}
					num3++;
					DecodeIntResult decodeIntResult;
					if (huffman)
					{
						decodeIntResult = this.huffmanDecoder.decodeInt(huffmanDSTable);
					}
					else
					{
						decodeIntResult = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iadsStats);
					}
					if (!decodeIntResult.booleanResult())
					{
						break;
					}
					num6 = decodeIntResult.intResult();
					num7 += (long)sOffset + num6;
				}
			}
		}

		public void readTextRegion2(bool huffman, bool symbolRefine, long noOfSymbolInstances, long logStrips, long noOfSymbols, long[][] symbolCodeTable, long symbolCodeLength, JBIG2Bitmap[] symbols, int defaultPixel, int combinationOperator, bool transposed, int referenceCorner, int sOffset, long[,] huffmanFSTable, long[,] huffmanDSTable, long[,] huffmanDTTable, long[,] huffmanRDWTable, long[,] huffmanRDHTable, long[,] huffmanRDXTable, long[,] huffmanRDYTable, long[,] huffmanRSizeTable, int template, short[] symbolRegionAdaptiveTemplateX, short[] symbolRegionAdaptiveTemplateY, JBIG2StreamDecoder decoder)
		{
			int num = 1 << (int)logStrips;
			this.clear(defaultPixel);
			long num2;
			if (huffman)
			{
				num2 = this.huffmanDecoder.decodeInt(huffmanDTTable).intResult();
			}
			else
			{
				num2 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iadtStats).intResult();
			}
			num2 *= (long)(-(long)num);
			int num3 = 0;
			long num4 = 0L;
			while ((long)num3 < noOfSymbolInstances)
			{
				long num5;
				if (huffman)
				{
					num5 = this.huffmanDecoder.decodeInt(huffmanDTTable).intResult();
				}
				else
				{
					num5 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iadtStats).intResult();
				}
				num2 += num5 * (long)num;
				long num6;
				if (huffman)
				{
					num6 = this.huffmanDecoder.decodeInt(huffmanFSTable).intResult();
				}
				else
				{
					num6 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iafsStats).intResult();
				}
				num4 += num6;
				long num7 = num4;
				for (;;)
				{
					if (num == 1)
					{
						num5 = 0L;
					}
					else if (huffman)
					{
						num5 = (long)decoder.readBits(logStrips);
					}
					else
					{
						num5 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iaitStats).intResult();
					}
					long num8 = num2 + num5;
					long num9;
					if (huffman)
					{
						if (symbolCodeTable != null)
						{
							num9 = this.huffmanDecoder.decodeInt(symbolCodeTable).intResult();
						}
						else
						{
							num9 = (long)decoder.readBits(symbolCodeLength);
						}
					}
					else
					{
						num9 = this.arithmeticDecoder.decodeIAID(symbolCodeLength, this.arithmeticDecoder.iaidStats);
					}
					if (num9 >= noOfSymbols)
					{
						if (JBIG2StreamDecoder.debug)
						{
							Console.WriteLine("Invalid symbol number in JBIG2 text region");
						}
					}
					else
					{
						long num10;
						if (symbolRefine)
						{
							if (huffman)
							{
								num10 = (long)decoder.readBit();
							}
							else
							{
								num10 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iariStats).intResult();
							}
						}
						else
						{
							num10 = 0L;
						}
						JBIG2Bitmap jbig2Bitmap;
						if (num10 != 0L)
						{
							long num11;
							long num12;
							long num13;
							long num14;
							if (huffman)
							{
								num11 = this.huffmanDecoder.decodeInt(huffmanRDWTable).intResult();
								num12 = this.huffmanDecoder.decodeInt(huffmanRDHTable).intResult();
								num13 = this.huffmanDecoder.decodeInt(huffmanRDXTable).intResult();
								num14 = this.huffmanDecoder.decodeInt(huffmanRDYTable).intResult();
								decoder.consumeRemainingBits();
								this.arithmeticDecoder.start();
							}
							else
							{
								num11 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardwStats).intResult();
								num12 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardhStats).intResult();
								num13 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardxStats).intResult();
								num14 = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iardyStats).intResult();
							}
							num13 = ((num11 >= 0L) ? num11 : (num11 - 1L)) / 2L + num13;
							num14 = ((num12 >= 0L) ? num12 : (num12 - 1L)) / 2L + num14;
							jbig2Bitmap = new JBIG2Bitmap(num11 + symbols[(int)num9].width, num12 + symbols[(int)num9].height, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
							jbig2Bitmap.readGenericRefinementRegion((long)template, false, symbols[(int)num9], num13, num14, symbolRegionAdaptiveTemplateX, symbolRegionAdaptiveTemplateY);
						}
						else
						{
							jbig2Bitmap = symbols[(int)num9];
						}
						long num15 = jbig2Bitmap.width - 1L;
						long num16 = jbig2Bitmap.height - 1L;
						if (transposed)
						{
							switch (referenceCorner)
							{
							case 0:
								this.combine(jbig2Bitmap, num8, num7, (long)combinationOperator);
								break;
							case 1:
								this.combine(jbig2Bitmap, num8, num7, (long)combinationOperator);
								break;
							case 2:
								this.combine(jbig2Bitmap, num8 - num15, num7, (long)combinationOperator);
								break;
							case 3:
								this.combine(jbig2Bitmap, num8 - num15, num7, (long)combinationOperator);
								break;
							}
							num7 += num16;
						}
						else
						{
							switch (referenceCorner)
							{
							case 0:
								this.combine(jbig2Bitmap, num7, num8 - num16, (long)combinationOperator);
								break;
							case 1:
								this.combine(jbig2Bitmap, num7, num8, (long)combinationOperator);
								break;
							case 2:
								this.combine(jbig2Bitmap, num7, num8 - num16, (long)combinationOperator);
								break;
							case 3:
								this.combine(jbig2Bitmap, num7, num8, (long)combinationOperator);
								break;
							}
							num7 += num15;
						}
					}
					num3++;
					DecodeIntResult decodeIntResult;
					if (huffman)
					{
						decodeIntResult = this.huffmanDecoder.decodeInt(huffmanDSTable);
					}
					else
					{
						decodeIntResult = this.arithmeticDecoder.decodeInt(this.arithmeticDecoder.iadsStats);
					}
					if (!decodeIntResult.booleanResult())
					{
						break;
					}
					num6 = decodeIntResult.intResult();
					num7 += (long)sOffset + num6;
				}
			}
		}

		public void clear(int defPixel)
		{
			this.data.setAll(defPixel == 1);
		}

		public void combine(JBIG2Bitmap bitmap, long x, long y, long combOp)
		{
			long num = bitmap.width;
			long num2 = bitmap.height;
			long num3 = num;
			if (x + num > this.width)
			{
				num3 = this.width - x;
			}
			if (y + num2 > this.height)
			{
				num2 = this.height - y;
			}
			long num4 = 0L;
			long num5 = y * this.width + x;
			if (combOp == 0L)
			{
				if (x == 0L && y == 0L && num2 == this.height && num == this.width)
				{
					for (long num6 = 0L; num6 < (long)this.data.w.Length; num6 += 1L)
					{
						checked
						{
							this.data.w[(int)((IntPtr)num6)] |= bitmap.data.w[(int)((IntPtr)num6)];
						}
					}
				}
				for (long num7 = y; num7 < y + num2; num7 += 1L)
				{
					num5 = num7 * this.width + x;
					this.data.or(num5, bitmap.data, num4, num3);
					num4 += num;
				}
				return;
			}
			if (combOp == 1L)
			{
				if (x == 0L && y == 0L && num2 == this.height && num == this.width)
				{
					for (int i = 0; i < this.data.w.Length; i++)
					{
						this.data.w[i] &= bitmap.data.w[i];
					}
				}
				for (long num8 = y; num8 < y + num2; num8 += 1L)
				{
					num5 = num8 * this.width + x;
					int num9 = 0;
					while ((long)num9 < num3)
					{
						this.data.set(num5, bitmap.data.get(num4 + (long)num9) && this.data.get(num5));
						num5 += 1L;
						num9++;
					}
					num4 += num;
				}
				return;
			}
			if (combOp == 2L)
			{
				if (x == 0L && y == 0L && num2 == this.height && num == this.width)
				{
					for (int j = 0; j < this.data.w.Length; j++)
					{
						this.data.w[j] ^= bitmap.data.w[j];
					}
					return;
				}
				for (long num10 = y; num10 < y + num2; num10 += 1L)
				{
					num5 = num10 * this.width + x;
					int num11 = 0;
					while ((long)num11 < num3)
					{
						this.data.set(num5, bitmap.data.get(num4 + (long)num11) ^ this.data.get(num5));
						num5 += 1L;
						num11++;
					}
					num4 += num;
				}
				return;
			}
			else
			{
				if (combOp == 3L)
				{
					for (long num12 = y; num12 < y + num2; num12 += 1L)
					{
						num5 = num12 * this.width + x;
						int num13 = 0;
						while ((long)num13 < num3)
						{
							bool flag = bitmap.data.get(num4 + (long)num13);
							bool flag2 = this.data.get(num5);
							this.data.set(num5, flag2 == flag);
							num5 += 1L;
							num13++;
						}
						num4 += num;
					}
					return;
				}
				if (combOp == 4L)
				{
					if (x == 0L && y == 0L && num2 == this.height && num == this.width)
					{
						for (long num14 = 0L; num14 < (long)this.data.w.Length; num14 += 1L)
						{
							checked
							{
								this.data.w[(int)((IntPtr)num14)] = bitmap.data.w[(int)((IntPtr)num14)];
							}
						}
						return;
					}
					for (long num15 = y; num15 < y + num2; num15 += 1L)
					{
						num5 = num15 * this.width + x;
						int num16 = 0;
						while ((long)num16 < num3)
						{
							this.data.set(num5, bitmap.data.get(num4 + (long)num16));
							num4 += 1L;
							num5 += 1L;
							num16++;
						}
						num4 += num;
					}
				}
				return;
			}
		}

		void duplicateRow(int yDest, int ySrc)
		{
			int num = 0;
			while ((long)num < this.width)
			{
				this.setPixel((long)num, (long)yDest, (long)this.getPixel(num, ySrc));
				num++;
			}
		}

		public long getWidth()
		{
			return this.width;
		}

		public long getHeight()
		{
			return this.height;
		}

		public byte[] getData(bool switchPixelColor)
		{
			byte[] array = new byte[this.height * this.line];
			long num = 0L;
			long num2 = 0L;
			long num3 = 0L;
			int num4 = 0;
			while ((long)num4 < this.height)
			{
				int num5 = 0;
				while ((long)num5 < this.width)
				{
					if ((num & (long)FastBitSet.mask) == 0L)
					{
						num3 = this.data.w[(int)((ulong)num >> FastBitSet.pot)];
					}
					long num6 = 7L - (num2 & 7L);
					byte[] array2 = array;
					IntPtr intPtr = checked((IntPtr)(num2 >> 3));
					array2[(int)intPtr] = (byte)(array2[(int)intPtr] | (byte)(((int)((ulong)num3 >> (int)num) & 1) << (int)num6));
					num += 1L;
					num2 += 1L;
					num5++;
				}
				num2 = this.line * 8L * (long)(num4 + 1);
				num4++;
			}
			if (switchPixelColor)
			{
				for (int i = 0; i < array.Length; i++)
				{
					byte[] array3 = array;
					int num7 = i;
					array3[num7] ^= byte.MaxValue;
				}
			}
			return array;
		}

		public JBIG2Bitmap getSlice(long x, long y, long width, long height)
		{
			JBIG2Bitmap jbig2Bitmap = new JBIG2Bitmap(width, height, this.arithmeticDecoder, this.huffmanDecoder, this.mmrDecoder);
			int num = 0;
			for (long num2 = y; num2 < height; num2 += 1L)
			{
				long num3 = num2 * this.width + x;
				for (long num4 = x; num4 < x + width; num4 += 1L)
				{
					if (this.data.get(num3))
					{
						jbig2Bitmap.data.set((long)num);
					}
					num++;
					num3 += 1L;
				}
			}
			return jbig2Bitmap;
		}

		void setPixel(long col, long row, FastBitSet data, long value)
		{
			long index = row * this.width + col;
			data.set(index, value == 1L);
		}

		public void setPixel(long col, long row, long value)
		{
			this.setPixel(col, row, this.data, value);
		}

		public int getPixel(int col, int row)
		{
			if (!this.data.get((long)row * this.width + (long)col))
			{
				return 0;
			}
			return 1;
		}

		public void expand(int newHeight, int defaultPixel)
		{
			FastBitSet fastBitSet = new FastBitSet((long)newHeight * this.width);
			int num = 0;
			while ((long)num < this.height)
			{
				int num2 = 0;
				while ((long)num2 < this.width)
				{
					this.setPixel((long)num2, (long)num, fastBitSet, (long)this.getPixel(num2, num));
					num2++;
				}
				num++;
			}
			this.height = (long)newHeight;
			this.data = fastBitSet;
		}

		public void setBitmapNumber(int segmentNumber)
		{
			this.bitmapNumber = segmentNumber;
		}

		public int getBitmapNumber()
		{
			return this.bitmapNumber;
		}

		public byte[] getBufferedImage()
		{
			byte[] array = this.getData(true);
			if (array == null)
			{
				return null;
			}
			int num = array.Length;
			byte[] array2 = new byte[num];
			Array.Copy(array, 0, array2, 0, num);
			return array2;
		}

		long width;

		long height;

		long line;

		int bitmapNumber;

		public FastBitSet data;

		ArithmeticDecoder arithmeticDecoder;

		HuffmanDecoder huffmanDecoder;

		MMRDecoder mmrDecoder;
	}
}
