using System;
using System.Web;
using Telerik.UrlRewriter.Configuration;
using Telerik.UrlRewriter.Utilities;

namespace Telerik.UrlRewriter
{
	public sealed class RewriterHttpModule : IHttpModule
	{
		void IHttpModule.Init(HttpApplication context)
		{
			context.BeginRequest += this.BeginRequest;
		}

		void IHttpModule.Dispose()
		{
		}

		public static RewriterConfiguration Configuration
		{
			get
			{
				return RewriterConfiguration.Current;
			}
		}

		public static string ResolveLocation(string location)
		{
			return RewriterHttpModule._rewriter.ResolveLocation(location);
		}

		public static string OriginalQueryString
		{
			get
			{
				return RewriterHttpModule._rewriter.OriginalQueryString;
			}
			set
			{
				RewriterHttpModule._rewriter.OriginalQueryString = value;
			}
		}

		public static string QueryString
		{
			get
			{
				return RewriterHttpModule._rewriter.QueryString;
			}
			set
			{
				RewriterHttpModule._rewriter.QueryString = value;
			}
		}

		public static string RawUrl
		{
			get
			{
				return RewriterHttpModule._rewriter.RawUrl;
			}
			set
			{
				RewriterHttpModule._rewriter.RawUrl = value;
			}
		}

		void BeginRequest(object sender, EventArgs e)
		{
			HttpContext.Current.Response.AddHeader("X-Powered-By", RewriterHttpModule.Configuration.XPoweredBy);
			RewriterHttpModule._rewriter.Rewrite();
		}

		static RewriterEngine _rewriter = new RewriterEngine(new HttpContextFacade(), RewriterConfiguration.Current);
	}
}
