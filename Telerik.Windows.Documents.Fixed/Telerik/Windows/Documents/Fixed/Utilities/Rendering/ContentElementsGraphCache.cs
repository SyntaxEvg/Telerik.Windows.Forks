using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	class ContentElementsGraphCache
	{
		public ContentElementsGraphCache()
		{
			this.rootElementToElementsGraphMapping = new Dictionary<IContentRootElement, ContentElementsGraph>();
		}

		public ContentElementsGraph GetGraph(IContentRootElement element)
		{
			ContentElementsGraph contentElementsGraph;
			if (!this.rootElementToElementsGraphMapping.TryGetValue(element, out contentElementsGraph))
			{
				contentElementsGraph = new ContentElementsGraph(element);
				this.rootElementToElementsGraphMapping.Add(element, contentElementsGraph);
			}
			return contentElementsGraph;
		}

		public void Clear()
		{
			this.rootElementToElementsGraphMapping.Clear();
		}

		readonly Dictionary<IContentRootElement, ContentElementsGraph> rootElementToElementsGraphMapping;
	}
}
