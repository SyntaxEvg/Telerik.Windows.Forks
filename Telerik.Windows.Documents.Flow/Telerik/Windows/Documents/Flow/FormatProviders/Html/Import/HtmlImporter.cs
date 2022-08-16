using System;
using System.IO;
using CsQuery;
using PreMailer.Net;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class HtmlImporter
	{
		public HtmlImporter()
		{
			this.contentManager = new HtmlContentManager();
		}

		public RadFlowDocument Import(Stream input, IHtmlImportContext context)
		{
			context.BeginImport();
			CQ cq = CQ.Create(input);
			this.ImportStyles(cq, context);
			this.ImportContent(cq, context);
			context.EndImport();
			return context.Document;
		}

		void ImportStyles(CQ cq, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			CssParser cssParser = new CssParser();
			cssParser.AddStyleSheet(context.Settings.DefaultStyleSheet);
			foreach (IDomObject domObject in cq.Select("style, link"))
			{
				IDomElement domObject2 = (IDomElement)domObject;
				HtmlReader htmlReader = new HtmlReader(domObject2);
				string currentChildElementName = htmlReader.GetCurrentChildElementName();
				HtmlElementBase htmlElementBase = this.contentManager.CreateElement(currentChildElementName, false);
				htmlElementBase.Read(htmlReader, context);
				if (!string.IsNullOrEmpty(htmlElementBase.InnerText))
				{
					cssParser.AddStyleSheet(htmlElementBase.InnerText);
				}
			}
			StyleNamesResolver styleNamesResolver = new StyleNamesResolver(cq);
			context.StyleNamesInfo = styleNamesResolver.Resolve();
			StyleEvaluator styleEvaluator = new StyleEvaluator(cq, cssParser.Styles, context.StyleNamesInfo);
			context.HtmlStyleRepository = styleEvaluator.Evaluate();
		}

		void ImportContent(CQ cq, IHtmlImportContext context)
		{
			HtmlReader htmlReader = new HtmlReader(cq.Remove("style, link"));
			if (htmlReader.HasChildElements())
			{
				do
				{
					string currentChildElementName = htmlReader.GetCurrentChildElementName();
					HtmlElementBase htmlElementBase = this.contentManager.CreateElement(currentChildElementName, true);
					if (htmlElementBase != null)
					{
						htmlElementBase.Read(htmlReader, context);
					}
				}
				while (htmlReader.MoveToNextChildElement());
			}
		}

		const string StyleElementsQuery = "style, link";

		readonly HtmlContentManager contentManager;
	}
}
