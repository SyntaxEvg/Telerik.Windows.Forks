using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	class HeaderFooterContentCollection : IEnumerable<HeaderFooterContent>, IEnumerable
	{
		internal HeaderFooterContentCollection(Action updateOnChange)
		{
			this.defaultContent = new HeaderFooterContent(updateOnChange);
			this.firstPageContent = new HeaderFooterContent(updateOnChange);
			this.evenPageContent = new HeaderFooterContent(updateOnChange);
		}

		internal HeaderFooterContentCollection(HeaderFooterContentCollection other, Action updateOnChange)
		{
			this.defaultContent = new HeaderFooterContent(other.defaultContent, updateOnChange);
			this.firstPageContent = new HeaderFooterContent(other.firstPageContent, updateOnChange);
			this.evenPageContent = new HeaderFooterContent(other.evenPageContent, updateOnChange);
		}

		public HeaderFooterContent DefaultContent
		{
			get
			{
				return this.defaultContent;
			}
		}

		public HeaderFooterContent FirstPageContent
		{
			get
			{
				return this.firstPageContent;
			}
		}

		public HeaderFooterContent EvenPageContent
		{
			get
			{
				return this.evenPageContent;
			}
		}

		internal void CopyFrom(HeaderFooterContentCollection other)
		{
			this.DefaultContent.CopyFrom(other.DefaultContent);
			this.FirstPageContent.CopyFrom(other.FirstPageContent);
			this.EvenPageContent.CopyFrom(other.EvenPageContent);
		}

		public IEnumerator<HeaderFooterContent> GetEnumerator()
		{
			return this.GetContents().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetContents().GetEnumerator();
		}

		IEnumerable<HeaderFooterContent> GetContents()
		{
			yield return this.DefaultContent;
			yield return this.FirstPageContent;
			yield return this.EvenPageContent;
			yield break;
		}

		public override bool Equals(object obj)
		{
			HeaderFooterContentCollection headerFooterContentCollection = obj as HeaderFooterContentCollection;
			return headerFooterContentCollection != null && (this.DefaultContent.Equals(headerFooterContentCollection.DefaultContent) && this.FirstPageContent.Equals(headerFooterContentCollection.FirstPageContent)) && this.EvenPageContent.Equals(headerFooterContentCollection.EvenPageContent);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.DefaultContent.GetHashCode(), this.FirstPageContent.GetHashCode(), this.EvenPageContent.GetHashCode());
		}

		readonly HeaderFooterContent defaultContent;

		readonly HeaderFooterContent firstPageContent;

		readonly HeaderFooterContent evenPageContent;
	}
}
