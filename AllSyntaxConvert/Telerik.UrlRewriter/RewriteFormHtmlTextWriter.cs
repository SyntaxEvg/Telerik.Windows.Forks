using System;
using System.Web;
using System.Web.UI;

namespace Telerik.UrlRewriter
{
	public class RewriteFormHtmlTextWriter : HtmlTextWriter
	{
		public RewriteFormHtmlTextWriter(HtmlTextWriter writer)
			: base(writer)
		{
			base.InnerWriter = writer.InnerWriter;
		}

		public override void WriteAttribute(string name, string value, bool fEncode)
		{
			if (name == "action" && HttpContext.Current.Items["ActionAlreadyWritten"] == null)
			{
				value = RewriterHttpModule.RawUrl;
				HttpContext.Current.Items["ActionAlreadyWritten"] = true;
			}
			base.WriteAttribute(name, value, fEncode);
		}
	}
}
