using System;
using System.Web;

namespace Telerik.UrlRewriter.Actions
{
	public class SetCookieAction : IRewriteAction
	{
		public SetCookieAction(string cookieName, string cookieValue)
		{
			if (cookieName == null)
			{
				throw new ArgumentNullException("cookieName");
			}
			if (cookieValue == null)
			{
				throw new ArgumentNullException("cookieValue");
			}
			this._name = cookieName;
			this._value = cookieValue;
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		public void Execute(RewriteContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			HttpCookie cookie = new HttpCookie(this.Name, this.Value);
			context.Cookies.Add(cookie);
		}

		public RewriteProcessing Processing
		{
			get
			{
				return RewriteProcessing.ContinueProcessing;
			}
		}

		string _name;

		string _value;
	}
}
