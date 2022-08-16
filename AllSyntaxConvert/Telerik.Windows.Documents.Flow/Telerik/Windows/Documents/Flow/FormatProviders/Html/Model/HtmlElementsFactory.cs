using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model
{
	static class HtmlElementsFactory
	{
		static HtmlElementsFactory()
		{
			HtmlElementsFactory.RegisterFactoryMethod("DEFAULT", (HtmlContentManager cm) => new DefaultElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("TEXT", (HtmlContentManager cm) => new TextElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("html", (HtmlContentManager cm) => new HtmlElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("head", (HtmlContentManager cm) => new HeadElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("body", (HtmlContentManager cm) => new BodyElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("style", (HtmlContentManager cm) => new StyleElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("link", (HtmlContentManager cm) => new LinkElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("title", (HtmlContentManager cm) => new TitleElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("p", (HtmlContentManager cm) => new ParagraphElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("span", (HtmlContentManager cm) => new TextBasedElement(cm, "span"));
			HtmlElementsFactory.RegisterFactoryMethod("b", (HtmlContentManager cm) => new TextBasedElement(cm, "b"));
			HtmlElementsFactory.RegisterFactoryMethod("strong", (HtmlContentManager cm) => new TextBasedElement(cm, "strong"));
			HtmlElementsFactory.RegisterFactoryMethod("strike", (HtmlContentManager cm) => new TextBasedElement(cm, "strike"));
			HtmlElementsFactory.RegisterFactoryMethod("s", (HtmlContentManager cm) => new TextBasedElement(cm, "s"));
			HtmlElementsFactory.RegisterFactoryMethod("em", (HtmlContentManager cm) => new TextBasedElement(cm, "em"));
			HtmlElementsFactory.RegisterFactoryMethod("i", (HtmlContentManager cm) => new TextBasedElement(cm, "i"));
			HtmlElementsFactory.RegisterFactoryMethod("dfn", (HtmlContentManager cm) => new TextBasedElement(cm, "dfn"));
			HtmlElementsFactory.RegisterFactoryMethod("var", (HtmlContentManager cm) => new TextBasedElement(cm, "var"));
			HtmlElementsFactory.RegisterFactoryMethod("cite", (HtmlContentManager cm) => new TextBasedElement(cm, "cite"));
			HtmlElementsFactory.RegisterFactoryMethod("u", (HtmlContentManager cm) => new TextBasedElement(cm, "u"));
			HtmlElementsFactory.RegisterFactoryMethod("sup", (HtmlContentManager cm) => new TextBasedElement(cm, "sup"));
			HtmlElementsFactory.RegisterFactoryMethod("sub", (HtmlContentManager cm) => new TextBasedElement(cm, "sub"));
			HtmlElementsFactory.RegisterFactoryMethod("code", (HtmlContentManager cm) => new TextBasedElement(cm, "code"));
			HtmlElementsFactory.RegisterFactoryMethod("kbd", (HtmlContentManager cm) => new TextBasedElement(cm, "kbd"));
			HtmlElementsFactory.RegisterFactoryMethod("samp", (HtmlContentManager cm) => new TextBasedElement(cm, "samp"));
			HtmlElementsFactory.RegisterFactoryMethod("pre", (HtmlContentManager cm) => new TextBasedElement(cm, "pre"));
			HtmlElementsFactory.RegisterFactoryMethod("tt", (HtmlContentManager cm) => new TextBasedElement(cm, "tt"));
			HtmlElementsFactory.RegisterFactoryMethod("ins", (HtmlContentManager cm) => new TextBasedElement(cm, "ins"));
			HtmlElementsFactory.RegisterFactoryMethod("del", (HtmlContentManager cm) => new TextBasedElement(cm, "del"));
			HtmlElementsFactory.RegisterFactoryMethod("label", (HtmlContentManager cm) => new TextBasedElement(cm, "label"));
			HtmlElementsFactory.RegisterFactoryMethod("q", (HtmlContentManager cm) => new TextBasedElement(cm, "q"));
			HtmlElementsFactory.RegisterFactoryMethod("center", (HtmlContentManager cm) => new CenterElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("br", (HtmlContentManager cm) => new LineBreakElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("ol", (HtmlContentManager cm) => new OrderedListElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("ul", (HtmlContentManager cm) => new UnorderedListElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("li", (HtmlContentManager cm) => new ListLevelElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("h1", (HtmlContentManager cm) => new ParagraphElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("h2", (HtmlContentManager cm) => new ParagraphElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("h3", (HtmlContentManager cm) => new ParagraphElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("h4", (HtmlContentManager cm) => new ParagraphElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("h5", (HtmlContentManager cm) => new ParagraphElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("h6", (HtmlContentManager cm) => new ParagraphElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("table", (HtmlContentManager cm) => new TableElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("tr", (HtmlContentManager cm) => new TableRowElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("td", (HtmlContentManager cm) => new TableCellElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("th", (HtmlContentManager cm) => new TableHeaderCellElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("thead", (HtmlContentManager cm) => new TableHeaderElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("tbody", (HtmlContentManager cm) => new TableBodyElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("tfoot", (HtmlContentManager cm) => new TableFooterElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("a", (HtmlContentManager cm) => new HyperlinkElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("img", (HtmlContentManager cm) => new ImageElement(cm));
			HtmlElementsFactory.RegisterFactoryMethod("div", (HtmlContentManager cm) => new BlockElement(cm, "div"));
		}

		public static bool TryCreateInstance(HtmlContentManager contentManager, string elementName, out HtmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<HtmlContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			element = null;
			Func<HtmlContentManager, HtmlElementBase> func;
			if (HtmlElementsFactory.factoryMethods.TryGetValue(elementName, out func))
			{
				element = func(contentManager);
				return true;
			}
			return false;
		}

		static void RegisterFactoryMethod(string elementName, Func<HtmlContentManager, HtmlElementBase> factoryMethod)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<Func<HtmlContentManager, HtmlElementBase>>(factoryMethod, "factoryMethod");
			HtmlElementsFactory.factoryMethods[elementName] = factoryMethod;
		}

		static readonly Dictionary<string, Func<HtmlContentManager, HtmlElementBase>> factoryMethods = new Dictionary<string, Func<HtmlContentManager, HtmlElementBase>>();
	}
}
