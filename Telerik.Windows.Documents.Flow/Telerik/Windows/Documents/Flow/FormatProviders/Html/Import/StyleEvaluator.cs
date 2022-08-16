using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using PreMailer.Net;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class StyleEvaluator
	{
		public StyleEvaluator(CQ cq, SortedList<string, StyleClass> styles, StyleNamesInfo stylesNamesInfo)
		{
			Guard.ThrowExceptionIfNull<CQ>(cq, "cq");
			Guard.ThrowExceptionIfNull<StyleNamesInfo>(stylesNamesInfo, "stylesInfo");
			this.cq = cq;
			this.styles = styles;
			this.stylesNamesInfo = stylesNamesInfo;
			this.cssSelectorParser = new CssSelectorParser();
		}

		public HtmlStyleRepository Evaluate()
		{
			HtmlStyleRepository htmlStyleRepository = new HtmlStyleRepository();
			this.CreateDefaultSelectors(htmlStyleRepository);
			this.AddSelectorsForStyles(htmlStyleRepository);
			this.AddSelectorsForElementSelectors(htmlStyleRepository);
			this.AddEmptySelectorsForAllNonDefinedStyles(htmlStyleRepository);
			this.InlineStyles();
			return htmlStyleRepository;
		}

		static void CopyAttributes(Selector selector, StyleClass style)
		{
			foreach (KeyValuePair<string, string> keyValuePair in style.Attributes)
			{
				selector.RegisterProperty(keyValuePair.Key, keyValuePair.Value);
			}
		}

		static string GetSelectorName(string styleName)
		{
			if (styleName.StartsWith("."))
			{
				return styleName.Substring(1);
			}
			return styleName;
		}

		void AddSelectorsForElementSelectors(HtmlStyleRepository htmlStyleRepository)
		{
			foreach (string text in StyleToElementSelectorMappings.GetSelectors())
			{
				bool flag = false;
				string text2 = null;
				string text3 = null;
				if (StyleToElementSelectorMappings.TryGetStyleId(text, out text2))
				{
					text3 = StyleNamesConverter.ConvertStyleNameOnExport(text2);
					flag = this.ExplicitlySetClassName(text, text3);
				}
				if (flag && text2 != null)
				{
					StyleClass style = this.styles[text];
					this.styles.Remove(text);
					if (!htmlStyleRepository.ContainsStyle(text3))
					{
						Selector selector = new Selector(text3, SelectorType.Style);
						StyleEvaluator.CopyAttributes(selector, style);
						htmlStyleRepository.RegisterStyle(text3, selector);
					}
				}
			}
		}

		bool ExplicitlySetClassName(string selector, string className)
		{
			bool result = false;
			foreach (IDomObject domObject in this.cq[selector])
			{
				result = true;
				domObject.AddClass(className);
			}
			return result;
		}

		void CreateDefaultSelectors(HtmlStyleRepository htmlStyleRepository)
		{
			htmlStyleRepository.DefaultParagraphStyle = this.CreateDefaultElementSelector("p", null);
			htmlStyleRepository.DefaultCharacterStyle = this.CreateDefaultElementSelector("span", new Action<StyleClass>(this.SaveDefaultSpanStyleClass));
		}

		void AddSelectorsForStyles(HtmlStyleRepository htmlStyleRepository)
		{
			foreach (KeyValuePair<string, StyleClass> keyValuePair in this.styles.ToList<KeyValuePair<string, StyleClass>>())
			{
				if (this.ShouldCreateStyle(keyValuePair.Key))
				{
					string selectorName = StyleEvaluator.GetSelectorName(keyValuePair.Key);
					if (!this.stylesNamesInfo.TableStyles.Contains(selectorName))
					{
						this.styles.Remove(keyValuePair.Key);
					}
					Selector selector = new Selector(StyleEvaluator.GetSelectorName(selectorName), SelectorType.Style);
					StyleEvaluator.CopyAttributes(selector, keyValuePair.Value);
					htmlStyleRepository.RegisterStyle(selector.Name, selector);
				}
			}
		}

		void AddEmptySelectorsForAllNonDefinedStyles(HtmlStyleRepository htmlStyleRepository)
		{
			CQ first = this.cq.Select("p[class]");
			CQ second = this.cq.Select("span[class]");
			IEnumerable<string> enumerable = (from element in first.Concat(second)
				where element.HasClasses
				select element.Classes.First<string>()).Distinct<string>();
			foreach (string text in enumerable)
			{
				if (!htmlStyleRepository.ContainsStyle(text))
				{
					Selector style = new Selector(text, SelectorType.Style);
					htmlStyleRepository.RegisterStyle(text, style);
				}
			}
		}

		void InlineStyles()
		{
			this.InlineDefaultSpanStyle();
			PreMailer.MoveCssInline(this.cq, this.styles, false);
		}

		void InlineDefaultSpanStyle()
		{
			if (this.defaultSpanStyle != null)
			{
				SortedList<string, StyleClass> sortedList = new SortedList<string, StyleClass>();
				sortedList.Add("span", this.defaultSpanStyle);
				CQ cq = this.cq.Select("span").Not("[class!=\"\"]");
				PreMailer.MoveCssInline(cq, sortedList, true);
			}
		}

		void SaveDefaultSpanStyleClass(StyleClass styleClass)
		{
			this.defaultSpanStyle = styleClass;
		}

		Selector CreateDefaultElementSelector(string selectorName, Action<StyleClass> saveStyleCallback = null)
		{
			Selector selector = new Selector(selectorName, SelectorType.Elements);
			StyleClass styleClass;
			if (this.styles.TryGetValue(selectorName, out styleClass))
			{
				this.styles.Remove(selectorName);
				StyleEvaluator.CopyAttributes(selector, styleClass);
				if (saveStyleCallback != null)
				{
					saveStyleCallback(styleClass);
				}
			}
			return selector;
		}

		bool ShouldCreateStyle(string selector)
		{
			CssSpecificity cssSpecificity = this.cssSelectorParser.CalculateSpecificity(selector);
			return cssSpecificity.Classes == 1 && cssSpecificity.ClassesAttributesPseudoElements == 1 && cssSpecificity.Elements == 0 && cssSpecificity.Ids == 0;
		}

		readonly CQ cq;

		readonly SortedList<string, StyleClass> styles;

		readonly StyleNamesInfo stylesNamesInfo;

		readonly CssSelectorParser cssSelectorParser;

		StyleClass defaultSpanStyle;
	}
}
