// Decompiled with JetBrains decompiler
// Type: Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities.BytesHelper
// Assembly: Telerik.Windows.Documents.Fixed, Version=2019.2.503.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// MVID: 6454440F-C021-4D95-99F0-3BFB2485AF25
// Assembly location: C:\Users\user\Downloads\FromFileToFileCore\FromFileToFileCore\Export_Word_Excel_PDF_CSV_HTML-master\ExportDemo1\ConsoleApp1\bin\x64\Debug\Telerik.Windows.Documents.Fixed.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
    internal static class BytesHelper
    {
        public static byte GetDecimalFromHex(char hex)
        {
            if (hex >= '0' && hex <= '9')
                return (byte)((uint)hex - 48U);
            return hex >= 'a' && hex <= 'f' ? (byte)((int)hex - 97 + 10) : (byte)((int)hex - 65 + 10);
        }

        public static string ToHexString(byte[] data)
        {
            StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
            foreach (byte num in data)
                stringBuilder.AppendFormat("{0:x2}", (object)num);
            return stringBuilder.ToString();
        }

        public static byte[] ToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");
            int length = hex.Length >> 1;
            byte[] numArray = new byte[length];
            for (int index = 0; index < length; ++index)
                numArray[index] = (byte)((BytesHelper.GetHexVal(hex[index << 1]) << 4) + BytesHelper.GetHexVal(hex[(index << 1) + 1]));
            return numArray;
        }

        public static int GetHexVal(char hex) => (int)hex - (hex < ':' ? 48 : (hex < 'a' ? 55 : 87));

        public static int GetInt(byte[] bytes)
        {
            byte[] bytes1 = BytesHelper.GetBytes(bytes);
            if (bytes1 == null)
                return -1;
            int num = 0;
            int length = bytes1.Length;
            for (int index = 0; index < length; ++index)
            {
                num |= length <= index ? 0 : (int)bytes1[index] & (int)byte.MaxValue;
                if (index < length - 1)
                    num <<= 8;
            }
            return num;
        }

        public static void GetBytesReverse(int value, byte[] result)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            for (int index1 = 0; index1 < bytes.Length; ++index1)
            {
                int index2 = result.Length - index1 - 1;
                if (index2 < 0 || index2 >= result.Length)
                    break;
                result[index2] = bytes[index1];
            }
        }

        public static byte[] MergeBytes(byte[] first, byte[] second)
        {
            LinkedList<byte> source = new LinkedList<byte>();
            foreach (byte num in first)
                source.AddFirst(num);
            foreach (byte num in second)
                source.AddLast(num);
            return source.ToArray<byte>();
        }

        public static byte[] GetBytes(Stream stream)
        {
            if (stream.Position > 0L)
                stream.Seek(0L, SeekOrigin.Begin);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public static void GetBytes(int value, byte[] result)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            for (int index = 0; index < result.Length; ++index)
                result[index] = bytes[index];
        }

        private static byte[] GetBytes(byte[] bytes)
        {
            Guard.ThrowExceptionIfNull<byte[]>(bytes, nameof(bytes));
            Guard.ThrowExceptionIfLessThanOrEqual<int>(0, bytes.Length, nameof(bytes));
            Guard.ThrowExceptionIfGreaterThan<int>(4, bytes.Length, nameof(bytes));
            int length1 = bytes.Length;
            int length2 = (length1 / 2 + length1 % 2) * 2;
            byte[] numArray = new byte[length2];
            if (length1 == 1)
            {
                numArray[0] = (byte)0;
                numArray[1] = bytes[0];
            }
            else if (length2 == 2)
            {
                numArray[0] = bytes[0];
                numArray[1] = bytes[1];
            }
            else if (length1 == 3)
            {
                numArray[0] = (byte)0;
                numArray[1] = bytes[0];
                numArray[2] = bytes[1];
                numArray[3] = bytes[2];
            }
            else
            {
                numArray[0] = bytes[0];
                numArray[1] = bytes[1];
                numArray[2] = bytes[2];
                numArray[3] = bytes[3];
            }
            return numArray;
        }
    }
}
