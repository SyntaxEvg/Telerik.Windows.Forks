using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class OperandsCollection
	{
		public OperandsCollection()
		{
			this.store = new LinkedList<PdfPrimitive>();
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public PdfPrimitive First
		{
			get
			{
				return this.store.First<PdfPrimitive>();
			}
		}

		public PdfPrimitive Last
		{
			get
			{
				return this.store.Last<PdfPrimitive>();
			}
		}

		public PdfPrimitive GetElementAt(Origin origin, int index)
		{
			if (origin == Origin.Begin)
			{
				LinkedListNode<PdfPrimitive> linkedListNode = this.store.First;
				for (int i = 0; i < index; i++)
				{
					linkedListNode = linkedListNode.Next;
				}
				return linkedListNode.Value;
			}
			LinkedListNode<PdfPrimitive> linkedListNode2 = this.store.Last;
			for (int j = 0; j < index; j++)
			{
				linkedListNode2 = linkedListNode2.Previous;
			}
			return linkedListNode2.Value;
		}

		public void AddLast(PdfPrimitive primitive)
		{
			this.store.AddLast(primitive);
		}

		public void AddFirst(PdfPrimitive primitive)
		{
			this.store.AddFirst(primitive);
		}

		public PdfPrimitive GetLast()
		{
			PdfPrimitive value = this.store.Last.Value;
			this.store.RemoveLast();
			return value;
		}

		public PdfPrimitive GetFirst()
		{
			PdfPrimitive value = this.store.First.Value;
			this.store.RemoveFirst();
			return value;
		}

		public T GetLast<T>(PostScriptReader reader, IPdfImportContext context) where T : PdfPrimitive
		{
			PdfPrimitive last = this.GetLast();
			PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor<T>();
			return (T)((object)pdfObjectDescriptor.Converter.Convert(typeof(T), reader, context, last));
		}

		public T GetLastAs<T>() where T : PdfPrimitive
		{
			return (T)((object)this.GetLast());
		}

		public void Clear()
		{
			this.store.Clear();
		}

		readonly LinkedList<PdfPrimitive> store;
	}
}
