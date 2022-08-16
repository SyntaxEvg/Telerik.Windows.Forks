using System;
using System.Text;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	abstract class PdfString : PdfSimpleType<byte[]>
	{
		public PdfString()
		{
		}

		public PdfString(byte[] initialData)
			: base(initialData)
		{
		}

		public abstract StringObjectType StringObjectType { get; }

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.String;
			}
		}

		internal static string GetString(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return string.Empty;
			}
			if (!PdfString.IsUnicode(bytes))
			{
				return PdfString.GetPdfEncodedTextString(bytes);
			}
			return PdfString.GetUtfEncodedTextString(bytes);
		}

		internal static bool IsUnicode(byte[] bytes)
		{
			return bytes != null && bytes.Length > 1 && bytes[0] == 254 && bytes[1] == byte.MaxValue;
		}

		internal static string GetUtfEncodedTextString(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return string.Empty;
			}
			int index = 0;
			int num = bytes.Length;
			bool flag = false;
			if (bytes.Length > 2)
			{
				flag = PdfString.IsUnicode(bytes);
			}
			if (flag)
			{
				index = 2;
				num -= 2;
			}
			return Encoding.BigEndianUnicode.GetString(bytes, index, num);
		}

		internal static string GetPdfEncodedTextString(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return string.Empty;
			}
			string[] names = PredefinedSimpleEncoding.PdfDocEncoding.GetNames();
			char[] array = new char[bytes.Length];
			for (int i = 0; i < bytes.Length; i++)
			{
				byte b = bytes[i];
				bool flag = (int)b <= PdfString.PrintableAsciiLastCharacterIndex;
				char c;
				if (flag)
				{
					c = (char)b;
				}
				else if ((int)b < names.Length)
				{
					string names2 = names[(int)b];
					c = AdobeGlyphList.GetUnicode(names2);
				}
				else
				{
					c = AdobeGlyphList.GetUnicode(PdfString.DefaultCharacterName);
				}
				array[i] = c;
			}
			return new string(array);
		}

		static string DefaultCharacterName = "emdash";

		static int PrintableAsciiLastCharacterIndex = 127;
	}
}
