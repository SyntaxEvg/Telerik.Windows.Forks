using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.Engine
{
	class Selector : global::System.Collections.Generic.IEnumerable<global::CsQuery.Engine.SelectorClause>, global::System.Collections.IEnumerable
	{
		public Selector()
		{
		}

		public Selector(SelectorClause clause)
		{
			this.Clauses.Add(clause);
		}

		public Selector(IEnumerable<SelectorClause> clauses)
		{
			this.Clauses.AddRange(clauses);
		}

		public Selector(string selector)
		{
			SelectorParser selectorParser = new SelectorParser();
			this.Clauses.AddRange(selectorParser.Parse(selector));
		}

		public Selector(IEnumerable<IDomObject> elements)
		{
			SelectorClause selectorClause = new SelectorClause();
			selectorClause.SelectorType = SelectorType.Elements;
			selectorClause.SelectElements = elements;
			this.Clauses.Add(selectorClause);
		}

		public Selector(IDomObject element)
		{
			SelectorClause selectorClause = new SelectorClause();
			selectorClause.SelectorType = SelectorType.Elements;
			selectorClause.SelectElements = new List<IDomObject>();
			((List<IDomObject>)selectorClause.SelectElements).Add(element);
			this.Clauses.Add(selectorClause);
		}

		public int Count
		{
			get
			{
				return this.Clauses.Count;
			}
		}

		public SelectorClause this[int index]
		{
			get
			{
				return this.Clauses[index];
			}
		}

		public bool IsHtml
		{
			get
			{
				return this.Count == 1 && this.Clauses[0].SelectorType == SelectorType.HTML;
			}
		}

		public void Add(SelectorClause clause)
		{
			this.Clauses.Add(clause);
		}

		public Selector ToFilterSelector()
		{
			Selector selector = this.Clone();
			foreach (SelectorClause selectorClause in selector.Clauses)
			{
				if (selectorClause.CombinatorType == CombinatorType.Root && selectorClause.SelectorType == SelectorType.PseudoClass)
				{
					selectorClause.TraversalType = TraversalType.Filter;
					selectorClause.CombinatorType = CombinatorType.Context;
				}
			}
			return selector;
		}

		public Selector ToContextSelector()
		{
			Selector selector = this.Clone();
			foreach (SelectorClause selectorClause in selector.Clauses)
			{
				if (selectorClause.CombinatorType == CombinatorType.Root)
				{
					selectorClause.CombinatorType = CombinatorType.Context;
					if (selectorClause.TraversalType == TraversalType.All)
					{
						selectorClause.TraversalType = TraversalType.Descendent;
					}
				}
			}
			return selector;
		}

		SelectorEngine GetEngine(IDomDocument document)
		{
			return new SelectorEngine(document, this);
		}

		protected List<SelectorClause> Clauses
		{
			get
			{
				if (this._Clauses == null)
				{
					this._Clauses = new List<SelectorClause>();
				}
				return this._Clauses;
			}
		}

		protected IEnumerable<SelectorClause> ClausesClone
		{
			get
			{
				if (this.Count > 0)
				{
					foreach (SelectorClause clause in this.Clauses)
					{
						yield return clause.Clone();
					}
				}
				yield break;
			}
		}

		public void Insert(int index, SelectorClause clause, CombinatorType combinatorType = CombinatorType.Chained)
		{
			if (combinatorType == CombinatorType.Root && this.Clauses.Count != 0)
			{
				throw new ArgumentException("Combinator type can only be root if there are no other selectors.");
			}
			if (this.Clauses.Count > 0 && index == 0)
			{
				this.Clauses[0].CombinatorType = combinatorType;
				clause.CombinatorType = CombinatorType.Root;
				clause.TraversalType = TraversalType.All;
			}
			this.Clauses.Insert(index, clause);
		}

		public global::System.Collections.Generic.IList<global::CsQuery.IDomObject> Select(global::CsQuery.IDomDocument document)
		{
			global::System.Collections.Generic.IEnumerable<global::CsQuery.IDomObject> context=null;
			return this.Select(document, context);
		}

		public global::System.Collections.Generic.IList<global::CsQuery.IDomObject> Select(global::CsQuery.IDomDocument document, global::CsQuery.IDomObject context)
		{
			return this.Select(document, global::CsQuery.Objects.Enumerate<global::CsQuery.IDomObject>(context));
		}

		public global::System.Collections.Generic.IList<global::CsQuery.IDomObject> Select(global::CsQuery.IDomDocument document, global::System.Collections.Generic.IEnumerable<global::CsQuery.IDomObject> context)
		{
			return this.GetEngine(document).Select(context);
		}

		public IEnumerable<IDomObject> Filter(IDomDocument document, IEnumerable<IDomObject> sequence)
		{
			HashSet<IDomObject> matches = new HashSet<IDomObject>(this.ToFilterSelector().Select(document, sequence));
			foreach (IDomObject item in sequence)
			{
				if (matches.Contains(item))
				{
					yield return item;
				}
			}
			yield break;
		}

		public bool Matches(IDomDocument document, IDomObject element)
		{
			return this.ToFilterSelector().Select(document, element).Any<IDomObject>();
		}

		public IEnumerable<IDomObject> Except(IDomDocument document, IEnumerable<IDomObject> sequence)
		{
			return sequence.Except(this.Select(document));
		}

		public Selector Clone()
		{
			return new Selector(this.ClausesClone);
		}

		public override string ToString()
		{
			string text = "";
			bool flag = true;
			foreach (SelectorClause selectorClause in this)
			{
				if (!flag)
				{
					if (selectorClause.CombinatorType == CombinatorType.Root)
					{
						text += ",";
					}
					else if (selectorClause.CombinatorType == CombinatorType.Grouped)
					{
						text += "&";
					}
				}
				text += selectorClause.ToString();
				flag = false;
			}
			return text;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return this.ToString().Equals(obj);
		}

		public IEnumerator<SelectorClause> GetEnumerator()
		{
			return this.Clauses.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Clauses.GetEnumerator();
		}

		List<SelectorClause> _Clauses;
	}
}
