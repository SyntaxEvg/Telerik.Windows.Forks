using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class CrossReferenceStream : Trailer
	{
		public CrossReferenceStream()
		{
			this.index = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Index", false, PdfPropertyRestrictions.MustBeDirectObject));
			this.w = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("W", true, PdfPropertyRestrictions.MustBeDirectObject));
		}

		public PdfArray Index
		{
			get
			{
				return this.index.GetValue();
			}
			set
			{
				this.index.SetValue(value);
			}
		}

		public PdfArray W
		{
			get
			{
				return this.w.GetValue();
			}
			set
			{
				this.w.SetValue(value);
			}
		}

		protected override void LoadOverride(PostScriptReader reader, IPdfImportContext context, PdfPrimitive primitive)
		{
			PdfElementType type = primitive.Type;
			if (type == PdfElementType.Stream)
			{
				PdfStream pdfStream = (PdfStream)primitive;
				base.Load(reader, context, pdfStream.Dictionary);
				byte[] data = pdfStream.ReadDecodedPdfData();
				this.LoadReferences(data, context);
				return;
			}
			throw new InvalidOperationException("CrossReferenceStream should be loaded from stream object!");
		}

		void LoadReferences(byte[] data, IPdfImportContext context)
		{
			int[] array = this.CalculateIndexArrayForImport();
			int[] array2 = this.CalculateWArray();
			int num = array.Length / 2;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				int num3 = array[i * 2];
				int num4 = array[i * 2 + 1];
				for (int j = 0; j < num4; j++)
				{
					CrossReferenceEntry crossReferenceEntry = new CrossReferenceEntry();
					int type = ((array2[0] > 0) ? CrossReferenceStream.GetInt(data, num2, array2[0]) : 1);
					crossReferenceEntry.Type = CrossReferenceStream.GetEntyType(type);
					num2 += array2[0];
					crossReferenceEntry.Field1 = (long)CrossReferenceStream.GetInt(data, num2, array2[1]);
					num2 += array2[1];
					crossReferenceEntry.Field2 = CrossReferenceStream.GetInt(data, num2, array2[2]);
					num2 += array2[2];
					int objectNumber = num3 + j;
					context.CrossReferences.AddCrossReferenceEntry(objectNumber, crossReferenceEntry);
				}
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			KeyValuePair<int, CrossReferenceEntry>[] array = (from item in context.CrossReferenceCollection.Entries
				orderby item.Key
				select item).ToArray<KeyValuePair<int, CrossReferenceEntry>>();
			this.Index = CrossReferenceStream.CalculateIndexArrayForExport(context, array);
			int[] array2 = new int[]
			{
				1,
				CrossReferenceStream.CalculateMinBytesLength(writer.Position),
				1
			};
			this.W = array2.ToPdfArray();
			PdfObject.WritePdfPropertiesDictionary(this, writer, context, true);
			int num = 0;
			for (int i = 0; i < array2.Length; i++)
			{
				num += array2[i];
			}
			int num2 = array.Length * num;
			byte[] array3 = new byte[num2];
			int num3 = 0;
			foreach (KeyValuePair<int, CrossReferenceEntry> keyValuePair in array)
			{
				CrossReferenceEntry value = keyValuePair.Value;
				CrossReferenceStream.SetInt(array3, num3, array2[0], (value.Type == CrossReferenceEntryType.Used) ? 1L : 0L);
				num3 += array2[0];
				CrossReferenceStream.SetInt(array3, num3, array2[1], value.Field1);
				num3 += array2[1];
				CrossReferenceStream.SetInt(array3, num3, array2[2], (long)value.Field2);
				num3 += array2[2];
			}
			PdfStreamObjectBase.WriteEncodedData(writer, context, array3);
		}

		static PdfArray CalculateIndexArrayForExport(IPdfExportContext context, KeyValuePair<int, CrossReferenceEntry>[] orderedCrossReferenceCollection)
		{
			List<int> list = new List<int>();
			int? num = null;
			int num2 = 0;
			int num3 = 0;
			foreach (KeyValuePair<int, CrossReferenceEntry> keyValuePair in orderedCrossReferenceCollection)
			{
				if (num == null)
				{
					num = new int?(keyValuePair.Key);
					num3 = 1;
				}
				else if (num2 + 1 == keyValuePair.Key)
				{
					num3++;
				}
				else
				{
					list.Add(num.Value);
					list.Add(num3);
					num = new int?(keyValuePair.Key);
					num3 = 1;
				}
				num2 = keyValuePair.Key;
			}
			list.Add(num.Value);
			list.Add(num3);
			return list.ToPdfArray();
		}

		static CrossReferenceEntryType GetEntyType(int type)
		{
			switch (type)
			{
			case 1:
				return CrossReferenceEntryType.Used;
			case 2:
				return CrossReferenceEntryType.Compressed;
			default:
				return CrossReferenceEntryType.Free;
			}
		}

		static int GetInt(byte[] data, int position, int length)
		{
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				byte b = data[position + i];
				int num2 = length - 1 - i;
				int num3 = (int)b << num2 * 8;
				num |= num3;
			}
			return num;
		}

		static int CalculateMinBytesLength(long number)
		{
			int num = 64;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				long num3 = number >> i;
				num3 &= 1L;
				if (num3 == 1L)
				{
					num2 = i;
				}
			}
			return num2 / 8 + 1;
		}

		static void SetInt(byte[] data, int position, int length, long number)
		{
			for (int i = 0; i < length; i++)
			{
				int num = length - 1 - i;
				long num2 = number >> num * 8;
				byte b = (byte)num2;
				data[position + i] = b;
			}
		}

		int[] CalculateWArray()
		{
			return new int[]
			{
				((PdfInt)this.W[0]).Value,
				((PdfInt)this.W[1]).Value,
				((PdfInt)this.W[2]).Value
			};
		}

		int[] CalculateIndexArrayForImport()
		{
			if (this.Index == null)
			{
				return new int[]
				{
					0,
					base.Size.Value
				};
			}
			int[] array = new int[this.Index.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((PdfInt)this.Index[i]).Value;
			}
			return array;
		}

		const int DefaultEntryType = 1;

		readonly DirectProperty<PdfArray> index;

		readonly DirectProperty<PdfArray> w;
	}
}
