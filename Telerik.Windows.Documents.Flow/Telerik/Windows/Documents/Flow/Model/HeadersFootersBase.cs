using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public abstract class HeadersFootersBase<T> where T : HeaderFooterBase
	{
		internal HeadersFootersBase(RadFlowDocument document, Section section)
		{
			this.document = document;
			this.section = section;
		}

		public T First
		{
			get
			{
				return this.first;
			}
		}

		public T Even
		{
			get
			{
				return this.even;
			}
		}

		public T Default
		{
			get
			{
				return this.@default;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public Section Section
		{
			get
			{
				return this.section;
			}
		}

		public T Add()
		{
			return this.Add(HeaderFooterType.Default);
		}

		public T Add(HeaderFooterType headerFooterType)
		{
			switch (headerFooterType)
			{
			case HeaderFooterType.Default:
				this.@default = this.CreateHeaderFooterInstanceInternal();
				return this.Default;
			case HeaderFooterType.Even:
				this.even = this.CreateHeaderFooterInstanceInternal();
				return this.Even;
			case HeaderFooterType.First:
				this.first = this.CreateHeaderFooterInstanceInternal();
				return this.First;
			default:
				throw new ArgumentException("Unsupported header/footer type.");
			}
		}

		public void Remove(HeaderFooterType headerFooterType)
		{
			switch (headerFooterType)
			{
			case HeaderFooterType.Default:
				this.@default = default(T);
				return;
			case HeaderFooterType.Even:
				this.even = default(T);
				return;
			case HeaderFooterType.First:
				this.first = default(T);
				return;
			default:
				throw new ArgumentException("Unsupported header/footer type.");
			}
		}

		internal IEnumerable<T> GetInstances()
		{
			if (this.Default != null)
			{
				yield return this.Default;
			}
			if (this.First != null)
			{
				yield return this.First;
			}
			if (this.Even != null)
			{
				yield return this.Even;
			}
			yield break;
		}

		internal void CloneHeadersFootersFrom(HeadersFootersBase<T> headersFooters, CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<HeadersFootersBase<T>>(headersFooters, "headersFooters");
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			if (headersFooters.First != null)
			{
				T t = headersFooters.First;
				this.first = (T)((object)t.CloneCore(cloneContext));
				if (this.first != null)
				{
					T t2 = headersFooters.First;
					t2.OnAfterCloneCore(cloneContext, this.first);
				}
			}
			if (headersFooters.Even != null)
			{
				T t3 = headersFooters.Even;
				this.even = (T)((object)t3.CloneCore(cloneContext));
				if (this.even != null)
				{
					T t4 = headersFooters.Even;
					t4.OnAfterCloneCore(cloneContext, this.even);
				}
			}
			if (headersFooters.Default != null)
			{
				T t5 = headersFooters.Default;
				this.@default = (T)((object)t5.CloneCore(cloneContext));
				if (this.@default != null)
				{
					T t6 = headersFooters.Default;
					t6.OnAfterCloneCore(cloneContext, this.@default);
				}
			}
		}

		protected abstract T CreateHeaderFooterInstance();

		T CreateHeaderFooterInstanceInternal()
		{
			T t = this.CreateHeaderFooterInstance();
			if (t == null)
			{
				throw new InvalidOperationException("Header/footer is null.");
			}
			if (t.Document != this.Document)
			{
				throw new InvalidOperationException("Header/footer has invalid associated document.");
			}
			if (t.Parent != this.Section)
			{
				throw new InvalidOperationException("Header/footer has invalid parent.");
			}
			return t;
		}

		const string UnsupportedHeaderFooterTypeExceptionMessage = "Unsupported header/footer type.";

		readonly RadFlowDocument document;

		readonly Section section;

		T @default;

		T even;

		T first;
	}
}
