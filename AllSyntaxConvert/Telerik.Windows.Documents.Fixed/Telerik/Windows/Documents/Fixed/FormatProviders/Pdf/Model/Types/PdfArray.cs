using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfArray : PdfCollectionBase<int>, IPdfCollection, IEnumerable<PdfPrimitive>, IEnumerable
	{
		public PdfArray(params PdfPrimitive[] initialValues)
		{
			this.store = new List<PdfPrimitive>();
			this.store.AddRange(initialValues);
		}

		public PdfArray(IEnumerable<PdfPrimitive> initialValues)
			: this(initialValues.ToArray<PdfPrimitive>())
		{
		}

		public static PdfArray MatrixIdentity
		{
			get
			{
				return new PdfArray(new PdfPrimitive[]
				{
					new PdfInt(1),
					new PdfInt(0),
					new PdfInt(0),
					new PdfInt(1),
					new PdfInt(0),
					new PdfInt(0)
				});
			}
		}

		public override int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.Array;
			}
		}

		public override PdfPrimitive this[int index]
		{
			get
			{
				return this.store[index];
			}
			set
			{
				this.store[index] = value;
			}
		}

		public void Add(PdfPrimitive item)
		{
			this.store.Add(item);
		}

		public void AddRange(IEnumerable<PdfPrimitive> items)
		{
			this.store.AddRange(items);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			this.Write(writer, context, false);
		}

		public void Write(PdfWriter writer, IPdfExportContext context, bool writeItemsAsIndirectReferences)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.Write(PdfNames.PdfArrayStart);
			int num = this.store.Count - 1;
			for (int i = 0; i < this.store.Count; i++)
			{
				if (writeItemsAsIndirectReferences || this.store[i].ExportAs == PdfElementType.PdfStreamObject)
				{
					PdfPrimitive.WriteIndirectReference(writer, context, this.store[i]);
				}
				else
				{
					this.store[i].Write(writer, context);
				}
				if (i != num)
				{
					writer.WriteSeparator();
				}
			}
			writer.Write(PdfNames.PdfArrayEnd);
		}

		public IEnumerator<PdfPrimitive> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		protected override bool TryGetElementOverride(int index, out PdfPrimitive element)
		{
			if (index < 0 || index >= this.store.Count)
			{
				element = null;
				return false;
			}
			element = this.store[index];
			return true;
		}

		readonly List<PdfPrimitive> store;
	}
}
