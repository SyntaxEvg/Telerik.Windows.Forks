using System;
using System.Collections;

namespace Telerik.UrlRewriter.Actions
{
	public sealed class RewriteAction : SetLocationAction, IRewriteCondition
	{
		public RewriteAction(string location, RewriteProcessing processing)
			: base(location)
		{
			this._processing = processing;
		}

		public override void Execute(RewriteContext context)
		{
			base.Execute(context);
		}

		public override RewriteProcessing Processing
		{
			get
			{
				return this._processing;
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

		RewriteProcessing _processing;
	}
}
