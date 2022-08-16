using System;

namespace Telerik.UrlRewriter.Conditions
{
	public sealed class NegativeCondition : IRewriteCondition
	{
		public NegativeCondition(IRewriteCondition chainedCondition)
		{
			if (chainedCondition == null)
			{
				throw new ArgumentNullException("chainedCondition");
			}
			this._chainedCondition = chainedCondition;
		}

		public bool IsMatch(RewriteContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			return !this._chainedCondition.IsMatch(context);
		}

		IRewriteCondition _chainedCondition;
	}
}
