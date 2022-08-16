using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CsQuery.EquationParser;

namespace CsQuery.Engine
{
	class NthChildMatcher
	{
		protected bool IsJustNumber
		{
			get
			{
				return this._IsJustNumber;
			}
			set
			{
				this._IsJustNumber = value;
				this.IndexMatchesImpl = (value ? new Func<int, bool>(this.IndexMatchesNumber) : new Func<int, bool>(this.IndexMatchesFormula));
				this.GetMatchingChildrenImpl = (value ? new Func<IDomElement, IEnumerable<IDomObject>>(this.GetMatchingChildrenNumber) : new Func<IDomElement, IEnumerable<IDomObject>>(this.GetMatchingChildrenFormula));
			}
		}

		protected string OnlyNodeName
		{
			get
			{
				return this._OnlyNodeName;
			}
			set
			{
				this._OnlyNodeName = ((!string.IsNullOrEmpty(value)) ? value.ToUpper() : null);
			}
		}

		protected string Text
		{
			get
			{
				return this._Text;
			}
			set
			{
				this._Text = value;
				this.ParseEquation(value);
			}
		}

		public bool IsNthChildOfType(IDomElement element, string formula, bool fromLast = false)
		{
			return this.IndexMatches(this.IndexOf(element, true, fromLast), formula, fromLast);
		}

		public bool IsNthChild(IDomElement element, string formula, bool fromLast = false)
		{
			return this.IndexMatches(this.IndexOf(element, false, fromLast), formula, fromLast);
		}

		public IEnumerable<IDomObject> NthChildsOfType(IDomContainer element, string formula, bool fromLast = false)
		{
			return from item in element.ChildElements
				where this.IsNthChildOfType(item, formula, fromLast)
				select item;
		}

		public IEnumerable<IDomObject> NthChilds(IDomContainer element, string formula, bool fromLast = false)
		{
			return this.GetMatchingChildren(element, formula, null, fromLast);
		}

		int IndexOf(IDomElement obj, bool onlyOfSameType, bool fromLast = false)
		{
			int num = 0;
			INodeList childNodes = obj.ParentNode.ChildNodes;
			int count = childNodes.Count;
			for (int i = 0; i < count; i++)
			{
				IDomObject effectiveChild = NthChildMatcher.GetEffectiveChild(childNodes, i, fromLast);
				if (effectiveChild.NodeType == NodeType.ELEMENT_NODE && (!onlyOfSameType || effectiveChild.NodeNameID == obj.NodeNameID))
				{
					if (object.ReferenceEquals(effectiveChild, obj))
					{
						return num;
					}
					num++;
				}
			}
			return -1;
		}

		public bool IndexMatches(int index, string formula, bool fromLast = false)
		{
			this.FromLast = fromLast;
			return this.IndexMatches(index, formula);
		}

		public bool IndexMatches(int index, string formula)
		{
			this.Text = formula;
			return this.IndexMatchesImpl(index);
		}

		public IEnumerable<IDomObject> GetMatchingChildren(IDomContainer obj, string formula, string onlyNodeName = null, bool fromLast = false)
		{
			this.OnlyNodeName = onlyNodeName;
			this.FromLast = fromLast;
			return this.GetMatchingChildren(obj, formula);
		}

		public IEnumerable<IDomObject> GetMatchingChildren(IDomContainer obj, string formula)
		{
			this.Text = formula;
			return this.GetMatchingChildren(obj);
		}

		public IEnumerable<IDomObject> GetMatchingChildren(IDomContainer obj)
		{
			if (obj.HasChildren)
			{
				if (this.IsJustNumber)
				{
					IDomElement child = this.GetNthChild(obj, this.MatchOnlyIndex);
					if (child != null)
					{
						yield return child;
					}
				}
				else
				{
					this.UpdateCacheInfo(obj.ChildNodes.Count);
					int elementIndex = 1;
					int newActualIndex = -1;
					IDomElement el = this.GetNextChild(obj, -1, out newActualIndex);
					while (newActualIndex >= 0)
					{
						if (this.cacheInfo.MatchingIndices.Contains(elementIndex))
						{
							yield return el;
						}
						el = this.GetNextChild(obj, newActualIndex, out newActualIndex);
						elementIndex++;
					}
				}
			}
			yield break;
		}

		public static IDomObject GetEffectiveChild(INodeList nodeList, int index, bool fromLast)
		{
			if (fromLast)
			{
				return nodeList[nodeList.Count - index - 1];
			}
			return nodeList[index];
		}

		public static int GetEffectiveIndex(INodeList nodeList, int index, bool fromLast)
		{
			if (fromLast)
			{
				return nodeList.Length - index - 1;
			}
			return index;
		}

		IDomElement GetNthChild(IDomContainer parent, int index)
		{
			int num = 1;
			int currentIndex;
			IDomElement nextChild = this.GetNextChild(parent, -1, out currentIndex);
			while (nextChild != null && num != index)
			{
				nextChild = this.GetNextChild(parent, currentIndex, out currentIndex);
				num++;
			}
			return nextChild;
		}

		IDomElement GetNextChild(IDomContainer parent, int currentIndex, out int newIndex)
		{
			int num = currentIndex;
			INodeList childNodes = parent.ChildNodes;
			int count = childNodes.Count;
			IDomObject domObject = null;
			while (++num < count)
			{
				domObject = this.GetEffectiveChild(childNodes, num);
				if (domObject.NodeType == NodeType.ELEMENT_NODE)
				{
					break;
				}
			}
			if (num < count)
			{
				newIndex = num;
				return (IDomElement)domObject;
			}
			newIndex = -1;
			return null;
		}

		IDomObject GetEffectiveChild(INodeList nodeList, int index)
		{
			return NthChildMatcher.GetEffectiveChild(nodeList, index, this.FromLast);
		}

		protected void ParseEquation(string equationText)
		{
			this.CheckForSimpleNumber(equationText);
			if (this.IsJustNumber)
			{
				return;
			}
			equationText = this.CheckForEvenOdd(equationText);
			if (!NthChildMatcher.ParsedEquationCache.TryGetValue(equationText, out this.cacheInfo))
			{
				this.cacheInfo = new NthChildMatcher.CacheInfo();
				this.Equation = (this.cacheInfo.Equation = this.GetEquation(equationText));
				NthChildMatcher.ParsedEquationCache.TryAdd(equationText, this.cacheInfo);
				return;
			}
			this.Equation = this.cacheInfo.Equation;
		}

		protected void CheckForSimpleNumber(string equation)
		{
			int matchOnlyIndex;
			if (int.TryParse(equation, out matchOnlyIndex))
			{
				this.MatchOnlyIndex = matchOnlyIndex;
				this.IsJustNumber = true;
				return;
			}
			this.IsJustNumber = false;
		}

		IEquation<int> GetEquation(string equationText)
		{
			IEquation<int> equation;
			try
			{
				equation = Equations.CreateEquation<int>(equationText);
			}
			catch (InvalidCastException innerException)
			{
				throw new ArgumentException(string.Format("The equation {{0}} could not be parsed.", equationText), innerException);
			}
			IVariable variable;
			try
			{
				variable = equation.Variables.Single<IVariable>();
			}
			catch (InvalidOperationException innerException2)
			{
				throw new ArgumentException(string.Format("The equation {{0}} must contain a single variable 'n'.", equation), innerException2);
			}
			if (variable.Name != "n")
			{
				throw new ArgumentException(string.Format("The equation {{0}} does not have a variable 'n'.", equation));
			}
			return equation;
		}

		protected string CheckForEvenOdd(string equation)
		{
			string text;
			if ((text = this._Text) != null)
			{
				if (text == "odd")
				{
					return "2n+1";
				}
				if (text == "even")
				{
					return "2n";
				}
			}
			return equation;
		}

		protected bool IndexMatchesNumber(int index)
		{
			return this.MatchOnlyIndex - 1 == index;
		}

		protected bool IndexMatchesFormula(int index)
		{
			int num;
			index = (num = index + 1);
			if (index > this.cacheInfo.MaxIndex)
			{
				this.UpdateCacheInfo(num);
			}
			return this.cacheInfo.MatchingIndices.Contains(num);
		}

		public IEnumerable<IDomObject> GetMatchingChildrenNumber(IDomElement element)
		{
			if (element.HasChildren)
			{
				IDomElement child = this.GetNthChild(element, this.MatchOnlyIndex);
				if (child != null)
				{
					yield return child;
				}
			}
			yield break;
		}

		public IEnumerable<IDomObject> GetMatchingChildrenFormula(IDomElement element)
		{
			if (element.HasChildren)
			{
				this.UpdateCacheInfo(element.ChildNodes.Count);
				int elementIndex = 1;
				int newActualIndex = -1;
				IDomElement el = this.GetNextChild(element, -1, out newActualIndex);
				while (newActualIndex >= 0)
				{
					if (this.cacheInfo.MatchingIndices.Contains(elementIndex))
					{
						yield return el;
					}
					el = this.GetNextChild(element, newActualIndex, out newActualIndex);
					elementIndex++;
				}
			}
			yield break;
		}

		protected void UpdateCacheInfo(int lastIndex)
		{
			if (this.cacheInfo.MaxIndex >= lastIndex)
			{
				return;
			}
			int num = this.cacheInfo.NextIterator;
			int num2 = -1;
			int num3 = 999999;
			while ((num2 < lastIndex && num <= lastIndex + 1) || (num3 > num2 && num2 > 0))
			{
				this.Equation.SetVariable<int>("n", num);
				if (num2 > 0)
				{
					num3 = num2;
				}
				num2 = this.Equation.Value;
				if (num2 > 0)
				{
					this.cacheInfo.MatchingIndices.Add(num2);
				}
				num++;
			}
			this.cacheInfo.MaxIndex = lastIndex;
			this.cacheInfo.NextIterator = num;
		}

		static ConcurrentDictionary<string, NthChildMatcher.CacheInfo> ParsedEquationCache = new ConcurrentDictionary<string, NthChildMatcher.CacheInfo>();

		NthChildMatcher.CacheInfo cacheInfo;

		int MatchOnlyIndex;

		IEquation<int> Equation;

		bool _IsJustNumber;

		string _Text;

		string _OnlyNodeName;

		bool FromLast;

		Func<int, bool> IndexMatchesImpl;

		Func<IDomElement, IEnumerable<IDomObject>> GetMatchingChildrenImpl;

		protected class CacheInfo
		{
			public CacheInfo()
			{
				this.MatchingIndices = new HashSet<int>();
			}

			public IEquation<int> Equation;

			public HashSet<int> MatchingIndices;

			public int NextIterator;

			public int MaxIndex;
		}
	}
}
