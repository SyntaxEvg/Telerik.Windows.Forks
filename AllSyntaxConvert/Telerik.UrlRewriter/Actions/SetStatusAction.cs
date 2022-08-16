using System;
using System.Net;

namespace Telerik.UrlRewriter.Actions
{
	public class SetStatusAction : IRewriteAction
	{
		public SetStatusAction(HttpStatusCode statusCode)
		{
			this._statusCode = statusCode;
		}

		public HttpStatusCode StatusCode
		{
			get
			{
				return this._statusCode;
			}
			set
			{
				this._statusCode = value;
			}
		}

		public virtual void Execute(RewriteContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.StatusCode = this.StatusCode;
		}

		public RewriteProcessing Processing
		{
			get
			{
				if (this.StatusCode >= HttpStatusCode.MultipleChoices)
				{
					return RewriteProcessing.StopProcessing;
				}
				return RewriteProcessing.ContinueProcessing;
			}
		}

		HttpStatusCode _statusCode;
	}
}
