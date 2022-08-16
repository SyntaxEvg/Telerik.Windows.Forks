using System;

namespace Telerik.UrlRewriter.Actions
{
	public class SetPropertyAction : IRewriteAction
	{
		public SetPropertyAction(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this._name = name;
			this._value = value;
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
			context.Properties.Set(this.Name, context.Expand(this.Value));
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
