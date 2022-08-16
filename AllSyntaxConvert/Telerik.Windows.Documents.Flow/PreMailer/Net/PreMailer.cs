using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;

namespace PreMailer.Net
{
	class PreMailer
	{
		PreMailer(CQ cq, SortedList<string, StyleClass> styles, bool useCqSelection)
		{
			this.document = cq;
			this.styles = styles;
			this.useCqSelection = useCqSelection;
			this.warnings = new List<string>();
			this.cssParser = new CssParser();
			this.cssSelectorParser = new CssSelectorParser();
		}

		public static InlineResult MoveCssInline(CQ cq, SortedList<string, StyleClass> styles, bool useCqSelection)
		{
			PreMailer preMailer = new PreMailer(cq, styles, useCqSelection);
			return preMailer.Process();
		}

		InlineResult Process()
		{
			CQ styleElements = this.GetStyleElements();
			this.RemoveStyleElements(styleElements);
			SortedList<string, StyleClass> stylesToApply = this.CleanUnsupportedSelectors(this.styles);
			Dictionary<IDomObject, List<StyleClass>> dictionary = this.FindElementsWithStyles(stylesToApply);
			Dictionary<IDomObject, StyleClass> elementStyles = this.MergeStyleClasses(dictionary);
			this.ApplyStyles(elementStyles);
			return new InlineResult(this.warnings);
		}

		CQ GetStyleElements()
		{
			return this.document.Find("style");
		}

		void RemoveStyleElements(CQ cssSourceNodes)
		{
			foreach (IDomObject domObject in cssSourceNodes)
			{
				domObject.Remove();
			}
		}

		SortedList<string, StyleClass> CleanUnsupportedSelectors(SortedList<string, StyleClass> selectors)
		{
			SortedList<string, StyleClass> sortedList = new SortedList<string, StyleClass>();
			List<StyleClass> list = new List<StyleClass>();
			foreach (KeyValuePair<string, StyleClass> keyValuePair in selectors)
			{
				if (this.cssSelectorParser.IsSupportedSelector(keyValuePair.Key))
				{
					sortedList.Add(keyValuePair.Key, keyValuePair.Value);
				}
				else
				{
					list.Add(keyValuePair.Value);
				}
			}
			if (!list.Any<StyleClass>())
			{
				return selectors;
			}
			foreach (StyleClass styleClass in list)
			{
				this.warnings.Add(string.Format("PreMailer.Net is unable to process the pseudo class/element '{0}' due to a limitation in CsQuery.", styleClass.Name));
			}
			return sortedList;
		}

		Dictionary<IDomObject, List<StyleClass>> FindElementsWithStyles(SortedList<string, StyleClass> stylesToApply)
		{
			Dictionary<IDomObject, List<StyleClass>> dictionary = new Dictionary<IDomObject, List<StyleClass>>();
			foreach (KeyValuePair<string, StyleClass> keyValuePair in stylesToApply)
			{
				CQ cq = this.document[keyValuePair.Value.Name];
				foreach (IDomObject domObject in cq)
				{
					if (!this.useCqSelection || this.document.Is(domObject))
					{
						List<StyleClass> list = (dictionary.ContainsKey(domObject) ? dictionary[domObject] : new List<StyleClass>());
						list.Add(keyValuePair.Value);
						dictionary[domObject] = list;
					}
				}
			}
			return dictionary;
		}

		Dictionary<IDomObject, List<StyleClass>> SortBySpecificity(Dictionary<IDomObject, List<StyleClass>> styles)
		{
			Dictionary<IDomObject, List<StyleClass>> dictionary = new Dictionary<IDomObject, List<StyleClass>>();
			foreach (KeyValuePair<IDomObject, List<StyleClass>> keyValuePair in styles)
			{
				if (keyValuePair.Key.Attributes != null)
				{
					List<StyleClass> list = (from x in keyValuePair.Value
						orderby this.cssSelectorParser.GetSelectorSpecificity(x.Name)
						select x).ToList<StyleClass>();
					if (string.IsNullOrWhiteSpace(keyValuePair.Key.Attributes["style"]))
					{
						keyValuePair.Key.SetAttribute("style", string.Empty);
					}
					else
					{
						list.Add(this.cssParser.ParseStyleClass("inline", keyValuePair.Key.Attributes["style"]));
					}
					dictionary[keyValuePair.Key] = list;
				}
			}
			return dictionary;
		}

		Dictionary<IDomObject, StyleClass> MergeStyleClasses(Dictionary<IDomObject, List<StyleClass>> styles)
		{
			Dictionary<IDomObject, StyleClass> dictionary = new Dictionary<IDomObject, StyleClass>();
			Dictionary<IDomObject, List<StyleClass>> dictionary2 = this.SortBySpecificity(styles);
			foreach (KeyValuePair<IDomObject, List<StyleClass>> keyValuePair in dictionary2)
			{
				StyleClass styleClass = new StyleClass();
				foreach (StyleClass styleClass2 in keyValuePair.Value)
				{
					styleClass.Merge(styleClass2, true);
				}
				dictionary[keyValuePair.Key] = styleClass;
			}
			return dictionary;
		}

		void ApplyStyles(Dictionary<IDomObject, StyleClass> elementStyles)
		{
			foreach (KeyValuePair<IDomObject, StyleClass> keyValuePair in elementStyles)
			{
				IDomObject key = keyValuePair.Key;
				key.SetAttribute("style", keyValuePair.Value.ToString());
			}
		}

		readonly CQ document;

		readonly SortedList<string, StyleClass> styles;

		readonly bool useCqSelection;

		readonly CssParser cssParser;

		readonly CssSelectorParser cssSelectorParser;

		readonly List<string> warnings;
	}
}
