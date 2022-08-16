using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Telerik.Windows.Documents.Core.Fonts.Utils
{
	class GlyphsSequence : IEnumerable<GlyphInfo>, IEnumerable
	{
		public GlyphsSequence()
		{
			this.store = new List<GlyphInfo>();
		}

		public GlyphsSequence(int capacity)
		{
			this.store = new List<GlyphInfo>(capacity);
		}

		public GlyphInfo this[int index]
		{
			get
			{
				return this.store[index];
			}
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public void Add(GlyphInfo glyphInfo)
		{
			this.store.Add(glyphInfo);
		}

		public void Add(ushort glyphID, GlyphForm form)
		{
			this.store.Add(new GlyphInfo(glyphID, form));
		}

		public void Add(ushort glyphId)
		{
			this.store.Add(new GlyphInfo(glyphId));
		}

		public void AddRange(IEnumerable<ushort> glyphIDs)
		{
			foreach (ushort glyphId in glyphIDs)
			{
				this.Add(glyphId);
			}
		}

		public void AddRange(IEnumerable<GlyphInfo> glyphIDs)
		{
			this.store.AddRange(glyphIDs);
		}

		public GlyphForm GetGlyphForm(int index)
		{
			return this.store[index].Form;
		}

		public IEnumerator<GlyphInfo> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.store[0]);
			for (int i = 1; i < this.store.Count; i++)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(this.store[i]);
			}
			return stringBuilder.ToString();
		}

		readonly List<GlyphInfo> store;
	}
}
