using System;
using System.Collections;

namespace Telerik.UrlRewriter.Actions
{
	public class ConditionalAction : IRewriteAction, IRewriteCondition
	{
		public IList Conditions
		{
			get
			{
				return this._conditions;
			}
		}

		public IList Actions
		{
			get
			{
				return this._actions;
			}
		}

		public virtual bool IsMatch(RewriteContext context)
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

		public virtual void Execute(RewriteContext context)
		{
			for (int i = 0; i < this.Actions.Count; i++)
			{
				IRewriteCondition rewriteCondition = this.Actions[i] as IRewriteCondition;
				if (rewriteCondition == null || rewriteCondition.IsMatch(context))
				{
					IRewriteAction rewriteAction = this.Actions[i] as IRewriteAction;
					rewriteAction.Execute(context);
					if (rewriteAction.Processing != RewriteProcessing.ContinueProcessing)
					{
						this._processing = rewriteAction.Processing;
						return;
					}
				}
			}
		}

		public RewriteProcessing Processing
		{
			get
			{
				return this._processing;
			}
		}

		RewriteProcessing _processing;

		ArrayList _actions = new ArrayList();

		ArrayList _conditions = new ArrayList();
	}
}
