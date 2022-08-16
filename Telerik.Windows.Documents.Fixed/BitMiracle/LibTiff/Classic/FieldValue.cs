using System;

namespace BitMiracle.LibTiff.Classic
{
	struct FieldValue
	{
		internal FieldValue(object o)
		{
			this.m_value = o;
		}

		internal static FieldValue[] FromParams(params object[] list)
		{
			FieldValue[] array = new FieldValue[list.Length];
			for (int i = 0; i < list.Length; i++)
			{
				if (list[i] is FieldValue)
				{
					array[i] = new FieldValue(((FieldValue)list[i]).Value);
				}
				else
				{
					array[i] = new FieldValue(list[i]);
				}
			}
			return array;
		}

		internal void Set(object o)
		{
			this.m_value = o;
		}

		public object Value
		{
			get
			{
				return this.m_value;
			}
		}

		public byte ToByte()
		{
			return Convert.ToByte(this.m_value);
		}

		public short ToShort()
		{
			return Convert.ToInt16(this.m_value);
		}

		public ushort ToUShort()
		{
			return Convert.ToUInt16(this.m_value);
		}

		public int ToInt()
		{
			return Convert.ToInt32(this.m_value);
		}

		public uint ToUInt()
		{
			return Convert.ToUInt32(this.m_value);
		}

		public float ToFloat()
		{
			return Convert.ToSingle(this.m_value);
		}

		public double ToDouble()
		{
			return Convert.ToDouble(this.m_value);
		}

		public override string ToString()
		{
			if (this.m_value is byte[])
			{
				return Tiff.Latin1Encoding.GetString(this.m_value as byte[]);
			}
			return Convert.ToString(this.m_value);
		}

		public byte[] GetBytes()
		{
			if (this.m_value == null)
			{
				return null;
			}
			Type type = this.m_value.GetType();
			if (type.IsArray)
			{
				if (this.m_value is byte[])
				{
					return this.m_value as byte[];
				}
				if (this.m_value is short[])
				{
					short[] array = this.m_value as short[];
					byte[] array2 = new byte[array.Length * 2];
					Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
					return array2;
				}
				if (this.m_value is ushort[])
				{
					ushort[] array3 = this.m_value as ushort[];
					byte[] array4 = new byte[array3.Length * 2];
					Buffer.BlockCopy(array3, 0, array4, 0, array4.Length);
					return array4;
				}
				if (this.m_value is int[])
				{
					int[] array5 = this.m_value as int[];
					byte[] array6 = new byte[array5.Length * 4];
					Buffer.BlockCopy(array5, 0, array6, 0, array6.Length);
					return array6;
				}
				if (this.m_value is uint[])
				{
					uint[] array7 = this.m_value as uint[];
					byte[] array8 = new byte[array7.Length * 4];
					Buffer.BlockCopy(array7, 0, array8, 0, array8.Length);
					return array8;
				}
				if (this.m_value is float[])
				{
					float[] array9 = this.m_value as float[];
					byte[] array10 = new byte[array9.Length * 4];
					Buffer.BlockCopy(array9, 0, array10, 0, array10.Length);
					return array10;
				}
				if (this.m_value is double[])
				{
					double[] array11 = this.m_value as double[];
					byte[] array12 = new byte[array11.Length * 8];
					Buffer.BlockCopy(array11, 0, array12, 0, array12.Length);
					return array12;
				}
			}
			else if (this.m_value is string)
			{
				return Tiff.Latin1Encoding.GetBytes(this.m_value as string);
			}
			return null;
		}

		public byte[] ToByteArray()
		{
			if (this.m_value == null)
			{
				return null;
			}
			Type type = this.m_value.GetType();
			if (type.IsArray)
			{
				if (this.m_value is byte[])
				{
					return this.m_value as byte[];
				}
				if (this.m_value is short[])
				{
					short[] array = this.m_value as short[];
					byte[] array2 = new byte[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array2[i] = (byte)array[i];
					}
					return array2;
				}
				if (this.m_value is ushort[])
				{
					ushort[] array3 = this.m_value as ushort[];
					byte[] array4 = new byte[array3.Length];
					for (int j = 0; j < array3.Length; j++)
					{
						array4[j] = (byte)array3[j];
					}
					return array4;
				}
				if (this.m_value is int[])
				{
					int[] array5 = this.m_value as int[];
					byte[] array6 = new byte[array5.Length];
					for (int k = 0; k < array5.Length; k++)
					{
						array6[k] = (byte)array5[k];
					}
					return array6;
				}
				if (this.m_value is uint[])
				{
					uint[] array7 = this.m_value as uint[];
					byte[] array8 = new byte[array7.Length];
					for (int l = 0; l < array7.Length; l++)
					{
						array8[l] = (byte)array7[l];
					}
					return array8;
				}
			}
			else if (this.m_value is string)
			{
				return Tiff.Latin1Encoding.GetBytes(this.m_value as string);
			}
			return null;
		}

		public short[] ToShortArray()
		{
			if (this.m_value == null)
			{
				return null;
			}
			Type type = this.m_value.GetType();
			if (type.IsArray)
			{
				if (this.m_value is short[])
				{
					return this.m_value as short[];
				}
				if (this.m_value is byte[])
				{
					byte[] array = this.m_value as byte[];
					if (array.Length % 2 != 0)
					{
						return null;
					}
					int num = array.Length / 2;
					short[] array2 = new short[num];
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						short num3 = BitConverter.ToInt16(array, num2);
						array2[i] = num3;
						num2 += 2;
					}
					return array2;
				}
				else
				{
					if (this.m_value is ushort[])
					{
						ushort[] array3 = this.m_value as ushort[];
						short[] array4 = new short[array3.Length];
						for (int j = 0; j < array3.Length; j++)
						{
							array4[j] = (short)array3[j];
						}
						return array4;
					}
					if (this.m_value is int[])
					{
						int[] array5 = this.m_value as int[];
						short[] array6 = new short[array5.Length];
						for (int k = 0; k < array5.Length; k++)
						{
							array6[k] = (short)array5[k];
						}
						return array6;
					}
					if (this.m_value is uint[])
					{
						uint[] array7 = this.m_value as uint[];
						short[] array8 = new short[array7.Length];
						for (int l = 0; l < array7.Length; l++)
						{
							array8[l] = (short)array7[l];
						}
						return array8;
					}
				}
			}
			return null;
		}

		public ushort[] ToUShortArray()
		{
			if (this.m_value == null)
			{
				return null;
			}
			Type type = this.m_value.GetType();
			if (type.IsArray)
			{
				if (this.m_value is ushort[])
				{
					return this.m_value as ushort[];
				}
				if (this.m_value is byte[])
				{
					byte[] array = this.m_value as byte[];
					if (array.Length % 2 != 0)
					{
						return null;
					}
					int num = array.Length / 2;
					ushort[] array2 = new ushort[num];
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						ushort num3 = BitConverter.ToUInt16(array, num2);
						array2[i] = num3;
						num2 += 2;
					}
					return array2;
				}
				else
				{
					if (this.m_value is short[])
					{
						short[] array3 = this.m_value as short[];
						ushort[] array4 = new ushort[array3.Length];
						for (int j = 0; j < array3.Length; j++)
						{
							array4[j] = (ushort)array3[j];
						}
						return array4;
					}
					if (this.m_value is int[])
					{
						int[] array5 = this.m_value as int[];
						ushort[] array6 = new ushort[array5.Length];
						for (int k = 0; k < array5.Length; k++)
						{
							array6[k] = (ushort)array5[k];
						}
						return array6;
					}
					if (this.m_value is uint[])
					{
						uint[] array7 = this.m_value as uint[];
						ushort[] array8 = new ushort[array7.Length];
						for (int l = 0; l < array7.Length; l++)
						{
							array8[l] = (ushort)array7[l];
						}
						return array8;
					}
				}
			}
			return null;
		}

		public int[] ToIntArray()
		{
			if (this.m_value == null)
			{
				return null;
			}
			Type type = this.m_value.GetType();
			if (type.IsArray)
			{
				if (this.m_value is int[])
				{
					return this.m_value as int[];
				}
				if (this.m_value is byte[])
				{
					byte[] array = this.m_value as byte[];
					if (array.Length % 4 != 0)
					{
						return null;
					}
					int num = array.Length / 4;
					int[] array2 = new int[num];
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						int num3 = BitConverter.ToInt32(array, num2);
						array2[i] = num3;
						num2 += 4;
					}
					return array2;
				}
				else
				{
					if (this.m_value is short[])
					{
						short[] array3 = this.m_value as short[];
						int[] array4 = new int[array3.Length];
						for (int j = 0; j < array3.Length; j++)
						{
							array4[j] = (int)array3[j];
						}
						return array4;
					}
					if (this.m_value is ushort[])
					{
						ushort[] array5 = this.m_value as ushort[];
						int[] array6 = new int[array5.Length];
						for (int k = 0; k < array5.Length; k++)
						{
							array6[k] = (int)array5[k];
						}
						return array6;
					}
					if (this.m_value is uint[])
					{
						uint[] array7 = this.m_value as uint[];
						int[] array8 = new int[array7.Length];
						for (int l = 0; l < array7.Length; l++)
						{
							array8[l] = (int)array7[l];
						}
						return array8;
					}
				}
			}
			return null;
		}

		public uint[] ToUIntArray()
		{
			if (this.m_value == null)
			{
				return null;
			}
			Type type = this.m_value.GetType();
			if (type.IsArray)
			{
				if (this.m_value is uint[])
				{
					return this.m_value as uint[];
				}
				if (this.m_value is byte[])
				{
					byte[] array = this.m_value as byte[];
					if (array.Length % 4 != 0)
					{
						return null;
					}
					int num = array.Length / 4;
					uint[] array2 = new uint[num];
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						uint num3 = BitConverter.ToUInt32(array, num2);
						array2[i] = num3;
						num2 += 4;
					}
					return array2;
				}
				else
				{
					if (this.m_value is short[])
					{
						short[] array3 = this.m_value as short[];
						uint[] array4 = new uint[array3.Length];
						for (int j = 0; j < array3.Length; j++)
						{
							array4[j] = (uint)array3[j];
						}
						return array4;
					}
					if (this.m_value is ushort[])
					{
						ushort[] array5 = this.m_value as ushort[];
						uint[] array6 = new uint[array5.Length];
						for (int k = 0; k < array5.Length; k++)
						{
							array6[k] = (uint)array5[k];
						}
						return array6;
					}
					if (this.m_value is int[])
					{
						int[] array7 = this.m_value as int[];
						uint[] array8 = new uint[array7.Length];
						for (int l = 0; l < array7.Length; l++)
						{
							array8[l] = (uint)array7[l];
						}
						return array8;
					}
				}
			}
			return null;
		}

		public float[] ToFloatArray()
		{
			if (this.m_value == null)
			{
				return null;
			}
			Type type = this.m_value.GetType();
			if (type.IsArray)
			{
				if (this.m_value is float[])
				{
					return this.m_value as float[];
				}
				if (this.m_value is double[])
				{
					double[] array = this.m_value as double[];
					float[] array2 = new float[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array2[i] = (float)array[i];
					}
					return array2;
				}
				if (this.m_value is byte[])
				{
					byte[] array3 = this.m_value as byte[];
					if (array3.Length % 4 != 0)
					{
						return null;
					}
					int num = 0;
					int num2 = array3.Length / 4;
					float[] array4 = new float[num2];
					for (int j = 0; j < num2; j++)
					{
						float num3 = BitConverter.ToSingle(array3, num);
						array4[j] = num3;
						num += 4;
					}
					return array4;
				}
			}
			return null;
		}

		public double[] ToDoubleArray()
		{
			if (this.m_value == null)
			{
				return null;
			}
			Type type = this.m_value.GetType();
			if (type.IsArray)
			{
				if (this.m_value is double[])
				{
					return this.m_value as double[];
				}
				if (this.m_value is float[])
				{
					float[] array = this.m_value as float[];
					double[] array2 = new double[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array2[i] = (double)array[i];
					}
					return array2;
				}
				if (this.m_value is byte[])
				{
					byte[] array3 = this.m_value as byte[];
					if (array3.Length % 8 != 0)
					{
						return null;
					}
					int num = 0;
					int num2 = array3.Length / 8;
					double[] array4 = new double[num2];
					for (int j = 0; j < num2; j++)
					{
						double num3 = BitConverter.ToDouble(array3, num);
						array4[j] = num3;
						num += 8;
					}
					return array4;
				}
			}
			return null;
		}

		object m_value;
	}
}
