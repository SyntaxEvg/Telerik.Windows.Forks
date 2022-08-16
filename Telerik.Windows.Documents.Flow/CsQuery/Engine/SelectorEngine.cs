using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery.ExtensionMethods;
using CsQuery.ExtensionMethods.Internal;
using CsQuery.HtmlParser;
using CsQuery.Implementation;

namespace CsQuery.Engine
{
	class SelectorEngine
	{
		public SelectorEngine(IDomDocument document, Selector selector)
		{
			this.Document = document;
			this.Selector = selector;
		}

		public Selector Selector { get; protected set; }

		public IDomDocument Document { get; protected set; }

		public IList<IDomObject> Select(IEnumerable<IDomObject> context)
		{
			HashSet<IDomObject> hashSet = new HashSet<IDomObject>();
			if (this.Selector == null)
			{
				throw new ArgumentNullException("The selector cannot be null.");
			}
			if (this.Selector.Count == 0)
			{
				return SelectorEngine.EmptyEnumerable().ToList<IDomObject>();
			}
			this.ActiveSelectors = new List<SelectorClause>(this.Selector);
			SelectorClause selectorClause = this.ActiveSelectors[0];
			if (selectorClause.SelectorType == SelectorType.HTML)
			{
				return DomDocument.Create(selectorClause.Html, HtmlParsingMode.Fragment, HtmlParsingOptions.Default, DocType.Default).ChildNodes.ToList<IDomObject>();
			}
			IEnumerable<IDomObject> enumerable = null;
			IEnumerable<IDomObject> enumerable2 = null;
			bool flag;
			if (context.IsNullOrEmpty<IDomObject>())
			{
				flag = true;
			}
			else
			{
				IDomObject domObject = context.First<IDomObject>();
				flag = !domObject.IsDisconnected && domObject.IsIndexed && domObject.Document == this.Document;
			}
			IDomIndexRanged domIndexRanged = null;
			IDomIndexSimple domIndexSimple = null;
			if (flag)
			{
				domIndexRanged = this.Document.DocumentIndex as IDomIndexRanged;
				domIndexSimple = this.Document.DocumentIndex as IDomIndexSimple;
			}
			this.activeSelectorId = 0;
			while (this.activeSelectorId < this.ActiveSelectors.Count)
			{
				SelectorClause selectorClause2 = this.ActiveSelectors[this.activeSelectorId].Clone();
				if (enumerable != null && (selectorClause2.CombinatorType == CombinatorType.Root || selectorClause2.CombinatorType == CombinatorType.Context))
				{
					hashSet.AddRange(enumerable);
					enumerable = null;
				}
				if (selectorClause2.CombinatorType != CombinatorType.Grouped)
				{
					enumerable2 = this.GetSelectionSource(selectorClause2, context, enumerable);
					enumerable = null;
				}
				List<ushort> list = new List<ushort>();
				SelectorType selectorType = (SelectorType)0;
				int num = 0;
				bool flag2 = true;
				switch (selectorClause2.TraversalType)
				{
				case TraversalType.Filter:
				case TraversalType.Adjacent:
				case TraversalType.Sibling:
					num = 0;
					flag2 = false;
					break;
				case TraversalType.Descendent:
					num = 1;
					flag2 = true;
					break;
				case TraversalType.Child:
					num = selectorClause2.ChildDepth;
					flag2 = false;
					break;
				}
				bool flag3 = enumerable2 == null && flag2 && num == 0;
				if (domIndexRanged != null || (domIndexSimple != null && flag3 && !selectorClause2.NoIndex))
				{
					if (selectorClause2.SelectorType.HasFlag(SelectorType.AttributeValue) && selectorClause2.AttributeSelectorType != AttributeSelectorType.NotExists && selectorClause2.AttributeSelectorType != AttributeSelectorType.NotEquals)
					{
						list.Add(33);
						list.Add(HtmlData.Tokenize(selectorClause2.AttributeName));
						if (selectorClause2.AttributeSelectorType == AttributeSelectorType.Exists)
						{
							selectorType = SelectorType.AttributeValue;
						}
					}
					else if (selectorClause2.SelectorType.HasFlag(SelectorType.Tag))
					{
						list.Add(43);
						list.Add(HtmlData.Tokenize(selectorClause2.Tag));
						selectorType = SelectorType.Tag;
					}
					else if (selectorClause2.SelectorType.HasFlag(SelectorType.ID))
					{
						list.Add(35);
						list.Add(HtmlData.TokenizeCaseSensitive(selectorClause2.ID));
						selectorType = SelectorType.ID;
					}
					else if (selectorClause2.SelectorType.HasFlag(SelectorType.Class))
					{
						list.Add(46);
						list.Add(HtmlData.TokenizeCaseSensitive(selectorClause2.Class));
						selectorType = SelectorType.Class;
					}
				}
				IEnumerable<IDomObject> enumerable3 = null;
				if (list.Count > 0)
				{
					if (enumerable2 == null)
					{
						enumerable3 = domIndexSimple.QueryIndex(list.ToArray());
					}
					else
					{
						HashSet<IDomObject> hashSet2 = new HashSet<IDomObject>();
						enumerable3 = hashSet2;
						foreach (IDomObject domObject2 in enumerable2)
						{
							ushort[] subKey = list.Concat(ushort.MaxValue).Concat(domObject2.NodePath).ToArray<ushort>();
							IEnumerable<IDomObject> elements = domIndexRanged.QueryIndex(subKey, num, flag2);
							hashSet2.AddRange(elements);
						}
					}
					selectorClause2.SelectorType &= ~selectorType;
					if (selectorClause2.SelectorType.HasFlag(SelectorType.AttributeValue))
					{
						selectorClause2.TraversalType = TraversalType.Filter;
					}
				}
				if (selectorClause2.SelectorType != (SelectorType)0)
				{
					IEnumerable<IDomObject> source;
					if ((source = enumerable3) == null)
					{
						source = enumerable2 ?? ((IEnumerable<IDomObject>)this.Document.ChildElements);
					}
					enumerable3 = this.GetMatches(source, selectorClause2);
				}
				enumerable = ((enumerable == null) ? enumerable3 : enumerable.Concat(enumerable3));
				this.activeSelectorId++;
			}
			hashSet.AddRange(enumerable);
			return hashSet.OrderBy((IDomObject item) => item.NodePath, PathKeyComparer.Comparer).ToList<IDomObject>();
		}

		protected IEnumerable<IDomObject> GetSelectionSource(SelectorClause clause, IEnumerable<IDomObject> context, IEnumerable<IDomObject> lastResult)
		{
			IEnumerable<IDomObject> result = null;
			IEnumerable<IDomObject> enumerable;
			if (clause.CombinatorType != CombinatorType.Chained)
			{
				enumerable = ((clause.CombinatorType == CombinatorType.Context) ? context : null);
			}
			else
			{
				enumerable = lastResult;
			}
			if (enumerable != null)
			{
				if (clause.TraversalType == TraversalType.Adjacent || clause.TraversalType == TraversalType.Sibling)
				{
					result = this.GetAdjacentOrSiblings(clause.TraversalType, enumerable);
					clause.TraversalType = TraversalType.Filter;
				}
				else
				{
					result = enumerable;
				}
			}
			return result;
		}

		protected IEnumerable<IDomObject> GetMatches(IEnumerable<IDomObject> source, SelectorClause selector)
		{
			HashSet<IDomObject> hashSet = new HashSet<IDomObject>();
			HashSet<IDomObject> hashSet2 = new HashSet<IDomObject>();
			if (selector.SelectorType.HasFlag(SelectorType.Elements))
			{
				IEnumerable<IDomObject> allChildOrDescendants = SelectorEngine.GetAllChildOrDescendants(selector.TraversalType, source);
				return allChildOrDescendants.Intersect(selector.SelectElements);
			}
			if (selector.SelectorType.HasFlag(SelectorType.PseudoClass))
			{
				if (selector.IsResultListPosition)
				{
					return SelectorEngine.GetResultPositionMatches(source, selector);
				}
			}
			else if (selector.SelectorType.HasFlag(SelectorType.All))
			{
				return SelectorEngine.GetAllChildOrDescendants(selector.TraversalType, source);
			}
			Stack<MatchElement> stack = new Stack<MatchElement>();
			foreach (IDomObject domObject in source)
			{
				IDomElement domElement = domObject as IDomElement;
				if (domElement != null && (selector.TraversalType == TraversalType.Child || !hashSet2.Contains(domElement)))
				{
					stack.Push(new MatchElement(domElement, 0));
					int num = 0;
					while (stack.Count != 0)
					{
						MatchElement matchElement = stack.Pop();
						if (this.Matches(selector, matchElement.Element, matchElement.Depth))
						{
							hashSet.Add(matchElement.Element);
							num++;
						}
						if (selector.TraversalType != TraversalType.Filter && (selector.TraversalType != TraversalType.Child || selector.ChildDepth > matchElement.Depth))
						{
							SelectorType selectorType = selector.SelectorType;
							IDomElement element = matchElement.Element;
							if (selector.IsDomPositionPseudoSelector && (selector.TraversalType == TraversalType.All || (selector.TraversalType == TraversalType.Child && selector.ChildDepth == matchElement.Depth + 1) || (selector.TraversalType == TraversalType.Descendent && selector.ChildDepth <= matchElement.Depth + 1)))
							{
								hashSet.AddRange(this.GetPseudoClassMatches(element, selector));
								selectorType &= ~SelectorType.PseudoClass;
							}
							if (selectorType != (SelectorType)0)
							{
								for (int i = element.ChildNodes.Count - 1; i >= 0; i--)
								{
									IDomElement domElement2 = element[i] as IDomElement;
									if (domElement2 != null && hashSet2.Add(domElement2) && domElement2.NodeType == NodeType.ELEMENT_NODE)
									{
										stack.Push(new MatchElement(domElement2, matchElement.Depth + 1));
									}
								}
							}
						}
					}
				}
			}
			return hashSet;
		}

		protected bool Matches(SelectorClause selector, IDomElement obj, int depth)
		{
			switch (selector.TraversalType)
			{
			case TraversalType.Descendent:
				if (depth == 0)
				{
					return false;
				}
				break;
			case TraversalType.Child:
				if (selector.ChildDepth != depth)
				{
					return false;
				}
				break;
			}
			if (selector.SelectorType.HasFlag(SelectorType.All))
			{
				return true;
			}
			if (selector.SelectorType.HasFlag(SelectorType.PseudoClass))
			{
				return this.MatchesPseudoClass(obj, selector);
			}
			if (obj.NodeType != NodeType.ELEMENT_NODE)
			{
				return false;
			}
			if (selector.SelectorType.HasFlag(SelectorType.ID) && selector.ID != obj.Id)
			{
				return false;
			}
			if (selector.SelectorType.HasFlag(SelectorType.Class) && !obj.HasClass(selector.Class))
			{
				return false;
			}
			if (selector.SelectorType.HasFlag(SelectorType.Tag) && !string.Equals(obj.NodeName, selector.Tag, StringComparison.CurrentCultureIgnoreCase))
			{
				return false;
			}
			if ((selector.SelectorType & SelectorType.AttributeValue) > (SelectorType)0)
			{
				return AttributeSelectors.Matches(obj, selector);
			}
			return selector.SelectorType != SelectorType.None;
		}

		protected static IEnumerable<IDomObject> GetResultPositionMatches(IEnumerable<IDomObject> list, SelectorClause selector)
		{
			IEnumerable<IDomObject> allChildOrDescendants = SelectorEngine.GetAllChildOrDescendants(selector.TraversalType, list);
			return ((IPseudoSelectorFilter)selector.PseudoSelector).Filter(allChildOrDescendants);
		}

		protected IEnumerable<IDomObject> GetPseudoClassMatches(IDomElement elm, SelectorClause selector)
		{
			IEnumerable<IDomObject> results = ((IPseudoSelectorChild)selector.PseudoSelector).ChildMatches(elm);
			foreach (IDomObject item in results)
			{
				yield return item;
			}
			if (selector.TraversalType == TraversalType.Descendent || selector.TraversalType == TraversalType.All)
			{
				foreach (IDomElement child in elm.ChildElements)
				{
					foreach (IDomObject item2 in this.GetPseudoClassMatches(child, selector))
					{
						yield return item2;
					}
				}
			}
			yield break;
		}

		protected bool MatchesPseudoClass(IDomElement element, SelectorClause selector)
		{
			return ((IPseudoSelectorChild)selector.PseudoSelector).Matches(element);
		}

		DomIndexFeatures GetFeatures(IDomIndex index)
		{
			return ((index is IDomIndexQueue) ? DomIndexFeatures.Queue : ((DomIndexFeatures)0)) | ((index is IDomIndexRanged) ? DomIndexFeatures.Range : ((DomIndexFeatures)0));
		}

		static IEnumerable<IDomObject> EmptyEnumerable()
		{
			yield break;
		}

		protected IEnumerable<IDomObject> GetAdjacentOrSiblings(TraversalType traversalType, IEnumerable<IDomObject> list)
		{
			IEnumerable<IDomObject> result;
			switch (traversalType)
			{
			case TraversalType.Adjacent:
				result = this.GetAdjacentElements(list);
				break;
			case TraversalType.Sibling:
				result = SelectorEngine.GetSiblings(list);
				break;
			default:
				result = list;
				break;
			}
			return result;
		}

		protected static IEnumerable<IDomObject> GetAllElements(IEnumerable<IDomObject> list)
		{
			foreach (IDomObject item in list)
			{
				yield return item;
				foreach (IDomElement descendant in SelectorEngine.GetDescendantElements(item))
				{
					yield return descendant;
				}
			}
			yield break;
		}

		protected static IEnumerable<IDomObject> GetAllChildOrDescendants(TraversalType traversalType, IEnumerable<IDomObject> list)
		{
			switch (traversalType)
			{
			case TraversalType.All:
				return SelectorEngine.GetAllElements(list);
			case TraversalType.Descendent:
				return SelectorEngine.GetDescendantElements(list);
			case TraversalType.Child:
				return SelectorEngine.GetChildElements(list);
			}
			return list;
		}

		protected IEnumerable<IDomObject> GetTraversalTargetElements(TraversalType traversalType, IEnumerable<IDomObject> list)
		{
			switch (traversalType)
			{
			case TraversalType.All:
				throw new InvalidOperationException("TraversalType.All should not be found at this point.");
			case TraversalType.Filter:
				return list;
			case TraversalType.Descendent:
				throw new InvalidOperationException("TraversalType.Descendant should not be found at this point.");
			case TraversalType.Child:
				return SelectorEngine.GetChildElements(list);
			case TraversalType.Adjacent:
				return this.GetAdjacentElements(list);
			case TraversalType.Sibling:
				return SelectorEngine.GetSiblings(list);
			default:
				throw new NotImplementedException("Unimplemented traversal type.");
			}
		}

		protected static IEnumerable<IDomElement> GetChildElements(IEnumerable<IDomObject> list)
		{
			foreach (IDomObject item in list)
			{
				foreach (IDomElement child in item.ChildElements)
				{
					yield return child;
				}
			}
			yield break;
		}

		public static IEnumerable<IDomElement> GetDescendantElements(IEnumerable<IDomObject> list)
		{
			foreach (IDomObject item in list)
			{
				foreach (IDomElement child in SelectorEngine.GetDescendantElements(item))
				{
					yield return child;
				}
			}
			yield break;
		}

		public static IEnumerable<IDomElement> GetDescendantElements(IDomObject element)
		{
			foreach (IDomElement child in element.ChildElements)
			{
				yield return child;
				foreach (IDomElement grandChild in SelectorEngine.GetDescendantElements(child))
				{
					yield return grandChild;
				}
			}
			yield break;
		}

		protected IEnumerable<IDomElement> GetAdjacentElements(IEnumerable<IDomObject> list)
		{
			return CQ.Map<IDomElement>(list, (IDomObject item) => item.NextElementSibling);
		}

		protected static IEnumerable<IDomElement> GetSiblings(IEnumerable<IDomObject> list)
		{
			foreach (IDomObject item in list)
			{
				IDomContainer parent = item.ParentNode;
				int index = item.Index + 1;
				int length = parent.ChildNodes.Count;
				while (index < length)
				{
					IDomElement node = parent.ChildNodes[index] as IDomElement;
					if (node != null)
					{
						yield return node;
					}
					index++;
				}
			}
			yield break;
		}

		List<SelectorClause> ActiveSelectors;

		int activeSelectorId;

		enum IndexMode
		{
			None,
			Basic,
			Subselect
		}
	}
}
