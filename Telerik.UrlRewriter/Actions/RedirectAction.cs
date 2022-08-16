using System;
using System.Collections;
using System.Net;

namespace Telerik.UrlRewriter.Actions
{
	public sealed class RedirectAction : SetLocationAction, IRewriteCondition
	{
		public RedirectAction(string location, bool permanent)
			: base(location)
		{
			if (location == null)
			{
				throw new ArgumentNullException("location");
			}
			this._permanent = permanent;
		}

		public override void Execute(RewriteContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			base.Execute(context);
			if (this._permanent)
			{
				context.StatusCode = HttpStatusCode.MovedPermanently;
				return;
			}
			context.StatusCode = HttpStatusCode.Found;
		}

		public override RewriteProcessing Processing
		{
			get
			{
				return RewriteProcessing.StopProcessing;
			}
		}

		public bool IsMatch(RewriteContext context)
		{
			foreach (object obj in this.Conditions)
			{
				IRewriteCondition rewriteCondition = (IRewriteCondition)obj;
				if (!rewriteCondition.IsMatch(context))
				{
					return false;
				}
			}
			return true;
		}

		public IList Conditions
		{
			get
			{
				return this._conditions;
			}
		}

		ArrayList _conditions = new ArrayList();

		bool _permanent;
	}
}
