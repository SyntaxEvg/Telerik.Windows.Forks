using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.Writers
{
	class CMapWriter
	{
		public void Write(PdfWriter writer, IPdfExportContext context, CMapEncoding encoding)
		{
			writer.WriteLine("/CIDInit /ProcSet findresource begin");
			writer.WriteLine("12 dict begin");
			writer.WriteLine("begincmap");
			CMapWriter.WriteCidSystemInfo(writer, encoding.CidSystemInfo.Value);
			CMapWriter.WriteNameDefinition(writer, "CMapName", encoding.CMapName.Value);
			CMapWriter.WriteIntegerDefinition(writer, "CMapType", encoding.CMapType.Value);
			Tuple<CharCode, CharCode>[] ranges = new Tuple<CharCode, CharCode>[]
			{
				new Tuple<CharCode, CharCode>(new CharCode(0, 2), new CharCode(65535, 2))
			};
			IEnumerable<Tuple<CharCode, string>> charCodeToUnicodeMappings = encoding.CharCodeToUnicodeMappings;
			CMapWriter.WriteItems<Tuple<CharCode, CharCode>>(writer, "codespacerange", ranges, delegate(PdfWriter w, Tuple<CharCode, CharCode> item)
			{
				w.WriteLine("<{0}><{1}>", new object[] { item.Item1, item.Item2 });
			});
			CMapWriter.WriteItems<Tuple<CharCode, string>>(writer, "bfchar", charCodeToUnicodeMappings, delegate(PdfWriter w, Tuple<CharCode, string> item)
			{
				w.WriteLine("<{0}><{1}>", new object[]
				{
					item.Item1,
					BytesHelper.ToHexString(Encoding.BigEndianUnicode.GetBytes(item.Item2))
				});
			});
			writer.WriteLine("endcmap CMapName currentdict /CMap defineresource pop end end");
		}

		static void WriteNameDefinition(PdfWriter writer, string propertyName, string value)
		{
			CMapWriter.WriteDefinition(writer, propertyName, string.Format("/{0}", value));
		}

		static void WriteIntegerDefinition(PdfWriter writer, string propertyName, int value)
		{
			CMapWriter.WriteDefinition(writer, propertyName, value.ToString());
		}

		static void WriteCidSystemInfo(PdfWriter writer, CidSystemInfo info)
		{
			CMapWriter.WriteDefinition(writer, "CIDSystemInfo", string.Format("<< /Registry ({0})/Ordering ({1})/Supplement {2}>>", info.Registry, info.Ordering, info.Supplement));
		}

		static void WriteDefinition(PdfWriter writer, string propertyName, string value)
		{
			writer.WriteLine("/{0} {1} def", new object[] { propertyName, value });
		}

		static void WriteItems<T>(PdfWriter writer, string header, IEnumerable<T> ranges, Action<PdfWriter, T> writeItemAction)
		{
			int num = ranges.Count<T>();
			int rangesCount = CMapWriter.GetRangesCount(num);
			for (int i = 0; i < rangesCount; i++)
			{
				int num2 = System.Math.Min(num, 100);
				int num3 = i * 100;
				if (num3 + num2 >= num)
				{
					num2 = num - num3;
				}
				writer.WriteLine("{0} begin{1}", new object[] { num2, header });
				for (int j = 0; j < num2; j++)
				{
					T arg = ranges.ElementAt(num3 + j);
					writeItemAction(writer, arg);
				}
				writer.WriteLine("end{0}", new object[] { header });
			}
		}

		static int GetRangesCount(int itemsCount)
		{
			int num = itemsCount / 100;
			if (itemsCount % 100 > 0)
			{
				num++;
			}
			return num;
		}

		const int MaxItemsInRange = 100;
	}
}
