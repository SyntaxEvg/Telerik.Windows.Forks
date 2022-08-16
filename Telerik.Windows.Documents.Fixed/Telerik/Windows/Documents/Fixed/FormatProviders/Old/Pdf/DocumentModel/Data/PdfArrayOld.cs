using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfArrayOld : PdfCollectionBaseOld<int>, IEnumerable<object>, IEnumerable
	{
		public PdfArrayOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.store = new List<object>();
		}

		public PdfArrayOld(PdfContentManager contentManager, IEnumerable<object> range)
			: base(contentManager)
		{
			this.store = new List<object>(range);
			base.IsLoaded = true;
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public object this[int index]
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

		public static PdfArrayOld CreateMatrixIdentity(PdfContentManager contentManager)
		{
			return new PdfArrayOld(contentManager, new object[] { 1, 0, 0, 1, 0, 0 });
		}

		public void Load(IEnumerable<object> content)
		{
			this.store.Clear();
			this.store.AddRange(content);
			base.IsLoaded = true;
		}

		public void Add(object obj)
		{
			this.store.Add(obj);
		}

		public IEnumerator<object> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			PdfArrayOld pdfArrayOld = indirectObject.Value as PdfArrayOld;
			if (pdfArrayOld != null)
			{
				this.Load(pdfArrayOld.store);
			}
			base.Load(indirectObject);
		}

		public Rect ToRect()
		{
			double x;
			Helper.UnboxDouble(this.GetDirectValue(0), out x);
			double y;
			Helper.UnboxDouble(this.GetDirectValue(1), out y);
			double x2;
			Helper.UnboxDouble(this.GetDirectValue(2), out x2);
			double y2;
			Helper.UnboxDouble(this.GetDirectValue(3), out y2);
			return new Rect(new Point(x, y), new Point(x2, y2));
		}

		public Matrix ToMatrix()
		{
			double m;
			Helper.UnboxDouble(this.store[0], out m);
			double m2;
			Helper.UnboxDouble(this.store[1], out m2);
			double m3;
			Helper.UnboxDouble(this.store[2], out m3);
			double m4;
			Helper.UnboxDouble(this.store[3], out m4);
			double offsetX;
			Helper.UnboxDouble(this.store[4], out offsetX);
			double offsetY;
			Helper.UnboxDouble(this.store[5], out offsetY);
			return new Matrix(m, m2, m3, m4, offsetX, offsetY);
		}

		public double[] ToDashPattern()
		{
			double[] array = new double[this.Count];
			for (int i = 0; i < this.Count; i++)
			{
				base.TryGetReal(i, out array[i]);
			}
			return array;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object value in this.store)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(value);
			}
			stringBuilder.Remove(0, 1);
			stringBuilder.Append(']');
			stringBuilder.Insert(0, '['.ToString());
			return stringBuilder.ToString();
		}

		public object[] ToArray()
		{
			return this.store.ToArray();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		protected override object GetElementAtIndex(int index)
		{
			return this.store[index];
		}

		protected override bool ContainsElementAtIndex(int index)
		{
			return index >= 0 && index < this.store.Count;
		}

		object GetDirectValue(int index)
		{
			IndirectReferenceOld indirectReferenceOld = this.store[index] as IndirectReferenceOld;
			if (!(indirectReferenceOld != null))
			{
				return this.store[index];
			}
			IndirectObjectOld indirectObjectOld;
			if (base.ContentManager.TryGetIndirectObject(indirectReferenceOld, false, out indirectObjectOld))
			{
				return indirectObjectOld.Value;
			}
			return null;
		}

		readonly List<object> store;
	}
}
