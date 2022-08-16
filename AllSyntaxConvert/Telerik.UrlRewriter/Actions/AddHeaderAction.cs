using System;

namespace Telerik.UrlRewriter.Actions
{
	public class AddHeaderAction : IRewriteAction
	{
		public AddHeaderAction(string header, string value)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this._header = header;
			this._value = value;
		}

		public string Header
		{
			get
			{
				return this._header;
			}
			set
			{
				this._header = value;
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
			context.Headers.Add(this.Header, this.Value);
		}

		public RewriteProcessing Processing
		{
			get
			{
				return RewriteProcessing.ContinueProcessing;
			}
		}

		string _header;

		string _value;
	}
}
