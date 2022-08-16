using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery.ExtensionMethods.Internal;
using CsQuery.HtmlParser;

namespace CsQuery.Engine
{
	class SelectorClause
	{
		public SelectorClause()
		{
			this.Initialize();
		}

		protected void Initialize()
		{
			this.SelectorType = (SelectorType)0;
			this.AttributeSelectorType = AttributeSelectorType.Equals;
			this.CombinatorType = CombinatorType.Root;
			this.TraversalType = TraversalType.All;
			this.AttributeValueStringComparison = StringComparison.CurrentCulture;
			this.AttributeValue = "";
		}

		bool IsCaseInsensitiveAttributeValue
		{
			get
			{
				return HtmlData.IsCaseInsensitiveValues(this.AttributeNameTokenID);
			}
		}

		public SelectorType SelectorType { get; set; }

		public CombinatorType CombinatorType { get; set; }

		public TraversalType TraversalType { get; set; }

		public AttributeSelectorType AttributeSelectorType { get; set; }

		public IPseudoSelector PseudoSelector { get; set; }

		public string Tag
		{
			get
			{
				return this._Tag;
			}
			set
			{
				this._Tag = ((value == null) ? value : value.ToUpper());
			}
		}

		public string Criteria { get; set; }

		public int PositionIndex { get; set; }

		public int ChildDepth { get; set; }

		public string AttributeName
		{
			get
			{
				return this._AttributeName;
			}
			set
			{
				if (value == null)
				{
					this._AttributeName = null;
					this.AttributeNameTokenID = 0;
					this.AttributeValueStringComparison = StringComparison.CurrentCulture;
					return;
				}
				this._AttributeName = value.ToLower();
				this.AttributeNameTokenID = HtmlData.Tokenize(value);
				this.AttributeValueStringComparison = (this.IsCaseInsensitiveAttributeValue ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			}
		}

		public string AttributeValue
		{
			get
			{
				return this._AttributeValue;
			}
			set
			{
				this._AttributeValue = value ?? "";
			}
		}

		public ushort AttributeNameTokenID { get; set; }

		public StringComparison AttributeValueStringComparison { get; set; }

		public StringComparer AttributeValueStringComparer
		{
			get
			{
				if (this._AttributeValueStringComparer == null)
				{
					this._AttributeValueStringComparer = this.AttributeValueStringComparison.ComparerFor();
				}
				return this._AttributeValueStringComparer;
			}
		}

		public string Class { get; set; }

		public string ID { get; set; }

		public string Html { get; set; }

		public IEnumerable<IDomObject> SelectElements { get; set; }

		public bool IsDomPositionPseudoSelector
		{
			get
			{
				return this.SelectorType == SelectorType.PseudoClass && !this.IsResultListPosition;
			}
		}

		public bool IsResultListPosition
		{
			get
			{
				return this.SelectorType == SelectorType.PseudoClass && this.PseudoSelector is IPseudoSelectorFilter;
			}
		}

		public bool IsFunction
		{
			get
			{
				return this.PseudoSelector.MaximumParameterCount != 0;
			}
		}

		public bool IsNew
		{
			get
			{
				return this.SelectorType == (SelectorType)0 && this.ChildDepth == 0 && this.TraversalType == TraversalType.All && this.CombinatorType == CombinatorType.Root;
			}
		}

		public bool IsComplete
		{
			get
			{
				return this.SelectorType != (SelectorType)0;
			}
		}

		public bool NoIndex { get; set; }

		public void Clear()
		{
			this.AttributeName = null;
			this.AttributeSelectorType = (AttributeSelectorType)0;
			this.AttributeValue = null;
			this.ChildDepth = 0;
			this.Class = null;
			this.Criteria = null;
			this.Html = null;
			this.ID = null;
			this.NoIndex = false;
			this.PositionIndex = 0;
			this.SelectElements = null;
			this.Tag = null;
			this.Initialize();
		}

		public SelectorClause Clone()
		{
			return new SelectorClause
			{
				SelectorType = this.SelectorType,
				TraversalType = this.TraversalType,
				CombinatorType = this.CombinatorType,
				AttributeName = this.AttributeName,
				AttributeSelectorType = this.AttributeSelectorType,
				AttributeValue = this.AttributeValue,
				ChildDepth = this.ChildDepth,
				Class = this.Class,
				Criteria = this.Criteria,
				Html = this.Html,
				ID = this.ID,
				NoIndex = this.NoIndex,
				PositionIndex = this.PositionIndex,
				SelectElements = this.SelectElements,
				Tag = this.Tag,
				PseudoSelector = this.PseudoSelector
			};
		}

		public override int GetHashCode()
		{
			return this.GetHash(this.SelectorType) + this.GetHash(this.TraversalType) + this.GetHash(this.CombinatorType) + this.GetHash(this.AttributeName) + this.GetHash(this.AttributeSelectorType) + this.GetHash(this.AttributeValue) + this.GetHash(this.Class) + this.GetHash(this.Criteria) + this.GetHash(this.Html) + this.GetHash(this.ID) + this.GetHash(this.NoIndex) + this.GetHash(this.PositionIndex) + this.GetHash(this.SelectElements) + this.GetHash(this.Tag);
		}

		public override bool Equals(object obj)
		{
			SelectorClause selectorClause = obj as SelectorClause;
			return selectorClause != null && selectorClause.SelectorType == this.SelectorType && selectorClause.TraversalType == this.TraversalType && selectorClause.CombinatorType == this.CombinatorType && selectorClause.AttributeName == this.AttributeName && selectorClause.AttributeSelectorType == this.AttributeSelectorType && selectorClause.AttributeValue == this.AttributeValue && selectorClause.ChildDepth == this.ChildDepth && selectorClause.Class == this.Class && selectorClause.Criteria == this.Criteria && selectorClause.Html == this.Html && selectorClause.ID == this.ID && selectorClause.NoIndex == this.NoIndex && selectorClause.PositionIndex == this.PositionIndex && selectorClause.SelectElements == this.SelectElements && selectorClause.Tag == this.Tag;
		}

		int GetHash(object obj)
		{
			if (obj != null)
			{
				return obj.GetHashCode();
			}
			return 0;
		}

		public override string ToString()
		{
			string text = "";
			switch (this.TraversalType)
			{
			case TraversalType.Descendent:
				text += " ";
				break;
			case TraversalType.Child:
				text += " > ";
				break;
			case TraversalType.Adjacent:
				text += " + ";
				break;
			case TraversalType.Sibling:
				text += " ~ ";
				break;
			}
			if (this.SelectorType.HasFlag(SelectorType.Elements))
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"<ElementList[",
					this.SelectElements.Count<IDomObject>(),
					"]> "
				});
			}
			if (this.SelectorType.HasFlag(SelectorType.HTML))
			{
				object obj2 = text;
				text = string.Concat(new object[]
				{
					obj2,
					"<HTML[",
					this.Html.Length,
					"]> "
				});
			}
			if (this.SelectorType.HasFlag(SelectorType.Tag))
			{
				text += this.Tag;
			}
			if (this.SelectorType.HasFlag(SelectorType.ID))
			{
				text = text + "#" + this.ID;
			}
			if (this.SelectorType.HasFlag(SelectorType.AttributeValue))
			{
				text = text + "[" + this.AttributeName;
				if (!string.IsNullOrEmpty(this.AttributeValue))
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						".",
						this.AttributeSelectorType.ToString(),
						".'",
						this.AttributeValue,
						"'"
					});
				}
				text += "]";
			}
			if (this.SelectorType.HasFlag(SelectorType.Class))
			{
				text = text + "." + this.Class;
			}
			if (this.SelectorType.HasFlag(SelectorType.All))
			{
				text += "*";
			}
			if (this.SelectorType.HasFlag(SelectorType.PseudoClass))
			{
				text = text + ":" + this.PseudoSelector.Name;
				if (this.PseudoSelector.Arguments != null && this.PseudoSelector.Arguments.Length > 0)
				{
					text = text + "(" + string.Join(",", new string[] { this.PseudoSelector.Arguments }) + ")";
				}
			}
			return text;
		}

		string _Tag;

		string _AttributeName;

		string _AttributeValue;

		StringComparer _AttributeValueStringComparer;
	}
}
