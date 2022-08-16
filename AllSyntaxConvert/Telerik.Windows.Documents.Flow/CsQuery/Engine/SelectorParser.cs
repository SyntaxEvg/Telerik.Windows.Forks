using System;
using CsQuery.StringScanner;
using CsQuery.StringScanner.Patterns;

namespace CsQuery.Engine
{
	class SelectorParser
	{
		protected SelectorClause Current
		{
			get
			{
				if (this._Current == null)
				{
					this._Current = new SelectorClause();
				}
				return this._Current;
			}
		}

		public Selector Parse(string selector)
		{
			this.Selectors = new Selector();
			string text = (selector ?? string.Empty).Trim();
			if (this.IsHtml(selector))
			{
				this.Current.Html = text;
				this.Current.SelectorType = SelectorType.HTML;
				this.Selectors.Add(this.Current);
				return this.Selectors;
			}
			this.scanner = Scanner.Create(text);
			while (!this.scanner.Finished)
			{
				char c = this.scanner.Current;
				if (c <= '.')
				{
					if (c == ' ')
					{
						this.scanner.NextNonWhitespace();
						this.NextTraversalType = TraversalType.Descendent;
						continue;
					}
					if (c != '#')
					{
						switch (c)
						{
						case '*':
							this.StartNewSelector(SelectorType.All);
							this.scanner.Next();
							continue;
						case '+':
							this.StartNewSelector(TraversalType.Adjacent);
							this.scanner.NextNonWhitespace();
							continue;
						case ',':
							this.FinishSelector();
							this.NextCombinatorType = CombinatorType.Root;
							this.NextTraversalType = TraversalType.All;
							this.scanner.NextNonWhitespace();
							continue;
						case '.':
							this.StartNewSelector(SelectorType.Class);
							this.scanner.Next();
							this.Current.Class = this.scanner.Get(MatchFunctions.CssClassName);
							continue;
						}
					}
					else
					{
						this.scanner.Next();
						if (!this.scanner.Finished)
						{
							this.StartNewSelector(SelectorType.ID);
							this.Current.ID = this.scanner.Get(MatchFunctions.HtmlIDValue());
							continue;
						}
						continue;
					}
				}
				else
				{
					switch (c)
					{
					case ':':
					{
						this.scanner.Next();
						string text2 = this.scanner.Get(new Func<int, char, bool>(MatchFunctions.PseudoSelector)).ToLower();
						string key;
						switch (key = text2)
						{
						case "input":
							this.AddTagSelector("input", false);
							this.AddTagSelector("textarea", true);
							this.AddTagSelector("select", true);
							this.AddTagSelector("button", true);
							continue;
						case "text":
							this.StartNewSelector(SelectorType.Tag | SelectorType.AttributeValue);
							this.Current.Tag = "input";
							this.Current.AttributeSelectorType = AttributeSelectorType.Equals;
							this.Current.AttributeName = "type";
							this.Current.AttributeValue = "text";
							this.StartNewSelector(SelectorType.Tag | SelectorType.AttributeValue, CombinatorType.Grouped, this.Current.TraversalType);
							this.Current.Tag = "input";
							this.Current.AttributeSelectorType = AttributeSelectorType.NotExists;
							this.Current.AttributeName = "type";
							this.Current.SelectorType |= SelectorType.Tag;
							this.Current.Tag = "input";
							continue;
						case "checkbox":
						case "radio":
						case "button":
						case "file":
						case "image":
						case "password":
							this.AddInputSelector(text2, "input", false);
							continue;
						case "reset":
						case "submit":
							this.AddInputSelector(text2, null, false);
							continue;
						case "checked":
						case "selected":
						case "disabled":
							this.StartNewSelector(SelectorType.AttributeValue);
							this.Current.AttributeSelectorType = AttributeSelectorType.Exists;
							this.Current.AttributeName = text2;
							continue;
						case "enabled":
							this.StartNewSelector(SelectorType.AttributeValue);
							this.Current.AttributeSelectorType = AttributeSelectorType.NotExists;
							this.Current.AttributeName = "disabled";
							continue;
						case "first-letter":
						case "first-line":
						case "before":
						case "after":
							throw new NotImplementedException("The CSS pseudoelement selectors are not implemented in CsQuery.");
						case "target":
						case "link":
						case "hover":
						case "active":
						case "focus":
						case "visited":
							throw new NotImplementedException("Pseudoclasses that require a browser aren't implemented.");
						}
						if (!this.AddPseudoSelector(text2))
						{
							throw new ArgumentException("Unknown pseudo-class :\"" + text2 + "\". If this is a valid CSS or jQuery selector, please let us know.");
						}
						continue;
					}
					case ';':
					case '=':
						break;
					case '<':
						this.Current.Html = text;
						this.scanner.End();
						continue;
					case '>':
						this.StartNewSelector(TraversalType.Child);
						this.Current.ChildDepth = 1;
						this.scanner.NextNonWhitespace();
						continue;
					default:
						if (c != '[')
						{
							if (c == '~')
							{
								this.StartNewSelector(TraversalType.Sibling);
								this.scanner.NextNonWhitespace();
								continue;
							}
						}
						else
						{
							this.StartNewSelector(SelectorType.AttributeValue);
							IStringScanner stringScanner = this.scanner.ExpectBoundedBy('[', true).ToNewScanner();
							this.Current.AttributeName = stringScanner.Get(MatchFunctions.HTMLAttribute());
							stringScanner.SkipWhitespace();
							if (stringScanner.Finished)
							{
								this.Current.AttributeSelectorType = AttributeSelectorType.Exists;
								continue;
							}
							string text3 = stringScanner.Get(new string[] { "=", "^=", "*=", "~=", "$=", "!=", "|=" });
							if (stringScanner.Finished)
							{
								this.Current.AttributeSelectorType = AttributeSelectorType.Exists;
								continue;
							}
							IStringScanner stringScanner2 = stringScanner.Expect(this.expectsOptionallyQuotedValue()).ToNewScanner();
							this.Current.AttributeValue = (stringScanner2.Finished ? "" : stringScanner2.Get(new EscapedString()));
							string key2;
							switch (key2 = text3)
							{
							case "=":
								this.Current.SelectorType |= SelectorType.AttributeValue;
								this.Current.AttributeSelectorType = AttributeSelectorType.Equals;
								continue;
							case "^=":
								this.Current.SelectorType |= SelectorType.AttributeValue;
								this.Current.AttributeSelectorType = AttributeSelectorType.StartsWith;
								if (this.Current.AttributeValue == "")
								{
									this.Current.AttributeValue = string.Concat('\0');
									continue;
								}
								continue;
							case "*=":
								this.Current.SelectorType |= SelectorType.AttributeValue;
								this.Current.AttributeSelectorType = AttributeSelectorType.Contains;
								continue;
							case "~=":
								this.Current.SelectorType |= SelectorType.AttributeValue;
								this.Current.AttributeSelectorType = AttributeSelectorType.ContainsWord;
								continue;
							case "$=":
								this.Current.SelectorType |= SelectorType.AttributeValue;
								this.Current.AttributeSelectorType = AttributeSelectorType.EndsWith;
								continue;
							case "!=":
								this.Current.AttributeSelectorType = AttributeSelectorType.NotEquals;
								continue;
							case "|=":
								this.Current.SelectorType |= SelectorType.AttributeValue;
								this.Current.AttributeSelectorType = AttributeSelectorType.StartsWithOrHyphen;
								continue;
							}
							throw new ArgumentException("Unknown attibute matching operator '" + text3 + "'");
						}
						break;
					}
				}
				string tagName = "";
				if (this.scanner.TryGet(MatchFunctions.HTMLTagSelectorName(), out tagName))
				{
					this.AddTagSelector(tagName, false);
				}
				else
				{
					if (this.scanner.Index != 0)
					{
						throw new ArgumentException(this.scanner.LastError);
					}
					this.Current.Html = text;
					this.Current.SelectorType = SelectorType.HTML;
					this.scanner.End();
				}
			}
			this.FinishSelector();
			if (this.Selectors.Count == 0)
			{
				SelectorClause clause = new SelectorClause
				{
					SelectorType = SelectorType.None,
					TraversalType = TraversalType.Filter
				};
				this.Selectors.Add(clause);
			}
			return this.Selectors;
		}

		bool AddPseudoSelector(string key)
		{
			IPseudoSelector pseudoSelector;
			if (PseudoSelectors.Items.TryGetInstance(key, out pseudoSelector))
			{
				this.StartNewSelector(SelectorType.PseudoClass);
				this.Current.PseudoSelector = pseudoSelector;
				if (!this.scanner.Finished && this.scanner.Current == '(')
				{
					pseudoSelector.Arguments = this.scanner.GetBoundedBy('(', true);
				}
				else
				{
					pseudoSelector.Arguments = null;
				}
				return true;
			}
			return false;
		}

		void AddTagSelector(string tagName, bool combineWithPrevious = false)
		{
			if (!combineWithPrevious)
			{
				this.StartNewSelector(SelectorType.Tag);
			}
			else
			{
				this.StartNewSelector(SelectorType.Tag, CombinatorType.Grouped, this.Current.TraversalType);
			}
			this.Current.Tag = tagName;
		}

		void AddInputSelector(string type, string tag = null, bool combineWithPrevious = false)
		{
			if (!combineWithPrevious)
			{
				this.StartNewSelector(SelectorType.AttributeValue);
			}
			else
			{
				this.StartNewSelector(SelectorType.AttributeValue, CombinatorType.Grouped, this.Current.TraversalType);
			}
			if (tag != null)
			{
				this.Current.SelectorType |= SelectorType.Tag;
				this.Current.Tag = tag;
			}
			this.Current.AttributeSelectorType = AttributeSelectorType.Equals;
			this.Current.AttributeName = "type";
			this.Current.AttributeValue = type;
			if (type == "button")
			{
				this.StartNewSelector(SelectorType.Tag, CombinatorType.Grouped, this.Current.TraversalType);
				this.Current.Tag = "button";
			}
		}

		protected IExpectPattern expectsOptionallyQuotedValue()
		{
			return new OptionallyQuoted("]");
		}

		protected void StartNewSelector(SelectorType selectorType)
		{
			this.StartNewSelector(selectorType, this.NextCombinatorType, this.NextTraversalType);
		}

		protected void StartNewSelector(CombinatorType combinatorType, TraversalType traversalType)
		{
			this.StartNewSelector((SelectorType)0, combinatorType, traversalType);
		}

		protected void StartNewSelector(TraversalType traversalType)
		{
			this.StartNewSelector((SelectorType)0, this.NextCombinatorType, traversalType);
		}

		protected void StartNewSelector(SelectorType selectorType, CombinatorType combinatorType, TraversalType traversalType)
		{
			if ((this.Current.IsComplete && this.Current.SelectorType != SelectorType.All) || traversalType != TraversalType.Filter)
			{
				this.FinishSelector();
				this.Current.CombinatorType = combinatorType;
				this.Current.TraversalType = traversalType;
			}
			this.Current.SelectorType = selectorType;
		}

		protected void FinishSelector()
		{
			if (this.Current.IsComplete)
			{
				SelectorClause clause = this.Current.Clone();
				this.Selectors.Add(clause);
			}
			this.Current.Clear();
			this.NextTraversalType = TraversalType.Filter;
			this.NextCombinatorType = CombinatorType.Chained;
		}

		protected void ClearCurrent()
		{
			this._Current = null;
		}

		public bool IsHtml(string text)
		{
			return !string.IsNullOrEmpty(text) && text[0] == '<';
		}

		IStringScanner scanner;

		Selector Selectors;

		SelectorClause _Current;

		TraversalType NextTraversalType = TraversalType.All;

		CombinatorType NextCombinatorType = CombinatorType.Root;
	}
}
