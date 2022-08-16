using System;

namespace Telerik.UrlRewriter.Actions
{
	public abstract class SetLocationAction : IRewriteAction
	{
		protected SetLocationAction(string location)
		{
			if (location == null)
			{
				throw new ArgumentNullException("location");
			}
			this._location = location;
		}

		public string Location
		{
			get
			{
				return this._location;
			}
			set
			{
				this._location = value;
			}
		}

		public virtual void Execute(RewriteContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.Location = context.ResolveLocation(context.Expand(this.Location));
		}

		public virtual RewriteProcessing Processing
		{
			get
			{
				return RewriteProcessing.StopProcessing;
			}
		}

		string _location;
	}
}
