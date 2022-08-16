using System;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	sealed class Pack
	{
		internal static void UInt32_To_BE(uint n, byte[] bs)
		{
			bs[0] = (byte)(n >> 24);
			bs[1] = (byte)(n >> 16);
			bs[2] = (byte)(n >> 8);
			bs[3] = (byte)n;
		}

		internal static void UInt32_To_BE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 24);
			bs[++off] = (byte)(n >> 16);
			bs[++off] = (byte)(n >> 8);
			bs[++off] = (byte)n;
		}

		internal static uint BE_To_UInt32(byte[] bs)
		{
			uint num = (uint)((uint)bs[0] << 24);
			num |= (uint)((uint)bs[1] << 16);
			num |= (uint)((uint)bs[2] << 8);
			return num | (uint)bs[3];
		}

		internal static uint BE_To_UInt32(byte[] bs, int off)
		{
			uint num = (uint)((uint)bs[off] << 24);
			num |= (uint)((uint)bs[++off] << 16);
			num |= (uint)((uint)bs[++off] << 8);
			return num | (uint)bs[++off];
		}

		internal static ulong BE_To_UInt64(byte[] bs)
		{
			uint num = Pack.BE_To_UInt32(bs);
			uint num2 = Pack.BE_To_UInt32(bs, 4);
			return ((ulong)num << 32) | (ulong)num2;
		}

		internal static ulong BE_To_UInt64(byte[] bs, int off)
		{
			uint num = Pack.BE_To_UInt32(bs, off);
			uint num2 = Pack.BE_To_UInt32(bs, off + 4);
			return ((ulong)num << 32) | (ulong)num2;
		}

		internal static void UInt64_To_BE(ulong n, byte[] bs)
		{
			Pack.UInt32_To_BE((uint)(n >> 32), bs);
			Pack.UInt32_To_BE((uint)n, bs, 4);
		}

		internal static void UInt64_To_BE(ulong n, byte[] bs, int off)
		{
			Pack.UInt32_To_BE((uint)(n >> 32), bs, off);
			Pack.UInt32_To_BE((uint)n, bs, off + 4);
		}

		internal static void UInt32_To_LE(uint n, byte[] bs)
		{
			bs[0] = (byte)n;
			bs[1] = (byte)(n >> 8);
			bs[2] = (byte)(n >> 16);
			bs[3] = (byte)(n >> 24);
		}

		internal static void UInt32_To_LE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)n;
			bs[++off] = (byte)(n >> 8);
			bs[++off] = (byte)(n >> 16);
			bs[++off] = (byte)(n >> 24);
		}

		internal static uint LE_To_UInt32(byte[] bs)
		{
			uint num = (uint)bs[0];
			num |= (uint)((uint)bs[1] << 8);
			num |= (uint)((uint)bs[2] << 16);
			return num | (uint)((uint)bs[3] << 24);
		}

		internal static uint LE_To_UInt32(byte[] bs, int off)
		{
			uint num = (uint)bs[off];
			num |= (uint)((uint)bs[++off] << 8);
			num |= (uint)((uint)bs[++off] << 16);
			return num | (uint)((uint)bs[++off] << 24);
		}

		internal static ulong LE_To_UInt64(byte[] bs)
		{
			uint num = Pack.LE_To_UInt32(bs);
			uint num2 = Pack.LE_To_UInt32(bs, 4);
			return ((ulong)num2 << 32) | (ulong)num;
		}

		internal static ulong LE_To_UInt64(byte[] bs, int off)
		{
			uint num = Pack.LE_To_UInt32(bs, off);
			uint num2 = Pack.LE_To_UInt32(bs, off + 4);
			return ((ulong)num2 << 32) | (ulong)num;
		}

		internal static void UInt64_To_LE(ulong n, byte[] bs)
		{
			Pack.UInt32_To_LE((uint)n, bs);
			Pack.UInt32_To_LE((uint)(n >> 32), bs, 4);
		}

		internal static void UInt64_To_LE(ulong n, byte[] bs, int off)
		{
			Pack.UInt32_To_LE((uint)n, bs, off);
			Pack.UInt32_To_LE((uint)(n >> 32), bs, off + 4);
		}
	}
}
