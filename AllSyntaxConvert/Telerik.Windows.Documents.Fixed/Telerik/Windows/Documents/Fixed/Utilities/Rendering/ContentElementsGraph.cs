using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	class ContentElementsGraph
	{
		public ContentElementsGraph(IContentRootElement contentElement)
		{
			Guard.ThrowExceptionIfNull<IContentRootElement>(contentElement, "contentElement");
			this.elementsInClipping = new Dictionary<Clipping, ContentElementsGraph.OrderedHashSet<ContentElementBase>>();
			this.rootElements = new ContentElementsGraph.OrderedHashSet<ContentElementBase>();
			this.BuildGraph(contentElement);
		}

		public IEnumerable<ContentElementBase> GetRootElements()
		{
			return this.rootElements.Elements;
		}

		public IEnumerable<ContentElementBase> GetClippingChildren(Clipping clipping)
		{
			Guard.ThrowExceptionIfNull<Clipping>(clipping, "clipping");
			return this.elementsInClipping[clipping].Elements;
		}

		void BuildGraph(IContentRootElement contentRootElement)
		{
			foreach (ContentElementBase contentElementBase in contentRootElement.Content)
			{
				if (contentElementBase.Clipping == null)
				{
					this.rootElements.Add(contentElementBase);
				}
				else
				{
					Clipping clipping = contentElementBase.Clipping;
					this.AddEdge(clipping, contentElementBase);
					this.BuildClippingsEdges(contentElementBase.Clipping);
				}
			}
		}

		void AddEdge(Clipping clipping, ContentElementBase contentElement)
		{
			ContentElementsGraph.OrderedHashSet<ContentElementBase> orderedHashSet;
			if (this.elementsInClipping.TryGetValue(clipping, out orderedHashSet))
			{
				orderedHashSet.Add(contentElement);
				return;
			}
			orderedHashSet = new ContentElementsGraph.OrderedHashSet<ContentElementBase>();
			orderedHashSet.Add(contentElement);
			this.elementsInClipping.Add(clipping, orderedHashSet);
		}

		void BuildClippingsEdges(Clipping clipping)
		{
			HashSet<Clipping> hashSet = new HashSet<Clipping>();
			hashSet.Add(clipping);
			Clipping clipping2 = clipping.Clipping;
			while (clipping2 != null)
			{
				if (hashSet.Contains(clipping2))
				{
					throw new ArgumentException("Cycling connection between clippings cannot be created.");
				}
				this.AddEdge(clipping2, clipping);
				clipping = clipping2;
				clipping2 = clipping2.Clipping;
				hashSet.Add(clipping);
			}
			this.rootElements.Add(clipping);
		}

		readonly Dictionary<Clipping, ContentElementsGraph.OrderedHashSet<ContentElementBase>> elementsInClipping;

		readonly ContentElementsGraph.OrderedHashSet<ContentElementBase> rootElements;

		class OrderedHashSet<T>
		{
			public OrderedHashSet()
			{
				this.elements = new HashSet<T>();
				this.elementsList = new List<T>();
			}

			public IEnumerable<T> Elements
			{
				get
				{
					foreach (T element in this.elementsList)
					{
						yield return element;
					}
					yield break;
				}
			}

			public void Add(T element)
			{
				int count = this.elements.Count;
				this.elements.Add(element);
				if (this.elements.Count > count)
				{
					this.elementsList.Add(element);
				}
			}

			readonly HashSet<T> elements;

			readonly List<T> elementsList;
		}
	}
}
