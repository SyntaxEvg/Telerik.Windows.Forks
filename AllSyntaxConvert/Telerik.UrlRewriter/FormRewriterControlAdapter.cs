using System;
using System.Web.UI;
using System.Web.UI.Adapters;

namespace Telerik.UrlRewriter
{
	public class FormRewriterControlAdapter : ControlAdapter
	{
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(new RewriteFormHtmlTextWriter(writer));
		}
	}
}
