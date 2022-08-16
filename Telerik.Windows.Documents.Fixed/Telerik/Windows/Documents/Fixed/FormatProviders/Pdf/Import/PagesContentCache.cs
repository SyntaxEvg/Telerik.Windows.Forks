using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class PagesContentCache : IPagesContentCache
	{
		public PagesContentCache()
		{
			this.pageToSavedPageContent = new Dictionary<RadFixedPage, Tuple<ContentElementCollection, AnnotationCollection>>();
		}

		public void CachePageContent(RadFixedPage page)
		{
			ContentElementCollection contentElementCollection = new ContentElementCollection(page);
			this.CopyPageContent(page.Content, contentElementCollection);
			AnnotationCollection annotationCollection = new AnnotationCollection(page);
			this.CopyPageAnnotations(page.Annotations, annotationCollection);
			this.pageToSavedPageContent[page] = new Tuple<ContentElementCollection, AnnotationCollection>(contentElementCollection, annotationCollection);
		}

		public bool TryLoadCachedPageContent(RadFixedPage page)
		{
			Tuple<ContentElementCollection, AnnotationCollection> tuple;
			bool flag = this.pageToSavedPageContent.TryGetValue(page, out tuple);
			if (flag)
			{
				this.CopyPageContent(tuple.Item1, page.Content);
				this.CopyPageAnnotations(tuple.Item2, page.Annotations);
			}
			return flag;
		}

		public void Clear()
		{
			this.pageToSavedPageContent.Clear();
		}

		void CopyPageContent(ContentElementCollection source, ContentElementCollection destination)
		{
			foreach (ContentElementBase contentElementBase in source)
			{
				PositionContentElement item = (PositionContentElement)contentElementBase;
				destination.Add(item);
			}
		}

		void CopyPageAnnotations(AnnotationCollection source, AnnotationCollection destination)
		{
			foreach (Annotation item in source)
			{
				destination.Add(item);
			}
		}

		readonly Dictionary<RadFixedPage, Tuple<ContentElementCollection, AnnotationCollection>> pageToSavedPageContent;
	}
}
