using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Watermarks
{
	public class WatermarkCollection : IEnumerable<Watermark>, IEnumerable
	{
		public WatermarkCollection(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.document = document;
			this.innerList = new List<Watermark>();
		}

		public Watermark this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		public void Add(Watermark watermark)
		{
			Guard.ThrowExceptionIfNull<Watermark>(watermark, "watermark");
			Guard.ThrowExceptionIfNotNull<RadFlowDocument>(watermark.Document, "watermark.Document");
			watermark.Document = this.document;
			this.innerList.Add(watermark);
		}

		public bool Remove(Watermark watermark)
		{
			Guard.ThrowExceptionIfNull<Watermark>(watermark, "watermark");
			watermark.Document = null;
			return this.innerList.Remove(watermark);
		}

		public void Clear()
		{
			foreach (Watermark watermark in this.innerList)
			{
				watermark.Document = null;
			}
			this.innerList.Clear();
		}

		public IEnumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		IEnumerator<Watermark> IEnumerable<Watermark>.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		internal void AddClonedChildrenFrom(WatermarkCollection fromCollection)
		{
			Guard.ThrowExceptionIfNull<WatermarkCollection>(fromCollection, "fromCollection");
			foreach (object obj in fromCollection)
			{
				Watermark watermark = (Watermark)obj;
				Watermark watermark2 = watermark.Clone();
				if (watermark2 != null)
				{
					this.Add(watermark2);
				}
			}
		}

		readonly RadFlowDocument document;

		readonly List<Watermark> innerList;
	}
}
