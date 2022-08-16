using System;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Telerik.UrlRewriter
{
	[ComVisible(false)]
	[ToolboxData("<{0}:Form runat=server></{0}:RewrittenForm>")]
	public class Form : HtmlForm
	{
		protected override void RenderChildren(HtmlTextWriter writer)
		{
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			base.RenderChildren(writer);
			writer.RenderEndTag();
		}

		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			writer.WriteAttribute("name", this.GetName());
			base.Attributes.Remove("name");
			writer.WriteAttribute("method", this.GetMethod());
			base.Attributes.Remove("method");
			writer.WriteAttribute("action", this.GetAction(), true);
			base.Attributes.Remove("action");
			base.Attributes.Render(writer);
			if (this.ID != null)
			{
				writer.WriteAttribute("id", this.GetID());
			}
		}

		string GetID()
		{
			return this.ClientID;
		}

		string GetName()
		{
			return this.Name;
		}

		string GetMethod()
		{
			return base.Method;
		}

		string GetAction()
		{
			return RewriterHttpModule.RawUrl;
		}
	}
}
